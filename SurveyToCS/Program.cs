using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SurveyToCS
{
	class Program
	{
		static void Main(string[] args)
		{
			//	TODO: When using make sure string json & string className are set properly

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pbr.json";
			//string className = "SurveyPBRModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\sondage-survey-internal\ComplaintFormCore\wwwroot\sample-data\survey_rrosh.json";
			//string className = "SurveyRROSHModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";
			//string className = "SurveyPIAToolModel";

			string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pa_complaint.json";
			string className = "SurveyPAModel";

			string classNamespace = "ComplaintFormCore.Models";
			SurveyObject survey;

			try
			{
				// read file into a string and deserialize JSON to a type
				survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(pathToJSONFile));
			}
			catch(Exception ex)
			{
				Console.WriteLine("Cannot find the file " + pathToJSONFile);
				return;
			}

			foreach (var page in survey.pages)
			{
				Common.AddParentPage(page, page.elements);
			}

			string line = Console.ReadLine();
			if (line == "c")
			{
				ClassPropertiesBuilder.CreateClassObject(survey, classNamespace, className);
			}
			else if (line == "v")
			{
				FluentValidatorBuilder.CreateValidators(survey, classNamespace, className);
			}
			else if (line == "t")
			{
				TestDataBuilder.CreateTestData(survey);
			}
		}
	}

	public class VisibleIfCondition
	{
		public string Property { get; set; }
		public string Operator { get; set; }

		public string Value { get; set; }

		public VisibleIfCondition(string visibleIf)
		{
			//x => x.HasLegalAuthority

			//  {HasLegalAuthority}  = false
			//  {ContactATIPQ16} contains 'receive_email'
			//  {SingleOrMultiInstitutionPIA} anyof ['single','single_related']
			//  {IsInformationPhysicalFormat} = true and {IsInformationPhysicalConvertedCopy} = true
			//  {ProvinceIncidence} anyof [2,6,9] and {ComplaintAboutHandlingInformationOutsideProvince} anyof ['no', 'not_sure'] and {IsAgainstFwub} contains 'no' and {DidOrganizationDirectComplaintToOpc} contains 'no'"

		}
	}

	public static class StringExtensions
	{
		public static string SafeReplace(this string input, string find, string replace, bool matchWholeWord)
		{
			string textToFind = matchWholeWord ? string.Format(@"\b{0}\b", find) : find;
			//var test = Regex.Replace(input, textToFind, replace);
			return Regex.Replace(input, textToFind, replace);
		}
	}
}
