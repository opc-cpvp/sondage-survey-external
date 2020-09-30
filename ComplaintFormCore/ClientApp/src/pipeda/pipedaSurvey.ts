import Vue from "vue";
import * as Survey from "survey-vue";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export class PipedaTool {

    private storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";

    public init(jsonUrl: string, lang: string, token: string): void {

        SurveyInit.initSurvey();

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

                    const uri = `/api/PIPEDASurvey/Validate?complaintId="${sender.complaintId as string}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        body: JSON.stringify(sender.data)
                    }).then(response => {
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
                    }).catch(error => {
                        console.warn(error);
                        Ladda.stopAll();
                    });
                });

                _survey.onComplete.add((sender, options) => {
                    console.log(sender.data);
                    Ladda.stopAll();
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

                    //  We are checking if we are going forward AND we are not at the starting page
                    if (!options.isNextPage || !options.oldCurrentPage) {
                        return;
                    }

                    options.allowChanging = false;
                    options.allowChanging = true;
                });

                _survey.onAfterRenderQuestion.add((sender, options) => {

                });

                survey.onAfterRenderPage.add((sender, options) => {

                    if (options.page.name === "page_part_a_jurisdiction_unable" || options.page.name === "page_part_a_jurisdiction_particulars") {

                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {

                            //  We are setting some dynamic variables depending on the province of incidence selected by the user.
                            const selectedProvinceId = selectedProvinceQuestion.value;

                            _survey.setVariable("province_incidence_prefix_1", "en");   // Default varaible
                            _survey.setVariable("province_incidence_prefix_2", "de");   // Default varaible

                            if (selectedProvinceId === "2") {
                                //  Qwebec
                                if (sender.locale === "fr") {
                                    _survey.setVariable("province_link", "https://www.cai.gouv.qc.ca/");
                                    _survey.setVariable("province_incidence_prefix_1", "au");
                                    _survey.setVariable("province_incidence_prefix_2", "du");
                                } else {
                                    _survey.setVariable("province_link", "https://www.cai.gouv.qc.ca/english/");
                                }

                            } else if (selectedProvinceId === "6") {
                                //  B.C.
                                _survey.setVariable("province_link", "https://www.oipc.bc.ca/for-the-public/");

                                if (sender.locale === "fr") {
                                    _survey.setVariable("province_incidence_prefix_2", "de la");
                                }
                            } else if (selectedProvinceId === "9") {
                                //  Alberta
                                _survey.setVariable("province_link", "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx");

                                if (sender.locale === "fr") {
                                    _survey.setVariable("province_incidence_prefix_2", "de l'");
                                }
                            }
                        }
                    }
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    SurveyLocalStorage.saveStateLocally(sender, this.storageName_PIPEDA);
                });

                SurveyInit.initSurveyModelEvents(_survey);

                SurveyInit.initSurveyModelProperties(_survey);

                const defaultData = { };

                // Load the initial state
                SurveyLocalStorage.loadStateLocally(_survey, this.storageName_PIPEDA, JSON.stringify(defaultData));

                SurveyLocalStorage.saveStateLocally(_survey, this.storageName_PIPEDA);

                // Save the state back to local storage
                //  this.onCurrentPageChanged_saveState(_survey);

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
