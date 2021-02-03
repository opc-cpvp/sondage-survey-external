using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace SurveyToCS
{
	public class ClassPropertiesBuilder
	{
		public static void CreateClassObject(SurveyObject survey, string _namespace, string className)
		{
			//	TODO: Question type 'matrixdropdown' is not implemented here.

			List<Element> survey_matrix_elements = new List<Element>();
			foreach (var page in survey.pages)
			{
				Common.GetMatrixElements(survey_matrix_elements, page.elements);
			}

			List<Element> survey_dynamic_elements = new List<Element>();
			foreach (var page in survey.pages)
			{
				Common.GetDynamicElements(survey_dynamic_elements, page.elements);
			}

			StringBuilder csharp = new StringBuilder();
			csharp.AppendLine("using System.Collections.Generic;");
			csharp.AppendLine("using System;");
			csharp.AppendLine();

			csharp.Append("namespace ");
			csharp.AppendLine(_namespace);
			csharp.AppendLine("{");

			csharp.Append("public class ");
			csharp.Append(className);
			csharp.AppendLine("{");

			foreach (var page in survey.pages)
			{
				AddClassProperties(csharp, page.elements.Where(x => Common._simpleTypeElements.Contains(x.type)).ToList(), page);
			}

			#region Add the paneldynamic and matrixdynamic questions as class properties

			if (survey_dynamic_elements.Count > 0)
			{
				csharp.AppendLine();

				//	Grouping the dynamic questions by value name or name
				var groupedByValueName = from item in survey_dynamic_elements
										 group item by item.valueName != null ? item.valueName : item.name into newGroup
										 orderby newGroup.Key
										 select newGroup;

				foreach (var dynamicItem in groupedByValueName)
				{
					AddDynamicProperties(csharp, dynamicItem.Key);
				}
			}
			#endregion

			#region Add the matrix type questions as class properties

			if (survey_matrix_elements.Count > 0)
			{
				csharp.AppendLine();
				AddMatrixProperties(csharp, survey_matrix_elements);
			}
			#endregion

			csharp.Append("}"); // end main class

			// Add the matrix type questions as new classes
			if (survey_matrix_elements.Count > 0)
			{
				csharp.AppendLine();
				AddMatrixClasses(csharp, survey_matrix_elements);
			}

			// Add the paneldynamic and the matrixdynamic type questions as new classes
			if (survey_dynamic_elements.Count > 0)
			{
				csharp.AppendLine();
				AddDynamicClasses(csharp, survey_dynamic_elements);
			}

			csharp.AppendLine("}");// end namespace

			Console.WriteLine(csharp.ToString());
			Console.ReadLine();
		}

		/// <summary>
		/// Add the Properties to the class, the { get; set; }
		/// </summary>
		private static void AddClassProperties(StringBuilder csharp, List<Element> elements, Page pageObj)
		{
			foreach (var element in elements)
			{
				if (element.type == "panel")
				{
					AddClassProperties(csharp, element.elements.Where(x => Common._simpleTypeElements.Contains(x.type)).ToList(), pageObj);
				}
				else
				{
					string elementName = Common.CapitalizeFirstLetter(!string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name);

					BuildElementSummary(csharp, element, pageObj);

					csharp.Append("public ");

					//  NOTE:	We always set the booleans, int or datetime to nullable, otherwise the default value is set on the property
					//			(false, 0 or DateTime.MinValue) and our validation doesn't work

					switch (element.type)
					{
						case "boolean":
							csharp.Append(" bool? ");
							csharp.Append(elementName);
							break;
						case "matrixdynamic":
						case "paneldynamic" :
							throw new Exception("this code should never execute");
						case "checkbox":
						case "tagbox":
							csharp.Append("List<string> ");
							csharp.Append(elementName);
							break;
						case "file":
							csharp.Append("List<SurveyFile> ");
							csharp.Append(elementName);
							break;
						case "datepicker":
						case "text" when element.inputType == "date":
							csharp.Append(" DateTime? ");
							csharp.Append(elementName);
							break;
						case "text" when element.inputType == "number":
							csharp.Append(" int? ");
							csharp.Append(elementName);
							break;
						default:
							csharp.Append(" string ");
							csharp.Append(elementName);
							break;
					}

					csharp.AppendLine(" { get; set; }");
					csharp.AppendLine();
				}
			}
		}

		/// <summary>
		/// Add the matrixdynamic and paneldynamic properties as collection of items to the main class
		/// </summary>
		private static void AddDynamicProperties(StringBuilder csharp, string elementName)
		{
			csharp.Append("public ");
			csharp.Append("List<");
			csharp.Append(Common.CapitalizeFirstLetter(elementName));
			csharp.Append("> ");
			csharp.Append(Common.CapitalizeFirstLetter(elementName));
			csharp.AppendLine(" { get; set; }");
			csharp.AppendLine();
		}

		/// <summary>
		/// For each of the matrixdynamic or paneldynamic questions grouped by valueName a new class is created with
		/// all the columns becoming class properties
		/// </summary>
		private static void AddDynamicClasses(StringBuilder csharp, List<Element> dynamicElements)
		{
			var groupedByValueName = from item in dynamicElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				csharp.Append("public class ");
				csharp.Append(dynamicItem.Key);
				csharp.AppendLine("{");

				foreach (var item in dynamicItem)
				{
					if (item.type == "matrixdynamic")
					{
						foreach (var column in item.columns)
						{
							AddDynamicPropertyClassItem(csharp, column);
						}
					}
					else if (item.type == "paneldynamic")
					{
						foreach (var templateItem in item.templateElements)
						{
							AddDynamicPropertyClassItem(csharp, templateItem);
						}
					}
					else
					{
						throw new Exception("something is wrong here... it should be either a matrixdynamic or a paneldynamic");
					}
				}

				csharp.AppendLine("}");
			}
		}

		private static void AddDynamicPropertyClassItem(StringBuilder csharp, Element element)
		{
			var name = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;

			BuildElementSummary(csharp, element);

			//  NOTE: We always set the booleans, int or datetime to nullable, otherwise the default value is set on the property
			//  and our validation doesn't work

			if (element.type == "boolean")
			{
				csharp.Append("public bool? ");
			}
			else if (element.inputType == "date" || element.type == "datepicker")
			{
				csharp.Append("public DateTime? ");
			}
			else if (element.inputType == "number")
			{
				csharp.Append("public int? ");
			}
			else if (element.type == "file")
			{
				csharp.Append("public List<SurveyFile> ");
			}
			else if (element.type == "matrixdynamic")
			{
				string className = Common.CapitalizeFirstLetter(name) + "Object";
				csharp.Append("public class ");
				csharp.Append(className);
				csharp.AppendLine("{");

				foreach (var column in element.columns)
				{
					AddDynamicPropertyClassItem(csharp, column);
				}

				csharp.AppendLine("}");

				csharp.Append("public List<");
				csharp.Append(className);
				csharp.Append("> ");
			}
			else
			{
				csharp.Append("public string ");
			}

			csharp.Append(Common.CapitalizeFirstLetter(name));
			csharp.AppendLine(" { get; set; }");
			csharp.AppendLine();
		}

		private static void BuildElementSummary(StringBuilder csharp, Element element, Page page = null)
		{
			csharp.AppendLine("/// <summary>");

			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

			if (page != null)
			{
				csharp.Append("/// Page: ");
				csharp.Append(page.name);
				csharp.AppendLine("<br/>");

				if (!string.IsNullOrWhiteSpace(page.section))
				{
					csharp.Append("/// Section: ");
					csharp.Append(page.section);
					csharp.AppendLine("<br/>");
				}
			}

			//csharp.Append("/// Question ");
			//csharp.AppendLine("<br/>");

			if (element.title != null)
			{
				csharp.Append("/// ");

				if (element.title.en.Length > 75)
				{
					csharp.Append(element.title.en.Substring(0, 75));
					csharp.Append("...");
				}
				else
				{
					csharp.Append(element.title.en);
				}

				csharp.AppendLine("<br/>");
			}

			if (type == "checkbox" || type == "radiogroup" || type == "dropdown")
			{
				csharp.Append("/// ");
				csharp.Append("Possible choices: [");

				if (element.choices != null)
				{
					csharp.AppendJoin(", ", element.choices.Select(c => c.value));
				}
				else if (element.choicesByUrl != null)
				{
					csharp.Append(element.choicesByUrl.url);
				}

				csharp.Append("]");
				csharp.AppendLine("<br/>");
			}

			if (element.isRequired && !string.IsNullOrWhiteSpace(element.visibleIf))
			{
				csharp.Append("/// Required condition: ");
				csharp.Append(element.visibleIf);
				csharp.AppendLine("<br/>");
			}

			csharp.Append("/// Survey question type: ");
			csharp.Append(type);

			if (!string.IsNullOrEmpty(element.inputType))
			{
				csharp.Append(" (");
				csharp.Append(element.inputType);
				csharp.Append(")");
			}

			csharp.AppendLine();

			csharp.AppendLine("/// </summary>");
		}

		/// <summary>
		/// Add the matrix type questions as main class properties
		/// </summary>
		private static void AddMatrixProperties(StringBuilder csharp, List<Element> matrixElements)
		{
			//	First we group the matrix elements by valueName or name
			var groupedByValueName = from item in matrixElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				//	Each group becomes a new property

				string elementName = Common.CapitalizeFirstLetter(dynamicItem.Key);

				//	TODO:	Figure out a way to show a summary
				//			BuildElementSummary(csharp, dynamicItem.First(), dynamicItem.First().parent);

				csharp.Append("public ");
				csharp.Append(elementName);
				csharp.Append(" ");
				csharp.Append(elementName);
				csharp.AppendLine(" { get; set; }");
				csharp.AppendLine();
			}
		}

		/// <summary>
		/// For each of the matrix questions grouped by valueName a new class is created with
		/// all the rows becoming class properties
		/// </summary>
		private static void AddMatrixClasses(StringBuilder csharp, List<Element> matrixElements)
		{
			//	First we group the matrix elements by valueName or name
			var groupedByValueName = from item in matrixElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				//	Each group becomes a new class

				csharp.Append("public class ");
				csharp.Append(dynamicItem.Key);
				csharp.AppendLine("{");

				foreach (var item in dynamicItem)
				{
					//	... and each column element of the matrix becomes a property of this new class
					//	We need to Deserialize the Object items.rows since this Element property is of type Object
					foreach (var row in JsonConvert.DeserializeObject<List<Row>>(item.rows.ToString()))
					{
						csharp.Append("public string ");
						csharp.Append(Common.CapitalizeFirstLetter(row.value));
						csharp.AppendLine(" { get; set; }");
						csharp.AppendLine();
					}
				}

				csharp.AppendLine("}");
			}
		}
	}
}
