import Vue from "vue";
import * as Survey from "survey-vue";
import {
    saveStateLocally,
    loadStateLocally
} from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import { defaultData } from "./pia_test_data";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export class PiaETool {

    //  TODO: Figure out if we want to store those const in surveyLocalStorage.ts or in each files
    private storageName_PIA = "SurveyJS_LoadState_PIA";


    public init(jsonUrl: string, lang: string, token: string): void {

        SurveyInit.initSurvey();

        //  This is a new concept. Users should be allowed to skip pages clicking the 'Skip' button in the
        //  navigation panel. This button is only calling Survey.nextPage(). We just need a way to know if
        //  a page has the 'Skip' button. So that is it: showSkipButton
        Survey.JsonObject.metaData.addProperty("page", {
            name: "showSkipButton:boolean",
            default: false
        });

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

                    const uri = `/api/PIASurvey/Validate?complaintId="${sender.complaintId as string}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        // body: JSON.stringify(testData)
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

                    if (options.oldCurrentPage.name === "page_before_begin_q_0_1") {

                        const hasLegalAuthority: Survey.Question = sender.getQuestionByName("HasLegalAuthority");
                        if (hasLegalAuthority && hasLegalAuthority.value === false) {
                            SurveyHelper.printWarningMessage("Based on your answer you should reconsider proceeding with this initiative. You may revisit the OPC�s e-Tool once you have determined your legal authority for this program or activity.", "", sender.getLocale());
                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_6") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ16");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            if (contactATIP.value === "receive_email") {
                                // Send an email to the user
                                this.sendEmail(sender.complaintId as string, JSON.stringify(sender.data), sender.getLocale());

                            } else {
                                SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "french version of the message", sender.getLocale());
                            }

                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_8") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ18");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            if (contactATIP.value === "receive_email") {
                                // Send an email to the user
                                this.sendEmail(sender.complaintId as string, JSON.stringify(sender.data), sender.getLocale());

                            } else {
                                SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "french version of the message", sender.getLocale());
                            }

                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_10") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ110");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            if (contactATIP.value === "receive_email") {
                                // Send an email to the user
                                this.sendEmail(sender.complaintId as string, JSON.stringify(sender.data), sender.getLocale());

                            } else {
                                SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "french version of the message", sender.getLocale());
                            }

                            return;
                        }
                    } else if (options.oldCurrentPage.name === "page_step_3_1_q_3_1_7") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ317");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            if (contactATIP.value === "receive_email") {
                                // Send an email to the user
                                this.sendEmail(sender.complaintId as string, JSON.stringify(sender.data), sender.getLocale());
                            } else {
                                SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "french version of the message", sender.getLocale());
                            }

                            return;
                        }
                    }

                    options.allowChanging = true;
                });

                _survey.onAfterRenderQuestion.add((sender, options) => {

                    if (options.question.name === "PersonContact") {

                        //  We are building a list of person contacts to use as choices for the
                        //  dropdown type question PersonContact at question 2.1.9

                        const personContact = options.question as Survey.QuestionDropdownModel;
                        personContact.choices = [];

                        //  1) We add another individual item
                        let otherName = "Another individual";
                        if (sender.getLocale() === "fr") {
                            otherName = "Autre individu";
                        }

                        const itemOther: Survey.ItemValue = new Survey.ItemValue("another", otherName);
                        personContact.choices.push(itemOther);

                        //  2) Question 2.1.5 - Who is the head of the government institution
                        const headYourInstitutionFullname = _survey.getQuestionByName("HeadYourInstitutionFullname") as Survey.QuestionTextModel;
                        if (headYourInstitutionFullname) {
                            const item: Survey.ItemValue =
                                new Survey.ItemValue(headYourInstitutionFullname.value, headYourInstitutionFullname.value);
                            personContact.choices.push(item);
                        }

                        //  3) Question 2.1.7 - Senior official or executive responsible
                        const seniorOfficialFullname = _survey.getQuestionByName("SeniorOfficialFullname") as Survey.QuestionTextModel;
                        if (seniorOfficialFullname) {
                            const item: Survey.ItemValue = new Survey.ItemValue(seniorOfficialFullname.value, seniorOfficialFullname.value);
                            personContact.choices.push(item);
                        }

                        const singleOrMultiInstitutionPIA = _survey.getQuestionByName("SingleOrMultiInstitutionPIA") as Survey.QuestionRadiogroupModel;
                        if (singleOrMultiInstitutionPIA.selectedItem && singleOrMultiInstitutionPIA.selectedItem.value === "multi") {

                            const behalfMultipleInstitutionOthers = _survey.getQuestionByValueName("BehalfMultipleInstitutionOthers");
                            if (behalfMultipleInstitutionOthers && behalfMultipleInstitutionOthers.value) {
                                const arrayOfItem = behalfMultipleInstitutionOthers.value as any[];
                                arrayOfItem.forEach(item => {

                                    //  Question 2.1.6 - Head of the government institution or delegate
                                    if (item.OtherInstitutionHeadFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.OtherInstitutionHeadFullname)) {
                                            const itemOtherInstitutionHeadFullname: Survey.ItemValue =
                                                new Survey.ItemValue(item.OtherInstitutionHeadFullname, item.OtherInstitutionHeadFullname);
                                            personContact.choices.push(itemOtherInstitutionHeadFullname);
                                        }
                                    }

                                    //  Question 2.1.8 - Senior official or executive responsible
                                    if (item.SeniorOfficialOtherFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.SeniorOfficialOtherFullname)) {
                                            const itemSeniorOther: Survey.ItemValue =
                                                new Survey.ItemValue(item.SeniorOfficialOtherFullname, item.SeniorOfficialOtherFullname);
                                            personContact.choices.push(itemSeniorOther);
                                        }
                                    }
                                });
                            }
                        }
                    }
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    saveStateLocally(sender, this.storageName_PIA);

                    //  Across all questions in Step 3, users who selected �Update or addendum� in Question 2.1.10 should
                    //  have a �Skip this question� option that allows them to advance in the tool without answering the question
                    const skipButton = document.getElementById("btnSkip") ?? new HTMLElement();
                    skipButton.classList.remove("hidden");
                    skipButton.classList.remove("inline");
                    skipButton.classList.add(options.newCurrentPage && options.newCurrentPage.showSkipButton ? "inline" : "hidden");

                    this.setNavigationBreadcrumbs(_survey);
                });

                SurveyInit.initSurveyModelEvents(_survey);

                SurveyInit.initSurveyModelProperties(_survey);

                //  const defaultData = {};

                // Load the initial state
                loadStateLocally(_survey, this.storageName_PIA, JSON.stringify(defaultData));

                saveStateLocally(_survey, this.storageName_PIA);

                // Save the state back to local storage
                //  this.onCurrentPageChanged_saveState(_survey);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(_survey);

                this.setNavigationBreadcrumbs(_survey);

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: _survey
                    }
                });
            });
    }

    public sendEmail(complaintId: string, data: string, locale: string): void {

        const uri = `/api/PIASurvey/SendEmail?complaintId="${complaintId}`;

        fetch(uri, {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json; charset=utf-8"
            },
            body: data
        }).then(response => {
            if (response.ok) {

                SurveyHelper.printWarningMessage("A summary has been sent to the email address provided", "french version of the message", locale);

            } else {
                if (response.json) {
                    void response.json().then(problem => {
                        SurveyHelper.printProblemDetails(problem, locale);
                    });
                }
                Ladda.stopAll();
                return response;
            }
        }).catch(error => {
            console.warn(error);
            Ladda.stopAll();
        });
    }

    public gotoSection(surveyObj: Survey.SurveyModel, section: number): void {

        //  This is part of the navigation. When a user clicks on a breadcrums item it takes them
        //  directly where they want.

        if (section === 0) {
            surveyObj.currentPage = 0;
        } else if (section === 1) {
            surveyObj.currentPage = 5;
        } else if (section === 2) {
            surveyObj.currentPage = 14;
        } else if (section === 3) {
            surveyObj.currentPage = 39;
        }
    }

    private setNavigationBreadcrumbs(surveyObj: Survey.VueSurveyModel): void {

        //  TODO: Probably disable some items when the user has not gone thru all the question.

        const ul_progress_navigation = document.getElementById("ul_pia_navigation") as HTMLUListElement;

        if (survey.isDisplayMode) {
            ul_progress_navigation.className = "hidden";
        } else {
            ul_progress_navigation.className = "breadcrumb";
        }

        if (ul_progress_navigation) {

            const items = document.getElementsByClassName("breadcrumb-item");

            Array.from(items).forEach(li => {
                //  Reset the original class on each <li> item
                li.className = "breadcrumb-item";
            });

            if (surveyObj.currentPageNo < 5) {
                const li = document.getElementById("li_breadcrumb_0");
                if (li) {
                    li.className += " active";
                }
            } else if (surveyObj.currentPageNo < 14) {
                const li = document.getElementById("li_breadcrumb_1");
                if (li) {
                    li.className += " active";
                }
            } else if (surveyObj.currentPageNo < 39) {
                const li = document.getElementById("li_breadcrumb_2");
                if (li) {
                    li.className += " active";
                }
            } else if (surveyObj.currentPageNo < 999) {
                const li = document.getElementById("li_breadcrumb_3");
                if (li) {
                    li.className += " active";
                }
            }
        }
    }
}
