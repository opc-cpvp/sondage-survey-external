using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SurveyToCS
{
	public class FluentValidatorBuilder
	{
		private static List<Element> _surveyAllElements;

		/// <summary>
		/// This flag is use if you want to go cowboy and not have the condition set as comments.
		/// Set to true it will add the conditions (visibleIf or requiredIf) as TODO inside a C# comment.
		/// Set to false, the conditions will be in code. For complexe conditions this may create build errors
		/// </summary>
		private static bool _commentedOut = true;

		private static List<CalculatedValues> _calculatedValues;

		public static string CreateValidators(SurveyObject survey, string _namespace, string className, bool commentedOut)
		{
			_commentedOut = commentedOut;

			_surveyAllElements = new List<Element>();
			_calculatedValues = survey.calculatedValues;

			foreach (var page in survey.pages)
			{
				Common.AddSurveyElement(_surveyAllElements, page, page.elements);
			}

			//  Get a list of dynamic properties, from the matrixdynamic and the paneldynamic.
			//  Then if there is such items, we create the classes validators
			List<Element> survey_dynamic_elements = new List<Element>();
			foreach (var page in survey.pages)
			{
				Common.GetDynamicElements(survey_dynamic_elements, page.elements);
			}

			StringBuilder csharp = new StringBuilder();
			csharp.AppendLine("using System.Collections.Generic;");
			csharp.AppendLine("using FluentValidation;");
			csharp.AppendLine("using System.Linq;");
			csharp.AppendLine("using ComplaintFormCore.Resources;");
			csharp.AppendLine("using ComplaintFormCore.Web_Apis.Models;");

			csharp.AppendLine();

			csharp.Append("namespace ");
			csharp.AppendLine(_namespace);
			csharp.AppendLine("{");

			//  STEP 1: Create the class(s) implementing AbstractValidator

			//  Create the main class validator
			csharp.Append("public partial class ");
			csharp.Append(className);
			csharp.Append("Validator: AbstractValidator<");
			csharp.Append(className);
			csharp.Append(">");
			csharp.AppendLine("{");

			//  constructor
			csharp.Append("public ");
			csharp.Append(className);
			csharp.Append("Validator(SharedLocalizer _localizer)");
			csharp.AppendLine("{");

			csharp.AppendLine("ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;");
			csharp.AppendLine();

			foreach (var page in survey.pages)
			{
				//  We just don't want to deal with the non-html elements
				BuildPageValidator(csharp, page.elements.Where(e => e.type != "html").ToList(), page);
			}

			if (survey_dynamic_elements.Count > 0)
			{
				SetDynamicValidators(csharp, survey_dynamic_elements);
			}

			List<Element> survey_matrix_elements = new List<Element>();
			foreach (var page in survey.pages)
			{
				Common.GetMatrixElements(survey_matrix_elements, page.elements);
			}

			if (survey_matrix_elements.Count > 0)
			{
				csharp.AppendLine();
				BuildMatrixValidators(csharp, survey_matrix_elements);
			}

			csharp.AppendLine("}"); // end constructor

			//if (survey.calculatedValues != null)
			//{
			//	foreach (var methods in survey.calculatedValues)
			//	{
			//		BuildCalculatedValues(csharp, methods);
			//	}
			//}

			csharp.AppendLine("}"); // end main class
			csharp.AppendLine("}");// end namespace

			return csharp.ToString();
		}

		private static void BuildPageValidator(StringBuilder csharp, List<Element> elements, Page parentPage, Element parentPanel = null)
		{
			foreach (var element in elements)
			{
				if (element.type == "panel")
				{
					BuildPageValidator(csharp, element.elements.Where(e => e.type != "html").ToList(), parentPage, element);
				}
				else
				{
					BuildElementValidator(csharp, element, parentPage, parentPanel);
				}
			}
		}

		private static void BuildElementValidator(StringBuilder csharp, Element element, Page parentPage = null, Element parentPanel = null)
		{
			string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			#region Comment such as the element name and which page the property is in

			csharp.Append("// ");
			csharp.Append(elementName);

			if (parentPage != null)
			{
				csharp.Append(" (Page: ");
				csharp.Append(parentPage.name);
				csharp.Append(")");
			}
			#endregion

			csharp.AppendLine();

			string visibleIf = GetVisibleIfFullCondition(element, parentPage, parentPanel);
			string requiredIf = GetRequiredIfFullCondition(element, parentPage, parentPanel);

			if (element.isRequired == true)
			{
				BuildRequiredValidator(csharp, elementName, visibleIf);
			}

			if (!string.IsNullOrWhiteSpace(requiredIf))
			{
				BuildRequiredValidator(csharp, elementName, requiredIf);
			}

			if (type == "comment")
			{
				BuildMaxLengthValidator(csharp, element, visibleIf);
			}
			else if (type == "text" && element.inputType != "date" && element.inputType != "number")
			{
				BuildMaxLengthValidator(csharp, element, visibleIf);
			}

			if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
			{
				//  Check if selected option is in the list of valid options
				BuildListValidator(csharp, element, visibleIf);
			}

			if (type == "text" && element.inputType == "email")
			{
				BuildEmailValidator(csharp, elementName, visibleIf);
			}

			csharp.AppendLine();
		}

		private static bool ElementHasRules(Element element)
		{
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			if (element.isRequired == true)
			{
				return true;
			}

			if (type == "comment")
			{
				return true;
			}

			if (type == "text" && element.inputType != "date" && element.inputType != "number")
			{
				return true;
			}

			if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
			{
				return true;
			}

			if (type == "text" && element.inputType == "email")
			{
				return true;
			}

			return false;
		}

		private static void BuildDynamicElementValidator(StringBuilder csharp, Element element, string childParameter, string page_name)
		{
			string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			csharp.Append("// ");
			csharp.Append(elementName);
			csharp.Append(" (Page: ");
			csharp.Append(page_name);
			csharp.Append(")");
			csharp.AppendLine();

			string visibleIf = GetVisibleIfFullCondition(element, null, null);

			if (element.isRequired == true)
			{
				csharp.Append(childParameter);
				csharp.Append(".");
				BuildRequiredValidator(csharp, elementName, visibleIf);
			}

			if (type == "comment")
			{
				csharp.Append(childParameter);
				csharp.Append(".");
				BuildMaxLengthValidator(csharp, element, visibleIf);
			}
			else if (type == "text" && element.inputType != "date" && element.inputType != "number")
			{
				csharp.Append(childParameter);
				csharp.Append(".");
				BuildMaxLengthValidator(csharp, element, visibleIf);
			}
			if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
			{
				//  Check if selected option is in the list of valid options
				csharp.Append(childParameter);
				csharp.Append(".");
				BuildListValidator(csharp, element, visibleIf);
			}

			if (type == "text" && element.inputType == "email")
			{
				csharp.Append(childParameter);
				csharp.Append(".");
				BuildEmailValidator(csharp, elementName, visibleIf);
			}
		}

		private static void SetDynamicValidators(StringBuilder csharp, List<Element> dynamicElements)
		{
			var groupedByValueName = from item in dynamicElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				csharp.Append("// ");
				csharp.AppendLine(dynamicItem.Key);

				foreach (var element in dynamicItem)
				{
					//RuleForEach(x => x.PersonalInformationCategory).ChildRules(child => {
					//    child.RuleFor(x => x.Category).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
					//});

					string visibleIf = GetVisibleIfFullCondition(element, element.parent, null);

					if (element.type == "matrixdynamic")
					{
						foreach (var column in element.columns)
						{
							if (!ElementHasRules(column))
							{
								continue;
							}

							csharp.Append("RuleForEach(x => x.");
							csharp.Append(dynamicItem.Key);
							csharp.AppendLine(").ChildRules(child => {");

							BuildDynamicElementValidator(csharp, column, "child", element.parent.name);

							csharp.Append("})");

							if (!string.IsNullOrWhiteSpace(visibleIf))
							{
								csharp.Append(".When( ");
								csharp.Append(visibleIf); ;
								csharp.Append(")");
							}

							csharp.AppendLine(";");
							csharp.AppendLine();
						}
					}
					else if (element.type == "paneldynamic")
					{
						foreach (var templateItem in element.templateElements)
						{
							if (!ElementHasRules(templateItem))
							{
								continue;
							}

							csharp.Append("RuleForEach(x => x.");
							csharp.Append(dynamicItem.Key);
							csharp.AppendLine(").ChildRules(child => {");

							BuildDynamicElementValidator(csharp, templateItem, "child", element.parent.name);

							csharp.Append("})");

							if (!string.IsNullOrWhiteSpace(visibleIf))
							{
								csharp.Append(".When( ");
								csharp.Append(visibleIf); ;
								csharp.Append(")");
							}

							csharp.AppendLine(";");
							csharp.AppendLine();
						}
					}
				}
			}
		}

		private static void BuildMatrixValidators(StringBuilder csharp, List<Element> matrixElements)
		{
			var groupedByValueName = from item in matrixElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				csharp.Append("// ");
				csharp.AppendLine(dynamicItem.Key);

				csharp.Append("RuleFor(x => x.");
				csharp.Append(dynamicItem.Key);
				csharp.AppendLine(").ChildRules(child => {");

				foreach (var element in dynamicItem)
				{
					string visibleIf = GetVisibleIfFullCondition(element, element.parent, null);
					List<string> possibleValues = element.columns.Select(c => c.value).ToList();

					foreach (var row in JsonConvert.DeserializeObject<List<Row>>(element.rows.ToString()))
					{
						csharp.Append("child.RuleFor(x => x.");

						csharp.Append(row.value);
						csharp.Append(").Must(x => new List<string> { ");

						for (int i = 0; i < possibleValues.Count; i++)
						{
							csharp.Append("\"");
							csharp.Append(possibleValues[i]);
							csharp.Append("\"");

							if (i < possibleValues.Count - 1)
							{
								csharp.Append(",");
							}
						}

						csharp.Append("}.Contains(x))");

						if (!string.IsNullOrWhiteSpace(visibleIf))
						{
							csharp.Append(".When( ");
							csharp.Append(visibleIf); ;
							csharp.Append(")");
						}

						csharp.Append(".WithMessage(");
						csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"SelectedValueNotValid\")); ");
					}
				}

				csharp.Append("})");
				csharp.AppendLine(";");
				csharp.AppendLine();
			}
		}

		/// <summary>
		/// Build a list of visible condition for the element. Each element can have visibleIf property set but also
		/// the parent panel or the parent page can have visibleIf and we need to include it
		/// </summary>
		private static string GetVisibleIfCondition(Element element, Page parentPage, Element parentPanel = null)
		{
			string visibleIf = string.Empty;

			//  First we get all of the visibleIf conditions from the element and the parent element into a string
			if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf))
			{
				visibleIf += parentPage.visibleIf;
			}

			if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.visibleIf))
			{
				if (!string.IsNullOrWhiteSpace(visibleIf))
				{
					visibleIf += " && ";
				}

				visibleIf += parentPanel.visibleIf;
			}

			if (!string.IsNullOrWhiteSpace(element.visibleIf))
			{
				//  There is a condition
				if (!string.IsNullOrWhiteSpace(visibleIf))
				{
					visibleIf += " && ";
				}

				visibleIf += element.visibleIf;
			}

			if (string.IsNullOrWhiteSpace(visibleIf) == false)
			{
				return ReplaceData(visibleIf);
			}

			return string.Empty;
		}

		/// <summary>
		/// Build a list of required condition for the element. Each element can have requiredIf property set but also
		/// the parent panel or the parent page can have visibleIf and we need to include it
		/// </summary>
		private static string GetRequiredIfCondition(Element element, Page parentPage, Element parentPanel = null)
		{
			string requiredIf = string.Empty;
			bool hasRequiredIf = false;

			if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf))
			{
				//  YES! Because requiredIf is only for Question it is still possible
				//  that the page of an element has a visibleIf condition
				requiredIf += parentPage.visibleIf;
			}

			if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.requiredIf))
			{
				if (!string.IsNullOrWhiteSpace(requiredIf))
				{
					requiredIf += " && ";
				}

				requiredIf += parentPanel.requiredIf;
				hasRequiredIf = true;
			}

			if (!string.IsNullOrWhiteSpace(element.requiredIf))
			{
				//  There is a condition
				if (!string.IsNullOrWhiteSpace(requiredIf))
				{
					requiredIf += " && ";
				}

				requiredIf += element.requiredIf;
				hasRequiredIf = true;
			}

			if (!hasRequiredIf)
			{
				return string.Empty;
			}

			if (string.IsNullOrWhiteSpace(requiredIf) == false)
			{
				MatchCollection anyofs = Regex.Matches(requiredIf, Common._anyof_pattern);

				if (anyofs.Count > 0)
				{
					ReplaceAnyOf(ref requiredIf);
				}

				ReplaceProperty(ref requiredIf);
				ReplaceOperators(ref requiredIf);
				return requiredIf;
			}

			return string.Empty;
		}

		private static string GetVisibleIfFullCondition(Element element, Page parentPage, Element parentPanel = null)
		{
			string visibleIf = GetVisibleIfCondition(element, parentPage, parentPanel);

			if (string.IsNullOrWhiteSpace(visibleIf) == false)
			{
				if (_commentedOut)
				{
					return "x => true/* TODO: " + visibleIf + "*/";
				}
				else
				{
					return "x => "+  visibleIf;
				}
			}

			return string.Empty;
		}

		private static string GetRequiredIfFullCondition(Element element, Page parentPage, Element parentPanel = null)
		{
			string requiredIf = GetRequiredIfCondition(element, parentPage, parentPanel);

			if (string.IsNullOrWhiteSpace(requiredIf) == false)
			{
				if (_commentedOut)
				{
					return "x => true/* TODO: " + requiredIf + "*/";
				}
				else
				{
					return "x => " + requiredIf;
				}
			}

			return string.Empty;

		}

		private static string ReplaceData(string visibleIf)
		{
			//	Panel such as "visibleIf": "{panel.CompleteState} contains 'yes_already_completed'"
			//	is ised in paneldynamic for column visiblity.
			visibleIf = visibleIf.Replace("panel.", "");

			MatchCollection anyofs = Regex.Matches(visibleIf, Common._anyof_pattern);

			if (anyofs.Count > 0)
			{
				ReplaceAnyOf(ref visibleIf);
			}

			//  Replace {Property} by x.Property
			ReplaceProperty(ref visibleIf);
			ReplaceOperators(ref visibleIf);
			return visibleIf;
		}

		private static void ReplaceAnyOf(ref string condition)
		{
			foreach (Match match_anyof in Regex.Matches(condition, Common._anyof_pattern))
			{
				//  {InstitutionAgreedRequestOnInformalBasis} anyof ['yes','not_sure']
				// x.NatureOfComplaint.Intersect(new List<string> { "NatureOfComplaintDelay", "NatureOfComplaintExtensionOfTime", "NatureOfComplaintDenialOfAccess", "NatureOfComplaintLanguage" }).Any()
				Regex rgx_property = new Regex(Common._property_pattern);
				Match matchProperty = rgx_property.Match(match_anyof.Value);

				//  Proerty {property} becomes propery
				string matchPropertyClean = matchProperty.Value.Replace("{", "").Replace("}", "");

				//  Find the conditional property
				Element conditonalProperty = _surveyAllElements.Where(x => x.name == matchPropertyClean).First();
				string replacement = string.Empty;

				//	We assume that for any questions using choicesByUrl the value will be integer
				bool isIntegerValueItems = conditonalProperty.choicesByUrl != null && !string.IsNullOrWhiteSpace(conditonalProperty.choicesByUrl.url);

				if (conditonalProperty.type == "tagbox" || conditonalProperty.type == "checkbox")
				{
					//  tagbox & checkbox can have multiple choices so they are List<string> properties
					replacement = "x." + matchPropertyClean + ".Intersect";
				}
				else
				{
					//  This applies to type that has only one value selectable such as dropdown or radiogroup
					//  Very important to leave a space between the { property } otherwise the {} will be overwritten below

					if(isIntegerValueItems)
					{
						replacement = "new List<int>() { x." + matchPropertyClean + " != null ? (int)x."+ matchPropertyClean + " : 0 }.Intersect";
					}
					else
					{
						replacement = "new List<string>() { x." + matchPropertyClean + " }.Intersect";
					}
				}

				Regex rgx_items = new Regex(Common._anyof_pattern_items);
				Match matchArray = rgx_items.Match(match_anyof.Value);

				if (isIntegerValueItems)
				{
					replacement += "(new List<int>() {" + matchArray.Value.Replace("'", "\"").Replace("[", "").Replace("]", "");
				}
				else
				{
					replacement += "(new List<string>() {" + matchArray.Value.Replace("'", "\"").Replace("[", "").Replace("]", "");
				}

				replacement += "}).Any()";

				condition = condition.Replace(match_anyof.Value, replacement);
			}
		}

		/// <summary>
		/// Replace {Property} by x.Property
		/// </summary>
		private static void ReplaceProperty(ref string condition)
		{
			foreach (Match match_propery in Regex.Matches(condition, Common._property_pattern))
			{
				string prop_name = match_propery.Value.Replace("{", "").Replace("}", "");
				Element elem = _surveyAllElements.Where(e => e.name == prop_name).FirstOrDefault();

				if (elem != null)
				{
					if(elem.type == "tagbox") // || checkbox ?
					{
						string propertyName = match_propery.Value.Replace(match_propery.Value, match_propery.Value.Replace("{", "x.").Replace("}", ""));

						Match match_propery_single_item = Regex.Match(condition, Common._arraySingleElementSelected);
						if (match_propery_single_item.Success)
						{
							//	{WhereWhomInfoCollected} = ['directly'] -> x.WhereWhomInfoCollected.Count == 1 && x.WhereWhomInfoCollected.Contains("directly")
							condition = propertyName + ".Count == 1 && " + propertyName + ".Contains(" + match_propery_single_item.Value.Replace("[", "").Replace("]", "") + ")";
						}
						else
						{
							//	{WhereWhomInfoCollected} contains 'other' -> x.WhereWhomInfoCollected.Contains("other")
							Match match_condition_value = Regex.Match(condition, Common._wordInSingleQuotes);
							if (match_condition_value.Success)
							{
								condition = propertyName + ".Contains(" + match_condition_value.Value + ")";
							}
							else
							{
								//	This should not happen
								condition = condition.Replace(match_propery.Value, match_propery.Value.Replace("{", "x.").Replace("}", ""));
							}
						}

						continue;
					}
				}
				else if(_calculatedValues != null && _calculatedValues.Where(c => c.name == prop_name).Any())
				{
					//	The property match for the condtion is a calculated value

					var calculatedValue = _calculatedValues.Where(c => c.name == prop_name).FirstOrDefault();
					if(calculatedValue != null)
					{
						string expression = calculatedValue.expression;
						condition = ReplaceData(expression);
						continue;
					}
				}

				condition = condition.Replace(match_propery.Value, match_propery.Value.Replace("{", "x.").Replace("}", ""));
			}
		}

		private static void ReplaceOperators(ref string condition)
		{
			condition = condition.Replace("'true'", "true").Replace(" = ", " == ").SafeReplace("contains", "==", true).Replace("<>", "!=").Replace("'", "\"").SafeReplace("or", "||", true).SafeReplace("and", "&&", true);
		}

		private static void BuildRequiredValidator(StringBuilder csharp, string elementName, string condition)
		{
			csharp.Append("RuleFor(x => x.");
			csharp.Append(elementName);
			csharp.Append(").NotEmpty()");

			if (!string.IsNullOrWhiteSpace(condition))
			{
				csharp.Append(".When( ");
				csharp.Append(condition); ;
				csharp.Append(")");
			}

			csharp.Append(".WithMessage(");
			csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsRequired\")); ");
		}

		private static void BuildMaxLengthValidator(StringBuilder csharp, Element element, string condition)
		{
			int maxLength = 5000;
			string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			if (type == "text")
			{
				maxLength = 200;
			}

			if (element.maxLength != null)
			{
				maxLength = (int)element.maxLength;
			}

			csharp.Append("RuleFor(x => x.");
			csharp.Append(elementName);
			csharp.Append(").Length(0,");
			csharp.Append(maxLength);
			csharp.Append(")");

			if (!string.IsNullOrWhiteSpace(condition))
			{
				csharp.Append(".When( ");
				csharp.Append(condition); ;
				csharp.Append(")");
			}

			csharp.Append(".WithMessage(");
			csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsOverCharacterLimit\")); ");
		}

		private static void BuildListValidator(StringBuilder csharp, Element element, string condition)
		{
			//  Check if selected option is in the list of valid options

			//RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));

			string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			if (type == "checkbox" || type == "tagbox")
			{
				csharp.Append("RuleForEach(x => x.");
			}
			else
			{
				csharp.Append("RuleFor(x => x.");
			}

			csharp.Append(elementName);

			csharp.Append(").Must(x => new List<string> { ");

			for (int i = 0; i < element.choices.Count; i++)
			{
				var choice = element.choices[i];

				csharp.Append("\"");
				csharp.Append(choice.value);
				csharp.Append("\"");

				if (i < element.choices.Count - 1)
				{
					csharp.Append(",");
				}
			}

			csharp.Append("}.Contains(x))");

			if (!string.IsNullOrWhiteSpace(condition))
			{
				csharp.Append(".When( ");
				csharp.Append(condition); ;
				csharp.Append(")");
			}

			csharp.Append(".WithMessage(");
			csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"SelectedValueNotValid\")); ");
		}

		private static void BuildCalculatedValues(StringBuilder csharp, CalculatedValues calculatedValue)
		{
			csharp.Append("private void ");
			csharp.Append(calculatedValue.name);
			csharp.AppendLine(" ()");
			csharp.AppendLine("{");

			csharp.AppendLine("//   TODO: Translate this into code");
			csharp.Append("//   ");
			csharp.AppendLine(calculatedValue.expression);

			csharp.AppendLine("}");
		}

		private static void BuildEmailValidator(StringBuilder csharp, string elementName, string condition)
		{
			csharp.Append("RuleFor(x => x.");
			csharp.Append(elementName);
			csharp.Append(").EmailAddress()");

			if (!string.IsNullOrWhiteSpace(condition))
			{
				csharp.Append(".When( ");
				csharp.Append(condition); ;
				csharp.Append(")");
			}

			csharp.AppendLine(";");
		}
	}
}
