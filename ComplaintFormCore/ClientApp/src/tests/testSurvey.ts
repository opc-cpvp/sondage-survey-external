import Vue from "vue";
import * as Survey from "survey-vue";
declare let $: any; // import $ from "jquery";

import * as SurveyInit from "../surveyInit";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import { printProblemDetails } from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";

// declare global {
//    // TODO: get rid of this global variable
//    var survey: Survey.SurveyModel; // eslint-disable-line no-var
// }

export class TestSurvey {
    public init(jsonUrl: string, lang: string, token: string): void {
        function onCurrentPageChanged_saveState(survey) {
            SurveyLocalStorage.saveStateLocally(survey, SurveyLocalStorage.storageName_Test);
        }

        SurveyInit.initSurvey();

        // const jsonUrl = "/sample-data/survey_pa_complaint.json";

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {
                const survey = new Survey.Model(json);
                globalThis.survey = survey;

                survey.complaintId = token;

                //  This needs to be here
                survey.locale = lang;

                SurveyInit.initSurveyModelEvents(survey);

                SurveyInit.initSurveyModelProperties(survey);

                const defaultData = {};

                // Load the initial state
                SurveyLocalStorage.loadStateLocally(survey, SurveyLocalStorage.storageName_Test, JSON.stringify(defaultData));

                // Save the state back to local storage
                onCurrentPageChanged_saveState(survey);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(survey);

                survey.onCompleting.add((sender, options) => {
                    options.allowComplete = false;

                    const data = JSON.stringify(sender.data, null, 3);

                    const xhr = new XMLHttpRequest();
                    xhr.open("POST", `/api/PASurvey/Validate?complaintId="${sender.complaintId as string}`, false);
                    xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                    xhr.onload = xhr.onerror = () => {
                        if (xhr.status === 200) {
                            options.allowComplete = true;
                        } else {
                            const validationResponse = JSON.parse(xhr.response);
                            printProblemDetails(validationResponse, sender.locale);
                        }
                    };
                    xhr.send(data);
                });

                survey.onComplete.add((sender, options) => {
                    const params = { complaintId: sender.complaintId };
                    const query = Object.keys(params)
                        .map(k => `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`)
                        .join("&");
                    const uri = "/api/PASurvey/Complete?" + query;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        body: JSON.stringify(sender.data)
                    })
                        .then(response => {
                            switch (response.status) {
                                case 200: {
                                    console.log(sender.data);
                                    break;
                                }
                                case 400:
                                case 500:
                                    if (response.json) {
                                        void response.json().then(problem => {
                                            printProblemDetails(problem, sender.locale);
                                        });
                                    }

                                    return response;

                                default:
                                    return response;
                            }
                        })
                        .catch(error => {
                            console.warn(error);
                        });
                });

                survey.onCurrentPageChanged.add(onCurrentPageChanged_saveState);

                // ****Event *****************************************************

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey
                    }
                });
            });
    }
}
