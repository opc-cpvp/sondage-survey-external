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
        return this.list.find(
            d => d.questionName === question.name && this.IsAnswerTheSame(d.questionAnswer, question.value)
        ) as PiaSurveyRiskDefaultValue;
    }

    private getList(): PiaSurveyRiskDefaultValue[] {
        if (defaultValues) {
            (defaultValues as any).list.forEach(d => {
                this.list.push({
                    questionName: d.questionName,
                    questionAnswer: d.questionAnswer,
                    descriptionOfRisk: this.getNewDescriptionOfRisk(d)
                });
            });
        }

        return this.list;
    }

    private IsAnswerTheSame(defaultAnswer: string | boolean, questionAnswer: unknown): boolean {
        if (typeof questionAnswer == "boolean") {
            return defaultAnswer === questionAnswer;
        } else if (typeof questionAnswer == "string") {
            return (defaultAnswer as string).toLowerCase() === questionAnswer.toLowerCase();
        }

        return false;
    }

    private getNewDescriptionOfRisk(jsonDefaultValue: any): PiaSurveyDescriptionOfRisk {
        return {
            en: jsonDefaultValue.descriptionOfRisk.en,
            fr: jsonDefaultValue.descriptionOfRisk.fr
        };
    }
}
