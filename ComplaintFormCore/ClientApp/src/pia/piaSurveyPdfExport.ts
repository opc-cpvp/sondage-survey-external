import { surveyPdfExport } from "../surveyPDF";
import { PiaSurvey } from "./piaSurvey";

export class PiaSurveyPdfExport extends surveyPdfExport {
    readonly riskDescription = "panel_risk_desc_q";
    readonly riskDescriptionYes = "panel_risk_desc_q_yes";
    readonly riskAssessment = "panel_risk_assessment_4_2_1";

    private piaSurvey: PiaSurvey;

    constructor(piaSurvey: PiaSurvey) {
        super();
        this.piaSurvey = piaSurvey;
    }

    protected doAdvancedProcessing(): void {
        this.survey_pdf.onRenderQuestion.add((survey, options) => {
            const qName = options?.question?.name;

            if (qName !== this.riskDescription && qName !== this.riskDescriptionYes && qName !== this.riskAssessment) {
                return;
            }

            (options.question as any).title = this.piaSurvey?.risks?.getUpdatedPdfQuestionTitle(options.question);

            // Create new question
            const flatRadiogroup = options.repository.create(this.survey_pdf, options.question as any, options.controller, "radiogroup");
            for (let i = options.bricks.length - 1; i >= 0; i--) {
                options.bricks.pop();
            }
            // Re-create bricks.
            return new Promise<void>(resolve => {
                flatRadiogroup
                    .generateFlats(options.point)
                    .then(radioBricks => {
                        options.bricks = options.bricks.concat(radioBricks);
                        resolve();
                    })
                    .catch(error => {
                        console.warn(error);
                    });
            });
        });
    }
}
