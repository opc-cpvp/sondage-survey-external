using System.Collections.Generic;
using System.Text;

namespace SurveyToCS
{
	internal class Condition
	{
		public ModelProperty Property { get; set; }
		public string Constant { get; set; }
		public string Operator { get; set; }
	}

	internal class ValidatorRule
	{
		// Sjs - isRequired
		public bool Required { get; set; } = false;

		// FV - Must
		public List<string> AcceptedValues { get; set; } = new List<string>();

		// Sjs - visibleIf
		public List<Condition> Conditions { get; set; } = new List<Condition>();

		// FV - ChildRules
		public List<ValidatorRule> ValidatorRules { get; set; } = new List<ValidatorRule>();
	}

	public class ValidatorBuilder
	{
		private readonly Dictionary<Page, List<ModelProperty>> _pageProperties;

		public ValidatorBuilder(SurveyObject survey)
		{
			_pageProperties = new SurveyParser(survey).GetPropertiesByPage();
		}

		private void GenerateRules(StringBuilder builder)
		{
		}

		public string GenerateValidator(string @namespace, string className)
		{
			var builder = new StringBuilder();

			// Add using statements
			builder.AppendLine("using FluentValidation;");
			builder.AppendLine("using System;");
			builder.AppendLine("using System.Collections.Generic;");
			builder.AppendLine("using System.Linq;");
			builder.AppendLine();

			// Add namespace
			builder.AppendLine($"namespace {@namespace}");
			builder.AppendLine("{");

			// Add class
			builder.AppendLine($"public partial class {className}Validator : AbstractValidator<{className}>");
			builder.AppendLine("{");

			// Add constructor
			builder.AppendLine($"public {className}Validator()");
			builder.AppendLine("{");

			// Add options
			builder.AppendLine("ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;");

			GenerateRules(builder);

			// End constructor
			builder.AppendLine("}");

			// End class
			builder.AppendLine("}");

			// End namespace
			builder.AppendLine("}");

			return builder.ToString();
		}

		public static string CreateValidators(SurveyObject survey, string @namespace, string className)
		{
			var builder = new ValidatorBuilder(survey);

			return builder.GenerateValidator(@namespace, className);
		}
	}
}
