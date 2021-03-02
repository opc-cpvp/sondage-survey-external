using Newtonsoft.Json;
using System;
using System.IO;
using TextCopy;

namespace SurveyToCS
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Clear();
			Console.WriteLine("Choose an option:");
			Console.WriteLine("1) Type 'c' to generate the C# class with properties");
			Console.WriteLine("2) Type 'v' to generate the FluentValidation class");
			Console.WriteLine("3) Type 't' to generate test data");
			Console.WriteLine("4) Type 'g' to generate test data");
			Console.WriteLine("5) Type 'r' to rebuild to json with translated strings");
			Console.Write("\r\nSelect an option: ");

			//	TODO:	When using make sure string json & string className are set properly
			//			Maybe this can be user input.

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pbr.json";
			//string className = "SurveyPBRModel";

			//string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\sondage-survey-internal\ComplaintFormCore\wwwroot\sample-data\survey_rrosh.json";
			//string className = "SurveyRROSHModel";

			string pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";
			string className = "SurveyPIAToolModel";

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
				string result = ClassPropertiesBuilder.CreateClassObject(survey, classNamespace, className);
				ClipboardService.SetText(result);
				Console.WriteLine(result);
			}
			else if (line == "v")
			{
				Console.WriteLine("Do you want to generate the visibleIf conditions as commented TODOs? Type y/n:");
				string line_validation = Console.ReadLine();
				bool commentedOut = line_validation == "y";

				string result = FluentValidatorBuilder.CreateValidators(survey, classNamespace, className, commentedOut);
				ClipboardService.SetText(result);
				Console.WriteLine(result);
			}
			else if (line == "t")
			{
				string result = TestDataBuilder.CreateTestData(survey);
				ClipboardService.SetText(result);
				Console.WriteLine(result);
			}
			else if (line == "g")
			{
				string result = ExportForTranslation.CreateCSV(survey);
				ClipboardService.SetText(result);
				Console.WriteLine(result);
			}
			else if (line == "r")
			{
				string result = ExportForTranslation.ReBuildJSON("", pathToJSONFile);
				ClipboardService.SetText(result);
				//Console.WriteLine(result);
			}
		}
	}
}
