using System.Collections.Generic;

namespace SurveyToCS
{
	public class ValidatorBuilder
	{
		private readonly Dictionary<Page, List<ModelProperty>> _pageProperties;

		public ValidatorBuilder(SurveyObject survey)
		{
			_pageProperties = new SurveyParser(survey).GetPropertiesByPage();
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
