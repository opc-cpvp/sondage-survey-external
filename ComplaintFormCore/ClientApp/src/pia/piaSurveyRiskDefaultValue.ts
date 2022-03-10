import { PiaSurveyDescriptionOfRisk } from "./piaSurveyDescriptionOfRisk";

export class PiaSurveyRiskDefaultValue {
    public questionName!: string;
    public questionAnswer!: unknown;
    public descriptionOfRisk!: PiaSurveyDescriptionOfRisk;

    public constructor(questionName: string, questionAnswer: unknown, descriptionOfRisk: PiaSurveyDescriptionOfRisk) {
        this.questionName = questionName;
        this.questionAnswer = questionAnswer;
        this.descriptionOfRisk = descriptionOfRisk;
    }
}
