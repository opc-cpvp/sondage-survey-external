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

			string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\sondage-survey-internal\ComplaintFormCore\wwwroot\sample-data\survey_rrosh.json";
			string className = "SurveyRROSHModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";
			//string className = "SurveyPIAToolModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pa_complaint.json";
			//string className = "SurveyPAModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pipeda_complaint.json";
			//string className = "SurveyPipedaModel";

			string classNamespace = "ComplaintFormCore.Models";
			SurveyObject survey;

			try
			{
				// read file into a string and deserialize JSON to a type
				survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(pathToJSONFile));
			}
			catch
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
}
