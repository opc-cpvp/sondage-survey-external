export class PiaSurveyRiskDefaultValue {
    public questionName!: string;
    public questionAnswer!: unknown;
    public descriptionOfRisk!: string;

    public constructor(questionName: string, questionAnswer: unknown, descriptionOfRisk: string) {
        this.questionName = questionName;
        this.questionAnswer = questionAnswer;
        this.descriptionOfRisk = descriptionOfRisk;
    }
}
