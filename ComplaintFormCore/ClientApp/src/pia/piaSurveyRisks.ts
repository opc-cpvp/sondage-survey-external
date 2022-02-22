import { Question, SurveyModel } from "survey-vue";
import { PiaSurveyRisk } from "./piaSurveyRisk";
import { PiaSurveyRiskDefaultValues } from "./piaSurveyRiskDefaultValues";

export class PiaSurveyRisks {
    public currentList: PiaSurveyRisk[];
    public defaultValues: PiaSurveyRiskDefaultValues;

    public constructor() {
        this.defaultValues = new PiaSurveyRiskDefaultValues();
        this.currentList = [];
    }

    // Method that processes each question.
    public checkIfRisk(question: Question): void {
        // First remove any existing risk item(s) for this question name.
        this.currentList = this.currentList.filter(r => r.questionName !== question.name);

        // Then check if this question + answer combo exists in the list of "risk" related questions.
        const defaultValue = this.defaultValues.getDefaultValue(question);

        if (defaultValue) {
            // Add new risk item.
            this.currentList.push(new PiaSurveyRisk(question.name, question.title, question.value, defaultValue.descriptionOfRisk));
        }
    }

    public processSectionFour(survey: SurveyModel): void {
        if (!(survey.currentPage || survey.currentPage.name === "page_step_4" || survey.currentPage.name === "page_step_4_2")) {
            return;
        }

        // Get the root panel control.
        const rootPanel = survey.currentPage.questions[0];
        if (!(rootPanel && rootPanel.panels)) {
            return;
        }

        // Set the panel count using the risks collection.
        rootPanel.maxPanelCount = rootPanel.mixPanelCount = rootPanel.panelCount = this.currentList.length;

        for (let i = 0; i < rootPanel.panels.length; i++) {
            const p = rootPanel.panels[i];

            if (survey.currentPage.name === "page_step_4") {
                // Update panel properties.
                p.name = "risk_" + this.currentList[i].questionName;
                // Update relevant question info.
                p.questions[0].title = p.questions[0].title.replace("[TEXT OF QUESTION]", this.currentList[i].questionText);
                p.questions[0].title = p.questions[0].title.replace("[RESPONSE]", this.currentList[i].questionAnswer);
                p.questions[1].title = p.questions[1].title.replace(
                    "[DESCRIPTION OF THE RISK]",
                    this.currentList[i].defaultDescriptionOfRisk
                );
                p.questions[3].defaultValue = p.questions[3].defaultValue = this.currentList[i].defaultDescriptionOfRisk;
            } else {
                // Update panel properties.
                p.name = "riskAssessment_" + this.currentList[i].questionName;
                // Update relevant question info.
                p.questions[0].title = p.questions[0].title.replace(
                    "[DESCRIPTION OF THE RISK]",
                    this.currentList[i].defaultDescriptionOfRisk
                );
            }
        }
    }
}
