import * as Survey from "survey-vue";
import * as SurveyHelper from "./surveyHelper";
import * as Ladda from "ladda";
import * as SurveyLocalStorage from "./surveyLocalStorage";

//  Function for updating (show/hide) the navigation buttons
export function onCurrentPageChanged_updateNavButtons(survey: Survey.SurveyModel): void {
    //  NOTES:
    //  survey.isFirstPage is the start page but for some reasons when we view the preview, survey.isFirstPage
    //      gets set to true. This maybe a bug in survey.js or else there is a reason I don't understand

    // document
    //    .getElementById("btnEndSession")
    //    .style
    //    .display = !survey.isFirstPage || survey.isDisplayMode
    //        ? "inline"
    //        : "none";

    const startButton = document.getElementById("btnStart") ?? new HTMLElement();
    startButton.style.display = survey.isFirstPage && !survey.isDisplayMode ? "inline" : "none";

    const previousButton = document.getElementById("btnSurveyPrev") ?? new HTMLElement();
    previousButton.style.display = !survey.isFirstPage ? "inline" : "none";

    const nextButton = document.getElementById("btnSurveyNext") ?? new HTMLElement();
    nextButton.style.display = !survey.isFirstPage && !survey.isLastPage ? "inline" : "none";

    const showPreviewButton = document.getElementById("btnShowPreview") ?? new HTMLElement();
    showPreviewButton.style.display = !survey.isDisplayMode &&
            (survey.isLastPage || survey.passedPreviewPage === true)
            ? "inline"
            : "none";

    const completeButton = document.getElementById("btnComplete") ?? new HTMLElement();
    completeButton.style.display = survey.isDisplayMode ? "inline" : "none";

    SurveyHelper.clearProblemDetails();
}

export function showPreview(survey: Survey.SurveyModel): void {
    //  Set the survey property that will hold the information as to if the user has reached the 'Preview'
    survey.passedPreviewPage = true;

    //  Calling the native showPreview method
    survey.showPreview();
}

export function completeSurvey(button: HTMLButtonElement, survey: Survey.SurveyModel): void {
    var spinner = Ladda.create(button);
    spinner.start();

    survey.doComplete();
}

export function startSurvey(survey: Survey.SurveyModel): void {
    SurveyLocalStorage.clearLocalStorage(SurveyLocalStorage.storageName_PA);
    survey.nextPage();
}

export function endSession(): void {
    const url = "/Home/Index";
    window.location.href = url;
}

export function showProcessing() {
    const waitingdiv = document.getElementById("waitingdiv");
    if (waitingdiv) {
        waitingdiv.style.display = "";
    }

    alert("This alert delay the execution");
}

export function hideProcessing() {
    const waitingdiv = document.getElementById("waitingdiv");
    if (waitingdiv) {
        waitingdiv.style.display = "none";
    }
}