using System.Collections.Generic;

namespace SurveyToCS
{
	public class ValidatorBuilder
	{
		private readonly List<ModelProperty> _modelProperties;

		public ValidatorBuilder(SurveyObject survey)
		{
			_modelProperties = new SurveyParser(survey).GetProperties();
		}

		public string GenerateValidator(string @namespace, string className)
		{
			return string.Empty;
		}

		public static string CreateValidators(SurveyObject survey, string @namespace, string className)
		{
			var builder = new ValidatorBuilder(survey);

			return builder.GenerateValidator(@namespace, className);
		}
	}
}
