import * as Survey from "survey-vue";

export const storageName_PA = "SurveyJS_LoadState_PA";
export const storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";
export const storageName_Test = "SurveyJS_LoadState_Test";
export const storageName_PBR = "SurveyJS_LoadState_PBR";
export const storageName_PID = "SurveyJS_LoadState_PID";

export function saveStateLocally(survey: Survey.SurveyModel, storageName: string): void {
    const res = {
        currentPageNo: survey.currentPageNo,
        data: survey.data
    };

    if (survey.isDisplayMode === true) {
        res.currentPageNo = 999;
    } else if (survey.state === "completed") {
        res.currentPageNo = 1000;
    }

    // Here should be the code to save the data into your database

    window.localStorage.setItem(storageName, JSON.stringify(res));
}

export function loadStateLocally(survey: Survey.SurveyModel, storageName: string, defaultDataAsJsonString: string): void {
    // Here should be the code to load the data from your database

    const storageSt = window.localStorage.getItem(storageName) || "";

    let res: { currentPageNo: number; data: any };
    if (storageSt) {
        res = JSON.parse(storageSt); // Create the survey state for the demo. This line should be deleted in the real app.
    } else {
        // If nothing was found we set the default values for the json as well as set the current page to 0
        res = {
            currentPageNo: 0,
            data: JSON.parse(defaultDataAsJsonString)
        };
    }

    if (res.data) {
        survey.data = res.data;
    }

    // Set the loaded data into the survey.
    if (res.currentPageNo === 999) {
        survey.showPreview();
    } else if (res.currentPageNo === 1000) {
        // go to completed page
    } else {
        survey.currentPageNo = res.currentPageNo;
    }
}

export function clearLocalStorage(storageName: string): void {
    window.localStorage.setItem(storageName, "");
}
