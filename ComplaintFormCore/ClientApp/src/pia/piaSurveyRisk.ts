export class PiaSurveyRisk {
    public questionName = "";
    public questionText = "";
    public questionAnswer = "";
    public defaultDescriptionOfRisk = "";

    public constructor(questionName: string, questionText: string, questionAnswer: string, defaultDescriptionOfRisk: string) {
        this.questionName = questionName;
        this.questionText = questionText;
        this.questionAnswer = questionAnswer;
        this.defaultDescriptionOfRisk = defaultDescriptionOfRisk;
    }
}
