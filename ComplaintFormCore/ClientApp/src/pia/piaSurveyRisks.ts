import { Question } from "survey-vue";
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
        // Check if this question exists in the list of "risk" related questions.
        const defaultValue = this.defaultValues.getDefaultValue(question.name);
        if (defaultValue) {
            // Check if risk item already in the array.
            const existingRisk = this.currentList.filter(r => r.questionName === question.name)[0];
            if (existingRisk) {
                if (!this.isRiskAnswer(defaultValue.questionAnswer, question.value)) {
                    // Risk item is in the collection already, but the question answer has changed. Need to remove it from the collection.
                    this.currentList = this.currentList.filter(r => r.questionName !== existingRisk.questionName);
                }
            } else {
                if (this.isRiskAnswer(defaultValue.questionAnswer, question.value)) {
                    // Create new risk item, fill in relevant properties and add to the list.
                    this.currentList.push(new PiaSurveyRisk(question.name, question.title, question.value, defaultValue.descriptionOfRisk));
                }
            }
        }
    }

    private isRiskAnswer(defaultAnswer: string, questionAnswer: string): boolean {
        return defaultAnswer === questionAnswer;
    }
}
