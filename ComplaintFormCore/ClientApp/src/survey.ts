import {
    defaultBootstrapCss,
    surveyLocalization,
    JsonObject,
    Question,
    StylesManager,
    SurveyError,
    SurveyModel,
    settings,
    PageModel
} from "survey-vue";
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
        this.survey = new SurveyModel();

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
        const heading = document.querySelector(".sv_page_title");
        if (heading) {
            heading.parentNode?.insertBefore(summary, heading.nextSibling);
            heading.scrollIntoView();
        }
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

        this.survey.onAfterRenderQuestion.add((sender: SurveyModel, options: any) => {
            this.handleOnAfterRenderQuestion(sender, options);
        });

        this.survey.onPageAdded.add((sender: SurveyModel, options: any) => {
            this.handleOnPageAdded(sender, options);
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
    protected handleOnAfterRenderQuestion(sender: SurveyModel, options: any): void {
        const question = options.question as Question;

        const html = options.htmlElement as HTMLElement;
        const heading = html?.querySelector(".sv_q_title");

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
            const required = document.createElement("strong");
            required.className = defaultBootstrapCss.question.requiredText;
            required.innerText = sender.requiredText;
            heading.appendChild(required);
        }

        // Added spacing to radio label
        if (question.getType() === "radiogroup") {
            html.querySelectorAll<HTMLSpanElement>("fieldset span.sv-string-viewer").forEach(value => {
                value.innerText = ` ${value.innerText}`;
            });
        }
    }

    // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
    protected handleOnCurrentPageChanged(sender: SurveyModel, options: any): void {
        this.saveSurveyState();
        // Scroll page back to top on page changed
        sender.scrollToTopOnPageChange();
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

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    private handleOnPageAdded(sender: SurveyModel, options: any): void {
        const page = options.page as PageModel;

        // Check if the added page is the preview page
        if (page.name !== "all") {
            return;
        }

        // Set the page title otherwise, it won't be rendered
        page.title = sender.previewText;
    }

    private setSurveyProperties(): void {
        // Override the main-color to match the theme
        StylesManager.ThemeColors["bootstrap"]["$main-color"] = "#00627e";

        // Set Theme
        StylesManager.applyTheme("bootstrap");

        // Override defaultBootstrapCss Properties
        defaultBootstrapCss.error.icon = "";
        defaultBootstrapCss.error.item = "label label-danger";
        defaultBootstrapCss.error.root = "";
        defaultBootstrapCss.matrixdynamic.buttonAdd = "btn btn-default";
        defaultBootstrapCss.matrixdynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.navigation.complete = "btn btn-primary";
        defaultBootstrapCss.navigation.edit = "btn btn-primary";
        defaultBootstrapCss.navigation.next = "btn btn-primary";
        defaultBootstrapCss.navigation.prev = "btn btn-default";
        defaultBootstrapCss.navigation.preview = "btn btn-primary";
        defaultBootstrapCss.navigation.start = "btn btn-primary";
        defaultBootstrapCss.page.title = "sv_page_title";
        defaultBootstrapCss.panel.container = "sv_p_container well";
        defaultBootstrapCss.paneldynamic.buttonAdd = "btn btn-default";
        defaultBootstrapCss.paneldynamic.buttonRemove = "btn btn-danger";
        defaultBootstrapCss.progressTextInBar = "";
        defaultBootstrapCss.question.description = "";
        defaultBootstrapCss.question.mainRoot = "sv_qstn form-group";
        defaultBootstrapCss.question.title = "sv_q_title";
        defaultBootstrapCss.question.titleRequired = "required";
        defaultBootstrapCss.question.requiredText = "required";
        defaultBootstrapCss.radiogroup.root = "form-inline";

        settings.titleTags.page = "h1";
        settings.titleTags.panel = "label";
        settings.titleTags.question = "label";

        this.survey.clearInvisibleValues = "onHidden"; // Clear the question value when it becomes invisible. If a question has value and it was invisible initially then survey clears the value on completing.
        this.survey.focusOnFirstError = false;
        this.survey.questionErrorLocation = "top";
        this.survey.requiredText = surveyLocalization.getString("requiredText");
        this.survey.showCompletedPage = true;
        this.survey.showPreviewBeforeComplete = "showAllQuestions"; // Allow respondents to preview answers before submitting the survey results.
        this.survey.showProgressBar = "bottom";
        this.survey.showQuestionNumbers = "off";
        this.survey.storeOthersAsComment = false; // Gets or sets whether the "Others" option text is stored as question comment.
        this.survey.questionTitlePattern = "numTitle"; // Remove the required text from question titles since we'll handle it manually.

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
        surveyLocalization.locales["en"].requiredError = "This field is required";

        surveyLocalization.locales["fr"].addPanel = "Ajouter une nouvelle ligne";
        surveyLocalization.locales["fr"].chooseFile = "Ajouter le(s) fichier(s)...";
        surveyLocalization.locales["fr"].chooseFileCaption = "Sélectionner un fichier";
        surveyLocalization.locales["fr"].clearCaption = "Effacer";
        surveyLocalization.locales["fr"].completingSurveyBefore = "Nos dossiers indiquent que vous avez déjà rempli ce questionnaire.";
        surveyLocalization.locales["fr"].emptyRowsText = "Il n'y a aucune ligne.";
        surveyLocalization.locales["fr"].exceedMaxSize = "La taille du fichier ne doit pas dépasser {0}";
        surveyLocalization.locales["fr"].invalidEmail = "Merci d'entrer une adresse de courriel valide.";
        surveyLocalization.locales["fr"].invalidExpression = "L'expression : {0} doit donner la valeur 'true'.";
        surveyLocalization.locales["fr"].loadingFile = "Téléchargement...";
        surveyLocalization.locales["fr"].maxError = "Le nombre ne doit pas être supérieur à {0}";
        surveyLocalization.locales["fr"].maxSelectError = "Merci de ne sélectionner pas plus de {0} réponse(s).";
        surveyLocalization.locales["fr"].minError = "Le nombre ne doit pas être inférieur à {0}";
        surveyLocalization.locales["fr"].minSelectError = "Merci de sélectionner au moins {0} réponse(s).";
        surveyLocalization.locales["fr"].modalApplyButtonText = "Appliquer";
        surveyLocalization.locales["fr"].modalCancelButtonText = "Annuler";
        surveyLocalization.locales["fr"].multipletext_itemname = "texte";
        surveyLocalization.locales["fr"].noFileChosen = "Aucun fichier sélectionné";
        surveyLocalization.locales["fr"].otherItemText = "Autre";
        surveyLocalization.locales["fr"].panelDynamicProgressText = "Enregistrement {0} de {1}";
        surveyLocalization.locales["fr"].progressText = "Page {0} de {1}";
        surveyLocalization.locales["fr"].removeFileCaption = "Supprimer ce fichier";
        surveyLocalization.locales["fr"].requiredError = "Merci de répondre à la question.";
        surveyLocalization.locales["fr"].requiredInAllRowsError = "Merci de répondre à toutes les questions.";
        surveyLocalization.locales["fr"].savingDataError = "Une erreur s'est produite et a empêché la sauvegarde des résultats.";
        surveyLocalization.locales["fr"].selectAllItemText = "Sélectionner tout";
        surveyLocalization.locales["fr"].signaturePlaceHolder = "Signer ici";
        surveyLocalization.locales["fr"].uploadingFile =
            "Votre fichier est en cours de téléchargement. Merci de patienter quelques secondes et de réessayer.";
        surveyLocalization.locales["fr"].urlGetChoicesError =
            "La requête indique un champ de données vide ou la propriété 'path' est incorrecte";
        surveyLocalization.locales["fr"].urlRequestError = "La requête a généré une erreur '{0}'. {1}";

        // Define custom localizable strings
        surveyLocalization.locales["en"].errorText = "Error";
        surveyLocalization.locales["en"].requiredText = "(required)";
        surveyLocalization.locales["en"].validationError = "The form could not be submitted because {0} error was found.";
        surveyLocalization.locales["en"].validationErrors = "The form could not be submitted because {0} errors were found.";

        surveyLocalization.locales["fr"].errorText = "Erreur";
        surveyLocalization.locales["fr"].requiredText = "(obligatoire)";
        surveyLocalization.locales["fr"].validationError = "Le formulaire n'a pu être soumis car {0} erreur a été trouvée.";
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
