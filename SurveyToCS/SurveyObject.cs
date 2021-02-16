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
		public Traduction title { get; set; }
		public string visibleIf { get; set; }

		/// <summary>
		/// This is exclusivly for PIA Tool survey for now
		/// </summary>
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

		public List<Element> elements { get; set; }

		public string type { get; set; }

		public string name { get; set; }

		public string valueName { get; set; }

		public Traduction title { get; set; }

		public string titleLocation { get; set; }

		public bool? hasTitle { get; set; }

		public Traduction description { get; set; }

		public string visibleIf { get; set; }

		public string inputType { get; set; }

		/// <summary>
		/// matrixdynamic property
		/// </summary>
		public string cellType { get; set; }

		public bool? isRequired { get; set; }

		public string requiredIf { get; set; }
		public ChoicesByUrl choicesByUrl { get; set; }

		#region paneldynamic properties

		public string renderMode { get; set; }
		public bool? allowAddPanel { get; set; }
		public bool? allowRemovePanel { get; set; }
		public Traduction addPanel { get; set; }
		public string templateTitle { get; set; }
		public List<Element> templateElements { get; set; }

		#endregion

		public bool? useDisplayValuesInTitle { get; set; }

		public List<Element> columns { get; set; }

		public int? maxLength { get; set; }

		public int? colCount { get; set; }

		//	This property being an Object is weird by the problem is 'rows' is being used in
		//	'matrix' type question as well as in 'comment'. Both as different data type.
		public object rows { get; set; }

		public string value { get; set; }

		public Traduction html { get; set; }

		public int? minRowCount { get; set; }

		public int? rowCount { get; set; }

		public Traduction addRowText { get; set; }

		public Traduction removeRowText { get; set; }

		public Traduction confirmDeleteText { get; set; }

		public bool? confirmDelete { get; set; }

		public bool? hasOther { get; set; }

		public bool? showHeader { get; set; }

		public Traduction panelAddText { get; set; }

		public List<Choices> choices { get; set; }

		public bool? hasComment { get; set; }

		public bool? allowMultiple { get; set; }

		public bool? waitForUpload { get; set; }

		public bool? storeDataAsText { get; set; }

		public bool? showMeter { get; set; }

		public bool? showPreview { get; set; }

		public int? maxSize { get; set; }

		public int? totalSize { get; set; }

		public string acceptedTypes { get; set; }
	}

	public class Traduction
	{
		public string en { get; set; }
		public string fr { get; set; }
	}

	public class Choices
	{
		public Traduction text { get; set; }

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
