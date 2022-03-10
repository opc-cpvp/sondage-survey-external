import { PiaSurveyDescriptionOfRisk } from "./piaSurveyDescriptionOfRisk";

export interface PiaSurveyRiskDefaultValue {
    questionName: string;
    questionAnswer: string | boolean;
    descriptionOfRisk: PiaSurveyDescriptionOfRisk;
}
