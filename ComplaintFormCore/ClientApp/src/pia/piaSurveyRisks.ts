import { PageModel, Question } from "survey-vue";
import { PiaSurveyRisk } from "./piaSurveyRisk";
import { PiaSurveyRiskDefaultValues } from "./piaSurveyRiskDefaultValues";
import { AdornersOptions, CompositeBrick, HTMLBrick } from "survey-pdf";

export class PiaSurveyRisks {
    readonly stepFourPageName = "page_step_4";
    readonly stepFourTwoPageName = "page_step_4_2";
    readonly questionTag = "[TEXT OF QUESTION]";
    readonly responseTag = "[RESPONSE]";
    readonly riskTag = "[DESCRIPTION OF THE RISK]";
    readonly panelIdPrefix = "#";
    readonly panelDescriptionsName = "panelRiskDescriptions";
    readonly panelAssessmentName = "panelRiskAssessment";

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

    public processSectionFour(page: PageModel): void {
        // Process only 2 section 4 pages or a preview page.
        if (page.name !== this.stepFourPageName && page.name !== this.stepFourTwoPageName && page.name !== "all") {
            return;
        }

        // Preview page.
        if (page.name === "all") {
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
                this.updateRiskAssessmentTitles(p.questions, this.currentList[i]);
            }
        }
    }

    public processPdfQuestion(options: AdornersOptions): void {
        if (options.question.name !== this.panelDescriptionsName && options.question.name !== this.panelAssessmentName) {
            return;
        }

        const compositeBricks = options.bricks.filter(b => b instanceof CompositeBrick);
        compositeBricks.forEach(c => {
            const unfoldedComposites = c.unfold();
            if (unfoldedComposites) {
                // Process HTMLBrick-s.
                const htmlBricks = unfoldedComposites.filter(b => b instanceof HTMLBrick);
                htmlBricks.forEach(h => {
                    const unfoldedHtml = (h as HTMLBrick).unfold()[0] as any;
                    if (unfoldedHtml) {
                        unfoldedHtml.html = this.updateTitle(unfoldedHtml.html, this.questionTag, "Nenad Testing - Text of Question");
                        unfoldedHtml.html = this.updateTitle(unfoldedHtml.html, this.responseTag, "Nenad Testing - Response");
                        unfoldedHtml.html = this.updateTitle(unfoldedHtml.html, this.riskTag, "Nenad Testing - Description of Risk");
                    }
                });

                // TODO: - use values from this.currentList above - how to get the panelId for PDF for matching???
                // Can type: multipletext have more than one line of text?
                // multipletext type not showing on the PDF - why?
            }
        });
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
}
