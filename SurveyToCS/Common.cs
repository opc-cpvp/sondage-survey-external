using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyToCS
{
	public class Common
	{
		public static readonly List<string> _simpleTypeElements = new List<string>() { "text", "comment", "dropdown", "tagbox", "radiogroup", "boolean", "panel", "file", "checkbox" };
		public static readonly List<string> _dynamicElements = new List<string>() { "matrixdynamic", "paneldynamic" };

		public static readonly string _anyof_pattern = @"{[a-zA-Z_]+}\sanyof\s\[[a-zA-Z,'_]+\]";
		public static readonly string _property_pattern = @"({[a-zA-Z_]+})";
		public static readonly string _anyof_pattern_items = @"[[a-zA-Z,'_]+\]";

		public static string CapitalizeFirstLetter(string str)
		{
			return char.ToUpper(str[0]) + str.Substring(1);
		}

		/// <summary>
		/// Add the parent page for each question element in the survey.
		/// </summary>
		public static void AddParentPage(Page pageObj, List<Element> elements)
		{
			foreach (var element in elements)
			{
				if (element.type == "panel")
				{
					AddParentPage(pageObj, element.elements);
				}
				else
				{
					element.parent = pageObj;
				}
			}
		}

		/// <summary>
		/// Build a list of all questions elements in the survey including the questions
		/// inside panels
		/// </summary>
		public static void AddSurveyElement(List<Element> all_elements, Page pageObj, List<Element> elements)
		{
			foreach (var element in elements)
			{
				if (element.type == "panel")
				{
					AddSurveyElement(all_elements, pageObj, element.elements);
				}
				else
				{
					all_elements.Add(element);
				}
			}
		}

		/// <summary>
		/// Build a list of "matrixdynamic" and "paneldynamic" type questions
		/// </summary>
		public static void GetDynamicElements(List<Element> survey_dynamic_elements, List<Element> page_or_panel_elements)
		{
			foreach (var element in page_or_panel_elements)
			{
				if (element.type == "panel")
				{
					GetDynamicElements(survey_dynamic_elements, element.elements);
				}
				else
				{
					if (_dynamicElements.Contains(element.type))
					{
						survey_dynamic_elements.Add(element);
					}
				}
			}
		}

		/// <summary>
		/// Build a list of "matrix" type questions
		/// </summary>
		public static void GetMatrixElements(List<Element> survey_matrix_elements, List<Element> page_or_panel_elements)
		{
			foreach (var element in page_or_panel_elements)
			{
				if (element.type == "panel")
				{
					GetMatrixElements(survey_matrix_elements, element.elements);
				}
				else
				{
					if (element.type == "matrix")
					{
						survey_matrix_elements.Add(element);
					}
				}
			}
		}
	}
}
