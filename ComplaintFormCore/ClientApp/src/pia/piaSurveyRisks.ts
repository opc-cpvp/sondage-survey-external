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
        // First remove any existing risk item(s) for this question name.
        this.currentList = this.currentList.filter(r => r.questionName !== question.name);

        // Then check if this question + answer combo exists in the list of "risk" related questions.
        const defaultValue = this.defaultValues.getDefaultValue(question);

        if (defaultValue) {
            // Add new risk item.
            this.currentList.push(new PiaSurveyRisk(question.name, question.title, question.value, defaultValue.descriptionOfRisk));
        }
    }
}
