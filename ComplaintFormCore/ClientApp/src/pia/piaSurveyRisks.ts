import { PageModel, Question, SurveyModel } from "survey-vue";
import { PiaSurveyRisk } from "./piaSurveyRisk";
import { PiaSurveyRiskDefaultValues } from "./piaSurveyRiskDefaultValues";

export class PiaSurveyRisks {
    readonly stepFourPageName = "page_step_4";
    readonly stepFourTwoPageName = "page_step_4_2";
    readonly questionTag = "[TEXT OF QUESTION]";
    readonly responseTag = "[RESPONSE]";
    readonly riskTag = "[DESCRIPTION OF THE RISK]";
    readonly riskPrefix = "risk_";
    readonly assessmentPrefix = "riskAssessment_";

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

    public processSectionFourPreview(survey: SurveyModel): void {
        survey.pages.forEach(p => {
            this.processSectionFour(p);
        });
    }

    public processSectionFour(page: PageModel): void {
        if (page.name !== this.stepFourPageName && page.name !== this.stepFourTwoPageName) {
            return;
        }

        // Get the root panel control.
        const rootPanel = page.questions[0];
        if (!(rootPanel && rootPanel.panels)) {
            return;
        }

        // Set the panel count using the risks collection.
        rootPanel.maxPanelCount = rootPanel.mixPanelCount = rootPanel.panelCount = this.currentList.length;

        for (let i = 0; i < rootPanel.panels.length; i++) {
            const p = rootPanel.panels[i];

            if (page.name === this.stepFourPageName) {
                // Update panel properties.
                p.name = this.riskPrefix + this.currentList[i].questionName;
                // Update relevant question info.
                p.questions[0].title = p.questions[0].title.replace(this.questionTag, this.currentList[i].questionText);
                p.questions[0].title = p.questions[0].title.replace(this.responseTag, this.currentList[i].questionAnswer);
                p.questions[1].title = p.questions[1].title.replace(this.riskTag, this.currentList[i].defaultDescriptionOfRisk);
                p.questions[3].defaultValue = p.questions[3].defaultValue = this.currentList[i].defaultDescriptionOfRisk;
            } else {
                // Update panel properties.
                p.name = this.assessmentPrefix + this.currentList[i].questionName;
                // Update relevant question info.
                p.questions[0].title = p.questions[0].title.replace(this.riskTag, this.currentList[i].defaultDescriptionOfRisk);
            }
        }
    }
}
