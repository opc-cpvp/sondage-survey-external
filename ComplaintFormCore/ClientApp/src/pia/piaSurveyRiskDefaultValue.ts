export class PiaSurveyRiskDefaultValue {
    public questionName!: string;
    public questionAnswer!: string;
    public descriptionOfRisk!: string;

    public constructor(questionName: string, questionAnswer: string, descriptionOfRisk: string) {
        this.questionName = questionName;
        this.questionAnswer = questionAnswer;
        this.descriptionOfRisk = descriptionOfRisk;
    }
}
