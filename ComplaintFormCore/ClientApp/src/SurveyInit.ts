declare let $: any;
import * as showdown from "showdown";
// declare let survey: { locale: string };

import * as SurveyHelper from "./surveyHelper";
import * as SurveyLocalStorage from "./surveyLocalStorage";
import * as Survey from "survey-vue";

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
    Survey.FunctionFactory.Instance.register(
        "HasSelectedItem",
        SurveyHelper.HasSelectedItem
    );

    // This is how we replace string from Survey.js (englishStrings or frenchSurveyStrings) for localization.
    Survey.surveyLocalization.locales["en"].requiredError =
        "This field is required";
    Survey.surveyLocalization.locales["fr"].requiredError =
        "Ce champ est obligatoire";

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

    if (survey.locale === "fr") {
        survey.requiredText = "(obligatoire)";
    } else {
        survey.requiredText = "(required)";
    }
}

export function initSurveyModelEvents(survey: Survey.SurveyModel): void {
    survey.onAfterRenderPage.add((sender, options) => {
        // Change page title to <h1>.
        // This way is not working, it's creating an error on Preview:  DOMException: Failed to execute 'insertBefore' on 'Node':
        //     because the tag <h4> is not there anymore and during the preview, the page titles are being transformed into <h2>
        // switchPageTitleToH1();

        window.document.title = options.page.title;

        switchPanelTitleToH2();
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

        if (options.question.getType() === "comment") {
            // This is a little strange but for 'comment' the root is <textarea>
            classes.root = "form-control";
        } else {
            classes.root += " form-group";

            if (options.question.getType() === "file") {
                // Hide the file decorator
                classes.fileDecorator += " sv-hidden";

                // Hide the 'Clean' button
                classes.removeButton = "sv-hidden";
            } else if (options.question.getType() === "dropdown") {
                classes.control += " form-control";
            } else if (options.question.getType() === "radiogroup") {
                classes.materialDecorator = "";
            }
        }
    });

    survey.onUpdatePanelCssClasses.add((sender, options) => {
        const classes = options.cssClasses;

        if (
            sender.isDisplayMode === true &&
            options.panel.hideOnPreview === true
        ) {
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
        if (options.errors && options.errors.length > 0) {
            $("#div_errors_list").html(
                buildErrorMessage(options.errors, survey.locale)
            );
            $("#div_errors_list").show();
        } else {
            $("#div_errors_list").html("");
            $("#div_errors_list").hide();
        }
    });

    survey.onGetQuestionTitle.add((sender, options) => {
        //  This is to add * at the beginning of a required question. The property requiredText
        //  is set as 'required' later in the code
        if (options.question.owner.isRequired) {
            options.title =
                "<span class='sv_q_required_text'>&ast; </span>" +
                options.title;
        }
    });

    //  Use for our custom navigation
    survey.onCurrentPageChanged.add((sender, options) => {
        onCurrentPageChanged_updateNavButtons(sender);
    });
}

//  Function for updating (show/hide) the navigation buttons
export function onCurrentPageChanged_updateNavButtons(
    survey: Survey.SurveyModel
): void {
    //  NOTES:
    //  survey.isFirstPage is the start page but for some reasons when we view the preview, survey.isFirstPage
    //      gets set to true. This maybe a bug in survey.js or else there is a reason I don't understand

    // document
    //    .getElementById("btnEndSession")
    //    .style
    //    .display = !survey.isFirstPage || survey.isDisplayMode
    //        ? "inline"
    //        : "none";

    const startButton =
        document.getElementById("btnStart") ?? new HTMLElement();
    startButton.style.display =
        survey.isFirstPage && !survey.isDisplayMode ? "inline" : "none";

    const previousButton =
        document.getElementById("btnSurveyPrev") ?? new HTMLElement();
    previousButton.style.display = !survey.isFirstPage ? "inline" : "none";

    const nextButton =
        document.getElementById("btnSurveyNext") ?? new HTMLElement();
    nextButton.style.display =
        !survey.isFirstPage && !survey.isLastPage ? "inline" : "none";

    const showPreviewButton =
        document.getElementById("btnShowPreview") ?? new HTMLElement();
    showPreviewButton.style.display =
        !survey.isDisplayMode &&
        (survey.isLastPage || survey.passedPreviewPage === true)
            ? "inline"
            : "none";

    const completeButton =
        document.getElementById("btnComplete") ?? new HTMLElement();
    completeButton.style.display = survey.isDisplayMode ? "inline" : "none";
}

export function showPreview(survey: Survey.SurveyModel): void {
    //  Set the survey property that will hold the information as to if the user has reached the 'Preview'
    survey.passedPreviewPage = true;

    //  Calling the native showPreview method
    survey.showPreview();
}

export function startSurvey(survey: Survey.SurveyModel): void {
    SurveyLocalStorage.clearLocalStorage(SurveyLocalStorage.storageName_PA);
    survey.nextPage();
}

export function endSession(): void {
    const url = "/Home/Index";
    window.location.href = url;
}

//  This function will build a <section> with a list of errors to be displayed at the top of the page
export function buildErrorMessage(errors, lang: string): string {
    let message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (lang === "fr") {
        message += "Le formulaire ne pouvait pas être soumis parce que ";
    } else {
        message += "The form could not be submitted because ";
    }

    message += errors.length;

    if (errors.length > 1) {
        if (lang === "fr") {
            message += " erreurs ont été trouvée";
        } else {
            message += " errors where found";
        }
    } else {
        if (lang === "fr") {
            message += " erreur a été trouvée";
        } else {
            message += " error was found";
        }
    }

    message += "</h2>";

    message += "<ul>";

    $.each(errors, (key: string, value) => {
        const errorIndex = key + 1;

        message += "<li>";

        if (value.errorOwner.getType() === "radiogroup") {
            //  We are selecting the first option to href to
            message += "<a href='#" + value.errorOwner.inputId + "_0'>";
        } else {
            message += "<a href='#" + value.errorOwner.inputId + "'>";
        }

        if (lang === "fr") {
            message += "Erreur " + errorIndex + ": ";
        } else {
            message += "Error " + errorIndex + ": ";
        }

        message += value.errorOwner.title;
        message += " - " + value.getText();
        message += "</a>";
        message += "</li>";
    });

    message += "</ul>";
    message += "</section>";

    return message;
}

export function switchPanelTitleToH2(): void {
    // TODO: fix this
    //$("h4.sv_p_title").replaceWith(() => {
    //    return "";
    //     //return `<h2>${$(target).text()}</h2>`;
    //});
}
