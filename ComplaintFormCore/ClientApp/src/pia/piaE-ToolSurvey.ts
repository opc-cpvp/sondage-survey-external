import Vue from "vue";
import * as Survey from "survey-vue";
import {
    saveStateLocally,
    loadStateLocally
} from "../surveyLocalStorage";
import {
    initSurvey,
    initSurveyModelEvents,
    initSurveyModelProperties,
} from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export class PiaETool {

    //  TODO: Figure out if we want to store those const in surveyLocalStorage.ts or in each files
    private storageName_PIA = "SurveyJS_LoadState_PIA";


    public init(jsonUrl: string, lang: string, token: string): void {

        initSurvey();

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {

                const _survey = new Survey.Model(json);
                globalThis.survey = _survey;

                _survey.complaintId = token;

                //  This needs to be here
                _survey.locale = lang;

                // The event onCompleting is fired before the survey is completed and the onComplete event is fired after.
                // You can prevent the survey from completing by setting options.allowComplete to false

                //  We are going to use this variable to handle if the validation has passed or not.
                let isValidSurvey = false;

                _survey.onCompleting.add((sender, options) => {

                    if (isValidSurvey === true) {
                        options.allowComplete = true;
                        return;
                    }

                    options.allowComplete = false;

                    const uri = `/api/PASurvey/Validate?complaintId="${sender.complaintId as string}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        body: JSON.stringify(sender.data)
                    }).then(response => {
                        switch (response.status) {
                            case 200: {
                                //  Validation is good then we set the variable so the next call to doComplete()
                                //  will bypass the validation
                                isValidSurvey = true;
                                _survey.doComplete();
                                break;
                            }
                            case 400:
                            case 500:
                                if (response.json) {
                                    void response.json().then(problem => {
                                        SurveyHelper.printProblemDetails(problem, sender.locale);
                                    });
                                }
                                Ladda.stopAll();
                                return response;
                            default:
                                Ladda.stopAll();
                                return response;
                        }
                    }).catch(error => {
                        console.warn(error);
                        Ladda.stopAll();
                    });
                });

                _survey.onComplete.add((sender, options) => {

                    const uri = `/api/PASurvey/Complete?complaintId="${sender.complaintId as string}`;

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
                                    //  Hide the navigation buttons
                                    const div_navigation = document.getElementById("div_navigation");
                                    if (div_navigation) {
                                        div_navigation.style.display = "none";
                                    }

                                    //  Update the file reference number
                                    void response.json()
                                        .then(responseData => {
                                            const sp_survey_file_number = document.getElementById("sp_survey_file_number");
                                            if (sp_survey_file_number) {
                                                sp_survey_file_number.innerHTML = responseData.referenceNumber;
                                            }
                                        }).catch(error => {
                                            console.warn(error);
                                        });

                                    saveStateLocally(_survey, this.storageName_PIA);

                                    console.log(sender.data);
                                    Ladda.stopAll();
                                    break;
                                }
                                case 400:
                                case 500:
                                    if (response.json) {
                                        void response.json().then(problem => {
                                            SurveyHelper.printProblemDetails(problem, sender.locale);
                                        });
                                    }
                                    Ladda.stopAll();
                                    return response;

                                default:
                                    Ladda.stopAll();
                                    return response;
                            }
                        })
                        .catch(error => {
                            console.warn(error);
                            Ladda.stopAll();
                        });
                });

                _survey.onCurrentPageChanging.add((sender, options) => {

                    //  The event is fired before the current page changes to another page.
                    //  Typically it happens when a user click the 'Next' or 'Prev' buttons.
                    //  sender - the survey object that fires the event.
                    //  option.oldCurrentPage - the previous current/active page.
                    //  option.newCurrentPage - a new current/active page.
                    //  option.allowChanging - set it to `false` to disable the current page changing. It is `true` by default.
                    //  option.isNextPage - commonly means, that end-user press the next page button.
                    //              In general, it means that options.newCurrentPage is the next page after options.oldCurrentPage
                    //  option.isPrevPage - commonly means, that end-user press the previous page button.
                    //              In general, it means that options.newCurrentPage is the previous page before options.oldCurrentPage

                    options.allowChanging = false;

                    if (options.isNextPage === true && options.oldCurrentPage && options.oldCurrentPage.name === "page_before_begin_q_0_1") {

                        const hasLegalAuthority: Survey.Question = sender.getQuestionByName("HasLegalAuthority");
                        if (hasLegalAuthority && hasLegalAuthority.value === false) {
                            SurveyHelper.printWarningMessage("Based on your answer you should reconsider proceeding with this initiative. You may revisit the OPC’s e-Tool once you have determined your legal authority for this program or activity.", "", sender.getLocale());
                            return;
                        }

                    } else if (options.isNextPage === true && options.oldCurrentPage && options.oldCurrentPage.name === "page_step_1_q_1_6") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIP");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
                            return;
                        }
                    } else if (options.isNextPage === true && options.oldCurrentPage && options.oldCurrentPage.name === "page_step_1_q_1_8") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ1-8");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
                            return;
                        }
                    } else if (options.isNextPage === true && options.oldCurrentPage && options.oldCurrentPage.name === "page_step_1_q_1_10") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ1-10");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
                            return;
                        }
                    }

                    options.allowChanging = true;
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    saveStateLocally(sender, this.storageName_PIA);
                });

                initSurveyModelEvents(_survey);

                initSurveyModelProperties(_survey);

                const defaultData  = { };

                // Load the initial state
                loadStateLocally(_survey, this.storageName_PIA, JSON.stringify(defaultData));

                saveStateLocally(_survey, this.storageName_PIA);

                // Save the state back to local storage
                // this.onCurrentPageChanged_saveState(_survey);

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
