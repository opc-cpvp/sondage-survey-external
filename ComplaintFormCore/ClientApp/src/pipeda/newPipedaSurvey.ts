import { SurveyModel } from "survey-vue";
import { SurveyBase } from "../survey";
import { QuestionRadiogroupModel, Question, SurveyError } from "../../node_modules/survey-vue/survey.vue";
import { PipedaProvincesData } from "./pipedaProvinceData";
import { Province } from "../surveyHelper";
import { FileMeterWidget } from "../widgets/filemeterwidget";

export class NewPipedaSurvey extends SurveyBase {
    private authToken: string;

    public constructor(locale: "en" | "fr" = "en", authToken: string, storageName: string) {
        super(locale, storageName);
        this.authToken = authToken;

        // Since our completed page relies on a variable, we'll hide it until the variable is set.
        this.survey.showCompletedPage = false;
    }

    protected registerWidgets(): void {
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

        this.survey.onAfterRenderPage.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderPagePipeda(sender, options);
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

        const validationUrl = `/api/PipedaSurvey/Validate?complaintId=${this.authToken}`;

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
            const completeUrl = `/api/PipedaSurvey/Complete?complaintId=${this.authToken}`;

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

    private handleOnAfterRenderPagePipeda(sender: SurveyModel, options: any): void {
        const pagesRequiringProvinceTranslations = [
            "page_part_a_jurisdiction_unable_1",
            "page_part_a_jurisdiction_particulars",
            "page_part_a_customer_or_employee",
            "page_part_a_jurisdiction_unable_2"
        ];

        if (pagesRequiringProvinceTranslations.some(p => p === options.page.name)) {
            //  Set the french province prefixes for those pages.

            const selectedProvinceQuestion = sender.getQuestionByName("ProvinceIncidence") as QuestionRadiogroupModel;
            if (selectedProvinceQuestion.value) {
                const selectedProvinceId = Number(selectedProvinceQuestion.value);

                //  en, au, à...
                sender.setVariable("province_incidence_prefix_au", PipedaProvincesData[selectedProvinceId].French.FrenchPrefix_Au);

                //  de, du, de la...
                sender.setVariable("province_incidence_prefix_du", PipedaProvincesData[selectedProvinceId].French.FrenchPrefix_Du);
            }
        }

        if (options.page.name === "page_part_a_jurisdiction_unable_1") {
            const selectedProvinceQuestion = sender.getQuestionByName("ProvinceIncidence") as QuestionRadiogroupModel;

            if (selectedProvinceQuestion.value) {
                //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                const selectedProvinceId = selectedProvinceQuestion.value as number;

                if (sender.locale === "fr") {
                    sender.setVariable("province_link", PipedaProvincesData[selectedProvinceId].French.Province_link);
                } else {
                    sender.setVariable("province_link", PipedaProvincesData[selectedProvinceId].English.Province_link);
                }
            }
        } else if (options.page.name === "page_part_a_jurisdiction_unable_2") {
            const selectedProvinceQuestion = sender.getQuestionByName("ProvinceIncidence") as QuestionRadiogroupModel;

            if (selectedProvinceQuestion.value) {
                //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                const selectedProvinceId = selectedProvinceQuestion.value as number;

                if (sender.locale === "fr") {
                    sender.setVariable("link_province_opc", PipedaProvincesData[selectedProvinceId].French.Link_province_opc);
                    sender.setVariable("link_more_info", PipedaProvincesData[selectedProvinceId].French.Link_province_opc);
                } else {
                    sender.setVariable("link_province_opc", PipedaProvincesData[selectedProvinceId].English.Link_province_opc);
                    sender.setVariable("link_more_info", PipedaProvincesData[selectedProvinceId].French.Link_more_info);
                }
            }
        }
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
                    }&filename=${file.name as string}`,
                    size: responseData[file.name].size
                }))
            );
        })();
    }

    private handleOnClearFiles(sender: SurveyModel, options: any): void {
        options.callback("success");
    }
}
