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
			return _survey.pages.SelectMany(p => p.elements)
				.Where(e => !string.IsNullOrWhiteSpace(e.name)) // Ignore elements without names
				.GroupBy(e => e.GetNormalizedName())            // Group by element name
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
				.GroupBy(e => e.GetNormalizedName())            // Group by element name
				.Select(e => ParsePropertyElement(e.Key, e))
				.ToList();

			// Identify the main element by name, otherwise use the first instance
			var element = elements.SingleOrDefault(e => e.name == propertyName) ?? elements.First();
			property.Element = element;
			property.Type = property.IsComplexObject ? property.Name : GetPropertyType(element);
			property.IsList = property.IsComplexObject ? true : IsElementList(element);

			return property;
		}

		private static bool IsElementList(Element element)
		{
			var elementType = element.GetNormalizedType();
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
			var elementType = element.GetNormalizedType();
			switch (elementType)
			{
				case ElementTypes.Boolean:
					return element.IsOptional() ? PropertyTypes.NullableBoolean : PropertyTypes.Boolean;
				case ElementTypes.CheckBox:
				case ElementTypes.TagBox:
					return PropertyTypes.String;
				case ElementTypes.DatePicker:
					return element.IsOptional() ? PropertyTypes.NullableDateTime : PropertyTypes.DateTime;
				case ElementTypes.File:
					return PropertyTypes.SurveyFile;
				case ElementTypes.Text:
					switch (element.inputType)
					{
						case ElementInputTypes.Date:
						case ElementInputTypes.DateTime:
						case ElementInputTypes.DateTimeLocal:
						case ElementInputTypes.Time:
							return element.IsOptional() ? PropertyTypes.NullableDateTime : PropertyTypes.DateTime;
						case ElementInputTypes.Number:
						case ElementInputTypes.Range:
							return element.IsOptional() ?  PropertyTypes.NullableInteger : PropertyTypes.Integer;
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
