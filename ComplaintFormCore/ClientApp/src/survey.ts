import {
    defaultBootstrapCss,
    surveyLocalization,
    JsonObject,
    Model,
    PageModel,
    Question,
    QuestionHtmlModel,
    StylesManager,
    SurveyError,
    SurveyModel
} from "survey-vue";
import { Converter } from "showdown";
import Vue from "vue";
import { SurveyLocalStorage } from "./surveyLocalStorage";
import { ProblemDetails } from "./models/problemDetails";

export abstract class SurveyBase {
    protected storageName: string;
    protected survey: Model;

    private readonly converter = new Converter({
        simpleLineBreaks: true,
        tasklists: true
    });

    public constructor(locale: "en" | "fr" = "en", storageName: string) {
        this.survey = new Model();

        this.survey.locale = locale;
        this.setSurveyLocalizations();
        this.setSurveyProperties();

        this.registerWidgets();
        this.registerEventHandlers();
        this.registerCustomProperties();

        this.storageName = storageName;
    }

    public displayErrorSummary(questionErrors: Map<Question, SurveyError[]>): void {
        const currentPage = this.survey.currentPage as PageModel;
        const errorText = surveyLocalization.getString("errorText") as string;

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

                if (this.survey.isDisplayMode === true) {
                    //  This is the preview mode
                    link.href = `#${question.id}`;
                } else {
                    link.href = `#${question.inputId}`;
                }

                link.innerText = `${errorText} ${index++}: ${title.innerText} - ${error.getText()}`;
                item.appendChild(link);
                list.appendChild(item);
            }
        });

        const validationError = surveyLocalization.getString("validationError")["format"](index - 1) as string;

        const header = document.createElement("h2");
        header.innerText = validationError;

        summary.appendChild(header);
        summary.appendChild(list);

        const errorsQuestion = new QuestionHtmlModel("errors");
        errorsQuestion.html = summary.outerHTML;

        currentPage.addQuestion(errorsQuestion, 0);
        currentPage.scrollToTop();
    }

    public displayProblemDetails(problemDetails: ProblemDetails): void {
        const questionErrors = new Map<Question, SurveyError[]>();

        const currentPage = this.survey.currentPage as PageModel;

        //  We first clear the previous error html queation if there was
        const errorsQuestion = currentPage.getQuestionByName("errors") as QuestionHtmlModel;
        if (errorsQuestion !== null) {
            currentPage.removeQuestion(errorsQuestion);
        }

        Object.keys(problemDetails.errors).forEach(key => {
            const question = this.survey.getQuestionByName(key);
            if (question) {
                const errors: SurveyError[] = [];
                const valueError = problemDetails.errors[key];

                valueError.forEach((item: string) => {
                    const surveyError: SurveyError = new SurveyError(item, question);
                    errors.push(surveyError);
                });

                questionErrors.set(question, errors);
            }
        });

        this.displayErrorSummary(questionErrors);
    }

    public loadSurveyFromUrl(surveyUrl: string): Promise<void> {
        return fetch(surveyUrl)
            .then(response => response.json())
            .then(json => {
                this.survey.fromJSON(json);
            });
    }

    public postLoadSurvey(defaultData: string): void {
        const storage: SurveyLocalStorage = new SurveyLocalStorage();
        storage.loadStateLocally(this.survey, this.storageName, defaultData);
        storage.saveStateLocally(this.survey, this.storageName);

        this.registerCSSEventHandlers();
    }

    public renderSurvey(): void {
        new Vue({
            el: "#surveyElement",
            data: {
                survey: this.survey
            }
        });
    }

    protected registerWidgets(): void {}

    protected registerEventHandlers(): void {
        this.survey.onTextMarkdown.add((sender: SurveyModel, options: any) => {
            this.handleOnTextMarkdown(sender, options);
        });

        this.survey.onValidatedErrorsOnCurrentPage.add((sender: SurveyModel, options: any) => {
            this.handleOnValidatedErrorsOnCurrentPage(sender, options);
        });

        this.survey.onCurrentPageChanging.add((sender: SurveyModel, options: any) => {
            //  Clear the error div of the user is clicking Next
            if (options.isPrevPage) {
                const errorsQuestion = options.oldCurrentPage.getQuestionByName("errors") as QuestionHtmlModel;
                if (errorsQuestion !== null) {
                    options.oldCurrentPage.removeQuestion(errorsQuestion);
                }
            }
        });

        this.survey.onCurrentPageChanged.add((sender: SurveyModel, options: any) => {
            new SurveyLocalStorage().saveStateLocally(sender, this.storageName);
        });

        this.survey.onGetQuestionTitle.add((sender: SurveyModel, options: any) => {
            this.handleOnGetQuestionTitle(sender, options);
        });

        this.survey.onAfterRenderPage.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderPage(sender, options);
        });
    }

    protected registerCSSEventHandlers(): void {
        this.survey.onUpdatePanelCssClasses.add((sender: SurveyModel, options: any) => {
            this.handleOnUpdatePanelCssClasses(sender, options);
        });

        this.survey.onUpdateQuestionCssClasses.add((sender: SurveyModel, options: any) => {
            this.handleOnUpdateQuestionCssClasses(sender, options);
        });
    }

    private setSurveyProperties(): void {
        // Set Theme
        StylesManager.applyTheme("bootstrap");

        // Override defaultBootstrapCss Properties
        defaultBootstrapCss.error.icon = "";
        defaultBootstrapCss.matrixdynamic.buttonAdd = "btn btn-secondary";
        defaultBootstrapCss.matrixdynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.matrixdynamic.root = "table";
        defaultBootstrapCss.navigationButton = "btn btn-primary";
        defaultBootstrapCss.page.title = "sv_title";
        defaultBootstrapCss.question.title = "sv_q_title";
        defaultBootstrapCss.question.titleRequired = "required";

        // onHidden -> survey clears the question value when the question becomes invisible.
        // If a question has an answer value and it was invisible initially, a survey clears the value on completing.
        this.survey.clearInvisibleValues = "onHidden";
        this.survey.focusOnFirstError = false;
        this.survey.questionErrorLocation = "top";
        this.survey.requiredText = surveyLocalization.getString("requiredText");
        this.survey.showCompletedPage = true;
        this.survey.showPreviewBeforeComplete = "showAllQuestions";
        this.survey.showProgressBar = "bottom";
        this.survey.showQuestionNumbers = "off";
    }

    private setSurveyLocalizations(): void {
        // Override surveyjs strings
        surveyLocalization.locales["en"].otherItemText = "Other";
        surveyLocalization.locales["fr"].otherItemText = "Autre";

        surveyLocalization.locales["en"].requiredError = "This field is required";
        surveyLocalization.locales["fr"].requiredError = "Ce champ est obligatoire";

        // Define custom localizable strings
        surveyLocalization.locales["en"].errorText = "Error";
        surveyLocalization.locales["fr"].errorText = "Erreur";

        surveyLocalization.locales["en"].requiredText = "(required)";
        surveyLocalization.locales["fr"].requiredText = "(obligatoire)";

        surveyLocalization.locales["en"].validationError = "The form could not be submitted because {0} errors were found.";
        surveyLocalization.locales["fr"].validationError = "Le formulaire n'a pu être soumis car {0} erreurs ont été trouvées.";
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
        //  First we clear the previous 'errors' question if there was
        const currentPage = this.survey.currentPage as PageModel;

        const errorsQuestion = currentPage.getQuestionByName("errors") as QuestionHtmlModel;
        if (errorsQuestion !== null) {
            currentPage.removeQuestion(errorsQuestion);
        }

        if (!currentPage.hasErrors()) {
            return;
        }

        const questions = options.questions as Question[];

        const questionErrors = new Map<Question, SurveyError[]>();
        for (const question of questions) {
            questionErrors.set(question, question.errors);
        }

        this.displayErrorSummary(questionErrors);
    }

    private handleOnUpdateQuestionCssClasses(sender: SurveyModel, options: any): void {
        const classes = options.cssClasses;

        //  If it is the preview mode...
        if (sender.isDisplayMode === true) {
            if (options.question.getType() === "html" && options.question.name !== "errors") {
                //  This will remove the html questions in Preview mode.
                //  BUT not for the html question holding the errors on the Preview page.
                classes.root += " sv-hidden";
            } else if (options.question.getType() === "comment") {
                //  do not show the 'description' property
                classes.description += " sv-hidden";
            } else if (options.question.getType() === "file") {
                classes.placeholderInput += " sv-hidden";
            }
        }
    }

    private handleOnUpdatePanelCssClasses(sender: SurveyModel, options: any): void {
        const classes = options.cssClasses;

        if (sender.isDisplayMode === true && options.panel.hideOnPreview === true) {
            //  This is to hide panel we don't want to show on preview.
            //  Panels that contains information html for example.
            //  It also hides the 'pages'! The reason is because on preview, the pages become panels
            //  and therefore going thru this code.
            classes.panel.container = "sv-hidden";
        } else {
            //  This is a class found in GoC and was used in the original project.
            //  It adds a border around a panel and a different backgroud color
            classes.panel.container += " well";
        }
    }

    private handleOnGetQuestionTitle(sender: SurveyModel, options: any): void {
        //  This is to add * at the beginning of a required question. The property requiredText
        //  is set as 'required' later in the code
        if (options.question.isRequired) {
            options.title = `<span class='sv_q_required_text'>&ast;&nbsp;</span>${options.title as string}`;
        }
    }

    private handleOnAfterRenderPage(sender: SurveyModel, options: any): void {
        window.document.title = options.page.title;
    }

    // TODO: This method should actually be converted into a widget.
    private registerCustomProperties(): void {
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
