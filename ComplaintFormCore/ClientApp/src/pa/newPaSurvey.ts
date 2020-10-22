import { SurveyModel } from "survey-vue";
import { SurveyBase } from "../survey";
import { PaSurveyComplete } from "./paSurveyComplete";

export class NewPaSurvey extends SurveyBase {
    private authToken: string;

    public constructor(locale: "fr" | "en" = "en", authToken: string) {
        super(locale);
        this.authToken = authToken;

        this.registerCallbacks();
    }

    private registerCallbacks(): void {
        this.survey.onCompleting.add((sender: SurveyModel, options: any) => {
            this.handleOnCompleting(sender, options);
        });

        this.survey.onComplete.add((sender: SurveyModel, options: any) => {
            this.handleOnComplete(sender, options);
        });

        this.survey.onValidateQuestion.add((sender: SurveyModel, options: any) => {
            this.handleOnValidateQuestion(sender, options);
        });
    }

    private handleOnCompleting(sender: SurveyModel, options: any): void {
        const validationUrl = `/api/PASurvey/Validate?complaintId="${this.authToken}`;

        // prevent the survey from completing as we'll be handling the validation manually
        options.allowComplete = false;

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
                // printProblemDetails(problem, sender.locale);
                return;
            }

            this.survey.doComplete();
        })();
    }

    private handleOnComplete(sender: SurveyModel, options: any): void {
        void (async () => {
            const completeUrl = `/api/PASurvey/Complete?complaintId="${this.authToken}`;

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
                // TODO: Replace this with something more generic
                // printProblemDetails(problem, sender.locale);
                return;
            }

            const responseData = await response.json();
            const event = new PaSurveyComplete(responseData.referenceNumber);

            dispatchEvent(event);

            /*
            const sp_survey_file_number = document.getElementById("sp_survey_file_number");
            if (sp_survey_file_number) {
                sp_survey_file_number.innerText = responseData.referenceNumber;
            }

            saveStateLocally(_survey, storageName_PA);
            */
        })();
    }

    private handleOnValidateQuestion(sender: SurveyModel, options: any): void {}
}
