using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyToCS
{
	public class ModelBuilder
	{
		private readonly List<ModelProperty> _modelProperties;

		public ModelBuilder(SurveyObject survey)
		{
			_modelProperties = new SurveyParser(survey).GetProperties();
		}

		private void GenerateClasses(StringBuilder builder, ModelProperty property)
		{
			builder.AppendLine($"public class {property.Name}");
			builder.AppendLine("{");

			foreach (var subProperty in property.Properties)
			{
				builder.AppendLine($"public {(subProperty.IsList ? $"List<{subProperty.Type}>" : subProperty.Type)} {subProperty.Name} {{ get; set; }}");
			}

			builder.AppendLine("}");
			builder.AppendLine();

			foreach (var subProperty in property.Properties.Where(p => p.IsComplexObject))
			{
				GenerateClasses(builder, subProperty);
			}
		}

		public string GenerateModel(string @namespace, string className)
		{
			var builder = new StringBuilder();

			// Add using statements
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine();

			// Add namespace
			builder.AppendLine($"namespace {@namespace}");
			builder.AppendLine("{");

			var rootClass = new ModelProperty
			{
				Name = className,
				Properties = _modelProperties
			};

			GenerateClasses(builder, rootClass);

			// End namespace
			builder.AppendLine("}");

			return builder.ToString();
		}

		public static string CreateModel(SurveyObject survey, string @namespace, string className)
		{
			var builder = new ModelBuilder(survey);

			return builder.GenerateModel(@namespace, className);
		}
	}
}
