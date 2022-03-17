using System;
using System.Collections.Generic;
using System.Text;

namespace SurveyToCS
{
	public class SurveyObject
	{
		public List<Page> pages { get; set; }
		public List<CalculatedValues> calculatedValues { get; set; }
	}

	public class Page
	{
		public string name { get; set; }
		public string title { get; set; }
		public string visibleIf { get; set; }
		public string section { get; set; }
		public List<Element> elements { get; set; }
	}

	public class CalculatedValues
	{
		public string name { get; set; }
		public string expression { get; set; }
		public bool? includeIntoResult { get; set; }
	}

	public class Element
	{
		public Page parent { get; set; }
		public string type { get; set; }
		public string inputType { get; set; }
		public string cellType { get; set; }
		public string title { get; set; }
		public string name { get; set; }
		public string valueName { get; set; }
		public bool isRequired { get; set; }
		public string requiredIf { get; set; }
		public ChoicesByUrl choicesByUrl { get; set; }
		public string renderMode { get; set; }
		public bool? allowMultiple { get; set; }
		public bool? allowAddPanel { get; set; }
		public bool? allowRemovePanel { get; set; }
		public bool? useDisplayValuesInTitle { get; set; }
		public List<Element> elements { get; set; }
		public List<Element> templateElements { get; set; }
		public List<Element> columns { get; set; }
		public int? maxLength { get; set; }
		public List<Choices> choices { get; set; }
		public string visibleIf { get; set; }

		//	This property being an Object is weird by the problem is 'rows' is being used in
		//	'matrix' type question as well as in 'comment'. Both as different data type.
		public List<Row> rows { get; set; }
		public string value { get; set; }
		public bool? hasOther { get; set; }
		public string defaultValue { get; set; }
	}

	public class Title
	{
		public string en { get; set; }
		public string fr { get; set; }
	}

	public class Choices
	{
		public string value { get; set; }
	}

	public class ChoicesByUrl
	{
		public string url { get; set; }
		public string titleName { get; set; }
		public string valueName { get; set; }
	}

	public class Row
	{
		public string value { get; set; }
	}
}
