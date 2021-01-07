import {
    Question,
    SurveyError,
    SurveyModel,
    QuestionDropdownModel,
    ItemValue,
    QuestionPanelDynamicModel,
    JsonObject,
    QuestionSelectBase
} from "survey-vue";
import { SurveyBase } from "../survey";
import { FileMeterWidget } from "../widgets/filemeterwidget";

export class NewPiaToolSurvey extends SurveyBase {
    private authToken: string;

    public constructor(locale: "en" | "fr" = "en", authToken: string, storageName: string) {
        super(locale, storageName);
        this.authToken = authToken;

        // Since our completed page relies on a variable, we'll hide it until the variable is set.
        this.survey.showCompletedPage = false;

        //  this.setNavigationBreadcrumbs(this.survey);
    }

    public gotoSection(section: string): void {
        //  This is part of the navigation. When a user clicks on a breadcrums item it takes them
        //  directly where they want.

        //  Filter to get the first page with the desired section
        const page = this.survey.pages.filter(x => x.getPropertyValue("section") === section && x.isVisible === true)[0];
        if (page) {
            this.survey.currentPage = page;
            this.setNavigationBreadcrumbs(this.survey);
        }
    }

    public gotoPage(pageName: string): void {
        const page = this.survey.getPageByName(pageName);
        if (page) {
            this.survey.currentPage = page;
        }
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

        this.survey.onUploadFiles.add((sender: SurveyModel, options: any) => {
            this.handleOnUploadFiles(sender, options);
        });

        this.survey.onClearFiles.add((sender: SurveyModel, options: any) => {
            this.handleOnClearFiles(sender, options);
        });

        this.survey.onAfterRenderQuestion.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderQuestion(sender, options);
        });

        this.survey.onAfterRenderSurvey.add((sender: SurveyModel, options: any) => {
            this.setNavigationBreadcrumbs(sender);
        });

        this.survey.onCurrentPageChanged.add((sender: SurveyModel, options: any) => {
            this.setNavigationBreadcrumbs(sender);
        });
    }

    protected registerCustomProperties(): void {
        super.registerCustomProperties();

        JsonObject.metaData.addProperty("page", {
            name: "section:text",
            default: false
        });
    }

    private setNavigationBreadcrumbs(surveyObj: SurveyModel): void {
        //  TODO: Probably disable some items when the user has not gone thru all the question.

        const ul_progress_navigation = document.getElementById("ul_pia_navigation") as HTMLUListElement;

        if (!ul_progress_navigation) {
            return;
        }

        if (surveyObj.isDisplayMode) {
            //  We do not show the navigation bar in preview mode
            ul_progress_navigation.className = "hidden";
            return;
        }

        ul_progress_navigation.className = "breadcrumb";

        if (surveyObj.currentPage.section === null) {
            //  If for any reasons we forget to add the section property in the json at least nothing will happen
            return;
        }

        const items = document.getElementsByClassName("breadcrumb-item");

        Array.from(items).forEach(li => {
            //  Reset the original class on each <li> item
            li.className = "breadcrumb-item";
        });

        const section: string = surveyObj.currentPage.section;
        const li_breadcrumb = document.getElementById(`li_breadcrumb_${section}`);
        if (li_breadcrumb) {
            li_breadcrumb.className += " active";
        }
    }

    private handleOnAfterRenderQuestion(sender: SurveyModel, options: any): void {
        const question = options.question as Question;

        // Generate the list of contact persons based on previous answers
        if (question.name === "PersonContact") {
            const personContactQuestion = question as QuestionSelectBase;
            const anotherIndividual = this.survey.locale === "fr" ? "Autre individu" : "Another individual";
            const contacts: ItemValue[] = [new ItemValue("another", anotherIndividual)];

            const contactQuestions = [
                "HeadYourInstitutionFullname", // Question 2.1.5 - Who is the head of the government institution
                "pnd_OtherInstitutionHead", // Question 2.1.6 - Head of the government institution or delegate
                "SeniorOfficialFullname", // Question 2.1.7 - Senior official or executive responsible
                "panle_senior_officials_others" // Question 2.1.8 - Senior official or executive responsible
            ];
            contactQuestions.forEach(q => {
                const contactQuestion = this.survey.getQuestionByName(q);
                if (contactQuestion === null) {
                    return;
                }

                // Search for contact fields in the dynamic panels
                if (contactQuestion instanceof QuestionPanelDynamicModel) {
                    const items = contactQuestion.value as any[];
                    const fields = ["OtherInstitutionHeadFullname", "SeniorOfficialOtherFullname"];
                    items.forEach(item => {
                        fields.forEach(f => {
                            const contact = item[f];
                            if (contact === null || contacts.some(c => c.value === contact)) {
                                return;
                            }
                            contacts.push(new ItemValue(contact, contact));
                        });
                    });
                } else {
                    contacts.push(new ItemValue(contactQuestion.value, contactQuestion.value));
                }
            });

            personContactQuestion.choices = contacts;
        } else if (question.name === "pnd_PurposeOfNotAllDisclosed") {
            //  For this question, we need to populate the dropdown named 'ReceivingParties' inside the
            //  paneldynamic 'pnd_PurposeOfNotAllDisclosed' with what the user has selected in a previous question.

            const purposeOfNotAllDisclosed = question as QuestionPanelDynamicModel;
            const receivingParties = purposeOfNotAllDisclosed.templateElements.find(
                r => r.name === "ReceivingParties"
            ) as QuestionDropdownModel;
            if (receivingParties === undefined) {
                return;
            }

            const otherPartiesSharePersonalInformation = this.survey.getQuestionByName(
                "OtherPartiesSharePersonalInformation"
            ) as QuestionPanelDynamicModel;
            if (otherPartiesSharePersonalInformation === null) {
                return;
            }

            const parties: ItemValue[] = [];
            const items = otherPartiesSharePersonalInformation.value as any[];
            items.forEach(item => {
                const party = item.Party;
                if (party === null) {
                    return;
                }
                parties.push(new ItemValue(party, party));
            });

            receivingParties.choices = parties;
        }
    }

    private handleOnServerValidateQuestions(sender: SurveyModel, options: any): void {
        if (!this.survey.isLastPage) {
            options.complete();
            return;
        }

        const validationUrl = `/api/PIASurvey/Validate?complaintId=${this.authToken}`;

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
            const completeUrl = `/api/PIASurvey/Complete?complaintId=${this.authToken}`;

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
