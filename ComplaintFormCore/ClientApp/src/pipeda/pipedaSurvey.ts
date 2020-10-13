import Vue from "vue";
import * as Survey from "survey-vue";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import { testData_pipeda } from "./pipeda_test_data";
import { Province } from "../surveyHelper";
import { PipedaProvincesData } from "./pipedaProvinceData";

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

                    const uri = `/api/PipedaSurvey/Validate?complaintId="${sender.complaintId as string}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        // body: JSON.stringify(testData_pipeda)
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

                _survey.onAfterRenderQuestion.add((sender, options) => {});

                survey.onAfterRenderPage.add((sender, options) => {
                    const pagesRequiringProvinceTranslations = [
                        "page_part_a_jurisdiction_unable_1",
                        "page_part_a_jurisdiction_particulars",
                        "page_part_a_customer_or_employee",
                        "page_part_a_jurisdiction_unable_2"
                    ];

                    if (pagesRequiringProvinceTranslations.some(p => p === options.page.name)) {
                        //  Set the french province prefixes for those pages.

                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;
                        if (selectedProvinceQuestion.value) {
                            const selectedProvinceId = Number(selectedProvinceQuestion.value);

                            //  en, au, à...
                            _survey.setVariable(
                                "province_incidence_prefix_au",
                                PipedaProvincesData[selectedProvinceId].French.FrenchPrefix_Au
                            );

                            //  de, du, de la...
                            _survey.setVariable(
                                "province_incidence_prefix_du",
                                PipedaProvincesData[selectedProvinceId].French.FrenchPrefix_Du
                            );
                        }
                    }

                    if (options.page.name === "page_part_a_jurisdiction_unable_1") {
                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {
                            //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                            const selectedProvinceId = selectedProvinceQuestion.value as number;

                            if (sender.locale === "fr") {
                                _survey.setVariable("province_link", PipedaProvincesData[selectedProvinceId].French.Province_link);
                            } else {
                                _survey.setVariable("province_link", PipedaProvincesData[selectedProvinceId].English.Province_link);
                            }
                        }
                    } else if (options.page.name === "page_part_a_jurisdiction_unable_2") {
                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {
                            //  We are setting some dynamic urls depending on the province of incidence selected by the user.

                            const selectedProvinceId = selectedProvinceQuestion.value as number;

                            if (sender.locale === "fr") {
                                _survey.setVariable("link_province_opc", PipedaProvincesData[selectedProvinceId].French.Link_province_opc);
                                _survey.setVariable("link_more_info", PipedaProvincesData[selectedProvinceId].French.Link_province_opc);
                            } else {
                                _survey.setVariable("link_province_opc", PipedaProvincesData[selectedProvinceId].English.Link_province_opc);
                                _survey.setVariable("link_more_info", PipedaProvincesData[selectedProvinceId].French.Link_more_info);
                            }
                        }
                    } else if (options.page.name === "page_part_a_customer_or_employee") {
                        const selectedProvinceQuestion = _survey.getQuestionByName("ProvinceIncidence") as Survey.QuestionRadiogroupModel;

                        if (selectedProvinceQuestion.value) {
                            const selectedProvinceId = selectedProvinceQuestion.value;
                            const nonParticularProvinces = [
                                Province.Ontario,
                                Province.NovaScotia,
                                Province.NewBrunswick,
                                Province.Manitoba,
                                Province.PEI,
                                Province.Saskatchewan
                            ];

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

                const defaultData = {};

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
