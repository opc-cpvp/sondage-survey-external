import { Question } from "survey-vue";
import { PiaSurveyDescriptionOfRisk } from "./piaSurveyDescriptionOfRisk";
import { PiaSurveyRiskDefaultValue } from "./piaSurveyRiskDefaultValue";
import * as defaultValues from "./piaSurveyRiskDefaultValues.json";

export class PiaSurveyRiskDefaultValues {
    private list: PiaSurveyRiskDefaultValue[] = [];

    public constructor() {
        this.list = this.getList();
    }

    public getDefaultValue(question: Question): PiaSurveyRiskDefaultValue {
        return this.list.filter(r => r.questionName === question.name && this.IsAnswerTheSame(r.questionAnswer, question.value))[0];
    }

    private getList(): PiaSurveyRiskDefaultValue[] {
        if (defaultValues) {
            (defaultValues as any).list.forEach(d => {
                this.list.push(new PiaSurveyRiskDefaultValue(d.questionName, d.questionAnswer, this.getNewDescriptionOfRisk(d)));
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

    private getNewDescriptionOfRisk(jsonDefaultValue: any): PiaSurveyDescriptionOfRisk {
        return new PiaSurveyDescriptionOfRisk(jsonDefaultValue.descriptionOfRisk.en, jsonDefaultValue.descriptionOfRisk.fr);
    }
}
