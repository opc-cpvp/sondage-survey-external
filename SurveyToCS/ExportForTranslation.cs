using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SurveyToCS
{
	public class ExportForTranslation
	{
		public static string CreateCSV(SurveyObject survey)
		{
			string csvStr = string.Empty;

			foreach (var page in survey.pages)
			{

			}

			return csvStr;
		}

		public static string ReBuildJSON(string csvStr, string pathToJSONFile)
		{
			string json = string.Empty;

			// pathToJSONFile = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pbr.json";
			SurveyObject survey;

			//string jsonString = File.ReadAllText(pathToJSONFile);

			// Convert the JSON string to a JObject:
			//JObject jObject = JsonConvert.DeserializeObject(jsonString) as JObject;

			// Select a nested property using a single string:

			//JToken jToken = jObject.SelectToken("Pages").Where(p > pathToJSONFile.).SelectToken("title");
			//JToken jToken = jObject.SelectToken("Pages[?(@.name == 'page_breach_description_affected')].title[0].fr");

			// Update the value of the property:
			//jToken.Replace("myNewPassword123");

			// Convert the JObject back to a string:
			//string updatedJsonString = jObject.ToString();
			//	File.WriteAllText(pathToJSONFile, updatedJsonString);

			try
			{
				// read file into a string and deserialize JSON to a type
				survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(pathToJSONFile));
			}
			catch
			{
				Console.WriteLine("Cannot find the file " + pathToJSONFile);
				return "";
			}

			var test = survey.pages.Where(p => p.name == "page_step_2_1_q_2_1_6").First().elements.Where(e => e.name == "pnd_OtherInstitutionHead").First();

			if(test.type == "paneldynamic")
			{
				foreach(var item in test.templateElements)
				{
					item.title.fr = "xyz";
				}
			}
			else if(test.type == "matrixdynamic")
			{

			}

			//test.fr = "salut ca va";

			//string output = JsonConvert.SerializeObject(survey, Formatting.Indented);
			//string finaloutput = JToken.FromObject(survey).ToString();
			//string prettyJson = JToken.Parse(output).ToString(Formatting.Indented);
			//string jsonFormatted = jkho.Parse(output).ToString(Formatting.Indented);

			var options = new System.Text.Json.JsonSerializerOptions
			{
				IgnoreNullValues = true,
				WriteIndented = true,
				Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			};

			string jsonString = System.Text.Json.JsonSerializer.Serialize(survey, options);
			return jsonString;
		}
	}
}
