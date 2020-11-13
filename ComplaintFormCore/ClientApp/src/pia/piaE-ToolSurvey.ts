import Vue from "vue";
import * as Survey from "survey-vue";
import * as SurveyLocalStorage from "../surveyLocalStorage";
import * as SurveyInit from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import { piaTestData } from "./pia_test_data";
import * as widgets from "surveyjs-widgets";
import * as SurveyFile from "../surveyFile";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

export const PartiesSharePersonalInformationArray = [
    {
        text: {
            en: "Another program area within the same institution",
            fr: ""
        },
        value: "same_institution"
    },
    {
        text: {
            en: "Another federal government institution",
            fr: ""
        },
        value: "federal_government_institution"
    },
    {
        text: {
            en: "A provincial or territorial government in Canada",
            fr: ""
        },
        value: "provincial_in_canada"
    },
    {
        text: {
            en: "A regional or municipal government in Canada",
            fr: ""
        },
        value: "regional_in_canada"
    },
    {
        text: {
            en: "A government outside of Canada",
            fr: ""
        },
        value: "government_outside_canada"
    },
    {
        text: {
            en: "A non-governmental organization in Canada (for example, a non-for-profit or registered charity)",
            fr: ""
        },
        value: "non_governmental_organization_in_canada"
    },
    {
        text: {
            en: "A non-governmental organization outside of Canada",
            fr: ""
        },
        value: "non_governmental_organization_outside_canada"
    },
    {
        text: {
            en: "A private-sector organization in Canada",
            fr: ""
        },
        value: "private_sector_in_canada"
    },
    {
        text: {
            en: "A private-sector organization outside of Canada",
            fr: ""
        },
        value: "private_sector_outside_canada"
    },
    {
        text: {
            en: "Other",
            fr: ""
        },
        value: "other"
    }
];

export class PiaETool {
    // TODO: Figure out if we want to store those const in surveyLocalStorage.ts or in each files
    private storageName_PIA = "SurveyJS_LoadState_PIA";

    public init(jsonUrl: string, lang: string, token: string): void {
        SurveyInit.initSurvey();
        SurveyFile.initSurveyFile();

        Survey.JsonObject.metaData.addProperty("page", {
            name: "section:number",
            default: false
        });

        widgets.select2tagbox(Survey);

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

                    if (options.oldCurrentPage.name === "page_before_begin_q_0_1") {
                        const hasLegalAuthority: Survey.Question = sender.getQuestionByName("HasLegalAuthority");
                        if (hasLegalAuthority && hasLegalAuthority.value === false) {
                            SurveyHelper.printWarningMessage(
                                "Based on your answer you should reconsider proceeding with this initiative. You may revisit the OPC�s e-Tool once you have determined your legal authority for this program or activity.",
                                "",
                                sender.getLocale()
                            );
                            return;
                        }
                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_6") {
                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ16");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            if (contactATIP.value === "receive_email") {
                                // Send an email to the user
                                this.sendEmail(sender.complaintId as string, JSON.stringify(sender.data), sender.getLocale());
                            } else {
                                SurveyHelper.printWarningMessage(
                                    "a message is needed here to tell the user to quit the tool...",
                                    "french version of the message",
                                    sender.getLocale()
                                );
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
                                SurveyHelper.printWarningMessage(
                                    "a message is needed here to tell the user to quit the tool...",
                                    "french version of the message",
                                    sender.getLocale()
                                );
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
                                SurveyHelper.printWarningMessage(
                                    "a message is needed here to tell the user to quit the tool...",
                                    "french version of the message",
                                    sender.getLocale()
                                );
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
                                SurveyHelper.printWarningMessage(
                                    "a message is needed here to tell the user to quit the tool...",
                                    "french version of the message",
                                    sender.getLocale()
                                );
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
                        const headYourInstitutionFullname = _survey.getQuestionByName(
                            "HeadYourInstitutionFullname"
                        ) as Survey.QuestionTextModel;
                        if (headYourInstitutionFullname) {
                            const item: Survey.ItemValue = new Survey.ItemValue(
                                headYourInstitutionFullname.value,
                                headYourInstitutionFullname.value
                            );
                            personContact.choices.push(item);
                        }

                        //  3) Question 2.1.7 - Senior official or executive responsible
                        const seniorOfficialFullname = _survey.getQuestionByName("SeniorOfficialFullname") as Survey.QuestionTextModel;
                        if (seniorOfficialFullname) {
                            const item: Survey.ItemValue = new Survey.ItemValue(seniorOfficialFullname.value, seniorOfficialFullname.value);
                            personContact.choices.push(item);
                        }

                        const singleOrMultiInstitutionPIA = _survey.getQuestionByName(
                            "SingleOrMultiInstitutionPIA"
                        ) as Survey.QuestionRadiogroupModel;
                        if (singleOrMultiInstitutionPIA.selectedItem && singleOrMultiInstitutionPIA.selectedItem.value === "multi") {
                            const behalfMultipleInstitutionOthers = _survey.getQuestionByValueName("BehalfMultipleInstitutionOthers");
                            if (behalfMultipleInstitutionOthers && behalfMultipleInstitutionOthers.value) {
                                const arrayOfItem = behalfMultipleInstitutionOthers.value as any[];
                                arrayOfItem.forEach(item => {
                                    //  Question 2.1.6 - Head of the government institution or delegate
                                    if (item.OtherInstitutionHeadFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.OtherInstitutionHeadFullname)) {
                                            const itemOtherInstitutionHeadFullname: Survey.ItemValue = new Survey.ItemValue(
                                                item.OtherInstitutionHeadFullname,
                                                item.OtherInstitutionHeadFullname
                                            );
                                            personContact.choices.push(itemOtherInstitutionHeadFullname);
                                        }
                                    }

                                    //  Question 2.1.8 - Senior official or executive responsible
                                    if (item.SeniorOfficialOtherFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.SeniorOfficialOtherFullname)) {
                                            const itemSeniorOther: Survey.ItemValue = new Survey.ItemValue(
                                                item.SeniorOfficialOtherFullname,
                                                item.SeniorOfficialOtherFullname
                                            );

                                            personContact.choices.push(itemSeniorOther);
                                        }
                                    }
                                });
                            }
                        }
                    } else if (options.question.name === "pnd_PurposeOfNotAllDisclosed") {
                        //  For this question, we need to populate the dropdown named 'ReceivingParties' inside the
                        //  paneldynamic 'pnd_PurposeOfDisclosure' with what the user has selected in a previous question.

                        const pnd_PurposeOfDisclosure = options.question as Survey.QuestionPanelDynamicModel;
                        if (pnd_PurposeOfDisclosure) {
                            //  receivingParties is the dropdown to be populated
                            const receivingParties = pnd_PurposeOfDisclosure.templateElements[1] as Survey.QuestionDropdownModel;
                            if (receivingParties) {
                                //  We are going to get the user's selection from the matrixdynamic 'PartiesSharePersonalInformation'
                                const otherPartiesSharePersonalInformation = sender.getQuestionsByValueNameCore(
                                    "OtherPartiesSharePersonalInformation"
                                ) as Survey.QuestionMatrixDynamicModel;
                                const arrayOfItem = otherPartiesSharePersonalInformation[0].value as any[];
                                arrayOfItem.forEach(item => {
                                    const selectedItem: Survey.ItemValue = new Survey.ItemValue(item.Party, item.Party);

                                    receivingParties.choices.push(selectedItem);
                                });
                            }
                        }
                    }
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    SurveyLocalStorage.saveStateLocally(sender, this.storageName_PIA);

                    this.setNavigationBreadcrumbs(_survey);
                });

                SurveyInit.initSurveyModelEvents(_survey);

                SurveyInit.initSurveyModelProperties(_survey);

                const defaultData = {};

                // Load the initial state
                SurveyLocalStorage.loadStateLocally(_survey, this.storageName_PIA, JSON.stringify(piaTestData));

                SurveyLocalStorage.saveStateLocally(_survey, this.storageName_PIA);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(_survey);

                SurveyFile.initSurveyFileModelEvents(_survey, "pia");

                this.setNavigationBreadcrumbs(_survey);

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: _survey
                    }
                });
            });
    }

    //  This is a nextPage() function just for this page
    public nextPage(surveyObj: Survey.SurveyModel): void {
        if (surveyObj.currentPage.name === "page_step_3_3_7") {
            this.gotoPage(surveyObj, "page_step_3_3_5");
        } else {
            SurveyNavigation.nextPage(surveyObj);
        }
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
        })
            .then(response => {
                if (response.ok) {
                    SurveyHelper.printWarningMessage(
                        "A summary has been sent to the email address provided",
                        "french version of the message",
                        locale
                    );
                } else {
                    if (response.json) {
                        void response.json().then(problem => {
                            SurveyHelper.printProblemDetails(problem, locale);
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
    }

    public gotoSection(surveyObj: Survey.SurveyModel, section: number): void {
        //  This is part of the navigation. When a user clicks on a breadcrums item it takes them
        //  directly where they want.

        //  Filter to get the first page with the desired section
        const page = surveyObj.pages.filter(x => x.getPropertyValue("section") === section && x.isVisible === true)[0];
        if (page) {
            surveyObj.currentPage = page;
            this.setNavigationBreadcrumbs(surveyObj);
        }
    }

    public gotoPage(surveyObj: Survey.SurveyModel, pageName: string): void {
        const page = surveyObj.getPageByName(pageName);
        if (page) {
            surveyObj.currentPage = page;
        }
    }

    private setNavigationBreadcrumbs(surveyObj: Survey.SurveyModel): void {
        //  TODO: Probably disable some items when the user has not gone thru all the question.

        const ul_progress_navigation = document.getElementById("ul_pia_navigation") as HTMLUListElement;

        if (!ul_progress_navigation) {
            return;
        }

        if (survey.isDisplayMode) {
            //  We do not show the navigation bar in preview mode
            ul_progress_navigation.className = "hidden";
            return;
        }

        ul_progress_navigation.className = "breadcrumb";

        if (surveyObj.currentPage.section === null) {
            //  If for any reasons we forget to add the section property in the json at least nothing will happen
            return;
        }

        const items = document.getElementsByClassName("breadcrumb-item");

        Array.from(items).forEach(li => {
            //  Reset the original class on each <li> item
            li.className = "breadcrumb-item";
        });

        const section: string = surveyObj.currentPage.section;
        const li_breadcrumb = document.getElementById(`li_breadcrumb_${section}`);
        if (li_breadcrumb) {
            li_breadcrumb.className += " active";
        }
    }
}
