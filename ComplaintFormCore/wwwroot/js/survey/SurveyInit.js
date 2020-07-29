var file_uploaded = [];

function initSurvey(Survey) {

    //  Add a new property for each item choices (to the native text, value). This is used for checkboxes with addtional Html info
    //  but could be used for radiobutton as well.
    Survey.JsonObject.metaData.addProperty("itemvalue", {
        name: "htmlAdditionalInfo:text"
    });

    //  Adding properties to 'file' type question. Those are mainly for messaging/text purposes
    Survey.JsonObject.metaData.addProperties("file", [
        { name: "itemListTitle:text" },
        { name: "itemListRemoveText:text" },
        { name: "itemListNoAttachmentsText:text" },
        { name: "confirmRemoveMessage:text" },
        { name: "duplicateFileNameExceptionMessage:text" },
        { name: "multipleFileMaxSizeErrorMessage:text" }
    ]);

    // Register the function for use in SurveyJS expressions. This function validates that at least one selection was made in a <select>
    Survey
        .FunctionFactory
        .Instance
        .register("HasSelectedItem", HasSelectedItem);

    //  This is how we replace string from Survey.js (englishStrings or frenchSurveyStrings) for localization.
    Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
    Survey.surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";

    Survey.StylesManager.Enabled = false;

    //  This is a survey property that will hold the information as to if the user has reached the 'Preview'
    //  page at least once. The idea is if the user has reached the 'Preview' page he can always go back to it after
    //  editing a page. This will be usefull for very long survey after a user decided to edit an item from the preview page.
    Survey
        .JsonObject
        .metaData
        .addProperty("survey", { name: "passedPreviewPage:boolean", default: false });

    //  This is to hide page and panel we don't want to show on preview.
    //  Pages or Panels that contains exclusively information html for example.
    //  The reason why it is working for is because on preview, the pages become panels
    Survey
        .JsonObject
        .metaData
        .addProperty("panel", { name: "hideOnPreview:boolean", default: false });

    Survey
        .JsonObject
        .metaData
        .addProperty("page", { name: "hideOnPreview:boolean", default: false });

    //  This is an example of how to update the Survey settings
    //Survey.settings.minWidth = "109px"
    
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

    //https://surveyjs.io/Documentation/Library?id=surveymodel#checkErrorsMode
    survey.checkErrorsMode = "onValueChanged";

    survey.showProgressBar = "bottom";
    survey.goNextPageAutomatic = false;
    survey.showQuestionNumbers = "off";
    survey.showNavigationButtons = false;
    survey.showCompletedPage = true;

    if (survey.locale == "fr") {
        survey.requiredText = "(obligatoire)";
    }
    else {
        survey.requiredText = "(required)";
    }
}

function initSurveyModelEvents(survey) {

    survey.onAfterRenderPage.add(function (survey, options) {

        //  Change page title to <h1>. 
        //  This way is not working, it's creating an error on Preview:  DOMException: Failed to execute 'insertBefore' on 'Node':
        //      because the tag <h4> is not there anymore and during the preview, the page titles are being transformed into <h2>
        //switchPageTitleToH1();
        
        window.document.title = options.page.title;

        switchPanelTitleToH2();
    });

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

            //  If it is the preview mode...
            if (survey.isDisplayMode == true) {

                if (options.question.getType() === 'html') {

                    //  This will remove the html questions in Preview mode.
                    classes.root += " sv-hidden";
                }
                else if (options.question.getType() == "comment") {

                    //  do not show the 'description' property
                    classes.description += " sv-hidden";
                }
                else if (options.question.getType() == "file") {


                    classes.placeholderInput += " sv-hidden";
                }
            }

            //  Add the css class label-danger
            classes.error.locationTop += " label-danger";

            if (options.question.getType() == "comment") {
                // This is a little strange but for 'comment' the root is <textarea>
                classes.root = "form-control";
            }
            else {

                classes.root += " form-group";

                if (options.question.getType() == "file") {

                    // Hide the file decorator
                    classes.fileDecorator += " sv-hidden";

                    // Hide the 'Clean' button
                    classes.removeButton = "sv-hidden";
                }
                else if (options.question.getType() == "dropdown") {
                    classes.control += " form-control";
                }
                else if (options.question.getType() == "radiogroup") {
                    classes.materialDecorator = "";
                }
            }

        });  

    survey
        .onUpdatePanelCssClasses
        .add((survey, options) => {
            
            let classes = options.cssClasses;

            if (survey.isDisplayMode == true && options.panel.hideOnPreview == true) {

                //  This is to hide panel we don't want to show on preview.
                //  Panels that contains information html for example.
                //  It also hides the 'pages'! The reason is because on preview, the pages become panels
                //  and therefore going thru this code.
                classes.panel.container = "sv-hidden";
            }
            else {
                //  This is a class found in GoC and was used in the original project.
                //  It adds a border around a panel and a different backgroud color
                classes.panel.container += " well"; 
            } 
        }); 

    survey
        .onAfterRenderQuestion
        .add(function (survey, options) {

            if (options.question.getType() === "file") {

                //  This is to build the file preview, we're not using the native one
                var container = document.createElement("div");
                container.setAttribute("id", "div_file_" + options.question.name);
                container.className = "my-preview-container";             

                var fileElement = options
                    .htmlElement
                    .getElementsByClassName("sv_q_file")[0];

                if (!fileElement) {
                    fileElement = options
                        .htmlElement
                        .getElementsByClassName("sv-file__decorator")[0];
                }

                fileElement.append(container);
                
                options
                    .question
                    .onPropertyChanged
                    .add(function (question, options) {
                   
                        if (options.name === "value") {

                            //  Checking for files with the same name. We don't want that because when we 'removed' a file, all files with the same
                            //  name are being deleted. This could be solved if the file property "storeDataAsText" was set to false and files 
                            //  were uploaded into a server. But for now, we store the files in the local storage.

                            //var tempArray = [];

                            //(options.newValue).forEach(function (fileItem) {

                            //    if (tempArray.length > 0) {
                            //        if (tempArray.some(e => e.name === fileItem.name)) {
                            //            //  If a duplicate is found we are just adding a timstamp.
                            //            //  It is way easier then setting up errors on the question and asking the user
                            //            //  to remove one of the file.
                            //            fileItem.name += "_" + (new Date).getTime().toString() 
                            //        }
                            //    }

                            //    tempArray.push(fileItem);
                            //});

                            updateFilePreview(survey, question, container);
                        }
                    });

                updateFilePreview(survey, options.question, container);
            }
        });

    survey
        .onUploadFiles
        .add((sender, options) => {
            options
                .files
                .forEach(function (file) {

                    var formData = new FormData();
                    //formData.append('SurveyId', sender.surveyId);
                    formData.append('file', file, file.name);
                    //formData.append("filename", file.n)
             

                    $.ajax({
                        url: "/api/File?surveyId=" + sender.surveyId,
                        type: "POST",
                        success: function () {

                            options.callback("success", options.files.map(file => {
                                const newFilename = (new Date).getTime().toString() + file.name

                                var file_uploaded_item = {
                                    questionname: options.name,
                                    filename: file.name,
                                    size: file.size
                                };

                                file_uploaded.push(file_uploaded_item);

                                return {
                                    file: new File([file], file.name, { type: file.type, size: file.length })
                                };  
                            }));
                        },
                        error: function (xhr, status, error) {
                            //var err = eval("(" + xhr.responseText + ")");
                            alert(xhr.responseText);
                        },
                        async: true,
                        data: formData,
                        cache: false,
                        contentType: false,
                        processData: false,
                        timeout: 60000
                    });
                });
        });

    survey
        .onClearFiles
        .add(function (survey, options) {          
            options.callback('success');
        });

    survey
        .onValidatedErrorsOnCurrentPage
        .add(function (sender, options) {
            
            if (options.errors && options.errors.length > 0) {
                $("#div_errors_list").html(buildErrorMessage(options.errors));
                $("#div_errors_list").show();
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
        .add(function (survey, options) {
            onCurrentPageChanged_updateNavButtons(survey);
        });

    survey
        .onCompleting
        .add(function (sender, options) {
            options.allowComplete = confirm('Do you want to complete the survey?');
        });

}

//  This is to build a custom file preview container.
function updateFilePreview(survey, question, container) {

    container.innerHTML = "";

    var title = document.createElement("h3");
    title.innerHTML = getTranslation(question.itemListTitle);
    container.append(title);   

    if (question.value && question.value.length > 0) {

        var listView = document.createElement("ol");

        (question.value).forEach(function (fileItem) {

            var item = document.createElement("li");

            var span = document.createElement("span");
            span.className = "sv_q_file_preview";

            var div = document.createElement("div");

            //var size = Math.round(fileItem.content.length / 1000, 0) || 0;
            // var size = Math.round(fileItem.content.length / 1000, 0) || 0;
            var size = 33;
            var object_from_memory = file_uploaded.filter(function (item)
            {
                return (item.questionname == question.name && item.filename == fileItem.name);
            });

            size = object_from_memory || 0;

            var button = document.createElement("div");
            button.className = "btn sv-btn sv-file__choose-btn";            
            button.innerText = fileItem.name + " (" + size + " KB-B)";

            button.onclick = function () {

                fetch("/api/File?surveyId=" + survey.surveyId + "&filename=" + fileItem.name)
                    .then(resp => resp.blob())
                    .then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement('a');
                        a.style.display = 'none';
                        a.href = url;
                        a.download = fileItem.name;
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                    })
                    .catch(() => alert('oh no!'));
            }

            div.append(button);

            var buttonRemove = document.createElement("button");
            buttonRemove.setAttribute('type', 'button');
            buttonRemove.className = "btn sv_q_file_remove_button";
            buttonRemove.innerText = getTranslation(question.itemListRemoveText);

            if (survey.isDisplayMode == true) {
                buttonRemove.setAttribute('disabled', 'disabled');
            }

            buttonRemove.onclick = function () {
                if (confirm(getTranslation(question.confirmRemoveMessage))) {
                    question.removeFile({ name: fileItem.name });
                }
            }

            div.append(buttonRemove);

            span.appendChild(div);
            item.appendChild(span);

            listView.appendChild(item);
        });

        container.append(listView);
    }
    else {
        var title = document.createElement("p");
        title.innerHTML = getTranslation(question.itemListNoAttachmentsText);
        container.append(title);
    }    
}

function getFileSize(surveyId, filename) {

    $.ajax({
        url: "/api/File?surveyId=" + surveyId + "&filename=" + filename,
        type: "GET",
        success: function (data) {
            return data.size;
        },
        error: function (xhr, status, error) {
            //var err = eval("(" + xhr.responseText + ")");
            alert(xhr.responseText);
        }
    });
}

//  Function for updating (show/hide) the navigation buttons
function onCurrentPageChanged_updateNavButtons(survey) {

    //  NOTES:
    //  survey.isFirstPage is the start page but for some reasons when we view the preview, survey.isFirstPage 
    //      gets set to true. This maybe a bug in survey.js or else there is a reason I don't understand

    document
        .getElementById('btnEndSession')
        .style
        .display = !survey.isFirstPage || survey.isDisplayMode
            ? "inline"
            : "none";

    document
        .getElementById('btnStart')
        .style.display = survey.isFirstPage && !survey.isDisplayMode
            ? "inline"
            : "none";

    document
        .getElementById('btnSurveyPrev')
        .style
        .display = !survey.isFirstPage
            ? "inline"
            : "none";
    document
        .getElementById('btnSurveyNext')
        .style
        .display = !survey.isFirstPage && !survey.isLastPage
            ? "inline"
            : "none";
    document
        .getElementById('btnShowPreview')
        .style
        .display = !survey.isDisplayMode && (survey.isLastPage || survey.passedPreviewPage == true)
            ? "inline"
            : "none";
    document
        .getElementById('btnComplete')
        .style
        .display = survey.isDisplayMode
            ? "inline"
            : "none";
}

function showPreview(survey) {

    //  Set the survey property that will hold the information as to if the user has reached the 'Preview'
    survey.passedPreviewPage = true;

    //  Calling the native showPreview method
    survey.showPreview();
}

function startSurvey(survey) {

    clearLocalStorage(storageName_PA);

    survey.nextPage();
}


function endSession() {
    var url = "/Home/Index";
    window.location.href = url;
}

//  This function will build a <section> with a list of errors to be displayed at the top of the page
function buildErrorMessage(errors) {

    var message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (survey.locale == "fr") {
        message += "Le formulaire ne pouvait pas être soumis parce que ";
    }
    else {
        message += "The form could not be submitted because ";
    }
   
    message += errors.length;

    if (errors.length > 1) {
        if (survey.locale == "fr") {
            message += " erreurs ont été trouvée";
        }
        else {
            message += " errors where found";
        }        
    }
    else {
        if (survey.locale == "fr") {
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

        if (survey.locale == "fr") {
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

function switchPageTitleToH1() {

    var pagetitle = $("h4.sv_page_title");

    pagetitle.replaceWith(function () {

        return "<h1 class='sv_page_title'><span style='position: static; '><span style='position: static;'>" + $(this).text() + "</span></span></h1>";
    });
}

function switchPanelTitleToH2() {

    $("h4.sv_p_title").replaceWith(function () {

        return '<h2>' + $(this).text() + '</h2>';
    });
}

