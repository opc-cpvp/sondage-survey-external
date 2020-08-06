function initSurveyFile(Survey) {

    //  Adding properties to 'file' type question. Those are mainly for messaging/text purposes
    Survey.JsonObject.metaData.addProperties("file", [
        { name: "itemListTitle:text" },
        { name: "itemListRemoveText:text" },
        { name: "itemListNoAttachmentsText:text" },
        { name: "confirmRemoveMessage:text" },
        { name: "duplicateFileNameExceptionMessage:text" },
        { name: "multipleFileMaxSizeErrorMessage:text" }
    ]);

}

function initSurveyFileModelEvents(survey) {

    survey
        .onAfterRenderQuestion
        .add(function (sender, options) {

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

                        //  Everytime a file gets uploaded or removed we are redrawing the file preview container.
                        if (options.name === "value") {
                            updateFilePreview(sender, question, container);
                        }
                    });

                updateFilePreview(sender, options.question, container);
            }
        });

    survey
        .onUploadFiles
        .add((sender, options) => {
            options
                .files
                .forEach(function (file) {

                    var newFilename = file.name;

                    if (options.question.value && options.question.value.some(e => e.name === file.name)) {

                        //  Checking for files with the same name. We don't want that because when we 'removed' a file, all files with the same
                        //  name are being deleted. This could be solved if the file property "storeDataAsText" was set to false and files 
                        //  were uploaded into a server. But for now, we store the files in the local storage.
                        //  If a duplicate is found we are just adding a timstamp.
                        //  It is way easier then setting up errors on the question and asking the user
                        //  to remove one of the file.
                        alert(getTranslation(options.question.duplicateFileNameExceptionMessage));

                        //  The first part can be just about anything but a timestamp is a sure way to avoid collisions
                        newFilename = (new Date).getTime().toString() + '_' + file.name;
                    }

                    var formData = new FormData();
                    formData.append('file', file, newFilename);

                    $.ajax({
                        url: "/api/File/Upload?complaintId=" + sender.complaintId,
                        type: "POST",
                        success: function () {

                            options.callback("success", options.files.map(file => {

                                //  We cannot store the file content in local storage because of the 5MB storage limit.
                                //  The problem was with the file size e.g. without file content, there is no way to know the
                                //  file size. The work around is to store the file size in the 'content' property.

                                return {
                                    file: new File([file], newFilename, { type: file.type }),
                                    content: file.size.toString()
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
        .add(function (sender, options) {
            options.callback('success');
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
        var index = 0;

        (question.value).forEach(function (fileItem) {

            var item = document.createElement("li");

            var span = document.createElement("span");
            // span.className = "sv_q_file_preview";

            var div = document.createElement("div");

            var fileSizeInBytes = fileItem.content || 0;
            var size = Math.round(fileSizeInBytes / 1000, 0) || 0;

            var button = document.createElement("div");
            button.className = "btn sv-btn sv-file__choose-btn";
            button.innerText = fileItem.name + " (" + size + " KB)";

            var buttonId = 'btn_' + question.name + '_' + index;
            index = index + 1;
            button.setAttribute('id', buttonId);

            button.onclick = function () {

                fetch("/api/File/Get?complaintId=" + survey.complaintId + "&filename=" + fileItem.name)
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
