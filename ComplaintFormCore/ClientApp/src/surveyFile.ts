import * as Survey from "survey-vue";
import { getTranslation, printProblemDetails } from "./surveyHelper";

export function initSurveyFile(): void {
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

export function initSurveyFileModelEvents(survey: Survey.SurveyModel): void {
    survey.onAfterRenderQuestion.add((sender, options) => {
        if (options.question.getType() === "file") {
            //  This is to build the file preview, we're not using the native one

            //  First we create a div container to hold all of the attachments
            const container = document.createElement("div");
            container.className = "my-preview-container";

            //  Then we find the root div for the file input
            let rootDivElement = options.htmlElement.getElementsByClassName("sv_q_file")[0] as HTMLDivElement;

            if (!rootDivElement) {
                //  TODO: figure out why this line of code ???
                rootDivElement = options.htmlElement.getElementsByClassName("sv-file__decorator")[0] as HTMLDivElement;
            }

            //  Then we add the file list container to the root div
            rootDivElement.appendChild(container);

            options.question.onPropertyChanged.add((question, opt) => {
                // Everytime a file gets uploaded or removed we are redrawing the file preview container.
                if (opt.name === "value") {
                    updateFilePreview(sender, question, container);
                }
            });

            updateFilePreview(sender, options.question, container);
        }
    });

    survey.onUploadFiles.add((sender, options) => {
        options.files.forEach((file: File) => {
            let newFilename = file.name;

            if (options.question.value && options.question.value.some(e => e.name === file.name)) {
                // Checking for files with the same name. We don't want that because when we
                // 'removed' a file, all files with the same name are being deleted. This
                // could be solved if the file property "storeDataAsText" was set to false
                // and files were uploaded into a server. But for now, we store the files in
                // the local storage. If a duplicate is found we are just adding a timstamp.
                // It is way easier then setting up errors on the question and asking the
                // user to remove one of the file.
                alert(getTranslation(options.question.duplicateFileNameExceptionMessage, sender.locale));

                //  The first part can be just about anything but a
                //  timestamp is a sure way to avoid collisions
                const fileNamePrefix = new Date().getTime().toString();
                newFilename = `${fileNamePrefix}_${file.name}`;
            }

            const formData = new FormData();
            formData.append("file", file, newFilename);

            const params = { complaintId: sender.complaintId };
            const query = Object.keys(params)
                .map(k => `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`)
                .join("&");
            const uri = `/api/File/Upload?${query}`;

            fetch(uri, {
                method: "POST",
                headers: {
                    // 'Accept': 'application/json',
                    // 'Content-Type': false
                },
                body: formData
            })
                .then(response => {
                    if (response.ok) {
                        options.callback(
                            "success",
                            options.files.map(f =>
                                // We cannot store the file content in local storage because of
                                // the 5MB storage limit. The problem was with the file size
                                // e.g. without file content, there is no way to know the file
                                // size. The work around is to store the file size in the
                                // 'content' property.

                                ({
                                    file: new File([f], newFilename, {
                                        type: f.type
                                    }),
                                    content: f.size.toString()
                                })
                            )
                        );
                    } else {
                        if (response.json) {
                            response
                                .json()
                                .then(problem => {
                                    printProblemDetails(problem, sender.locale);
                                })
                                .catch(error => {
                                    console.warn(error);
                                });
                        } else {
                            console.warn(response);
                        }
                    }
                })
                .catch(error => {
                    console.warn(error);
                });
        });
    });

    survey.onClearFiles.add((sender, options) => {
        options.callback("success");
    });
}

//  This is to build a custom file preview container.
export function updateFilePreview(survey: Survey.SurveyModel, question: Survey.QuestionFileModel, container: HTMLDivElement): void {
    container.innerHTML = "";

    if (question.itemListTitle && question.itemListTitle.length > 0) {
        const title = document.createElement("h3");
        title.innerHTML = getTranslation(question.itemListTitle, survey.locale);
        container.appendChild(title);
    }

    if (question.value && question.value.length > 0) {
        const listView = document.createElement("ol");
        let index = 0;

        question.value.forEach((fileItem: Survey.Question) => {
            const item = document.createElement("li");
            const div = document.createElement("div");

            const button = document.createElement("div");
            button.className = "btn sv-btn sv-file__choose-btn";

            const fileSizeInBytes = (fileItem.content as number) || 0;
            let size = 0;

            if (fileSizeInBytes < 1000) {
                button.innerText = `${fileItem.name} (${fileSizeInBytes} B)`;
            } else {
                size = Math.round(fileSizeInBytes / 1000);
                button.innerText = `${fileItem.name} (${size} KB)`;
            }

            const buttonId = `btn_${question.name}_${index}`;
            index = index + 1;
            button.setAttribute("id", buttonId);

            button.onclick = () => {
                fetch(`/api/File/Get?complaintId=${survey.complaintId as string}&filename=${fileItem.name}`)
                    .then(response => {
                        if (!response.ok) {
                            if (response.json) {
                                response
                                    .json()
                                    .then(problem => {
                                        printProblemDetails(problem, survey.locale);
                                    })
                                    .catch(error => {
                                        console.warn(error);
                                    });
                            }
                        }

                        return response;
                    })
                    .then(resp => resp.blob())
                    .then(blob => {
                        const url = window.URL.createObjectURL(blob);
                        const a = document.createElement("a");
                        // a.style.display = "none";
                        a.classList.add("hidden");
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

            div.appendChild(button);

            if (!survey.isDisplayMode) {
                //  If in 'Preview' mode we are not showing the remove buttons

                const buttonRemove = document.createElement("button");
                buttonRemove.setAttribute("type", "button");
                buttonRemove.className = "btn sv_q_file_remove_button";

                if (question.itemListRemoveText && question.itemListRemoveText.length > 0) {
                    buttonRemove.innerText = getTranslation(question.itemListRemoveText, survey.locale);
                } else {
                    buttonRemove.innerText = Survey.surveyLocalization.locales[survey.locale].removeRow;
                }

                buttonRemove.onclick = () => {
                    if (confirm(getTranslation(question.confirmRemoveMessage, survey.locale))) {
                        question.removeFile({ name: fileItem.name });
                    }
                };

                div.appendChild(buttonRemove);
            }

            item.appendChild(div);

            listView.appendChild(item);
        });

        container.appendChild(listView);
    } else {
        const titleElement = document.createElement("p");
        titleElement.innerHTML = getTranslation(question.itemListNoAttachmentsText, survey.locale);
        container.appendChild(titleElement);
    }
}
