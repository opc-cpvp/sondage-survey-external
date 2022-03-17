using System;
using System.Collections.Generic;
using System.Linq;

namespace SurveyToCS
{
	public class SurveyParser
	{
		public static readonly string[] IgnoredElementTypes = { ElementTypes.Html, ElementTypes.Panel };

		private SurveyObject _survey;
		public SurveyParser(SurveyObject survey)
		{
			_survey = survey;
		}

		public List<ModelProperty> GetProperties()
		{
			return _survey.pages
				.Aggregate(new List<Element>(), (list, p) =>
				{
					// Get page elements
					list.AddRange(p.elements);

					// Get panel elements
					var panelElements = p.elements
						.Where(e => e.type == ElementTypes.Panel)
						.SelectMany(p => p.elements);

					list.AddRange(panelElements);
					return list;
				})
				.Where(e => !string.IsNullOrWhiteSpace(e.name))    // Ignore elements without names
				.Where(e => !IgnoredElementTypes.Contains(e.type)) // Ignore elements by type
				.GroupBy(e => e.GetNormalizedName())               // Group by element name
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
					var properties = e.type switch
					{
						ElementTypes.Matrix => e.rows?.Select(r => new Element { name = r.value, type = e.GetNormalizedType() }),
						ElementTypes.MatrixDropDown => e.rows?.Select(r => new Element { name = r.value, type = ElementTypes.MatrixRow, columns = e.columns }),
						ElementTypes.MatrixDynamic => e.columns,
						ElementTypes.MatrixRow => e.columns,
						ElementTypes.PanelDynamic => e.templateElements,
						_ => Enumerable.Empty<Element>()
					};

					list.AddRange(properties);

					// Get panel elements
					var panelElements = properties
						.Where(e => e.type == ElementTypes.Panel)
						.SelectMany(p => p.elements);

					list.AddRange(panelElements);

					return list;
				})
				.Where(e => !string.IsNullOrWhiteSpace(e.name))    // Ignore elements without names
				.Where(e => !IgnoredElementTypes.Contains(e.type)) // Ignore elements by type
				.GroupBy(e => e.GetNormalizedName())               // Group by element name
				.Select(e => ParsePropertyElement(e.Key, e))
				.ToList();

			// Identify the main element by name, otherwise use the first instance
			var element = elements.SingleOrDefault(e => e.name == propertyName) ?? elements.First();
			property.Element = element;
			property.Type = property.IsComplexObject ? property.Name : GetPropertyType(element);
			property.IsList = IsElementList(element);

			return property;
		}

		private static bool IsElementList(Element element)
		{
			var elementType = element.GetNormalizedType();
			switch (elementType)
			{
				case ElementTypes.CheckBox:
				case ElementTypes.File:
				case ElementTypes.MatrixDynamic:
				case ElementTypes.PanelDynamic:
				case ElementTypes.TagBox:
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
					return PropertyTypes.NullableBoolean;
				case ElementTypes.CheckBox:
				case ElementTypes.TagBox:
					return PropertyTypes.String;
				case ElementTypes.DatePicker:
					return PropertyTypes.NullableDateTime;
				case ElementTypes.File:
					return PropertyTypes.SurveyFile;
				case ElementTypes.Text:
					switch (element.inputType)
					{
						case ElementInputTypes.Date:
						case ElementInputTypes.DateTime:
						case ElementInputTypes.DateTimeLocal:
						case ElementInputTypes.Time:
							return PropertyTypes.NullableDateTime;
						case ElementInputTypes.Number:
						case ElementInputTypes.Range:
							return PropertyTypes.NullableInteger;
						default:
							return PropertyTypes.String;
					}
				case ElementTypes.Comment:
				case ElementTypes.DropDown:
				case ElementTypes.Matrix:
				case ElementTypes.RadioGroup:
					return PropertyTypes.String;
				default:
					throw new Exception($"Unable to get property type for: '{elementType}'");
			}
		}
	}
}
