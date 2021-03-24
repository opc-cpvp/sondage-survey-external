using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyToCS
{
	public class ModelBuilder
	{
		private readonly SurveyObject _survey;
		private readonly List<ModelProperty> _modelProperties;

		public ModelBuilder(SurveyObject survey)
		{
			this._survey = survey;

			_modelProperties = this._survey.pages
				.SelectMany(p => p.elements)					// Select all elements on all the pages
				.Where(e => !string.IsNullOrWhiteSpace(e.name)) // Ignore elements without names
				.GroupBy(e => e.valueName ?? e.name)            // Group by element name
				.Aggregate(new List<ModelProperty>(), (list, propertyElement) =>
				{
					var modelProperty = this.ParsePropertyElement(propertyElement.Key, propertyElement);
					list.Add(modelProperty);
					return list;
				});
		}

		private ModelProperty ParsePropertyElement(string propertyName, IEnumerable<Element> elements)
		{
			var property = new ModelProperty { Name = propertyName };

			// get sub-properties (in the case of paneldynamic / matrix / matrixdynamic)
			var subElements = new List<Element>();
			subElements.AddRange(elements.SelectMany(e => e.columns ?? new List<Element>()));
			subElements.AddRange(elements.SelectMany(e => e.templateElements ?? new List<Element>()));

			var propertyElements = subElements
				.Where(e => !string.IsNullOrWhiteSpace(e.name)) // Ignore elements without names
				.GroupBy(e => e.valueName ?? e.name) // Group by valueName / name
				.ToDictionary(e => e.Key, e => e.ToList());

			property.Properties = propertyElements.Select(p => this.ParsePropertyElement(p.Key, p.Value)).ToList();

			if (!property.IsCollection)
			{
				property.Type = GetPropertyType(elements.Single());
			}

			return property;
		}

		private Type GetPropertyType(Element element)
		{
			switch (element.cellType ?? element.type)
			{
				case "boolean":
					return element.isRequired ? typeof(bool) : typeof(bool?);
				case "checkbox":
				case "tagbox":
					return typeof(List<string>);
				case "datepicker":
					return element.isRequired ? typeof(DateTime) : typeof(DateTime?);
				case "text":
					switch (element.inputType)
					{
						case "date":
						case "datetime":
						case "datetime-local":
						case "time":
							return element.isRequired ? typeof(DateTime) : typeof(DateTime?);
						case "number":
						case "range":
							return element.isRequired ? typeof(int) : typeof(int?);
						default:
							return typeof(string);
					}
				case "comment":
				case "dropdown":
				case "radiogroup":
					return typeof(string);
				default:
					return null;
			}
		}

		public string GenerateModel()
		{
			return string.Empty;
		}

		public static string CreateModel(SurveyObject survey, string @namespace, string className)
		{
			var builder = new ModelBuilder(survey);

			return builder.GenerateModel();
		}
	}

	internal class ModelProperty
	{
		public bool IsCollection => Properties.Any();
		public string Name { get; set; }
		public Type Type { get; set; }
		public List<ModelProperty> Properties { get; set; } = new List<ModelProperty>();

		public override string ToString()
		{
			return this.Name;
		}
	}
}
