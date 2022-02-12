import { defaultBootstrapCss, surveyLocalization, JsonObject, Model, Question, StylesManager, SurveyError, SurveyModel } from "survey-vue";
import { Converter } from "showdown";
import Vue from "vue";
import { LocalStorage } from "./localStorage";
import { SurveyState } from "./models/surveyState";

export abstract class SurveyBase {
    public readonly storageName: string;
    protected readonly survey: SurveyModel;
    protected readonly storage = new LocalStorage();

    private readonly converter = new Converter({
        simpleLineBreaks: true,
        tasklists: true
    });

    public constructor(locale: "en" | "fr" = "en", storageName: string) {
        this.storageName = storageName;
        this.survey = new Model();

        this.survey.locale = locale;
        this.setSurveyLocalizations();
        this.setSurveyProperties();

        this.registerWidgets();
        this.registerCustomProperties();
    }

    public displayErrorSummary(questionErrors: Map<Question, SurveyError[]>): void {
        const summaryId = "errors";

        // If there's already an error summary, remove it
        let summary = document.getElementById(summaryId);
        if (summary) {
            summary.remove();
        }

        // If there aren't any errors, exit
        if (questionErrors.size === 0) {
            return;
        }

        const errorText = surveyLocalization.getString("errorText") as string;

        // Create and populate the error summary
        summary = document.createElement("section");
        summary.id = summaryId;
        summary.className = "alert alert-danger";

        let index = 1;
        const list = document.createElement("ul");
        questionErrors.forEach((errors: SurveyError[], question: Question) => {
            const title = document.createElement("div");
            title.innerHTML = question.processedTitle;
            for (const error of errors) {
                const item = document.createElement("li");
                const link = document.createElement("a");
                link.href = `#${question.hasSingleInput ? question.inputId : question.id}`;
                link.innerText = `${errorText} ${index++}: ${title.innerText} - ${error.getText()}`;
                item.appendChild(link);
                list.appendChild(item);
            }
        });

        const errorCount = index - 1;
        const localizationKey = errorCount > 1 ? "validationErrors" : "validationError";
        const validationError = surveyLocalization.getString(localizationKey)["format"](errorCount) as string;

        // Set the error summary's title
        const header = document.createElement("h2");
        header.innerText = validationError;

        summary.appendChild(header);
        summary.appendChild(list);

        // Insert the error summary after the page's heading
        const heading = document.querySelector("h1");
        heading?.parentNode?.insertBefore(summary, heading.nextSibling);
        heading?.scrollIntoView();
    }

    public getSurveyModel(): SurveyModel {
        return this.survey;
    }

    public loadSurveyFromUrl(surveyUrl: string): Promise<void> {
        return fetch(surveyUrl)
            .then(response => response.json())
            .then(json => this.survey.fromJSON(json))
            .then(() => this.loadedSurveyFromUrl());
    }

    public renderSurvey(): void {
        new Vue({
            el: "#surveyElement",
            data: {
                survey: this.survey
            }
        });
    }

    protected loadedSurveyFromUrl(): void {
        this.loadSurveyState();

        //  This registerEventHandlers MUST be after loading the survey.
        //  Problem is whenever there is a paneldynamic in a survey, during loading of the paneldynamic,
        //  onCurrentPageChanged gets fired. This cause the page data to be deleted so when the users switch
        //  languages they are returned to page 1 of the survey.
        this.registerEventHandlers();
    }

    protected registerWidgets(): void {}

    protected registerEventHandlers(): void {
        this.survey.onTextMarkdown.add((sender: SurveyModel, options: any) => {
            this.handleOnTextMarkdown(sender, options);
        });

        this.survey.onValidatedErrorsOnCurrentPage.add((sender: SurveyModel, options: any) => {
            this.handleOnValidatedErrorsOnCurrentPage(sender, options);
        });

        this.survey.onCurrentPageChanged.add((sender: SurveyModel, options: any) => {
            this.handleOnCurrentPageChanged(sender, options);
        });

        this.survey.onAfterRenderPage.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderPage(sender, options);
        });

        this.survey.onAfterRenderQuestion.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderQuestion(sender, options);
        });
    }

    // TODO: This method should actually be converted into a widget.
    protected registerCustomProperties(): void {
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

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnAfterRenderPage(sender: SurveyModel, options: any): void {
        const html = options.htmlElement as HTMLElement;
        const headings = html?.querySelectorAll("h4");

        // Check for the presence of headings
        if (!headings) {
            return;
        }

        // Replace headings with appropriate tag
        headings.forEach((h, i) => {
            if (i > 0) {
                // Replace the headings (h4) with another heading (h2)
                h.outerHTML = `<h2 class="${h.className}">${h.innerHTML}</h2>`;
            } else {
                // Replace the heading (h4) with another heading (h1)
                h.outerHTML = `<h1 class="${h.className}">${h.innerHTML}</h1>`;
            }
        });
    }

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnAfterRenderQuestion(sender: SurveyModel, options: any): void {
        const question = options.question as Question;

        const html = options.htmlElement as HTMLElement;
        const heading = html?.querySelector("h5");

        // Check for the presence of a heading
        if (!heading) {
            return;
        }

        // Unwrap the divs contained within the heading to fix alignment with required field asterisk
        let div = heading.querySelector("div");
        while (div) {
            div.outerHTML = div.innerHTML;
            div = heading.querySelector("div");
        }

        // Replace the required text with a strong
        if (question.isRequired) {
            const required = heading.querySelector("span.sv_q_required_text");

            if (required) {
                required.outerHTML = `<strong class="${required.className}">${required.innerHTML}</strong>`;
            }
        }

        // Replace the heading with a label
        heading.outerHTML = `<label id="${question.ariaTitleId}" for="${question.inputId}" class="${heading.className}">${heading.innerHTML}</label>`;
    }

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnCurrentPageChanged(sender: SurveyModel, options: any): void {
        this.saveSurveyState();
    }

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnTextMarkdown(sender: SurveyModel, options: any): void {
        // convert the mardown text to html
        let str = this.converter.makeHtml(options.text);

        // remove root paragraphs <p></p>
        str = str.substring(3);
        str = str.substring(0, str.length - 4);

        // set html
        options.html = str;
    }

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnValidatedErrorsOnCurrentPage(sender: SurveyModel, options: any): void {
        const questions = options.questions as Question[];

        const questionErrors = new Map<Question, SurveyError[]>();
        for (const question of questions) {
            questionErrors.set(question, question.errors);
        }

        this.displayErrorSummary(questionErrors);
    }

    private setSurveyProperties(): void {
        // Set Theme
        StylesManager.applyTheme("bootstrap");

        // Override defaultBootstrapCss Properties
        defaultBootstrapCss.checkbox.root = ""; // Allows the 'Other' textbox to display at full width.
        defaultBootstrapCss.error.icon = "";
        defaultBootstrapCss.matrixdynamic.buttonAdd = "btn btn-default";
        defaultBootstrapCss.matrixdynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.matrixdynamic.root = "table";
        defaultBootstrapCss.navigationButton = "btn btn-primary";
        defaultBootstrapCss.page.title = "sv_title";
        defaultBootstrapCss.panel.container = "well";
        defaultBootstrapCss.paneldynamic.buttonAdd = "btn btn-default";
        defaultBootstrapCss.paneldynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.question.description = ""; //  Removes the default class (small)
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

        //  The default value for this is true. That means when using hasOther = true for a checkbox type question,
        //  SurveyJS creates an new property with name 'PropertyName-Comment'. Problem occurs when trying to convert
        //  the survey data into C# object, dashes are not permitted in C#. This is much simpler to just set
        //  storeOthersAsComment = false and then any 'other' item is passed to the back as is (e.g. in string).
        this.survey.storeOthersAsComment = false;

        //  This flag is to set the maxLength on all other as in hasOther
        this.survey.maxOthersLength = 100;

        //  This flag is to set the maxLength for all comments and text type question.
        this.survey.maxTextLength = 2000;
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

        surveyLocalization.locales["en"].validationError = "The form could not be submitted because {0} error was found.";
        surveyLocalization.locales["fr"].validationError = "Le formulaire n'a pu être soumis car {0} erreur a été trouvée.";

        surveyLocalization.locales["en"].validationErrors = "The form could not be submitted because {0} errors were found.";
        surveyLocalization.locales["fr"].validationErrors = "Le formulaire n'a pu être soumis car {0} erreurs ont été trouvées.";
    }

    private getSurveyState(): SurveyState {
        return {
            currentPageNo: this.survey.currentPageNo,
            data: this.survey.data
        } as SurveyState;
    }

    private saveSurveyState(): void {
        const state = this.getSurveyState();
        this.storage.save(this.storageName, state);
    }

    private setSurveyState(state: SurveyState) {
        // It's important that the data is set first, otherwise changing the currentPageNo will cause
        // onCurrentPageChanged to trigger which will override the data.
        this.survey.data = state.data;
        this.survey.currentPageNo = state.currentPageNo;
    }

    private loadSurveyState(): void {
        const state = this.storage.load(this.storageName) as SurveyState;

        if (state === null) {
            return;
        }

        this.setSurveyState(state);
    }
}
