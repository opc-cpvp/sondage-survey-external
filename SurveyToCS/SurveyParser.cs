using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyToCS
{
	internal static class PropertyTypes
	{
		public static readonly string Boolean = "bool";
		public static readonly string DateTime = "DateTime";
		public static readonly string Integer = "int";
		public static readonly string String = "string";
		public static readonly string SurveyFile = "SurveyFile";
		public static readonly string NullableBoolean = "bool?";
		public static readonly string NullableDateTime = "DateTime?";
		public static readonly string NullableInteger = "int?";
	}

	public class SurveyParser
	{
		private SurveyObject _survey;
		public SurveyParser(SurveyObject survey)
		{
			_survey = survey;
		}

		public List<ModelProperty> GetProperties()
		{
			return _survey.pages
				.SelectMany(page => page.elements)              // Select all elements on all the pages
				.Where(e => !string.IsNullOrWhiteSpace(e.name)) // Ignore elements without names
				.GroupBy(e => e.valueName ?? e.name)            // Group by element name
				.Aggregate(new List<ModelProperty>(), (list, e) =>
				{
					var modelProperty = ParsePropertyElement(e.Key, e);
					list.Add(modelProperty);
					return list;
				});
		}

		private ModelProperty ParsePropertyElement(string propertyName, IEnumerable<Element> elements)
		{
			var property = new ModelProperty { Name = propertyName };

			property.Properties = elements
				.Aggregate(new List<Element>(), (list, e) =>
				{
					list.AddRange(e.columns ?? Enumerable.Empty<Element>());
					list.AddRange(e.templateElements ?? Enumerable.Empty<Element>());
					return list;
				})
				.Where(e => !string.IsNullOrWhiteSpace(e.name)) // Ignore elements without names
				.GroupBy(e => e.valueName ?? e.name)            // Group by element name
				.Select(e => ParsePropertyElement(e.Key, e))
				.ToList();

			if (property.IsComplexObject)
			{
				property.Type = property.Name;
				property.IsList = true;
			}
			else
			{
				var element = elements.Single();
				property.Element = element;
				property.IsList = IsElementList(element);
				property.Type = GetPropertyType(element);
			}

			return property;
		}



		private static bool IsElementList(Element element)
		{
			var elementType = element.cellType ?? element.type;
			switch (elementType)
			{
				case "checkbox":
				case "tagbox":
				case "file":
					return true;
				default:
					return false;
			}
		}

		private static string GetPropertyType(Element element)
		{
			var elementType = element.cellType ?? element.type;
			switch (elementType)
			{
				case "boolean":
					return element.isRequired ? PropertyTypes.Boolean : PropertyTypes.NullableBoolean;
				case "checkbox":
				case "tagbox":
					return PropertyTypes.String;
				case "datepicker":
					return element.isRequired ? PropertyTypes.DateTime : PropertyTypes.NullableDateTime;
				case "file":
					return PropertyTypes.SurveyFile;
				case "text":
					switch (element.inputType)
					{
						case "date":
						case "datetime":
						case "datetime-local":
						case "time":
							return element.isRequired ? PropertyTypes.DateTime : PropertyTypes.NullableDateTime;
						case "number":
						case "range":
							return element.isRequired ? PropertyTypes.Integer : PropertyTypes.NullableInteger;
						default:
							return PropertyTypes.String;
					}
				case "comment":
				case "dropdown":
				case "radiogroup":
					return PropertyTypes.String;
				default:
					throw new Exception($"Unable to get property type for: '{elementType}'");
			}
		}
	}
}
