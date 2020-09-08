export class MultiLanguagePropery {
    fr: string;
    en: string;
    default: string;

    constructor(private multiLangProperty?: MultiLanguagePropery) {
        this.fr = "";
        this.en = "";
        this.default = "";
    }
}
