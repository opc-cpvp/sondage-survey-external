import { SurveyModel, PageModel, Question, PanelModel } from "survey-vue";
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
    readonly questionEarlier = "panel_risk_desc_q";
    readonly questionHereIs = "panel_risk_desc_q_yes";
    readonly questionEdit = "panel_risk_desc_q_yes_yes";
    readonly questionAssessment = "panel_risk_assessment_4_2_1";

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
        this.currentList = this.currentList.filter(r => r.questionName !== question?.name);

        // Then check if this question + answer combo exists in the list of "risk" related questions.
        const defaultValue = this.defaultValues.getDefaultValue(question);

        if (defaultValue) {
            // Add new risk item.
            this.currentList.push({
                panelId: "",
                questionName: question.name,
                questionText: question.title,
                questionAnswer: question.value,
                defaultDescriptionOfRisk: this.getDescription(defaultValue)
            });
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
        if (!rootPanel?.panels) {
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
                p.questions.find(q => q.name === this.questionEdit).defaultValue = this.currentList[i].defaultDescriptionOfRisk;
            } else {
                this.currentList[i].defaultDescriptionOfRisk = this.getUpdatedDescriptionOfRisk(survey, this.currentList[i]);
                this.updateRiskAssessmentTitles(p.questions, this.currentList[i]);
            }
        }
    }

    public getUpdatedPdfQuestionTitle(question: IQuestion): string {
        let retVal: string = (question as any)?.title;

        // Get the parent panel id.
        const panelId = this.getPanelId(question.parent as any);

        // Try to find a risk item that matches the current panel Id.
        const risk = this.currentList.find(r => r.panelId === panelId);

        if (risk) {
            retVal = this.updateTitle(retVal, this.questionTag, risk.questionText);
            retVal = this.updateTitle(retVal, this.responseTag, risk.questionAnswer);
            retVal = this.updateTitle(retVal, this.riskTag, risk.defaultDescriptionOfRisk);
        }

        return retVal;
    }

    private processPreviewPage(page: PageModel, panelName: string): void {
        // Find the root panel.
        const main = page.questions.find(q => q.name === panelName);
        if (!main?.panels) {
            return;
        }

        main.panels.forEach(p => {
            // Get the panel id.
            const panelId = this.getPanelId(p);

            if (panelId !== "") {
                // Try to find a risk item that matches the current panel Id.
                const risk = this.currentList.find(r => r.panelId === panelId);
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
        this.updateQuestionTitle(
            questions.find(q => q.name === this.questionEarlier),
            this.questionTag,
            risk.questionText
        );
        this.updateQuestionTitle(
            questions.find(q => q.name === this.questionEarlier),
            this.responseTag,
            risk.questionAnswer
        );
        this.updateQuestionTitle(
            questions.find(q => q.name === this.questionHereIs),
            this.riskTag,
            risk.defaultDescriptionOfRisk
        );
    }

    private updateRiskAssessmentTitles(questions: any, risk: PiaSurveyRisk): void {
        this.updateQuestionTitle(
            questions.find(q => q.name === this.questionAssessment),
            this.riskTag,
            risk.defaultDescriptionOfRisk
        );
    }

    private updateQuestionTitle(question: any, tag: string, tagValue: string): void {
        if (question) {
            question.title = this.updateTitle(question.title, tag, tagValue);
        }
    }

    private updateTitle(title: string, tag: string, tagValue: string): string {
        return title?.replace(tag, tagValue);
    }

    private getPanelId(panel: PanelModel): string {
        const defaultVal = "";

        if (!panel?.processedTitle) {
            return defaultVal;
        }

        const index = panel.processedTitle.indexOf(this.panelIdPrefix);
        if (index === -1) {
            return defaultVal;
        }

        return panel.processedTitle.substring(Number(index) + 1);
    }

    private getDescription(defaultValue: PiaSurveyRiskDefaultValue): string {
        return this.locale === this.localeFr ? defaultValue?.descriptionOfRisk?.fr : defaultValue?.descriptionOfRisk?.en;
    }

    private getUpdatedDescriptionOfRisk(survey: SurveyModel, risk: PiaSurveyRisk): string {
        // Default value - no change.
        let retVal = risk.defaultDescriptionOfRisk;

        // Find "identified risks" page.
        const page: PageModel = survey.pages.find(p => p.name === this.stepFourPageName);
        if (!page) {
            return retVal;
        }

        // Get the root panel.
        const rootPanel = page.questions.find(q => q.name === this.panelDescriptionsName);
        if (!rootPanel?.panels) {
            return retVal;
        }

        // Find child panel with a matching panelId.
        const panel = rootPanel.panels.find(p => this.getPanelId(p) === risk.panelId);
        if (!panel?.questions) {
            return retVal;
        }

        // If user answered first and second questions as "yes", then get the fourth question answer.
        const earlierValue = panel.questions.find(q => q.name === this.questionEarlier)?.value;
        const hereIsValue = panel.questions.find(q => q.name === this.questionHereIs)?.value;
        if (earlierValue === this.valueYes && hereIsValue === this.valueYes) {
            retVal = panel.questions.find(q => q.name === this.questionEdit)?.value ?? retVal;
        }

        return retVal;
    }
}
