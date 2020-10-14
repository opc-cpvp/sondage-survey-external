import * as showdown from "showdown";

import * as SurveyHelper from "./surveyHelper";
import * as Survey from "survey-vue";
import { ProblemDetails } from "./models/problemDetails";
import * as SurveyNavigation from "./surveyNavigation";

export function initSurvey(): void {
    Survey.JsonObject.metaData.addProperty("survey", {
        name: "complaintId:string",
        default: ""
    });

    //  Add a new property for each item choices (to the native text, value). This is used for checkboxes with addtional Html info
    //  but could be used for radiobutton as well.
    Survey.JsonObject.metaData.addProperty("itemvalue", {
        name: "htmlAdditionalInfo:text"
    });

    // Register the function for use in SurveyJS expressions. This function validates that at least one selection was made in a <select>
    Survey.FunctionFactory.Instance.register("HasSelectedItem", SurveyHelper.HasSelectedItem);

    // This is how we replace string from Survey.js (englishStrings or frenchSurveyStrings) for localization.
    Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
    Survey.surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";

    Survey.surveyLocalization.locales["en"].otherItemText = "Other";
    Survey.surveyLocalization.locales["fr"].otherItemText = "Autre";

    Survey.StylesManager.Enabled = false;

    // This is a survey property that will hold the information as to if the user has reached the 'Preview'
    // page at least once. The idea is if the user has reached the 'Preview' page he can always go back to it after
    // editing a page. This will be usefull for very long survey after a user decided to edit an item from the preview page.
    Survey.JsonObject.metaData.addProperty("survey", {
        name: "passedPreviewPage:boolean",
        default: false
    });

    // This is to hide page and panel we don't want to show on preview.
    // Pages or Panels that contains exclusively information html for example.
    // The reason why it is working for is because on preview, the pages become panels
    Survey.JsonObject.metaData.addProperty("panel", {
        name: "hideOnPreview:boolean",
        default: false
    });

    Survey.JsonObject.metaData.addProperty("page", {
        name: "hideOnPreview:boolean",
        default: false
    });

    Survey.JsonObject.metaData.addProperty("panel", {
        name: "hideOnPDF:boolean",
        default: false
    });

    Survey.JsonObject.metaData.addProperty("page", {
        name: "hideOnPDF:boolean",
        default: false
    });
}

export function initSurveyModelProperties(survey: Survey.SurveyModel): void {
    const myCss = {
        navigationButton: "btn btn-primary",
        html: "sq-html"
    };

    survey.css = myCss;

    // survey.showPreviewBeforeComplete = 'showAnsweredQuestions';
    survey.showPreviewBeforeComplete = "showAllQuestions";

    // onHidden -> survey clears the question value when the question becomes invisible.
    // If a question has an answer value and it was invisible initially, a survey clears the value on completing.
    survey.clearInvisibleValues = "onHidden";

    survey.questionErrorLocation = "top";

    // https://surveyjs.io/Documentation/Library?id=surveymodel#checkErrorsMode
    // check errors on every question value (i.e., answer) changing.
    survey.checkErrorsMode = "onValueChanged";

    survey.showProgressBar = "bottom";
    survey.goNextPageAutomatic = false;
    survey.showQuestionNumbers = "off";
    survey.showNavigationButtons = false;
    survey.showCompletedPage = true;

    //  This is only working in code, not working directly in the json
    if (survey.locale === "fr") {
        survey.requiredText = "(obligatoire)";
    } else {
        survey.requiredText = "(required)";
    }
}

export function initSurveyModelEvents(survey: Survey.SurveyModel): void {
    survey.onAfterRenderPage.add((sender, options) => {
        window.document.title = options.page.title;
    });

    // THIS IS THE SHOWDOWN MARKDOWN CODE***************
    // More details on this at https://github.com/showdownjs/showdown/wiki/Showdown-Options
    const converter = new showdown.Converter();
    converter.setOption("simpleLineBreaks", true);
    converter.setOption("tasklists", true);

    survey.onTextMarkdown.add((sender, options) => {
        // convert the mardown text to html
        let str = converter.makeHtml(options.text);

        // remove root paragraphs <p></p>
        str = str.substring(3);
        str = str.substring(0, str.length - 4);

        // set html
        options.html = str;
    });

    survey.onUpdateQuestionCssClasses.add((sender, options) => {
        const classes = options.cssClasses;

        //  If it is the preview mode...
        if (sender.isDisplayMode === true) {
            if (options.question.getType() === "html") {
                //  This will remove the html questions in Preview mode.
                classes.root += " sv-hidden";
            } else if (options.question.getType() === "comment") {
                //  do not show the 'description' property
                classes.description += " sv-hidden";
            } else if (options.question.getType() === "file") {
                classes.placeholderInput += " sv-hidden";
            }
        }

        //  Add the css class label-danger
        classes.error.locationTop += " label-danger";

        if (options.question.getType() === "comment" || options.question.getType() === "text") {
            // This is a little strange but for 'comment' the root is <textarea>
            classes.root += " form-control";
        } else {
            classes.root += " form-group";

            if (options.question.getType() === "file") {
                // Hide the file decorator
                //  classes.fileDecorator += " sv-hidden";

                // Hide the 'Clean' button
                classes.removeButton = "sv-hidden";
            } else if (options.question.getType() === "dropdown") {
                classes.control += " form-control";
            } else if (options.question.getType() === "radiogroup") {
                classes.materialDecorator = "";
            } else if (options.question.getType() === "matrixdynamic") {
                classes.button += " btn btn-primary";
            } else if (options.question.getType() === "paneldynamic") {
                classes.button += " btn btn-primary";
            }
        }
    });

    survey.onUpdatePanelCssClasses.add((sender, options) => {
        const classes = options.cssClasses;

        if (sender.isDisplayMode === true && options.panel.hideOnPreview === true) {
            //  This is to hide panel we don't want to show on preview.
            //  Panels that contains information html for example.
            //  It also hides the 'pages'! The reason is because on preview, the pages become panels
            //  and therefore going thru this code.
            classes.panel.container = "sv-hidden";
        } else {
            //  This is a class found in GoC and was used in the original project.
            //  It adds a border around a panel and a different backgroud color
            classes.panel.container += " well";
        }
    });

    survey.onValidatedErrorsOnCurrentPage.add((sender, options) => {
        const errorSection = document.getElementById("div_errors_list");

        if (errorSection) {
            if (options.errors && options.errors.length > 0) {
                const problem = new ProblemDetails();
                problem.detail = "";
                problem.errors = options.errors;
                SurveyHelper.printProblemDetails(problem, survey.locale);
            } else {
                SurveyHelper.clearProblemDetails();
            }
        }
    });

    survey.onGetQuestionTitle.add((sender, options) => {
        //  This is to add * at the beginning of a required question. The property requiredText
        //  is set as 'required' later in the code
        if (options.question.isRequired) {
            options.title = `<span class='sv_q_required_text'>&ast;&nbsp;</span>${options.title as string}`;
        }
    });

    survey.onAfterRenderQuestion.add((sender, options) => {
        if (sender.isDisplayMode) {
            //  We are hidding the description in 'Preview' mode
            switch (options.question.getType()) {
                case "boolean": {
                    const boolQuestion: Survey.QuestionBooleanModel = options.question;
                    boolQuestion.descriptionLocation = "hidden";
                    break;
                }
                case "radiogroup": {
                    const rbQuestion: Survey.QuestionRadiogroupModel = options.question;
                    rbQuestion.descriptionLocation = "hidden";
                    break;
                }
                case "comment": {
                    const cmtQuestion: Survey.QuestionCommentModel = options.question;
                    cmtQuestion.descriptionLocation = "hidden";
                    break;
                }
                default:
                    break;
            }
        }
    });

    //  Use for our custom navigation
    survey.onCurrentPageChanged.add(sender => {
        SurveyNavigation.onCurrentPageChanged_updateNavButtons(sender);
    });
}
