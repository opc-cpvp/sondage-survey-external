import Vue from "vue";
import * as Survey from "survey-vue";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import * as SurveyFile from "../surveyFile";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export class PbrSurvey {
    public init(jsonUrl: string, lang: string, token: string): void {
        SurveyInit.initSurvey();
        SurveyFile.initSurveyFile();

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {
                const _survey = new Survey.Model(json);
                globalThis.survey = _survey;

                _survey.complaintId = token;

                //  This needs to be here
                _survey.locale = lang;

                //  We are going to use this variable to handle if the validation has passed or not.
                const isValidSurvey = false;

                _survey.onCompleting.add((sender, options) => {
                    if (isValidSurvey === true) {
                        options.allowComplete = true;
                        return;
                    }

                    options.allowComplete = false;
                });

                _survey.onComplete.add((sender, options) => {
                    console.log(sender.data);
                    Ladda.stopAll();
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    SurveyLocalStorage.saveStateLocally(sender, SurveyLocalStorage.storageName_PBP);
                });

                SurveyInit.initSurveyModelEvents(_survey);

                SurveyInit.initSurveyModelProperties(_survey);

                const defaultData = {};

                // Load the initial state
                SurveyLocalStorage.loadStateLocally(_survey, SurveyLocalStorage.storageName_PBP, JSON.stringify(defaultData));

                SurveyLocalStorage.saveStateLocally(_survey, SurveyLocalStorage.storageName_PBP);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(_survey);

                SurveyFile.initSurveyFileModelEvents(_survey);

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: _survey
                    }
                });
            });
    }
}
