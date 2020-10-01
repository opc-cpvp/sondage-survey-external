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

                    const pagesRequiringProvinceTranslations = ["page_part_a_jurisdiction_unable_1", "page_part_a_jurisdiction_particulars", "page_part_a_customer_or_employee", "page_part_a_jurisdiction_unable_2"];

                    if (pagesRequiringProvinceTranslations.some(p => p === options.page.name)) {

                        //  Set the french province prefixes for those pages.

                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;
                        if (selectedProvinceQuestion.value) {
                            const selectedProvinceId = Number(selectedProvinceQuestion.value);

                            //  en, au, à...
                            _survey.setVariable("province_incidence_prefix_au", SurveyHelper.getProvinceFrenchPrefix_au(selectedProvinceId));

                            //  de, du, de la...
                            _survey.setVariable("province_incidence_prefix_du", SurveyHelper.getProvinceFrenchPrefix_du(selectedProvinceId));
                        }
                    }

                    if (options.page.name === "page_part_a_jurisdiction_unable_1") {

                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {

                            //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                            const selectedProvinceId = selectedProvinceQuestion.value as number;

                            if (selectedProvinceId === 2) {
                                //  Qwebec
                                if (sender.locale === "fr") {
                                    _survey.setVariable("province_link", "https://www.cai.gouv.qc.ca/");
                                } else {
                                    _survey.setVariable("province_link", "https://www.cai.gouv.qc.ca/english/");
                                }

                            } else if (selectedProvinceId === 6) {
                                //  B.C.
                                _survey.setVariable("province_link", "https://www.oipc.bc.ca/for-the-public/");
                            } else if (selectedProvinceId === 9) {
                                //  Alberta
                                _survey.setVariable("province_link", "https://www.oipc.ab.ca/action-items/request-a-review-file-a-complaint.aspx");
                            }
                        }
                    } else if (options.page.name === "page_part_a_jurisdiction_unable_2") {

                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;
                        if (selectedProvinceQuestion.value) {

                            //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                            const selectedProvinceId = selectedProvinceQuestion.value as number;

                            _survey.setVariable("link_similar_to_pipeda_en", "https://www.priv.gc.ca/en/privacy-topics/privacy-laws-in-canada/the-personal-information-protection-and-electronic-documents-act-pipeda/r_o_p/provincial-legislation-deemed-substantially-similar-to-pipeda/");

                            _survey.setVariable("link_similar_to_pipeda_fr", "https://www.priv.gc.ca/fr/sujets-lies-a-la-protection-de-la-vie-privee/lois-sur-la-protection-des-renseignements-personnels-au-canada/la-loi-sur-la-protection-des-renseignements-personnels-et-les-documents-electroniques-lprpde/r_o_p/lois-provinciales-essentiellement-similaires-a-la-lprpde/");

                            if (selectedProvinceId === 1) {  //  Ontario
                                if (sender.locale === "fr") {
                                    _survey.setVariable("link_province_opc", "https://www.ipc.on.ca/protection-de-la-vie-privee-particuliers/proteger-sa-vie-privee-2/?lang=fr");
                                } else {
                                    _survey.setVariable("link_province_opc", "https://www.ipc.on.ca/privacy/filing-a-privacy-complaint/");
                                }
                            } else if (selectedProvinceId === 3) {    //  Nova Scotia
                                if (sender.locale === "fr") {
                                    survey.setVariable("link_province_opc", "https://foipop.ns.ca/publictools");
                                } else {
                                    survey.setVariable("link_province_opc", "https://foipop.ns.ca/publictools");
                                }
                            } else if (selectedProvinceId === 4) {     //  New Brunswick

                                if (sender.locale === "fr") {
                                    survey.setVariable("link_province_opc", "https://oic-bci.ca/?lang=fr");
                                } else {
                                    _survey.setVariable("link_province_opc", "http://www.beta-theta.com/information-and-privacy.html");
                                }
                            } else if (selectedProvinceId === 5) {   // Manitoba
                                if (sender.locale === "fr") {
                                    _survey.setVariable("link_more_info", "https://www.ombudsman.mb.ca/info/access-and-privacy-fr.html");
                                } else {
                                    _survey.setVariable("link_more_info", "https://www.ombudsman.mb.ca/info/access-and-privacy-division.html");
                                }
                            } else if (selectedProvinceId === 7) {   // PEI

                                survey.setVariable("link_more_info", "https://www.assembly.pe.ca/");

                            } else if (selectedProvinceId === 8) {   // Saskatchewan

                                _survey.setVariable("link_more_info", "https://oipc.sk.ca/");

                            } else if (selectedProvinceId === 10) {    // Newfound land

                                survey.setVariable("link_province_opc", "https://www.oipc.nl.ca/public/investigations/privacy");
                            }
                        }
                    } else if (options.page.name === "page_part_a_customer_or_employee") {
                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {
                            const selectedProvinceId = selectedProvinceQuestion.value;
                            const nonParticularProvinces = ["1", "3", "4", "5", "7", "8"];

                            if (nonParticularProvinces.some(p => p === selectedProvinceId)) {
                                //  Referes to AnsweredOrganizationsQuestion()
                                _survey.setVariable("plural", "s");
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
