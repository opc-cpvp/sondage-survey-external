using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyToCS
{
	public class TestDataBuilder
	{
		public static string CreateTestData(SurveyObject survey)
		{
			StringBuilder csharp = new StringBuilder();

			foreach (var page in survey.pages)
			{
				BuildPropertyData(csharp, page.elements.Where(e => Common._simpleTypeElements.Contains(e.type)).ToList(), page);
			}

			List<Element> survey_dynamic_elements = new List<Element>();

			foreach (var page in survey.pages)
			{
				Common.GetDynamicElements(survey_dynamic_elements, page.elements);
			}

			if (survey_dynamic_elements.Count > 0)
			{
				BuildDynamicPropertyData(csharp, survey_dynamic_elements);
			}

			return csharp.ToString();
		}

		private static void BuildPropertyData(StringBuilder csharp, List<Element> elements, Page pageObj)
		{
			foreach (var element in elements)
			{
				if (element.type == "panel")
				{
					BuildPropertyData(csharp, element.elements.Where(e => Common._simpleTypeElements.Contains(e.type)).ToList(), pageObj);
				}
				else
				{
					string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;

					csharp.Append(Common.CapitalizeFirstLetter(elementName));
					csharp.Append(":");
					csharp.Append(GetPropertyValue(element));
					csharp.AppendLine(",");
				}
			}
		}

		private static void BuildDynamicPropertyData(StringBuilder csharp, List<Element> dynamicElements)
		{
			var groupedByValueName = from item in dynamicElements
									 group item by item.valueName != null ? item.valueName : item.name into newGroup
									 orderby newGroup.Key
									 select newGroup;

			foreach (var dynamicItem in groupedByValueName)
			{
				csharp.Append(dynamicItem.Key);
				csharp.AppendLine(": [");

				csharp.Append("{");
				foreach (var element in dynamicItem)
				{
					if (element.type == "matrixdynamic")
					{
						foreach (var column in element.columns)
						{
							string columnName = !string.IsNullOrWhiteSpace(column.valueName) ? column.valueName : column.name;
							csharp.AppendLine();
							csharp.Append(columnName);
							csharp.Append(":");
							csharp.Append(GetPropertyValue(column));
							csharp.Append(",");
						}
					}
					else if (element.type == "paneldynamic")
					{
						foreach (var column in element.templateElements)
						{
							string columnName = !string.IsNullOrWhiteSpace(column.valueName) ? column.valueName : column.name;
							csharp.AppendLine();
							csharp.Append(columnName);
							csharp.Append(":");
							csharp.Append(GetPropertyValue(column));
							csharp.Append(",");
						}
					}
				}

				csharp.AppendLine("},");
				csharp.AppendLine("],");
			}
		}

		private static string GetPropertyValue(Element element)
		{
			string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;
			StringBuilder retVal = new StringBuilder();

			if (type == "boolean")
			{
				retVal.Append("true");
			}
			else if (type == "radiogroup" || type == "dropdown")
			{
				retVal.Append("\"");

				if (element.choices != null && element.choices.Count > 0)
				{
					var rand = new Random();
					retVal.Append(element.choices[rand.Next(0, element.choices.Count - 1)].value);
				}

				retVal.Append("\"");
			}
			else if (type == "checkbox" || type == "tagbox")
			{
				retVal.Append("[\"");

				if (element.choices != null && element.choices.Count > 0)
				{
					var rand = new Random();
					retVal.Append(element.choices[rand.Next(0, element.choices.Count - 1)].value);
				}

				retVal.Append("\"]");
			}
			else if (type == "file")
			{
				retVal.Append("[]");
			}
			else if (type == "text")
			{
				retVal.Append("\"");

				if (element.inputType == "tel")
				{
					var rand = new Random();
					retVal.Append("+1613-");
					retVal.Append(rand.Next(200, 700).ToString());
					retVal.Append("-");
					retVal.Append(rand.Next(1000, 9999).ToString());
				}
				else if (element.inputType == "date")
				{
					retVal.Append(DateTime.Now.ToString("yyyy-MM-dd"));
				}
				else if (element.inputType == "email")
				{
					retVal.Append(RandomText(10) + "@gmail.com");
				}
				else if (element.inputType == "number")
				{
					var rand = new Random();
					retVal.Append(rand.Next(200, 700).ToString());
				}
				else if (element.inputType == "url")
				{
					var rand = new Random();
					retVal.Append("http://" + RandomText(10) + ".com");
				}
				else
				{
					retVal.Append(RandomText(25));
				}

				retVal.Append("\"");
			}
			else if (type == "comment")
			{
				retVal.Append("\"");
				retVal.Append(RandomText(75));
				retVal.Append(" ");
				retVal.Append(RandomText(75));
				retVal.Append(" ");
				retVal.Append(RandomText(75));
				retVal.Append("\"");
			}

			return retVal.ToString();
		}

		private static string RandomText(int length)
		{
			StringBuilder str_build = new StringBuilder();
			Random random = new Random();

			char letter;

			for (int i = 0; i < length; i++)
			{
				double flt = random.NextDouble();
				int shift = Convert.ToInt32(Math.Floor(25 * flt));
				letter = Convert.ToChar(shift + 65);
				str_build.Append(letter);
			}

			return str_build.ToString();
		}
	}
}
