import {
    defaultBootstrapCss,
    surveyLocalization,
    JsonObject,
    Model,
    PageModel,
    Question,
    QuestionHtmlModel,
    StylesManager,
    Survey,
    SurveyError,
    SurveyModel
} from "survey-vue";
import { Converter } from "showdown";
import Vue from "vue";

export abstract class SurveyBase extends Survey {
    private converter = new Converter({
        simpleLineBreaks: true,
        tasklists: true
    });

    public constructor(locale: "en" | "fr" = "en") {
        super();

        this.survey = new Model();
        this.survey.locale = locale;

        this.setSurveyProperties();
        this.setSurveyLocalizations();
        this.registerSurveyCallbacks();

        this.registerCustomProperties();
    }

    public displayErrorSummary(questionErrors: Map<Question, SurveyError[]>): void {
        const currentPage = this.survey.currentPage as PageModel;

        let errorsQuestion = currentPage.getQuestionByName("errors") as QuestionHtmlModel;
        if (errorsQuestion !== null) {
            currentPage.removeQuestion(errorsQuestion);
        }

        if (!currentPage.hasErrors()) {
            return;
        }

        const summary = document.createElement("section");
        summary.className = "alert alert-danger";

        let index = 1;
        const list = document.createElement("ul");
        questionErrors.forEach((errors: SurveyError[], question: Question) => {
            const title = document.createElement("div");
            title.innerHTML = question.processedTitle;
            for (const error of errors) {
                const item = document.createElement("li");
                const link = document.createElement("a");
                link.href = `#${question.inputId}`;
                link.innerText = `Error ${index++}: ${title.innerText} - ${error.getText()}`;
                item.appendChild(link);
                list.appendChild(item);
            }
        });

        const header = document.createElement("h2");
        header.innerText = `The form could not be submitted because ${index - 1} error(s) were found.`;

        summary.appendChild(header);
        summary.appendChild(list);

        errorsQuestion = new QuestionHtmlModel("errors");
        errorsQuestion.html = summary.outerHTML;

        currentPage.addQuestion(errorsQuestion, 0);
        currentPage.scrollToTop();
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
        defaultBootstrapCss.page.title = "sv_title";
        defaultBootstrapCss.question.title = "sv_q_title";
        defaultBootstrapCss.question.titleRequired = "required";

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

        this.survey.onValidatedErrorsOnCurrentPage.add((sender: SurveyModel, options: any) => {
            this.handleOnValidatedErrorsOnCurrentPage(sender, options);
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

    private handleOnValidatedErrorsOnCurrentPage(sender: SurveyModel, options: any): void {
        const questions = options.questions as Question[];

        const questionErrors = new Map<Question, SurveyError[]>();
        for (const question of questions) {
            questionErrors.set(question, question.errors);
        }

        this.displayErrorSummary(questionErrors);
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
    }
}