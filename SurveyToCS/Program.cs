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
using System.Text.RegularExpressions;

namespace SurveyToCS
{
    class Program
    {
        private static readonly List<string> _simpleTypeElements = new List<string>() { "text", "comment", "dropdown", "radiogroup", "boolean", "panel", "file", "checkbox" };
        private static readonly List<string> _dynamicElements = new List<string>() { "matrixdynamic", "paneldynamic" };

        static void Main(string[] args)
        {
            string json = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";

            //CreateClassObject(json, "ComplaintFormCore.Models", "SurveyPIAToolModel");
            // string line = Console.ReadLine();
             CreateValidators(json, new List<string>() { "ComplaintFormCore.Resources", "ComplaintFormCore.Web_Apis.Models"}, "ComplaintFormCore.Models", "SurveyPIAToolModel");
        }

        #region Properties

        private static void CreateClassObject(string jsonFile, string _namespace, string className)
        {
            // read file into a string and deserialize JSON to a type
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFile));

            StringBuilder csharp = new StringBuilder();
            csharp.AppendLine("using System.Collections.Generic;");
            csharp.AppendLine("using System;");
            csharp.AppendLine();

            csharp.Append("namespace ");
            csharp.AppendLine(_namespace);
            csharp.AppendLine("{");

            csharp.Append("public class ");
            csharp.Append(className);
            csharp.AppendLine("{");

            foreach (var page in survey.pages)
            {
                BuildProperty(csharp, page.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), page);
            }

            csharp.Append("}"); // end class

            List<Element> survey_dynamic_elements = new List<Element>();

            foreach (var page in survey.pages)
            {
                GetDynamicElements(survey_dynamic_elements, page.elements);
            }

            if (survey_dynamic_elements.Count > 0)
            {
                csharp.AppendLine();
                BuildDynamicProperties(csharp, survey_dynamic_elements);
            }

            csharp.AppendLine("}");// end namespace

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

                foreach (var item in dynamicItem)
                {
                    if (item.type == "matrixdynamic")
                    {
                        foreach (var column in item.columns)
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

                csharp.AppendLine("}");
            }
        }

        private static void BuildElementSummary(StringBuilder csharp, Element element, Page page = null)
        {
            csharp.AppendLine("/// <summary>");

            if (page != null)
            {
                csharp.Append("/// Page: ");
                csharp.Append(page.name);
                csharp.AppendLine("<br/>");

                if (!string.IsNullOrWhiteSpace(page.section))
                {
                    csharp.Append("/// Section: ");
                    csharp.Append(page.section);
                    csharp.AppendLine("<br/>");
                }
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

        #endregion

        #region Validators
        private static void CreateValidators(string jsonFileLocation, List<string> usings, string _namespace, string className)
        {
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFileLocation));

            //  Get a list of dynamic properties, from the matrixdynamic and the paneldynamic.
            //  Then if there is such items, we create the classes validators
            List<Element> survey_dynamic_elements = new List<Element>();
            foreach (var page in survey.pages)
            {
                GetDynamicElements(survey_dynamic_elements, page.elements);
            }

            StringBuilder csharp = new StringBuilder();
            csharp.AppendLine("using System.Collections.Generic;");
            csharp.AppendLine("using FluentValidation;");

            foreach(string item in usings)
            {
                csharp.Append("using ");
                csharp.Append(item);
                csharp.AppendLine(";");
            }

            csharp.AppendLine();

            csharp.Append("namespace ");
            csharp.AppendLine(_namespace);
            csharp.AppendLine("{");

            //  STEP 1: Create the class(s) implementing AbstractValidator

            //  Create the main class validator
            csharp.Append("public partial class ");
            csharp.Append(className);
            csharp.Append("Validator: AbstractValidator<");
            csharp.Append(className);
            csharp.Append(">");
            csharp.AppendLine("{");

            //  constructor
            csharp.Append("public ");
            csharp.Append(className);
            csharp.Append("Validator(SharedLocalizer _localizer)");
            csharp.AppendLine("{");

            csharp.AppendLine("ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;");
            csharp.AppendLine();

            foreach (var page in survey.pages)
            {
                //  We don't want to deal with the non-html elements just now
                BuildPageValidator(csharp, page.elements.Where(e => e.type != "html").ToList(), page);
            }

            if (survey_dynamic_elements.Count > 0)
            {
                SetDynamicValidators(csharp, survey_dynamic_elements);
            }

            csharp.AppendLine("}"); // end constructor
            csharp.AppendLine("}"); // end main class

            if (survey_dynamic_elements.Count > 0)
            {
                BuildDynamicValidators(csharp, survey_dynamic_elements);
            }

            csharp.AppendLine("}");// end namespace

            Console.WriteLine(csharp.ToString());
            Console.ReadLine();
        }

        private static void BuildPageValidator(StringBuilder csharp, List<Element> elements, Page parentPage, Element parentPanel = null)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    BuildPageValidator(csharp, element.elements.Where(e => e.type != "html").ToList(), parentPage, element);
                }
                else
                {
                    BuildElementValidator(csharp, element, parentPage, parentPanel);
                }
            }
        }

        private static void BuildElementValidator(StringBuilder csharp, Element element, Page parentPage = null, Element parentPanel = null)
        {
            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            csharp.Append("// ");
            csharp.Append(elementName);

            if (parentPage != null)
            {
                csharp.Append(" (Page: ");
                csharp.Append(parentPage.name);
                csharp.Append(")");
            }

            csharp.AppendLine();

            string visibleIf = GetVisibleIf(element, parentPage, parentPanel);

            if (element.isRequired)
            {
                BuildRequiredValidator(csharp, elementName, visibleIf);
            }

            if (type == "comment" || type == "text")
            {
                BuildMaxLengthValidator(csharp, element, visibleIf);
            }

            //  Check if selected option is in the list of valid options
            if ((type == "checkbox" || type == "radiogroup" || type == "dropdown") && element.choices != null && element.choices.Count > 0)
            {
                BuildListValidator(csharp, element, visibleIf);
            }

            if (element.inputType == "email")
            {
                csharp.Append("RuleFor(x => x.");
                csharp.Append(elementName);
                csharp.AppendLine(").EmailAddress();");
            }

            csharp.AppendLine();
        }

        private static void SetDynamicValidators(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName into newGroup
                                     orderby newGroup.Key
                                     select newGroup;

            foreach (var dynamicItem in groupedByValueName)
            {
                //RuleForEach(x => x.PersonalInformationCategory).SetValidator(new PersonalInformationCategoryValidator(_localizer));
                csharp.Append("RuleForEach(x => x.");
                csharp.Append(dynamicItem.Key);
                csharp.Append(").SetValidator(new ");
                csharp.Append(dynamicItem.Key);
                csharp.AppendLine("Validator(_localizer));");
            }
        }

        private static void BuildDynamicValidators(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName into newGroup
                                     orderby newGroup.Key
                                     select newGroup;

            foreach (var dynamicItem in groupedByValueName)
            {
                csharp.Append("public partial class ");
                csharp.Append(dynamicItem.Key);
                csharp.Append("Validator: AbstractValidator<");
                csharp.Append(dynamicItem.Key);
                csharp.Append(">");
                csharp.AppendLine("{");

                //  constructor
                csharp.Append("public ");
                csharp.Append(dynamicItem.Key);
                csharp.Append("Validator(SharedLocalizer _localizer)");
                csharp.AppendLine("{");

                csharp.AppendLine("ValidatorOptions.Global.CascadeMode = CascadeMode.Continue;");
                csharp.AppendLine();

                foreach (var element in dynamicItem)
                {
                    if (element.type == "matrixdynamic")
                    {
                        foreach (var column in element.columns)
                        {
                            BuildElementValidator(csharp, column);
                        }
                    }
                    else if (element.type == "paneldynamic")
                    {
                        foreach (var templateItem in element.templateElements)
                        {
                            BuildElementValidator(csharp, templateItem);
                        }
                    }
                }

                csharp.AppendLine("}"); // end constructor
                csharp.AppendLine("}"); // end class
            }
        }

        private static string GetVisibleIfFullCondition(Element element, Page parentPage, Element parentPanel = null)
        {
            string visibleIf = string.Empty;

            if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf))
            {
                visibleIf += parentPage.visibleIf;
            }

            if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.visibleIf))
            {
                if (!string.IsNullOrWhiteSpace(visibleIf))
                {
                    visibleIf += " and ";
                }

                visibleIf += parentPanel.visibleIf;
            }

            if (!string.IsNullOrWhiteSpace(element.visibleIf))
            {
                //  There is a condition
                if (!string.IsNullOrWhiteSpace(visibleIf))
                {
                    visibleIf += " and ";
                }

                visibleIf += element.visibleIf;
            }

            if(string.IsNullOrWhiteSpace(visibleIf) == false)
            {
                return "_ => true/* TODO: " + visibleIf + "*/";
            }

            return string.Empty;

        }
        private static string GetVisibleIf(Element element, Page parentPage, Element parentPanel = null)
        {
            string visibleIfFullCondition = GetVisibleIfFullCondition(element, parentPage, parentPanel);

            string visibleIf = string.Empty;

            if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf) && parentPage.visibleIf.Count(f => f == '{') == 1
                && (parentPage.visibleIf.Contains("contains") || parentPage.visibleIf.Contains("=")))
            {
                visibleIf += "x => x." + parentPage.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            }

            if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.visibleIf) && parentPanel.visibleIf.Count(f => f == '{') == 1
                && (parentPanel.visibleIf.Contains("contains") || parentPanel.visibleIf.Contains("=")))
            {
                if (string.IsNullOrWhiteSpace(visibleIf))
                {
                    visibleIf += "x => x.";
                }
                else
                {
                    visibleIf += " && x.";
                }

                visibleIf += parentPanel.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            }

            if (!string.IsNullOrWhiteSpace(element.visibleIf) && element.visibleIf.Count(f => f == '{') == 1
                && (element.visibleIf.Contains("contains") || element.visibleIf.Contains("=")))
            {
                if (string.IsNullOrWhiteSpace(visibleIf))
                {
                    visibleIf += "x => x.";
                }
                else
                {
                    visibleIf += " && x.";
                }

                visibleIf += element.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            }

            if(string.IsNullOrWhiteSpace(visibleIf) && string.IsNullOrWhiteSpace(visibleIfFullCondition) == false)
            {
                return visibleIfFullCondition;
            }

            return visibleIf;
        }

        private static void BuildRequiredValidator(StringBuilder csharp, string elementName, string visibleIf)
        {
            csharp.Append("RuleFor(x => x.");
            csharp.Append(elementName);
            csharp.Append(").NotEmpty()");

            if (!string.IsNullOrWhiteSpace(visibleIf))
            {
                ////{ProvinceIncidence} anyof [2,6,9] and [{ComplaintAboutHandlingInformationOutsideProvince}] anyof ['no', 'not_sure'] and {IsAgainstFwub} contains 'no' and {DidOrganizationDirectComplaintToOpc} contains 'no'"


                csharp.Append(".When( ");
                csharp.Append(visibleIf); ;
                csharp.Append(")");
            }

            csharp.Append(".WithMessage(");
            csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsRequired\")); ");
        }

        private static void BuildMaxLengthValidator(StringBuilder csharp, Element element, string visibleIf)
        {
            int maxLength = 5000;
            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            if (type == "text")
            {
                maxLength = 100;
            }

            if (element.maxLength != null)
            {
                maxLength = (int)element.maxLength;
            }

            csharp.Append("RuleFor(x => x.");
            csharp.Append(elementName);
            csharp.Append(").Length(0,");
            csharp.Append(maxLength);
            csharp.Append(")");

            if (!string.IsNullOrWhiteSpace(visibleIf))
            {
                csharp.Append(".When( ");
                csharp.Append(visibleIf); ;
                csharp.Append(")");
            }

            csharp.Append(".WithMessage(");
            csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsOverCharacterLimit\")); ");
        }

        private static void BuildListValidator(StringBuilder csharp, Element element, string visibleIf)
        {
            //  Check if selected option is in the list of valid options

            //RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));

            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            if (type == "checkbox")
            {
                csharp.Append("RuleForEach(x => x.");
            }
            else
            {
                csharp.Append("RuleFor(x => x.");
            }

            csharp.Append(elementName);

            csharp.Append(").Must(x => new List<string> { ");

            for (int i = 0; i < element.choices.Count; i++)
            {
                var choice = element.choices[i];

                csharp.Append("\"");
                csharp.Append(choice.value);
                csharp.Append("\"");

                if (i < element.choices.Count - 1)
                {
                    csharp.Append(",");
                }
            }

            csharp.Append("}.Contains(x))");

            if (!string.IsNullOrWhiteSpace(visibleIf))
            {
                csharp.Append(".When( ");
                csharp.Append(visibleIf); ;
                csharp.Append(")");
            }

            csharp.Append(".WithMessage(");
            csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"SelectedValueNotValid\")); ");
        }

        #endregion

        #region Test Data

        private static void CreateTestData(string jsonFile)
        {
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFile));

            StringBuilder csharp = new StringBuilder();

            foreach (var page in survey.pages)
            {
                BuildPropertyData(csharp, page.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), page);
            }
        }

        private static string BuildPropertyData(StringBuilder csharp, List<Element> elements, Page pageObj)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    BuildPropertyData(csharp, element.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), pageObj);
                }
                else
                {
                    csharp.Append("\"");
                    csharp.Append(CapitalizeFirstLetter(element.name));
                    csharp.Append("\":");

                    if (element.type == "boolean")
                    {
                        csharp.Append("true");
                    }
                    else if (element.type == "matrixdynamic")
                    {
                        //csharp.Append("List<");

                        //if (!string.IsNullOrWhiteSpace(element.name))
                        //{
                        //    csharp.Append(CapitalizeFirstLetter(element.name));
                        //    csharp.Append("> ");
                        //    csharp.Append(CapitalizeFirstLetter(element.name));
                        //}
                        //else if (!string.IsNullOrWhiteSpace(element.valueName))
                        //{
                        //    csharp.Append(CapitalizeFirstLetter(element.valueName));
                        //    csharp.Append("> ");
                        //    csharp.Append(CapitalizeFirstLetter(element.valueName));
                        //}
                    }
                    else if (element.type == "checkbox" || element.type == "file")
                    {
                        //csharp.Append("List<string> ");
                        //csharp.Append(CapitalizeFirstLetter(element.name));
                    }
                    else
                    {
                        //csharp.Append(" string ");
                        csharp.Append("\"");
                        csharp.Append("some test data");
                        csharp.Append("\"");
                    }

                    csharp.AppendLine();
                }
            }

            return "";
        }
        #endregion

        private static void GetDynamicElements(List<Element> survey_dynamic_elements, List<Element> page_or_panel_elements)
        {
            foreach (var element in page_or_panel_elements)
            {
                if (element.type == "panel")
                {
                    GetDynamicElements(survey_dynamic_elements, element.elements);
                }
                else
                {
                    if (_dynamicElements.Contains(element.type))
                    {
                        survey_dynamic_elements.Add(element);
                    }
                }
            }
        }

        private static string CapitalizeFirstLetter(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
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
}
