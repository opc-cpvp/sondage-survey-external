import { SurveyModel, PageModel, Question } from "survey-vue";
import { PiaSurveyRisk } from "./piaSurveyRisk";
import { PiaSurveyRiskDefaultValue } from "./piaSurveyRiskDefaultValue";
import { PiaSurveyRiskDefaultValues } from "./piaSurveyRiskDefaultValues";
import { IQuestion } from "survey-core";

export class PiaSurveyRisks {
    readonly stepFourPageName = "page_step_4";
    readonly stepFourTwoPageName = "page_step_4_2";
    readonly previewPageName = "all";
    readonly questionTag = "[TEXT OF QUESTION]";
    readonly responseTag = "[RESPONSE]";
    readonly riskTag = "[DESCRIPTION OF THE RISK]";
    readonly panelIdPrefix = "#";
    readonly panelDescriptionsName = "panelRiskDescriptions";
    readonly panelAssessmentName = "panelRiskAssessment";
    readonly localeEn = "en";
    readonly localeFr = "fr";
    readonly valueYes = "yes";

    public currentList: PiaSurveyRisk[];
    public defaultValues: PiaSurveyRiskDefaultValues;
    private locale: string = this.localeEn;

    public constructor(locale: "en" | "fr" = "en") {
        this.defaultValues = new PiaSurveyRiskDefaultValues();
        this.currentList = [];
        this.locale = locale;
    }

    // Method that processes each question.
    public checkIfRisk(question: Question): void {
        // First remove any existing risk item(s) for this question name.
        this.currentList = this.currentList.filter(r => r.questionName !== question.name);

        // Then check if this question + answer combo exists in the list of "risk" related questions.
        const defaultValue = this.defaultValues.getDefaultValue(question);

        if (defaultValue) {
            // Add new risk item.
            this.currentList.push(new PiaSurveyRisk(question.name, question.title, question.value, this.getDescription(defaultValue)));
        }
    }

    public processSectionFour(survey: SurveyModel): void {
        // Get current page.
        const page: PageModel = survey.currentPage;

        // Process only section 4 pages or a preview page.
        if (page.name !== this.stepFourPageName && page.name !== this.stepFourTwoPageName && page.name !== this.previewPageName) {
            return;
        }

        // Preview page.
        if (page.name === this.previewPageName) {
            this.processPreviewPage(page, this.panelDescriptionsName);
            this.processPreviewPage(page, this.panelAssessmentName);
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

            // Store panel id for reference.
            this.currentList[i].panelId = this.getPanelId(p);

            if (page.name === this.stepFourPageName) {
                this.updateRiskDescriptionTitles(p.questions, this.currentList[i]);
                p.questions[3].defaultValue = this.currentList[i].defaultDescriptionOfRisk;
            } else {
                this.currentList[i].defaultDescriptionOfRisk = this.getUpdatedDescriptionOfRisk(survey, this.currentList[i]);
                this.updateRiskAssessmentTitles(p.questions, this.currentList[i]);
            }
        }
    }

    public getUpdatedPdfQuestionTitle(question: IQuestion): string {
        let retVal: string = (question as any).title;

        // Get the parent panel id.
        const panelId = this.getPanelId(question.parent);

        // Try to find a risk item that matches the current panel Id.
        const risk = this.currentList.filter(r => r.panelId === panelId)[0];

        if (risk) {
            retVal = this.updateTitle(retVal, this.questionTag, risk.questionText);
            retVal = this.updateTitle(retVal, this.responseTag, risk.questionAnswer);
            retVal = this.updateTitle(retVal, this.riskTag, risk.defaultDescriptionOfRisk);
        }

        return retVal;
    }

    private processPreviewPage(page: PageModel, panelName: string): void {
        // Find the root panel.
        const main = page.questions.filter(q => q.name === panelName)[0];
        if (!main) {
            return;
        }

        main.panels.forEach(p => {
            // Get the panel id.
            const panelId = this.getPanelId(p);

            if (panelId !== "") {
                // Try to find a risk item that matches the current panel Id.
                const risk = this.currentList.filter(r => r.panelId === panelId)[0];
                if (risk) {
                    // Update title(s).
                    if (panelName === this.panelDescriptionsName) {
                        this.updateRiskDescriptionTitles(p.questions, risk);
                    } else {
                        this.updateRiskAssessmentTitles(p.questions, risk);
                    }
                }
            }
        });
    }

    private updateRiskDescriptionTitles(questions: any, risk: PiaSurveyRisk): void {
        questions[0].title = this.updateTitle(questions[0].title, this.questionTag, risk.questionText);
        questions[0].title = this.updateTitle(questions[0].title, this.responseTag, risk.questionAnswer);
        questions[1].title = this.updateTitle(questions[1].title, this.riskTag, risk.defaultDescriptionOfRisk);
    }

    private updateRiskAssessmentTitles(questions: any, risk: PiaSurveyRisk): void {
        questions[0].title = this.updateTitle(questions[0].title, this.riskTag, risk.defaultDescriptionOfRisk);
    }

    private updateTitle(title: string, tag: string, tagValue: string): string {
        return title.replace(tag, tagValue);
    }

    private getPanelId(panel: any): string {
        const defaultVal = "" as string;

        if (!panel || !panel.processedTitle) {
            return defaultVal;
        }

        const index = panel.processedTitle.indexOf(this.panelIdPrefix, panel.processedTitle);
        if (index === -1) {
            return defaultVal;
        }

        return panel.processedTitle.substring(Number(index) + 1) as string;
    }

    private getDescription(defaultValue: PiaSurveyRiskDefaultValue): string {
        return this.locale === this.localeFr ? defaultValue.descriptionOfRisk.fr : defaultValue.descriptionOfRisk.en;
    }

    private getUpdatedDescriptionOfRisk(survey: SurveyModel, risk: PiaSurveyRisk): string {
        // Default value - no change.
        let retVal = risk.defaultDescriptionOfRisk;

        // Find "identified risks" page.
        const page: PageModel = survey.pages.filter(p => p.name === this.stepFourPageName)[0];
        if (!page) {
            return retVal;
        }

        // Get the root panel.
        const rootPanel = page.questions[0];
        if (!(rootPanel && rootPanel.panels)) {
            return retVal;
        }

        // Find child panel with a matching panelId.
        const panel = rootPanel.panels.filter(p => this.getPanelId(p) === risk.panelId)[0];
        if (!(panel && panel.questions)) {
            return retVal;
        }

        // If user answered first and second questions as "yes", then get the fourth question answer.
        if (panel.questions[0].value === this.valueYes && panel.questions[1].value === this.valueYes) {
            retVal = panel.questions[3].value;
        }

        return retVal;
    }
}
