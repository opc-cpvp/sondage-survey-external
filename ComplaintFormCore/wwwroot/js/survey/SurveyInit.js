var global_language = "";

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

    //  We do not apply style here, we're using the GoC style
    //Survey
    //    .StylesManager
    //    .applyTheme("default");

    //  This is how we replace string from Survey.js (englishStrings or frenchSurveyStrings) for localization.
    Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
    Survey.surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";

    Survey.StylesManager.Enabled = false;

    //Survey.ChoicesRestfull.onBeforeSendRequest = function (sender, options) {
    //    options.request.setRequestHeader("Authorization", "Bearer " + authToken);
    //};

    //Survey.ChoicesRestfull.onBeforeSendRequest = function (sender, options) {
    //    options.request.setRequestHeader("language", Survey.surveyLocalization.locales);
    //};

    //new Survey.SurveyTemplateText().replaceText('<div><p>hgfhgf</p></div>', "question", "radiogroup");
}

function initSurveyModelProperties(survey) {

    var myCss = {
        navigationButton: "btn btn-primary",
        html: "sq-html"
    };

    survey.css = myCss;

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
    survey.setVariable("languageCode", global_language);    

    if (global_language == "fr") {
        survey.requiredText = "(obligatoire)";
    }
    else {
        survey.requiredText = "(required)";
    }
}

function initSurveyModelEvents(survey) {

    //  THIS IS THE SHOWDOWN MARKDOWN CODE***************
    //  More details on this at https://github.com/showdownjs/showdown/wiki/Showdown-Options
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
        .onUpdateQuestionCssClasses
        .add((survey, options) => {

        let classes = options.cssClasses;

        //  Add the css class label-danger
        classes.error.locationTop += " label-danger";

        if (options.question.getType() == "comment") {
            // This is a little strange but for 'comment' the root is <textarea>
            classes.root = "form-control";
            //let parentClasses = options.question.parent.cssClasses;
        }
        else {

            classes.root += " form-group";

            if (options.question.getType() == "checkbox") {
                classes.itemControl += " checkbox-info-popup-trigger";
            }
            else if (options.question.getType() == "file") {
                //classes.chooseFile = "btn btn-primary";
            }
            else if (options.question.getType() == "dropdown") {
                classes.control += " form-control";
            }
            else if (options.question.getType() == "comment") {
                //classes.comment = "form-control";
                //classes.root += " form-group";
                //let parentClasses = options.question.parent.cssClasses;
            }
            else if (options.question.getType() == "radiogroup") {
                //classes.materialDecorator = "";
            } 
        }       
    });

    survey
        .onUpdatePanelCssClasses
        .add((survey, options) => {

            let classes = options.cssClasses;
            classes.panel.container = "";
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
        });

    survey
        .onValidatedErrorsOnCurrentPage
        .add(function (sender, options) {
            
            if (options.errors && options.errors.length > 0) {
                $("#div_errors_list").html(buildErrorMessage(options.errors));
                $("#div_errors_list").show();

                updateLabelError(options.errors);
            }
            else {
                $("#div_errors_list").html("");
                $("#div_errors_list").hide();
            }
        });

    survey
        .onGetQuestionTitle.add(function (sender, options) {

            //  This is to add * at the beginning of a required question. The property requiredText
            //  is set as 'required' later in the code
            if (options.question.owner.isRequired) {
                options.title = "<span class='sv_q_required_text'>&ast; </span>" + options.title;
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

function buildErrorMessage(errors) {

    var message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (global_language == "fr") {
        message += "Le formulaire ne pouvait pas être soumis parce que ";
    }
    else {
        message += "The form could not be submitted because ";
    }
   
    message += errors.length;

    if (errors.length > 1) {
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

    $.each(errors, function (key, value) {

        var errorIndex = key + 1;

        message += "<li>";

        if (value.errorOwner.getType() == "radiogroup") {
            //  We are selecting the first option to href to
            message += "<a href='#" + value.errorOwner.inputId + "_0'>";
        }
        else {
            message += "<a href='#" + value.errorOwner.inputId + "'>";
        }

        if (global_language == "fr") {
            message += "Erreur " + errorIndex + ": ";
        }
        else {
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

function updateLabelError(errors) {

    $.each(errors, function (key, value) {

      
        var test = value.owner;
    });
}

