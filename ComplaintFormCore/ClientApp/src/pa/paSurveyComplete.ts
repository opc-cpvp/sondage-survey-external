export class PaSurveyComplete extends Event {
    public static readonly eventName = "PaSurveyComplete";
    public readonly referenceNumber: string;

    public constructor(referenceNumber: string) {
        super(PaSurveyComplete.eventName);
        this.referenceNumber = referenceNumber;
    }
}
