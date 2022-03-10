import { PiaSurveyDescriptionOfRisk } from "./piaSurveyDescriptionOfRisk";

export interface PiaSurveyRiskDefaultValue {
    questionName: string;
    questionAnswer: unknown;
    descriptionOfRisk: PiaSurveyDescriptionOfRisk;
}
