using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyToCS
{
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
				case ElementTypes.CheckBox:
				case ElementTypes.TagBox:
				case ElementTypes.File:
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
				case ElementTypes.Boolean:
					return element.isRequired ? PropertyTypes.Boolean : PropertyTypes.NullableBoolean;
				case ElementTypes.CheckBox:
				case ElementTypes.TagBox:
					return PropertyTypes.String;
				case ElementTypes.DatePicker:
					return element.isRequired ? PropertyTypes.DateTime : PropertyTypes.NullableDateTime;
				case ElementTypes.File:
					return PropertyTypes.SurveyFile;
				case ElementTypes.Text:
					switch (element.inputType)
					{
						case ElementInputTypes.Date:
						case ElementInputTypes.DateTime:
						case ElementInputTypes.DateTimeLocal:
						case ElementInputTypes.Time:
							return element.isRequired ? PropertyTypes.DateTime : PropertyTypes.NullableDateTime;
						case ElementInputTypes.Number:
						case ElementInputTypes.Range:
							return element.isRequired ? PropertyTypes.Integer : PropertyTypes.NullableInteger;
						default:
							return PropertyTypes.String;
					}
				case ElementTypes.Comment:
				case ElementTypes.DropDown:
				case ElementTypes.RadioGroup:
					return PropertyTypes.String;
				default:
					throw new Exception($"Unable to get property type for: '{elementType}'");
			}
		}
	}
}
