﻿var global_language = "";

function initSurvey(Survey) {

    //  Add a new property for each item choices (to the native text, value). This is used for checkboxes with addtional Html info
    //  but could be used for radiobutton as well.
    Survey.JsonObject.metaData.addProperty("itemvalue", {
        name: "htmlAdditionalInfo:text"
    });

    // Register the function for use in SurveyJS expressions. This function validates that at least one selection was made in a <select>
    Survey
        .FunctionFactory
        .Instance
        .register("HasSelectedItem", HasSelectedItem);

    //  You may add a new property into all question types, panel, page and survey. If you add a property into base type, like “question” then it will be available
    //  in all its successors.“questionbase” is base class for all questions and “question” is base class for all questions that has a value property.
    //  “html” question is derived from “questionbase” and not “question” type.
    Survey.JsonObject.metaData.addProperty("question",
        {
            name: "showOnPreview", default: true
        });

    Survey.JsonObject.metaData.addProperty("questionbase",
        {
            name: "showOnPreview", default: true
        });

    Survey.JsonObject.metaData.addProperty("page", {
        name: "showOnPreview", default: true
    });

    Survey.JsonObject.metaData.addProperty("panel", {
        name: "showOnPreview", default: true
    });

    //  We do not apply style here, we're using the GoC style
    //Survey
    //    .StylesManager
    //    .applyTheme("default");

    //  This is how we replace string from Survey.js (englishStrings or frenchSurveyStrings) for localization.
    Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
    Survey.surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";
}

function initSurveyModelProperties(survey) {

    //survey.showPreviewBeforeComplete = 'showAnsweredQuestions';
    survey.showPreviewBeforeComplete = 'showAllQuestions';

    //  onHidden -> survey clears the question value when the question becomes invisible.
    //  If a question has an answer value and it was invisible initially, a survey clears the value on completing.
    survey.clearInvisibleValues = "onHidden ";

    survey.questionErrorLocation = "top";
    survey.showProgressBar = "bottom";
    survey.goNextPageAutomatic = false;
    survey.showQuestionNumbers = "off";
    survey.showNavigationButtons = false;

    global_language = survey.locale;

    if (global_language == "fr") {
        survey.requiredText = "(obligatoire)";
    }
    else {
        survey.requiredText = "(required)";
    }
}

function initSurveyModelEvents(survey) {

    //  THIS IS THE SHOWDOWN MARKDOWN CODE***************
    var converter = new showdown.Converter();
    converter.simpleLineBreaks = true;
    converter.tasklists = true;

    survey
        .onTextMarkdown
        .add(function (survey, options) {

            //convert the mardown text to html
            var str = converter.makeHtml(options.text);

            //remove root paragraphs <p></p>
            str = str.substring(3);
            str = str.substring(0, str.length - 4);

            //set html
            options.html = str;
        });

    survey
        .onValueChanged
        .add(function (sender, options) {

            //if (options.question.getType() === 'comment') {
            //    if (!options.value || (!options.value.trim() && options.value.trim() !== 0 && options.value.trim() !== false)) {

            //        //  Adding a . for comment questions that are not answered so they can be displayed in the preview
            //        sender.getQuestionByName(options.name).value = ".";
            //    }
            //}
        });

    survey
        .onAfterRenderQuestion
        .add(function (survey, options) {

            //if (options.question.getType() === 'comment') {
            //    if (!options.question.value || (!options.question.value.trim() && options.question.value.trim() !== 0 && options.question.value.trim() !== false)) {

            //        //  Adding a . for comment questions that are not answered so they can be displayed in the preview
            //        options.question.value = ".";
            //    }
            //}


            //  If it is the preview mode...
            if (survey.isDisplayMode == true) {

                if (options.question.showOnPreview == false) {
                    //  don't show the question.... but how?
                }

                if (options.question.page.showOnPreview == false) {
                    //options.question.page.visible = false;
                }

                //  do not show the 'description' property
                options.question.description = "";

                //  This will remove the html questions in Preview mode.
                //if (options.question.getType() === 'html') {
                //    options.question.html = "";
                //}

            }

            //  This is to add * at the beginning of a required question. The property requiredText
            //  is set as 'required' later in the code
            if (options.question.isRequired == true && !options.question.title.includes("<span class='sv_q_required_text'>&ast; </span>")) {

                options.question.title = "<span class='sv_q_required_text'>&ast; </span>" + options.question.title;
            }
        });

    survey
        .onValidatedErrorsOnCurrentPage
        .add(function (sender, options) {
            
            if (options.errors && options.errors.length > 0) {
                $("#div_errors_list").html(buildErrorMessage(options.errors, options.questions));
                $("#div_errors_list").show();
            }
            else {
                $("#div_errors_list").html("");
                $("#div_errors_list").hide();
            }
        });

    //  Use for our custom navigation
    survey
        .onCurrentPageChanged
        .add(onCurrentPageChanged_updateNavButtons);

    survey.onCompleting.add(function (sender, options) {
        options.allowComplete = confirm('Do you want to complete the survey?');
    });

}

//  Function for updating (show/hide) the navigation buttons
function onCurrentPageChanged_updateNavButtons(survey) {

    document
        .getElementById('btnSurveyPrev')
        .style
        .display = !survey.isFirstPage
            ? "inline"
            : "none";
    document
        .getElementById('btnSurveyNext')
        .style
        .display = !survey.isLastPage
            ? "inline"
            : "none";
    document
        .getElementById('btnShowPreview')
        .style
        .display = survey.isLastPage && !survey.isDisplayMode
            ? "inline"
            : "none";
    document
        .getElementById('btnComplete')
        .style
        .display = survey.isDisplayMode
            ? "inline"
            : "none";
}

//  Will return true if a dropdown has a selected item otherwise false.
function HasSelectedItem(params) {
    var value = params[0];
    // alert(value)
    // value is the id of the selected item
    return value !== "";
}

function endSession() {
    var url = "/Home/Index";
    window.location.href = url;
}

function save() {
    alert("Not implemented. Probably need to trigger Complete on the survey.");
}

function buildErrorMessage(errorArray, questionArray) {

    var message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (global_language == "fr") {
        message += "Le formulaire ne pouvait pas être soumis parce que ";
    }
    else {
        message += "The form could not be submitted because ";
    }
   
    message += errorArray.length;

    if (errorArray.length > 1) {
        if (global_language == "fr") {
            message += " erreurs ont été trouvée";
        }
        else {
            message += " errors where found";
        }        
    }
    else {
        if (global_language == "fr") {
            message += " erreur a été trouvée";
        }
        else {
            message += " error was found";
        }
    }
   
    message += "</h2>";

    message += "<ul>";

    $.each(questionArray, function (key, val) {

        if (val.hasErrors) {
            var errorIndex = key + 1;

            message += "<li>";

            if (val.getType() == "radiogroup") {
                //  We are selecting the first option to href to
                message += "<a href='#" + val.inputId + "_0'>";
            }
            else {
                message += "<a href='#" + val.inputId + "'>";
            }

            if (global_language == "fr") {
                message += "Erreur " + errorIndex + ": ";
            }
            else {
                message += "Error " + errorIndex + ": ";
            }
           
            message += val.title;
            message += "</a>";
            message += "</li>";
        }       
    });
   
    message += "</ul>";
    message += "</section>";

    return message;
}

