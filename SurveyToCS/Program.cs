using Microsoft.CSharp;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace SurveyToCS
{
    class Program
    {
        private static readonly List<string> _simpleTypeElements = new List<string>() { "text", "comment", "dropdown", "radiogroup", "boolean", "panel", "file", "checkbox", "matrixdynamic" };
        private static readonly List<string> _dynamicElements = new List<string>() { "matrixdynamic", "paneldynamic" };

        static void Main(string[] args)
        {
           string json = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";

            ConvertFromFile(json, "ComplaintFormCore.Models", "SurveyPIAToolModel");
           // string line = Console.ReadLine();
            //CreateValidators(json);
        }

        private static void ConvertFromFile(string jsonFile, string _namespace, string className)
        {
            // read file into a string and deserialize JSON to a type
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFile));

            StringBuilder csharp = new StringBuilder();
           // csharp.AppendLine("using System.ComponentModel.DataAnnotations;");
            csharp.AppendLine("using System.Collections.Generic;");
            csharp.AppendLine("using System;");
            csharp.AppendLine();

            csharp.Append("namespace ");
            csharp.AppendLine(_namespace);
            csharp.AppendLine("{");

            csharp.Append("public class ");
            csharp.Append(className);
            //csharp.AppendLine(" : IValidatableObject");
            csharp.AppendLine("{");

            foreach (var page in survey.pages)
            {
                BuildProperty(csharp, page.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), page);
            }

            //csharp.AppendLine("public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)");
            //csharp.AppendLine("{");
            //csharp.AppendLine("throw new NotImplementedException();");
            //csharp.AppendLine("}");

            csharp.Append("}"); // end class

            List<Element> survey_dynamic_elements = new List<Element>();

            foreach (var page in survey.pages)
            {
                survey_dynamic_elements.AddRange(page.elements.Where(e => _dynamicElements.Contains(e.type)).ToList());
            }

            if(survey_dynamic_elements.Count > 0)
            {
                csharp.AppendLine();
                BuildDynamicProperties(csharp, survey_dynamic_elements);
            }

            csharp.AppendLine("}");// end namespace

            //CreateClassFile(className, csharp.ToString());

            Console.WriteLine(csharp.ToString());
            Console.ReadLine();
        }

        private static void BuildProperty(StringBuilder csharp, List<Element> elements, Page pageObj)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    BuildProperty(csharp, element.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), pageObj);
                }
                else
                {
                    BuildElementSummary(csharp, element, pageObj);

                    #region Attributes
                    //if (element.isRequired)
                    //{
                    //    if(string.IsNullOrWhiteSpace(element.visibleIf))
                    //    {
                    //        csharp.AppendLine("[Required]");
                    //    }
                    //    else
                    //    {
                    //        //[RequiredIf(nameof(IsNewprogram), false, "This field is required")]
                    //        csharp.AppendLine("[RequiredIf(\"nameof(property)\", \"value to valid against\", \"error message\")] // TODO: VERIFY THIS");
                    //    }
                    //}

                    //if ((element.type == "checkbox" || element.type == "radiogroup" || element.type == "dropdown") && element.choices != null)
                    //{
                    //    //[IsInList(new string[] { "receive_email", "no_email", "conduct_pia" }, "Invalid value. Possible values: receive_email, no_email, conduct_pia")]

                    //    csharp.Append("[IsInList(new string[] {");

                    //    for(int i=0; i<element.choices.Count;i++)
                    //    {
                    //        var choice = element.choices[i];

                    //        csharp.Append("\"");
                    //        csharp.Append(choice.value);
                    //        csharp.Append("\"");

                    //        if (i < element.choices.Count)
                    //        {
                    //            csharp.Append(",");
                    //        }
                    //    }

                    //    csharp.Append("}, \"Invalid value. Possible choices: ");
                    //    csharp.AppendJoin(", ", element.choices.Select(c => c.value));
                    //    csharp.AppendLine("\")]");
                    //}

                    //if (element.maxLength != null)
                    //{
                    //    csharp.Append("[StringLength(");
                    //    csharp.Append(((int)element.maxLength).ToString());
                    //    csharp.AppendLine(")]");
                    //}
                    //else
                    //{
                    //    //  Adding default length values
                    //    if (element.type == "comment")
                    //    {
                    //        csharp.AppendLine("[StringLength(5000)]");
                    //    }
                    //    else if (element.type == "text")
                    //    {
                    //        csharp.AppendLine("[StringLength(100)]");
                    //    }
                    //}
                    #endregion

                    csharp.Append("public ");

                    if (element.type == "boolean")
                    {
                        if (element.isRequired && string.IsNullOrWhiteSpace(element.visibleIf))
                        {
                            //  If required AND there is no condition to make that field visible
                            csharp.Append(" bool ");
                        }
                        else
                        {
                            csharp.Append(" bool? ");
                        }

                        csharp.Append(CapitalizeFirstLetter(element.name));
                    }
                    else if (element.type == "matrixdynamic")
                    {
                        csharp.Append("List<");

                        if (!string.IsNullOrWhiteSpace(element.name))
                        {
                            csharp.Append(CapitalizeFirstLetter(element.name));
                            csharp.Append("> ");
                            csharp.Append(CapitalizeFirstLetter(element.name));
                        }
                        else if (!string.IsNullOrWhiteSpace(element.valueName))
                        {
                            csharp.Append(CapitalizeFirstLetter(element.valueName));
                            csharp.Append("> ");
                            csharp.Append(CapitalizeFirstLetter(element.valueName));
                        }
                    }
                    else if (element.type == "checkbox" || element.type == "file")
                    {
                        csharp.Append("List<string> ");
                        csharp.Append(CapitalizeFirstLetter(element.name));
                    }
                    else
                    {
                        csharp.Append(" string ");
                        csharp.Append(CapitalizeFirstLetter(element.name));
                    }

                    csharp.AppendLine(" { get; set; }");
                    csharp.AppendLine();
                }
            }
        }

        private static void BuildDynamicProperties(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName into newGroup
                                     orderby newGroup.Key
                                     select newGroup;

            foreach (var dynamicItem in groupedByValueName)
            {
                csharp.Append("public class ");
                csharp.Append(dynamicItem.Key);
                csharp.AppendLine("{");

                foreach(var item in dynamicItem)
                {
                    if (item.type == "matrixdynamic")
                    {
                        foreach(var column in item.columns)
                        {
                            BuildElementSummary(csharp, column);

                            csharp.Append("public ");

                            if (column.cellType == "boolean")
                            {
                                if (column.isRequired)
                                {
                                    csharp.Append(" bool ");
                                }
                                else
                                {
                                    csharp.Append(" bool? ");
                                }
                            }
                            else
                            {
                                csharp.Append(" string ");
                            }

                            csharp.Append(CapitalizeFirstLetter(column.name));

                            csharp.AppendLine(" { get; set; }");
                            csharp.AppendLine();
                        }
                    }
                    else if (item.type == "paneldynamic")
                    {
                        foreach (var templateItem in item.templateElements)
                        {
                            BuildElementSummary(csharp, templateItem);

                            csharp.Append("public ");

                            if (templateItem.type == "boolean")
                            {
                                if (templateItem.isRequired)
                                {
                                    csharp.Append(" bool ");
                                }
                                else
                                {
                                    csharp.Append(" bool? ");
                                }
                            }
                            else
                            {
                                csharp.Append(" string ");
                            }

                            csharp.Append(CapitalizeFirstLetter(templateItem.valueName));

                            csharp.AppendLine(" { get; set; }");
                            csharp.AppendLine();
                        }
                    }
                }

                csharp.Append("}");
            }
        }

        private static void BuildElementSummary(StringBuilder csharp, Element element, Page page = null)
        {
            csharp.AppendLine("/// <summary>");

            if(page != null)
            {
                csharp.Append("/// Page: ");
                csharp.Append(page.name);
                csharp.AppendLine("<br/>");
            }

            csharp.Append("/// Question ");
            csharp.AppendLine("<br/>");

            if (element.title != null)
            {
                csharp.Append("/// ");

                if (element.title.en.Length > 75)
                {
                    csharp.Append(element.title.en.Substring(0, 75));
                    csharp.Append("...");
                }
                else
                {
                    csharp.Append(element.title.en);
                }

                csharp.AppendLine("<br/>");
            }

            if ((element.type == "checkbox" || element.type == "radiogroup" || element.type == "dropdown") && element.choices != null)
            {
                csharp.Append("/// ");
                csharp.Append("Possible choices: [");

                csharp.AppendJoin(", ", element.choices.Select(c => c.value));

                csharp.Append("]");
                csharp.AppendLine("<br/>");
            }

            if (element.isRequired && !string.IsNullOrWhiteSpace(element.visibleIf))
            {
                csharp.Append("/// Required condition: ");
                csharp.AppendLine(element.visibleIf);
            }

            csharp.AppendLine("/// </summary>");
        }

        private static string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        private static void CreateClassFile(string className, string text)
        {
            var folderName = "GeneratedClasses";
            var pathToSave = Path.Combine(folderName, className + ".cs");


            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            File.WriteAllText(pathToSave, text);

            //using (var stream = new FileStream(pathToSave, FileMode.Create))
            //{
            //    file.CopyTo(stream);
            //}

            //if (File.Exists(pathToSave))
            //{
            //    File.Delete(pathToSave);
            //}

            //File.Create(pathToSave);

            //using (TextWriter tw = new StreamWriter(pathToSave))
            //{
            //    tw.WriteLine(text);
            //}
        }

        //private static void Compile(string code)
        //{
        //    var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v3.5" } });
        //    var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, "foo.exe", true);
        //    parameters.GenerateExecutable = true;
        //    CompilerResults results = csc.CompileAssemblyFromSource(parameters, code);
        //    results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
        //}

        private static void CreateValidators(string json)
        {
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(json));
            StringBuilder csharp = new StringBuilder();

            foreach (var page in survey.pages)
            {
                BuildValidator(csharp, page.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), page);
            }

            Console.WriteLine(csharp.ToString());
            Console.ReadLine();
        }

        private static void BuildValidator(StringBuilder csharp, List<Element> elements, Page parentPage, Element parentPanel = null)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    BuildValidator(csharp, element.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), parentPage, element);
                }
                else
                {
                    string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;

                    csharp.Append("// ");
                    csharp.AppendLine(elementName);

                    if (element.isRequired)
                    {
                        csharp.Append("RuleFor(x => x.");
                        csharp.Append(elementName);
                        csharp.Append(").NotEmpty()");

                        string visibleIf = string.Empty;

                        if (!string.IsNullOrWhiteSpace(parentPage.visibleIf))
                        {
                            visibleIf += parentPage.visibleIf;
                        }

                        if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.visibleIf))
                        {
                            if (!string.IsNullOrWhiteSpace(visibleIf))
                            {
                                visibleIf += " && ";
                            }

                            visibleIf += parentPanel.visibleIf;
                        }

                        if (!string.IsNullOrWhiteSpace(element.visibleIf))
                        {
                            //  There is a condition
                            if (!string.IsNullOrWhiteSpace(visibleIf))
                            {
                                visibleIf += " && ";
                            }

                            visibleIf += element.visibleIf;
                        }

                        if (!string.IsNullOrWhiteSpace(visibleIf))
                        {
                            csharp.Append(".When(_ => true/* TODO: ");
                            csharp.Append(visibleIf.Replace("contains", "==")); ;
                            csharp.Append("*/)");
                        }

                        csharp.Append(".WithMessage(");
                        csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsRequired\")); ");
                    }

                    if (element.type == "comment" || element.type == "text")
                    {
                        int maxLength = 5000;

                        if(element.type == "text")
                        {
                            maxLength = 100;
                        }

                        if(element.maxLength != null)
                        {
                            maxLength = (int)element.maxLength;
                        }

                        csharp.Append("RuleFor(x => x.");
                        csharp.Append(elementName);
                        csharp.Append(").Length(0,");
                        csharp.Append(maxLength);
                        csharp.Append(")");

                        csharp.Append(".WithMessage(");
                        csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsOverCharacterLimit\")); ");
                    }

                    //  Check if selected option is in the list of valid options
                    if ((element.type == "checkbox" || element.type == "radiogroup" || element.type == "dropdown") && element.choices != null && element.choices.Count > 0)
                    {
                        //RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));

                        csharp.Append("RuleFor(x => x.");
                        csharp.Append(elementName);

                        csharp.Append(").Must(x => new List<string> { ");

                        for (int i = 0; i < element.choices.Count; i++)
                        {
                            var choice = element.choices[i];

                            csharp.Append("\"");
                            csharp.Append(choice.value);
                            csharp.Append("\"");

                            if (i < element.choices.Count)
                            {
                                csharp.Append(",");
                            }
                        }

                        csharp.Append("}.Contains(x))");

                        csharp.Append(".WithMessage(");
                        csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"SelectedValueNotValid\")); ");
                    }

                    if(element.inputType == "email")
                    {
                        csharp.Append("RuleFor(x => x.");
                        csharp.Append(elementName);
                        csharp.AppendLine(").EmailAddress();");
                    }

                    csharp.AppendLine();
                }
            }
        }
    }
}
