import { SurveyModel } from "survey-vue";
import { SurveyBase } from "../survey";
import { CheckboxWidget } from "../widgets/checkboxwidget";
import { FileMeterWidget } from "../widgets/filemeterwidget";

export class NewPaSurvey extends SurveyBase {
    private authToken: string;

    public constructor(locale: "en" | "fr" = "en", authToken: string, storageName: string) {
        super(locale, storageName);
        this.authToken = authToken;

        // Since our completed page relies on a variable, we'll hide it until the variable is set.
        this.survey.showCompletedPage = false;
    }

    protected registerWidgets(): void {
        CheckboxWidget.register();
        FileMeterWidget.register();
    }

    protected registerEventHandlers(): void {
        super.registerEventHandlers();

        this.survey.onServerValidateQuestions.add((sender: SurveyModel, options: any) => {
            this.handleOnServerValidateQuestions(sender, options);
        });

        this.survey.onComplete.add((sender: SurveyModel, options: any) => {
            this.handleOnComplete(sender, options);
        });

        this.survey.onUploadFiles.add((sender: SurveyModel, options: any) => {
            this.handleOnUploadFiles(sender, options);
        });

        this.survey.onClearFiles.add((sender: SurveyModel, options: any) => {
            this.handleOnClearFiles(sender, options);
        });

        this.survey.onAfterRenderQuestion.add((sender, options) => {
            if (options.question.getType() === "html" && options.question.name === "documentation_info") {
                this.updateDocumentationInfoSection(sender);
            }
        });
    }

    private handleOnServerValidateQuestions(sender: SurveyModel, options: any): void {
        if (!this.survey.isLastPage) {
            options.complete();
            return;
        }

        const validationUrl = `/api/PASurvey/Validate?complaintId=${this.authToken}`;

        void (async () => {
            // Validate the survey results
            const response = await fetch(validationUrl, {
                method: "POST",
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(sender.data)
            });

            if (!response.ok) {
                const problem = await response.json();
                // TODO: Replace this with something more generic
                // this.displayErrorSummary(questionErrors);
                return;
            }

            options.complete();
        })();
    }

    private handleOnComplete(sender: SurveyModel, options: any): void {
        void (async () => {
            const completeUrl = `/api/PASurvey/Complete?complaintId=${this.authToken}`;

            options.showDataSaving();

            // Complete the survey
            const response = await fetch(completeUrl, {
                method: "POST",
                headers: {
                    Accept: "application/json",
                    "Content-Type": "application/json; charset=utf-8"
                },
                body: JSON.stringify(sender.data)
            });

            if (!response.ok) {
                const problem = await response.json();
                options.showDataSavingError();
                return;
            }

            const responseData = await response.json();
            this.survey.setVariable("referenceNumber", responseData.referenceNumber);

            // Now that the variable is set, show the completed page.
            this.survey.showCompletedPage = true;

            options.showDataSavingSuccess();
        })();
    }

    private handleOnUploadFiles(sender: SurveyModel, options: any): void {
        void (async () => {
            const uploadUrl = `/api/File/Upload?complaintId=${this.authToken}`;
            const formData = new FormData();

            options.files.forEach(file => {
                formData.append(file.name, file);
            });

            // Complete the survey
            const response = await fetch(uploadUrl, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const problem = await response.json();
                // TODO: Replace this with something more generic
                // this.displayErrorSummary(questionErrors);
                return;
            }

            const responseData = await response.json();
            options.callback(
                "success",
                options.files.map(file => ({
                    file: file,
                    content: responseData[file.name].content,
                    size: responseData[file.name].size
                }))
            );
        })();
    }

    private handleOnClearFiles(sender: SurveyModel, options: any): void {
        options.callback("success");
    }

    // This function is to update the html for the question of type html named 'documentation_info'.
    // It will removed the hidden css on some of the <li> depending on some conditions
    private updateDocumentationInfoSection(surveyObj) {
        const ul_documentation_info = document.getElementById("ul_documentation_info");
        if (ul_documentation_info == null) {
            return;
        }

        if (surveyObj.data["RaisedPrivacyToAtipCoordinator"] === "yes") {
            const liNode = ul_documentation_info.querySelector(".raisedPrivacyToAtipCoordinator");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (surveyObj.data["FilingComplaintOnOwnBehalf"] === "someone_else") {
            const liNode = ul_documentation_info.querySelector(".filingComplaintOnOwnBehalf");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (surveyObj.data["NatureOfComplaint"].filter(x => x === "NatureOfComplaintDenialOfAccess").length > 0) {
            const liNode = ul_documentation_info.querySelector(".natureOfComplaint");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }
    }
}
