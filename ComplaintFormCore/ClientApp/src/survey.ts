import { Survey, SurveyModel } from "survey-vue";
import { Converter, ConverterOptions } from "showdown";

export abstract class SurveyBase extends Survey {
    private surveyUrl: string;

    public constructor(surveyUrl: string) {
        super();
        this.surveyUrl = surveyUrl;

        this.setSurveyProperties();
        this.setSurveyLocalizations();
        this.registerSurveyCallbacks();

        this.registerCustomProperties();
    }

    public async init(): Promise<void> {
        await this.loadSurvey();
    }

    private setSurveyProperties(): void {
        // Set Theme
        this.survey.StylesManager.applyTheme("bootstrap");

        // Override defaultBootstrapCss Properties
        this.survey.defaultBootstrapCss.navigationButton = "btn btn-primary";
        this.survey.defaultBootstrapCss.matrixdynamic.buttonAdd = "btn btn-secondary";
        this.survey.defaultBootstrapCss.matrixdynamic.buttonRemove = "btn btn-danger";

        // https://surveyjs.io/Documentation/Library?id=surveymodel#checkErrorsMode
        // check errors on every question value (i.e., answer) changing.
        this.survey.checkErrorsMode = "onValueChanged";
        // onHidden -> survey clears the question value when the question becomes invisible.
        // If a question has an answer value and it was invisible initially, a survey clears the value on completing.
        this.survey.clearInvisibleValues = "onHidden";
        this.survey.goNextPageAutomatic = false;
        this.survey.showPreviewBeforeComplete = "showAllQuestions";
        this.survey.questionErrorLocation = "top";
        this.survey.showCompletedPage = true;
        this.survey.showNavigationButtons = false;
        this.survey.showProgressBar = "bottom";
        this.survey.showQuestionNumbers = "off";
    }

    private loadSurvey(): Promise<void> {
        return fetch(this.surveyUrl).then(response => {
            const json = response.json();
            this.survey.fromJSON(json);
        });
    }

    private setSurveyLocalizations(): void {
        this.survey.surveyLocalization.locales["en"].otherItemText = "Other";
        this.survey.surveyLocalization.locales["fr"].otherItemText = "Autre";

        this.survey.surveyLocalization.locales["en"].requiredError = "This field is required";
        this.survey.surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";

        // TODO: might not work
        this.survey.surveyLocalization.locales["en"].requiredText = "(required)";
        this.survey.surveyLocalization.locales["fr"].requiredText = "(obligatoire)";
    }

    private registerSurveyCallbacks(): void {
        this.survey.onTextMarkdown.add((sender: SurveyModel, options: any) => {
            this.handleOnTextMarkdown(sender, options);
        });
    }

    private handleOnTextMarkdown(sender: SurveyModel, options: any): void {
        const converterOptions: ConverterOptions = {
            simpleLineBreaks: true,
            tasklists: true
        };
        const converter = new Converter(converterOptions);

        // convert the mardown text to html
        let str = converter.makeHtml(options.text);

        // remove root paragraphs <p></p>
        str = str.substring(3);
        str = str.substring(0, str.length - 4);

        // set html
        options.html = str;
    }

    // TODO: This method should actually be converted into a widget.
    private registerCustomProperties(): void {
        this.survey.JsonObject.metaData.addProperty("itemvalue", {
            name: "htmlAdditionalInfo:text"
        });

        // This is a survey property that will hold the information as to if the user has reached the 'Preview'
        // page at least once. The idea is if the user has reached the 'Preview' page he can always go back to it after
        // editing a page. This will be usefull for very long survey after a user decided to edit an item from the preview page.
        this.survey.JsonObject.metaData.addProperty("survey", {
            name: "passedPreviewPage:boolean",
            default: false
        });

        // This is to hide page and panel we don't want to show on preview.
        // Pages or Panels that contains exclusively information html for example.
        // The reason why it is working for is because on preview, the pages become panels
        this.survey.JsonObject.metaData.addProperty("panel", {
            name: "hideOnPreview:boolean",
            default: false
        });

        this.survey.JsonObject.metaData.addProperty("page", {
            name: "hideOnPreview:boolean",
            default: false
        });

        this.survey.JsonObject.metaData.addProperty("panel", {
            name: "hideOnPDF:boolean",
            default: false
        });

        this.survey.JsonObject.metaData.addProperty("page", {
            name: "hideOnPDF:boolean",
            default: false
        });
    }
}
