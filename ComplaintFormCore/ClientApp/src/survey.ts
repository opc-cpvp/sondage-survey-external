import {
    defaultBootstrapCss,
    surveyLocalization,
    JsonObject,
    Model,
    PageModel,
    Question,
    StylesManager,
    Survey,
    SurveyModel
} from "survey-vue";
import { Converter } from "showdown";
import Vue from "vue";

export abstract class SurveyBase extends Survey {
    private converter = new Converter({
        simpleLineBreaks: true,
        tasklists: true
    });

    public constructor(locale: "fr" | "en" = "en") {
        super();

        this.survey = new Model();
        this.survey.locale = locale;

        this.setSurveyProperties();
        this.setSurveyLocalizations();
        this.registerSurveyCallbacks();

        this.registerCustomProperties();
    }

    public loadSurveyFromUrl(surveyUrl: string): Promise<void> {
        return fetch(surveyUrl)
            .then(response => response.json())
            .then(json => {
                this.survey.fromJSON(json);
            });
    }

    public render(): void {
        new Vue({
            el: "#surveyElement",
            data: {
                survey: this.survey
            }
        });
    }

    private setSurveyProperties(): void {
        // Set Theme
        StylesManager.applyTheme("bootstrap");

        // Override defaultBootstrapCss Properties
        defaultBootstrapCss.error.icon = "";
        defaultBootstrapCss.matrixdynamic.buttonAdd = "btn btn-secondary";
        defaultBootstrapCss.matrixdynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.navigationButton = "btn btn-primary";
        defaultBootstrapCss.question.titleRequired = "required";

        // https://surveyjs.io/Documentation/Library?id=surveymodel#checkErrorsMode
        // check errors on every question value (i.e., answer) changing.
        this.survey.checkErrorsMode = "onValueChanged";
        // onHidden -> survey clears the question value when the question becomes invisible.
        // If a question has an answer value and it was invisible initially, a survey clears the value on completing.
        this.survey.clearInvisibleValues = "onHidden";
        this.survey.goNextPageAutomatic = false;
        this.survey.showPreviewBeforeComplete = "showAllQuestions";
        this.survey.questionErrorLocation = "top";
        this.survey.requiredText = this.survey.locale === "fr" ? "(obligatoire)" : "(required)";
        this.survey.showCompletedPage = true;
        this.survey.showProgressBar = "bottom";
        this.survey.showQuestionNumbers = "off";
    }

    private setSurveyLocalizations(): void {
        surveyLocalization.locales["en"].otherItemText = "Other";
        surveyLocalization.locales["fr"].otherItemText = "Autre";

        surveyLocalization.locales["en"].requiredError = "This field is required";
        surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";
    }

    private registerSurveyCallbacks(): void {
        this.survey.onTextMarkdown.add((sender: SurveyModel, options: any) => {
            this.handleOnTextMarkdown(sender, options);
        });

        this.survey.onAfterRenderPage.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderPage(sender, options);
        });

        this.survey.onAfterRenderQuestion.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderQuestion(sender, options);
        });
    }

    private handleOnTextMarkdown(sender: SurveyModel, options: any): void {
        // convert the mardown text to html
        let str = this.converter.makeHtml(options.text);

        // remove root paragraphs <p></p>
        str = str.substring(3);
        str = str.substring(0, str.length - 4);

        // set html
        options.html = str;
    }

    private handleOnAfterRenderPage(sender: SurveyModel, options: any): void {
        // Replaces the page title h4 with an h1
        const htmlElement = options.htmlElement as HTMLElement;
        const oldTitle = htmlElement.querySelector("h4") ?? htmlElement.querySelector("h1");

        if (oldTitle === null) {
            return;
        }

        const page = options.page as PageModel;

        const newTitle = document.createElement("h1");
        newTitle.innerText = page.title;

        oldTitle.parentElement?.replaceChild(newTitle, oldTitle);
    }

    private handleOnAfterRenderQuestion(sender: SurveyModel, options: any): void {
        // Replaces the question title h5 with a label
        const htmlElement = options.htmlElement as HTMLElement;
        const oldTitle = htmlElement.querySelector("h5") ?? htmlElement.querySelector("label");

        if (oldTitle === null) {
            return;
        }

        const question = options.question as Question;

        const newTitle = document.createElement("label");
        newTitle.setAttribute("for", question.inputId);
        newTitle.className = oldTitle.className;
        newTitle.innerHTML = oldTitle.innerHTML;

        oldTitle.parentElement?.replaceChild(newTitle, oldTitle);
    }

    // TODO: This method should actually be converted into a widget.
    private registerCustomProperties(): void {
        JsonObject.metaData.addProperty("itemvalue", {
            name: "htmlAdditionalInfo:text"
        });

        // This is a survey property that will hold the information as to if the user has reached the 'Preview'
        // page at least once. The idea is if the user has reached the 'Preview' page he can always go back to it after
        // editing a page. This will be usefull for very long survey after a user decided to edit an item from the preview page.
        JsonObject.metaData.addProperty("survey", {
            name: "passedPreviewPage:boolean",
            default: false
        });

        JsonObject.metaData.addProperty("page", {
            name: "hideOnPDF:boolean",
            default: false
        });

        JsonObject.metaData.addProperty("page", {
            name: "hideOnPreview:boolean",
            default: false
        });

        JsonObject.metaData.addProperty("panel", {
            name: "hideOnPDF:boolean",
            default: false
        });

        // This is to hide page and panel we don't want to show on preview.
        // Pages or Panels that contains exclusively information html for example.
        // The reason why it is working for is because on preview, the pages become panels
        JsonObject.metaData.addProperty("panel", {
            name: "hideOnPreview:boolean",
            default: false
        });

        JsonObject.metaData.addProperties("file", [
            { name: "itemListTitle:text" },
            { name: "itemListRemoveText:text" },
            { name: "itemListNoAttachmentsText:text" },
            { name: "confirmRemoveMessage:text" },
            { name: "duplicateFileNameExceptionMessage:text" },
            { name: "multipleFileMaxSizeErrorMessage:text" }
        ]);
    }
}
