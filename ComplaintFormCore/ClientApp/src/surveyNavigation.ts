import { SurveyModel } from "survey-vue";
import * as SurveyHelper from "./surveyHelper";
import * as Ladda from "ladda";

//  Function for updating (show/hide) the navigation buttons
export function onCurrentPageChanged_updateNavButtons(survey: SurveyModel): void {
    //  NOTES:
    //  survey.isFirstPage is the start page but for some reasons when we view the preview, survey.isFirstPage
    //      gets set to true. This maybe a bug in survey.js or else there is a reason I don't understand

    const startButton = document.getElementById("btnStart") ?? new HTMLElement();
    startButton.classList.remove("hidden");
    startButton.classList.remove("inline");
    startButton.classList.add(survey.isFirstPage && !survey.isDisplayMode ? "inline" : "hidden");

    const previousButton = document.getElementById("btnSurveyPrev") ?? new HTMLElement();
    previousButton.classList.remove("hidden");
    previousButton.classList.remove("inline");
    previousButton.classList.add(!survey.isFirstPage ? "inline" : "hidden");

    const nextButton = document.getElementById("btnSurveyNext") ?? new HTMLElement();
    nextButton.classList.remove("hidden");
    nextButton.classList.remove("inline");
    nextButton.classList.add(!survey.isFirstPage && !survey.isLastPage ? "inline" : "hidden");

    const showPreviewButton = document.getElementById("btnShowPreview") ?? new HTMLElement();
    showPreviewButton.classList.remove("hidden");
    showPreviewButton.classList.remove("inline");
    showPreviewButton.classList.add(
        !survey.isDisplayMode && (survey.isLastPage || (survey as any).passedPreviewPage === true) ? "inline" : "hidden"
    );

    const completeButton = document.getElementById("btnComplete") ?? new HTMLElement();
    completeButton.classList.remove("hidden");
    completeButton.classList.remove("inline");
    completeButton.classList.add(survey.isDisplayMode ? "inline" : "hidden");

    SurveyHelper.clearProblemDetails();
}

export function showPreview(survey: SurveyModel): void {
    //  Set the survey property that will hold the information as to if the user has reached the 'Preview'
    (survey as any).passedPreviewPage = true;

    //  Calling the native showPreview method
    survey.showPreview();
}

export function completeSurvey(button: HTMLButtonElement, survey: SurveyModel): void {
    const spinner = Ladda.create(button);
    spinner.start();

    survey.doComplete();
}

export function startSurvey(survey: SurveyModel): void {
    survey.nextPage();
}

export function prevPage(survey: SurveyModel): void {
    survey.prevPage();
}

export function nextPage(survey: SurveyModel): void {
    survey.nextPage();
}

export function endSession(): void {
    const url = "/Home/Index";
    window.location.href = url;
}
