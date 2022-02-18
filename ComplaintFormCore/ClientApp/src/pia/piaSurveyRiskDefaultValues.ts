import { Question } from "survey-vue";
import { PiaSurveyRiskDefaultValue } from "./piaSurveyRiskDefaultValue";

export class PiaSurveyRiskDefaultValues {
    private list: PiaSurveyRiskDefaultValue[] = [];

    public constructor() {
        this.list = this.getList();
    }

    public getDefaultValue(question: Question): PiaSurveyRiskDefaultValue {
        return this.list.filter(r => r.questionName === question.name && this.IsAnswerTheSame(r.questionAnswer, question.value))[0];
    }

    private getList(): PiaSurveyRiskDefaultValue[] {
        const defaultValues = require("./piaSurveyRiskDefaultValues.json");

        if (defaultValues) {
            defaultValues.list.forEach(d => {
                this.list.push(new PiaSurveyRiskDefaultValue(d.questionName, d.questionAnswer, d.descriptionOfRisk));
            });
        }

        return this.list;
    }

    private IsAnswerTheSame(defaultAnswer: any, questionAnswer: unknown): boolean {
        if (typeof questionAnswer == "boolean") {
            return defaultAnswer === questionAnswer;
        } else if (typeof questionAnswer == "string") {
            return defaultAnswer.toLowerCase() === questionAnswer.toLowerCase();
        }

        return false;
    }
}
