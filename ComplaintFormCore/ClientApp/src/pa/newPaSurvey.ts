import { Question, SurveyError, SurveyModel } from "survey-vue";
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

            const questions = [] as Question[];
            const errors = [] as SurveyError[];

            if (!response.ok) {
                const problem = await response.json();

                Object.keys(problem.errors).forEach(q => {
                    // options.errors in only able to set one error per question
                    options.errors[q] = problem.errors[q][0];

                    const question = this.survey.getQuestionByName(q);
                    if (question && question["errors"]) {
                        question.clearErrors();
                        questions.push(question);
                        for (const error of problem.errors[q]) {
                            errors.push(new SurveyError(error, question));
                        }
                    }
                });
            }

            options.complete();

            // TODO: Remove the following lines after updating surveyjs >= v1.8.21 (Bug #2566)
            if (this.survey.onValidatedErrorsOnCurrentPage.isEmpty) {
                return;
            }

            const validationOptions = {
                page: this.survey.currentPage,
                questions: questions,
                errors: errors
            };

            this.survey.onValidatedErrorsOnCurrentPage.fire(sender, validationOptions);
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
            this.storage.remove(this.storageName);

            options.showDataSavingSuccess();
        })();
    }

    private handleOnUploadFiles(sender: SurveyModel, options: any): void {
        void (async () => {
            const questionName: string = options.name;
            const uploadUrl = `/api/File/Upload?complaintId=${this.authToken}&questionName=${questionName}`;
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
                const questionErrors = new Map<Question, SurveyError[]>();

                Object.keys(problem.errors).forEach(q => {
                    const errors = problem.errors[q].map(error => new SurveyError(error, options.question));
                    questionErrors.set(options.question, errors);
                });

                this.displayErrorSummary(questionErrors);
                return;
            }

            const responseData = await response.json();
            options.callback(
                "success",
                options.files.map(file => ({
                    file: file,
                    content: `/api/File/Get?complaintId=${this.authToken}&fileUniqueId=${
                        responseData[file.name].content as string
                    }&filename=${file.name as string}`
                }))
            );
        })();
    }

    private handleOnClearFiles(sender: SurveyModel, options: any): void {
        options.callback("success");
    }
}
