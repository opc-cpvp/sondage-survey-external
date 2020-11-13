using Microsoft.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private static readonly List<string> _simpleTypeElements = new List<string>() { "text", "comment", "dropdown", "tagbox", "radiogroup", "boolean", "panel", "file", "checkbox" };
        private static readonly List<string> _dynamicElements = new List<string>() { "matrixdynamic", "paneldynamic" };

        private static List<Element> _surveyAllElements;

        static void Main(string[] args)
        {
            //string json = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";
            string json = @"C:\Users\jbrouillette\source\repos\online-complaint-form-pa_jf\ComplaintFormCore\wwwroot\sample-data\survey_pia_e_tool.json";
            string className = "SurveyPIAToolModel";

            string line = Console.ReadLine();
            if (line == "c")
            {
                CreateClassObject(json, "ComplaintFormCore.Models", className);
            }
            else if (line == "v")
            {
                CreateValidators(json, new List<string>() { "ComplaintFormCore.Resources", "ComplaintFormCore.Web_Apis.Models" }, "ComplaintFormCore.Models", className);
            }
            else if (line == "t")
            {
                CreateTestData(json);
            }
        }

        #region Properties

        private static void CreateClassObject(string jsonFile, string _namespace, string className)
        {
            // read file into a string and deserialize JSON to a type
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFile));

            foreach (var page in survey.pages)
            {
                AddParentPage(page, page.elements);
            }

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
                BuildProperty(csharp, page.elements.Where(x => _simpleTypeElements.Contains(x.type)).ToList(), page);
            }

            List<Element> survey_dynamic_elements = new List<Element>();

            foreach (var page in survey.pages)
            {
                GetDynamicElements(survey_dynamic_elements, page.elements);
            }

            if (survey_dynamic_elements.Count > 0)
            {
                csharp.AppendLine();

                var groupedByValueName = from item in survey_dynamic_elements
                                         group item by item.valueName != null ? item.valueName : item.name into newGroup
                                         orderby newGroup.Key
                                         select newGroup;

                foreach (var dynamicItem in groupedByValueName)
                {
                    //StringBuilder summary = new StringBuilder();

                    //summary.AppendLine("/// <summary>");

                    //foreach (var item in dynamicItem)
                    //{
                    //    string elementName = !string.IsNullOrWhiteSpace(item.valueName) ? item.valueName : item.name;

                    //    summary.Append("/// Property use for: ");
                    //    summary.AppendLine(elementName);
                    //}

                    //summary.AppendLine("/// </summary>");

                    BuildDynamicProperty(csharp, dynamicItem.Key);
                }
            }

            csharp.Append("}"); // end class

            if (survey_dynamic_elements.Count > 0)
            {
                csharp.AppendLine();
                BuildDynamicPropertyClasses(csharp, survey_dynamic_elements);
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
                    BuildProperty(csharp, element.elements.Where(x => _simpleTypeElements.Contains(x.type)).ToList(), pageObj);
                }
                else
                {
                    string elementName = CapitalizeFirstLetter(!string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name);

                    BuildElementSummary(csharp, element, pageObj);

                    csharp.Append("public ");

                    //  NOTE: We always set the booleans, int or datetime to nullable, otherwise the default value is set on the property
                    //  and our validation doesn't work

                    if (element.type == "boolean")
                    {
                        csharp.Append(" bool? ");
                        csharp.Append(elementName);
                    }
                    else if (element.type == "matrixdynamic" || element.type == "paneldynamic")
                    {
                        csharp.Append("List<");
                        csharp.Append(elementName);
                        csharp.Append("> ");
                        csharp.Append(elementName);
                    }
                    else if (element.type == "checkbox" || element.type == "tagbox")
                    {
                        csharp.Append("List<string> ");
                        csharp.Append(elementName);
                    }
                    else if (element.type == "file")
                    {
                        csharp.Append("List<SurveyFile> ");
                        csharp.Append(elementName);
                    }
                    else if (element.type == "text" && element.inputType == "date")
                    {
                        csharp.Append(" DateTime? ");
                        csharp.Append(elementName);
                    }
                    else if (element.type == "text" && element.inputType == "number")
                    {
                        csharp.Append(" int? ");
                        csharp.Append(elementName);
                    }
                    else
                    {
                        csharp.Append(" string ");
                        csharp.Append(elementName);
                    }

                    csharp.AppendLine(" { get; set; }");
                    csharp.AppendLine();
                }
            }
        }

        private static void BuildDynamicProperty(StringBuilder csharp, string elementName)
        {
            csharp.Append("public ");

            csharp.Append("List<");
            csharp.Append(CapitalizeFirstLetter(elementName));
            csharp.Append("> ");
            csharp.Append(CapitalizeFirstLetter(elementName));

            csharp.AppendLine(" { get; set; }");
            csharp.AppendLine();
        }

        private static void BuildDynamicPropertyClasses(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName != null ? item.valueName : item.name into newGroup
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

                            //  NOTE: We always set the booleans, int or datetime to nullable, otherwise the default value is set on the property
                            //  and our validation doesn't work

                            if (column.cellType == "boolean")
                            {
                                csharp.Append(" bool? ");
                            }
                            else if (column.inputType == "date")
                            {
                                csharp.Append(" DateTime? ");
                            }
                            else if (column.inputType == "number")
                            {
                                csharp.Append(" int? ");
                            }
                            else if (column.cellType == "file")
                            {
                                csharp.Append("List<SurveyFile> ");
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

                            //  NOTE: We always set the booleans, int or datetime to nullable, otherwise the default value is set on the property
                            //  and our validation doesn't work

                            if (templateItem.type == "boolean")
                            {
                                csharp.Append(" bool? ");
                            }
                            else if (templateItem.inputType == "date")
                            {
                                csharp.Append(" DateTime? ");
                            }
                            else if (templateItem.inputType == "number")
                            {
                                csharp.Append(" int? ");
                            }
                            else if (templateItem.type == "file")
                            {
                                csharp.Append("List<SurveyFile> ");
                            }
                            else
                            {
                                csharp.Append(" string ");
                            }

                            var name = !string.IsNullOrWhiteSpace(templateItem.valueName) ? templateItem.valueName : templateItem.name;
                            csharp.Append(CapitalizeFirstLetter(name));

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

            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

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

            //csharp.Append("/// Question ");
            //csharp.AppendLine("<br/>");

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

            if (type == "checkbox" || type == "radiogroup" || type == "dropdown")
            {
                csharp.Append("/// ");
                csharp.Append("Possible choices: [");

                if (element.choices != null)
                {
                    csharp.AppendJoin(", ", element.choices.Select(c => c.value));
                }
                else if (element.choicesByUrl != null)
                {
                    csharp.Append(element.choicesByUrl.url);
                }

                csharp.Append("]");
                csharp.AppendLine("<br/>");
            }

            if (element.isRequired && !string.IsNullOrWhiteSpace(element.visibleIf))
            {
                csharp.Append("/// Required condition: ");
                csharp.Append(element.visibleIf);
                csharp.AppendLine("<br/>");
            }

            csharp.Append("/// Survey question type: ");
            csharp.Append(type);

            if (!string.IsNullOrEmpty(element.inputType))
            {
                csharp.Append(" (");
                csharp.Append(element.inputType);
                csharp.Append(")");
            }

            csharp.AppendLine();

            csharp.AppendLine("/// </summary>");
        }

        #endregion

        #region Validators
        private static void CreateValidators(string jsonFileLocation, List<string> usings, string _namespace, string className)
        {
            SurveyObject survey = JsonConvert.DeserializeObject<SurveyObject>(File.ReadAllText(jsonFileLocation));

            foreach (var page in survey.pages)
            {
                AddParentPage(page, page.elements);
            }

            _surveyAllElements = new List<Element>();
            foreach (var page in survey.pages)
            {
                AddSurveyElement(_surveyAllElements, page, page.elements);
            }

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
            csharp.AppendLine("using System.Linq;");

            foreach (string item in usings)
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

            if (type == "comment")
            {
                BuildMaxLengthValidator(csharp, element, visibleIf);
            }

            if (type == "text" && element.inputType != "date" && element.inputType != "number")
            {
                BuildMaxLengthValidator(csharp, element, visibleIf);
            }

            //  Check if selected option is in the list of valid options
            if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
            {
                BuildListValidator(csharp, element, visibleIf);
            }

            if (type == "text" && element.inputType == "email")
            {
                csharp.Append("RuleFor(x => x.");
                csharp.Append(elementName);
                csharp.AppendLine(").EmailAddress()");

                if (!string.IsNullOrWhiteSpace(visibleIf))
                {
                    csharp.Append(".When( ");
                    csharp.Append(visibleIf); ;
                    csharp.Append(")");
                }

                csharp.AppendLine(";");
            }

            csharp.AppendLine();
        }

        private static bool ElementHasRules(Element element)
        {
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            if (element.isRequired)
            {
                return true;
            }

            if (type == "comment")
            {
                return true;
            }

            if (type == "text" && element.inputType != "date" && element.inputType != "number")
            {
                return true;
            }

            if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
            {
                return true;
            }

            if (type == "text" && element.inputType == "email")
            {
                return true;
            }

            return false;
        }

        private static void BuildDynamicElementValidator(StringBuilder csharp, Element element, string childParameter, string page_name)
        {
            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            csharp.Append("// ");
            csharp.Append(elementName);
            csharp.Append(" (Page: ");
            csharp.Append(page_name);
            csharp.Append(")");
            csharp.AppendLine();

            string visibleIf = GetVisibleIf(element, null, null);

            if (element.isRequired)
            {
                csharp.Append(childParameter);
                csharp.Append(".");
                BuildRequiredValidator(csharp, elementName, visibleIf);
            }

            if (type == "comment")
            {
                csharp.Append(childParameter);
                csharp.Append(".");
                BuildMaxLengthValidator(csharp, element, visibleIf);
            }

            if (type == "text" && element.inputType != "date" && element.inputType != "number")
            {
                csharp.Append(childParameter);
                csharp.Append(".");
                BuildMaxLengthValidator(csharp, element, visibleIf);
            }

            //  Check if selected option is in the list of valid options
            if ((type == "checkbox" || type == "radiogroup" || type == "dropdown" || type == "tagbox") && element.choices != null && element.choices.Count > 0)
            {
                csharp.Append(childParameter);
                csharp.Append(".");
                BuildListValidator(csharp, element, visibleIf);
            }

            if (type == "text" && element.inputType == "email")
            {
                csharp.Append(childParameter);
                csharp.Append(".");
                csharp.Append("RuleFor(x => x.");
                csharp.Append(elementName);
                csharp.AppendLine(").EmailAddress()");

                if (!string.IsNullOrWhiteSpace(visibleIf))
                {
                    csharp.Append(".When( ");
                    csharp.Append(visibleIf); ;
                    csharp.Append(")");
                }

                csharp.AppendLine(";");
            }
        }

        private static void SetDynamicValidators(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName != null ? item.valueName : item.name into newGroup
                                     orderby newGroup.Key
                                     select newGroup;

            foreach (var dynamicItem in groupedByValueName)
            {
                csharp.Append("// ");
                csharp.AppendLine(dynamicItem.Key);

                foreach (var element in dynamicItem)
                {
                    //RuleForEach(x => x.PersonalInformationCategory).ChildRules(child => {
                    //    child.RuleFor(x => x.Category).NotEmpty().WithMessage(_localizer.GetLocalizedStringSharedResource("FieldIsRequired"));
                    //});

                    string visibleIf = GetVisibleIf(element, element.parent, null);

                    if (element.type == "matrixdynamic")
                    {
                        foreach (var column in element.columns)
                        {
                            if (!ElementHasRules(column))
                            {
                                continue;
                            }

                            csharp.Append("RuleForEach(x => x.");
                            csharp.Append(dynamicItem.Key);
                            csharp.AppendLine(").ChildRules(child => {");

                            BuildDynamicElementValidator(csharp, column, "child", element.parent.name);

                            csharp.Append("})");

                            if (!string.IsNullOrWhiteSpace(visibleIf))
                            {
                                csharp.Append(".When( ");
                                csharp.Append(visibleIf); ;
                                csharp.Append(")");
                            }

                            csharp.AppendLine(";");
                            csharp.AppendLine();
                        }
                    }
                    else if (element.type == "paneldynamic")
                    {
                        foreach (var templateItem in element.templateElements)
                        {
                            if (!ElementHasRules(templateItem))
                            {
                                continue;
                            }

                            csharp.Append("RuleForEach(x => x.");
                            csharp.Append(dynamicItem.Key);
                            csharp.AppendLine(").ChildRules(child => {");

                            BuildDynamicElementValidator(csharp, templateItem, "child", element.parent.name);

                            csharp.Append("})");

                            if (!string.IsNullOrWhiteSpace(visibleIf))
                            {
                                csharp.Append(".When( ");
                                csharp.Append(visibleIf); ;
                                csharp.Append(")");
                            }

                            csharp.AppendLine(";");
                            csharp.AppendLine();
                        }
                    }
                }

               // csharp.AppendLine();
            }
        }

        private static string GetVisibleIfCondition(Element element, Page parentPage, Element parentPanel = null)
        {
            string visibleIf = string.Empty;

            string anyof_pattern = @"{[a-zA-Z_]+}\sanyof\s\[[a-zA-Z,'_]+\]";
            string anyof_pattern_items = @"[[a-zA-Z,'_]+\]";
            string property_pattern = @"({[a-zA-Z_]+})";

            if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf))
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

            if (string.IsNullOrWhiteSpace(visibleIf) == false)
            {
                MatchCollection anyofs = Regex.Matches(visibleIf, anyof_pattern);

                if (anyofs.Count > 0)
                {
                    string wholeVisibleIf = string.Empty;
                    foreach (Match match_anyof in Regex.Matches(visibleIf, anyof_pattern))
                    {
                        //  {InstitutionAgreedRequestOnInformalBasis} anyof ['yes','not_sure']
                        // x.NatureOfComplaint.Intersect(new List<string> { "NatureOfComplaintDelay", "NatureOfComplaintExtensionOfTime", "NatureOfComplaintDenialOfAccess", "NatureOfComplaintLanguage" }).Any()
                        Regex rgx_property = new Regex(property_pattern);
                        Match matchProperty = rgx_property.Match(match_anyof.Value);

                        //  Proerty {property} becomes propery
                        string matchPropertyClean = matchProperty.Value.Replace("{", "").Replace("}", "");

                        //  Find the conditional property
                        Element conditonalProperty = _surveyAllElements.Where(x => x.name == matchPropertyClean).First();
                        string replacement = string.Empty;

                        if (conditonalProperty.type == "tagbox" || conditonalProperty.type == "checkbox")
                        {
                            //  tagbox & checkbox can have multiple choices so they are List<string> properties
                            replacement = "x." + matchPropertyClean + ".Intersect";
                        }
                        else
                        {
                            //  This applies to type that has only one value selectable such as dropdown or radiogroup
                            //  Very important to leave a space between the { property } otherwise the {} will be overwritten below
                            replacement = "new List<string>() { x." + matchPropertyClean + " }.Intersect";
                        }

                        Regex rgx_items = new Regex(anyof_pattern_items);
                        Match matchArray = rgx_items.Match(match_anyof.Value);

                        replacement += "(new List<string>() {" + matchArray.Value.Replace("'", "\"").Replace("[", "").Replace("]", "");

                        replacement += "}).Any()";

                        visibleIf = visibleIf.Replace(match_anyof.Value, replacement);
                    }
                }

                foreach (Match match_propery in Regex.Matches(visibleIf, property_pattern))
                {
                    visibleIf = visibleIf.Replace(match_propery.Value, match_propery.Value.Replace("{", "x.").Replace("}", ""));
                }

                return visibleIf.Replace("=", "==").Replace("contains", "==").Replace("'", "\"").SafeReplace("or", "||", true).SafeReplace("and", "&&", true);
            }

            return string.Empty;
        }

                foreach (Match match_propery in Regex.Matches(visibleIf, property_pattern))
                {
                    visibleIf = visibleIf.Replace(match_propery.Value, match_propery.Value.Replace("{", "x.").Replace("}", ""));
                }

                return visibleIf.Replace("=", "==").Replace("contains", "==").Replace("'", "\"").SafeReplace("or", "||", true).SafeReplace("and", "&&", true);
            }

            return string.Empty;
        }

        private static string GetVisibleIfFullCondition(Element element, Page parentPage, Element parentPanel = null)
        {
            string visibleIf = GetVisibleIfCondition(element, parentPage, parentPanel);

            if (string.IsNullOrWhiteSpace(visibleIf) == false)
            {
                return "x => true/* TODO: " + visibleIf + "*/";
            }

            return string.Empty;

        }
        private static string GetVisibleIf(Element element, Page parentPage, Element parentPanel = null)
        {
            string visibleIfFullCondition = GetVisibleIfFullCondition(element, parentPage, parentPanel);

            //string visibleIf = string.Empty;

            //if (parentPage != null && !string.IsNullOrWhiteSpace(parentPage.visibleIf) && parentPage.visibleIf.Count(f => f == '{') == 1
            //    && (parentPage.visibleIf.Contains("contains") || parentPage.visibleIf.Contains("=")))
            //{
            //    visibleIf += "x => x." + parentPage.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            //}

            //if (parentPanel != null && !string.IsNullOrWhiteSpace(parentPanel.visibleIf) && parentPanel.visibleIf.Count(f => f == '{') == 1
            //    && (parentPanel.visibleIf.Contains("contains") || parentPanel.visibleIf.Contains("=")))
            //{
            //    if (string.IsNullOrWhiteSpace(visibleIf))
            //    {
            //        visibleIf += "x => x.";
            //    }
            //    else
            //    {
            //        visibleIf += " && x.";
            //    }

            //    visibleIf += parentPanel.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            //}

            //if (!string.IsNullOrWhiteSpace(element.visibleIf) && element.visibleIf.Count(f => f == '{') == 1
            //    && (element.visibleIf.Contains("contains") || element.visibleIf.Contains("=")))
            //{
            //    if (string.IsNullOrWhiteSpace(visibleIf))
            //    {
            //        visibleIf += "x => x.";
            //    }
            //    else
            //    {
            //        visibleIf += " && x.";
            //    }

            //    visibleIf += element.visibleIf.Replace("{", "").Replace("}", "").Replace("=", "==").Replace("contains", "==").Replace("'", "\"");
            //}

            //if(string.IsNullOrWhiteSpace(visibleIf) && string.IsNullOrWhiteSpace(visibleIfFullCondition) == false)
            //{
            return visibleIfFullCondition;
            //}

            //return visibleIf;
        }

        private static void BuildRequiredValidator(StringBuilder csharp, string elementName, string visibleIf = null)
        {
            csharp.Append("RuleFor(x => x.");
            csharp.Append(elementName);
            csharp.Append(").NotEmpty()");

            if (!string.IsNullOrWhiteSpace(visibleIf))
            {
                csharp.Append(".When( ");
                csharp.Append(visibleIf); ;
                csharp.Append(")");
            }

            csharp.Append(".WithMessage(");
            csharp.AppendLine("_localizer.GetLocalizedStringSharedResource(\"FieldIsRequired\")); ");
        }

        private static void BuildMaxLengthValidator(StringBuilder csharp, Element element, string visibleIf = null)
        {
            int maxLength = 5000;
            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            if (type == "text")
            {
                maxLength = 200;
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

        private static void BuildListValidator(StringBuilder csharp, Element element, string visibleIf = null)
        {
            //  Check if selected option is in the list of valid options

            //RuleFor(x => x.ContactATIPQ18).Must(x => new List<string> { "receive_email", "no_email", "conduct_pia" }.Contains(x)).WithMessage(_localizer.GetLocalizedStringSharedResource("ItemNotValid"));

            string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;

            if (type == "checkbox" || type == "tagbox")
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

            foreach (var page in survey.pages)
            {
                AddParentPage(page, page.elements);
            }

            StringBuilder csharp = new StringBuilder();

            foreach (var page in survey.pages)
            {
                BuildPropertyData(csharp, page.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), page);
            }

            List<Element> survey_dynamic_elements = new List<Element>();

            foreach (var page in survey.pages)
            {
                GetDynamicElements(survey_dynamic_elements, page.elements);
            }

            if (survey_dynamic_elements.Count > 0)
            {
                BuildDynamicPropertyData(csharp, survey_dynamic_elements);
            }

            Console.WriteLine(csharp.ToString());
            Console.ReadLine();
        }

        private static void BuildPropertyData(StringBuilder csharp, List<Element> elements, Page pageObj)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    BuildPropertyData(csharp, element.elements.Where(e => _simpleTypeElements.Contains(e.type)).ToList(), pageObj);
                }
                else
                {
                    string elementName = !string.IsNullOrWhiteSpace(element.valueName) ? element.valueName : element.name;

                    csharp.Append(CapitalizeFirstLetter(elementName));
                    csharp.Append(":");
                    csharp.Append(GetPropertyValue(element));
                    csharp.AppendLine(",");
                }
            }
        }

        private static void BuildDynamicPropertyData(StringBuilder csharp, List<Element> dynamicElements)
        {
            var groupedByValueName = from item in dynamicElements
                                     group item by item.valueName != null ? item.valueName : item.name into newGroup
                                     orderby newGroup.Key
                                     select newGroup;

            foreach (var dynamicItem in groupedByValueName)
            {
                csharp.Append(dynamicItem.Key);
                csharp.AppendLine(": [");

                csharp.Append("{");
                foreach (var element in dynamicItem)
                {
                    if (element.type == "matrixdynamic")
                    {
                        foreach (var column in element.columns)
                        {
                            csharp.AppendLine();
                            csharp.Append(column.name);
                            csharp.Append(":");
                            csharp.Append(GetPropertyValue(column));
                            csharp.Append(",");
                        }
                    }
                    else if (element.type == "paneldynamic")
                    {
                        foreach (var column in element.templateElements)
                        {
                            csharp.AppendLine();
                            csharp.Append(column.valueName);
                            csharp.Append(":");
                            csharp.Append(GetPropertyValue(column));
                            csharp.Append(",");
                        }
                    }
                }

                csharp.AppendLine("},");
                csharp.AppendLine("],");
            }
        }

        private static string GetPropertyValue(Element element)
        {
            string type = !string.IsNullOrWhiteSpace(element.type) ? element.type : element.cellType;
            StringBuilder retVal = new StringBuilder();

            if (type == "boolean")
            {
                retVal.Append("true");
            }
            else if (type == "radiogroup" || type == "dropdown")
            {
                retVal.Append("\"");

                if (element.choices != null && element.choices.Count > 0)
                {
                    var rand = new Random();
                    retVal.Append(element.choices[rand.Next(0, element.choices.Count - 1)].value);
                }

                retVal.Append("\"");
            }
            else if (type == "checkbox" || type == "tagbox")
            {
                retVal.Append("[\"");

                if (element.choices != null && element.choices.Count > 0)
                {
                    var rand = new Random();
                    retVal.Append(element.choices[rand.Next(0, element.choices.Count - 1)].value);
                }

                retVal.Append("\"]");
            }
            else if (type == "file")
            {
                retVal.Append("[]");
            }
            else if (type == "text")
            {
                retVal.Append("\"");

                if (element.inputType == "tel")
                {
                    var rand = new Random();
                    retVal.Append("+1613-");
                    retVal.Append(rand.Next(200, 700).ToString());
                    retVal.Append("-");
                    retVal.Append(rand.Next(1000, 9999).ToString());
                }
                else if (element.inputType == "date")
                {
                    retVal.Append(DateTime.Now.ToString("yyyy-MM-dd"));
                }
                else if (element.inputType == "email")
                {
                    retVal.Append(RandomText(10) + "@gmail.com");
                }
                else if (element.inputType == "number")
                {
                    var rand = new Random();
                    retVal.Append(rand.Next(200, 700).ToString());
                }
                else if (element.inputType == "url")
                {
                    var rand = new Random();
                    retVal.Append("http://" + RandomText(10) + ".com");
                }
                else
                {
                    retVal.Append(RandomText(25));
                }

                retVal.Append("\"");
            }
            else if (type == "comment")
            {
                retVal.Append("\"");
                retVal.Append(RandomText(250));
                retVal.Append("\"");
            }

            return retVal.ToString();
        }

        #endregion

        private static void AddParentPage(Page pageObj, List<Element> elements)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    AddParentPage(pageObj, element.elements);
                }
                else
                {
                    element.parent = pageObj;
                }
            }
        }

        private static void AddSurveyElement(List<Element> all_elements, Page pageObj, List<Element> elements)
        {
            foreach (var element in elements)
            {
                if (element.type == "panel")
                {
                    AddSurveyElement(all_elements, pageObj, element.elements);
                }
                else
                {
                    all_elements.Add(element);
                }
            }
        }

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

        private static string RandomText(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }

            return str_build.ToString();
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
            var test = Regex.Replace(input, textToFind, replace);
            return Regex.Replace(input, textToFind, replace);
        }
    }
}