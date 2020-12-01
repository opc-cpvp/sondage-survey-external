import * as Survey from "survey-vue";
import { LocalStorage } from "./models/localStorage";

export class SurveyLocalStorage {
    public saveStateLocally(survey: Survey.SurveyModel, storageName: string): void {
        const res = {
            currentPageNo: survey.currentPageNo,
            data: survey.data
        } as LocalStorage;

        if (survey.isDisplayMode === true) {
            res.currentPageNo = 999;
        } else if (survey.state === "completed") {
            res.currentPageNo = 1000;
        }

        // Here should be the code to save the data into your database

        window.localStorage.setItem(storageName, JSON.stringify(res));
    }

    public loadStateLocally(survey: Survey.SurveyModel, storageName: string, defaultDataAsJsonString: string): void {
        // Here should be the code to load the data from your database

        const storageSt = window.localStorage.getItem(storageName) || "";

        let res: LocalStorage;
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

    public clearLocalStorage(storageName: string): void {
        window.localStorage.setItem(storageName, "");
    }
}
