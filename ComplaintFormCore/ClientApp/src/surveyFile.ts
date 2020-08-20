import * as Survey from "survey-vue";
import { getTranslation, printProblemDetails } from "./surveyHelper";

export function initSurveyFile(): void {

    //  Adding properties to 'file' type question. Those are mainly for messaging/text purposes
    Survey.JsonObject.metaData.addProperties("file", [
        { name: "itemListTitle:text"                     },
        { name: "itemListRemoveText:text"                },
        { name: "itemListNoAttachmentsText:text"         },
        { name: "confirmRemoveMessage:text"              },
        { name: "duplicateFileNameExceptionMessage:text" },
        { name: "multipleFileMaxSizeErrorMessage:text"   }
    ]);

}

export function initSurveyFileModelEvents(survey: Survey.SurveyModel): void {
    survey
        .onAfterRenderQuestion
        .add((sender, options) => {

            if (options.question.getType() === "file") {

                //  This is to build the file preview, we're not using the native one
                const container = document.createElement("div");
                container.setAttribute("id", "div_file_" + options.question.name);
                container.className = "my-preview-container";

                let fileElement = options
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
                    .add((question, opt) => {

                        // Everytime a file gets uploaded or removed we are redrawing the file preview container.
                        if (opt.name === "value") {
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
                .forEach(file => {

                    let newFilename = file.name;

                    if (options.question.value && options.question.value.some(e => e.name === file.name)) {

                        // Checking for files with the same name. We don't want that because when we
                        // 'removed' a file, all files with the same name are being deleted. This
                        // could be solved if the file property "storeDataAsText" was set to false
                        // and files were uploaded into a server. But for now, we store the files in
                        // the local storage. If a duplicate is found we are just adding a timstamp.
                        // It is way easier then setting up errors on the question and asking the
                        // user to remove one of the file.
                        alert(getTranslation(options.question.duplicateFileNameExceptionMessage));

                        //  The first part can be just about anything but a
                        //  timestamp is a sure way to avoid collisions
                        newFilename = new Date().getTime().toString() + "_" + file.name;
                    }

                    const formData = new FormData();
                    formData.append("file", file, newFilename);

                    const params = { "complaintId": sender.complaintId };
                    const query = Object.keys(params).map(k => `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`).join("&");
                    const uri = "/api/File/Upload?" + query;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            // 'Accept': 'application/json',
                            // 'Content-Type': false
                        },
                        body: formData

                    }).then(response => {
                        switch (response.status) {
                            case 200:
                                options.callback("success", options.files.map(f => {

                                    // We cannot store the file content in local storage because of
                                    // the 5MB storage limit. The problem was with the file size
                                    // e.g. without file content, there is no way to know the file
                                    // size. The work around is to store the file size in the
                                    // 'content' property.

                                    return {
                                        file: new File([f], newFilename, { type: f.type }),
                                        content: f.size.toString()
                                    };
                                }));
                                break;
                            case 400:
                            case 500:
                                if (response.json) {
                                    response.json().then(problem => {
                                        printProblemDetails(problem);
                                    });
                                } else {
                                    alert("oopsy");
                                }
                                break;

                            default:
                                alert("oopsy");
                        }
                    }).catch(error => {
                        console.warn(error);
                    });
                });
        });

    survey
        .onClearFiles
        .add((sender, options) => {
            options.callback("success");
        });

}

//  This is to build a custom file preview container.
export function updateFilePreview(survey: Survey.SurveyModel, question, container): void {

    container.innerHTML = "";

    const title = document.createElement("h3");
    title.innerHTML = getTranslation(question.itemListTitle);
    container.append(title);

    if (question.value && question.value.length > 0) {

        const listView = document.createElement("ol");
        let index = 0;

        (question.value).forEach(fileItem => {

            const item = document.createElement("li");

            const span = document.createElement("span");
            // span.className = "sv_q_file_preview";

            const div = document.createElement("div");

            const button = document.createElement("div");
            button.className = "btn sv-btn sv-file__choose-btn";

            const fileSizeInBytes = fileItem.content || 0;
            let size = 0;

            if (fileSizeInBytes < 1000) {
                button.innerText = fileItem.name + " (" + fileSizeInBytes + " B)";
            } else {
                size = Math.round(fileSizeInBytes / 1000);
                button.innerText = fileItem.name + " (" + size + " KB)";
            }

            const buttonId = `btn_${question.name}_${index}`;
            index = index + 1;
            button.setAttribute("id", buttonId);

            button.onclick = () => {

                fetch("/api/File/Get?complaintId=" + survey.complaintId + "&filename=" + fileItem.name)
                    .then(response => {
                        switch (response.status) {
                            case 400:
                            case 500:
                                if (response.json) {
                                    response.json().then(problem => {
                                        printProblemDetails(problem);
                                    });
                                }
                                return response;

                            default:
                                return response;
                        }
                    })
                    .then(resp => resp.blob())
                    .then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement("a");
                        a.style.display = "none";
                        a.href = url;
                        a.download = fileItem.name;
                        document.body.appendChild(a);
                        a.click();
                        window.URL.revokeObjectURL(url);
                    })
                    .catch(error => {
                        console.warn("Could not upload the file");
                        console.warn(error);
                    });
            };

            div.append(button);

            const buttonRemove = document.createElement("button");
            buttonRemove.setAttribute("type", "button");
            buttonRemove.className = "btn sv_q_file_remove_button";
            buttonRemove.innerText = getTranslation(question.itemListRemoveText);

            if (survey.isDisplayMode === true) {
                buttonRemove.setAttribute("disabled", "disabled");
            }

            buttonRemove.onclick = () => {
                if (confirm(getTranslation(question.confirmRemoveMessage))) {
                    question.removeFile({ name: fileItem.name });
                }
            };

            div.append(buttonRemove);

            span.appendChild(div);
            item.appendChild(span);

            listView.appendChild(item);
        });

        container.append(listView);
    } else {
        const titleElement = document.createElement("p");
        titleElement.innerHTML = getTranslation(question.itemListNoAttachmentsText);
        container.append(titleElement);
    }
}
