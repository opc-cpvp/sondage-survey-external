import { SurveyBase } from "../survey";

export class NewPaSurvey extends SurveyBase {
    public constructor(surveyUrl: string, locale: "fr" | "en" = "en") {
        super(surveyUrl, locale);
    }
}
