import { PiaSurveyRiskDefaultValue } from "./piaSurveyRiskDefaultValue";

export class PiaSurveyRiskDefaultValues {
    private list: PiaSurveyRiskDefaultValue[] = [];

    public constructor() {
        this.list = this.getList();
    }

    public getDefaultValue(questionName: string): PiaSurveyRiskDefaultValue {
        return this.list.filter(r => r.questionName === questionName)[0];
    }

    private getList(): PiaSurveyRiskDefaultValue[] {
        this.list.push(
            new PiaSurveyRiskDefaultValue(
                "WillITLegacySystemRetained",
                false,
                "The institution’s current IT legacy systems and services that will be retained, or those that will be substantially modified, are not compliant with privacy requirements."
            )
        );
        this.list.push(
            new PiaSurveyRiskDefaultValue(
                "DoesStaffReceivedTraining",
                false,
                "The institution does not ensure that staff receive privacy-related training."
            )
        );
        this.list.push(
            new PiaSurveyRiskDefaultValue(
                "ProcessHandlingPrivacyComplaint",
                "yes_not_established",
                "The institution has not yet established a process for handling a privacy complaint or inquiry."
            )
        );
        // ...
        // TODO:
        //      - ADD ALL OTHER DEFAULT VALUES FROM THE WORD DOCUMENT.
        //      - UPDATE "questionName" PROPERTY WITH CORRECT VALUES AFTER WE FIGURE OUT THE FORMAT OF THIS "QUESTION ID"

        return this.list;
    }
}
