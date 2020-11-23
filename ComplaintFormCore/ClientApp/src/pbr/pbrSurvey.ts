import Vue from "vue";
import * as Survey from "survey-vue";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import { pbr_test_data } from "./pbr_test_data";
import * as SurveyHelper from "../surveyHelper";
import * as widgets from "surveyjs-widgets";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export class PbrSurvey {
    public init(jsonUrl: string, lang: string, token: string): void {
        SurveyInit.initSurvey();

        //  Initialize jqueryuidatepicker used for datepickers
        widgets.jqueryuidatepicker(Survey);

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {
                const _survey = new Survey.Model(json);
                globalThis.survey = _survey;

                _survey.complaintId = token;

                //  This needs to be here
                _survey.locale = lang;

                //  We are going to use this variable to handle if the validation has passed or not.
                let isValidSurvey = false;

                _survey.onCompleting.add((sender, options) => {
                    if (isValidSurvey === true) {
                        options.allowComplete = true;
                        return;
                    }

                    options.allowComplete = false;

                    const uri = `/api/PBRSurvey/Validate?complaintId="${sender.complaintId as string}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        // body: JSON.stringify(pbr_test_data)
                        body: JSON.stringify(sender.data)
                    })
                        .then(response => {
                            if (response.ok) {
                                //  Validation is good then we set the variable so the next call to doComplete()
                                //  will bypass the validation
                                isValidSurvey = true;
                                _survey.doComplete();
                            } else {
                                if (response.json) {
                                    void response.json().then(problem => {
                                        SurveyHelper.printProblemDetails(problem, sender.locale);
                                    });
                                }
                                Ladda.stopAll();
                                return response;
                            }
                        })
                        .catch(error => {
                            console.warn(error);
                            Ladda.stopAll();
                        });
                });

                _survey.onComplete.add((sender, options) => {
                    console.log(sender.data);

                    const div_navigation = document.getElementById("div_navigation");
                    if (div_navigation) {
                        div_navigation.style.display = "none";
                    }

                    Ladda.stopAll();
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    SurveyLocalStorage.saveStateLocally(sender, SurveyLocalStorage.storageName_PBR);
                });

                SurveyInit.initSurveyModelEvents(_survey);

                SurveyInit.initSurveyModelProperties(_survey);

                const defaultData = {};

                // Load the initial state
                SurveyLocalStorage.loadStateLocally(_survey, SurveyLocalStorage.storageName_PBR, JSON.stringify(defaultData));

                SurveyLocalStorage.saveStateLocally(_survey, SurveyLocalStorage.storageName_PBR);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(_survey);

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: _survey
                    }
                });
            });
    }
}
