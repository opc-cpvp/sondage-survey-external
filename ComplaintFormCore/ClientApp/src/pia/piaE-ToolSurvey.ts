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
                            SurveyHelper.printWarningMessage("Based on your answer you should reconsider proceeding with this initiative. You may revisit the OPC’s e-Tool once you have determined your legal authority for this program or activity.", "", sender.getLocale());
                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_6") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ1-6");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_8") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ1-8");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
                            return;
                        }

                    } else if (options.oldCurrentPage.name === "page_step_1_q_1_10") {

                        const contactATIP: Survey.Question = sender.getQuestionByName("ContactATIPQ1-10");
                        if (contactATIP && contactATIP.value !== "conduct_pia") {
                            SurveyHelper.printWarningMessage("a message is needed here to tell the user to quit the tool...", "", sender.getLocale());
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
                                let arrayOfItem = behalfMultipleInstitutionOthers.value as any[];
                                arrayOfItem.forEach(item => {

                                    //  Question 2.1.6 - Head of the government institution or delegate
                                    if (item.OtherInstitutionHeadFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.OtherInstitutionHeadFullname)) {
                                            const itemOther: Survey.ItemValue = new Survey.ItemValue(item.OtherInstitutionHeadFullname, item.OtherInstitutionHeadFullname);
                                            personContact.choices.push(itemOther);
                                        }
                                    }

                                    //  Question 2.1.8 - Senior official or executive responsible
                                    if (item.SeniorOfficialOtherFullname) {
                                        if (!personContact.choices.some(contact => contact.value === item.SeniorOfficialOtherFullname)) {
                                            const itemSeniorOther: Survey.ItemValue = new Survey.ItemValue(item.SeniorOfficialOtherFullname, item.SeniorOfficialOtherFullname);
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
                });

                initSurveyModelEvents(_survey);

                initSurveyModelProperties(_survey);

                const defaultData = {
                    "ContactATIPQ16": "conduct_pia",
                    "ContactATIPQ18": "conduct_pia",
                    "ContactATIPQ110": "conduct_pia",
                    "HasLegalAuthority": true,
                    "HeadYourInstitutionEmail": "jack@gmail.com",
                    "HeadYourInstitutionFullname": "Jack Travis",
                    "HeadYourInstitutionSection": "My section",
                    "HeadYourInstitutionTitle": "Boss",
                    "IsNewprogram": false,
                    "IsProgamContractedOut": false,
                    "IsProgramInvolvePersonalInformation": true,
                    "IsTreasuryBoardApproval": true,
                    "LeadInstitutionConsultedOther": "yes",
                    "PersonalInfoUsedFor": "non_admin_purpose",
                    "ProgamHasMajorChanges": true,
                    "ProgramName": "ze programme",
                    "SingleOrMultiInstitutionPIA": "multi",
                    "SubjectOfPIA": "other",
                    "RelevantLegislationPolicies": "jgfjg",
                    "SeniorOfficialEmail": "adam@yates.com",
                    "SeniorOfficialFullname": "adam yates",
                    "SeniorOfficialSection": "michelton",
                    "SeniorOfficialTitle": "rider",
                    "BehalfMultipleInstitutionOthers": [
                        {
                            "OtherInstitutionHeadFullname": "Hugo Roule",
                            "OtherInstitutionEmail": "yougo@hotmail.com",
                            "OtherInstitutionHeadTitle": "Rouleur",
                            "BehalfMultipleInstitutionOther": "2",
                            "OtherInstitutionSection": "Astana",
                            "SeniorOfficialOtherFullname": "Julian Alapolak",
                            "SeniorOfficialOtherTitle": "Puncheur",
                            "SeniorOfficialOtherSection": "QuickStep",
                            "SeniorOfficialOtherEmail": "juju@hotmail.com"
                        },
                        {
                            "OtherInstitutionHeadTitle": "leader",
                            "OtherInstitutionSection": "Astana",
                            "BehalfMultipleInstitutionOther": "Mental Institution",
                            "OtherInstitutionHeadFullname": "Lopez A",
                            "OtherInstitutionEmail": "lopez@gmail.com",
                            "SeniorOfficialOtherFullname": "Tadej Pogachar",
                            "SeniorOfficialOtherTitle": "Sprinteur",
                            "SeniorOfficialOtherSection": "UAE",
                            "SeniorOfficialOtherEmail": "tadeg@mymail.ca"
                        }
                    ],
                    "BehalfMultipleInstitutionLead": "1",
                    "PersonContact": "adam yates",
                    "NewOrUpdatedPIA": "new_pia",
                    "name": "terter",
                    "UserEmailAddress": "test@gmail.com",
                    "BehalfSingleInstitution": "2",
                    "BehalfSingleInstitutionOther": "Another institution",
                    "RelatedPIANameInstitution": "related instituion",
                    "RelatedPIANameProgram": "related program namexxxxx",
                    "RelatedPIADescription": "related descriptiohjfj",
                    "AnotherContactFullname": "Another full name",
                    "AnotherContactTitle": "Another title",
                    "AnotherContactSection": "Another section",
                    "AnotherContactEmail": "anothercontact@gmail.com",
                    "UpdatePIANumberAssigned": "update_pia_existing_reference_number",
                    "UpdatePIAAllReferenceNumbersAssigned": "A3Rt67U8",
                    "DetailsPreviousSubmission": "this is a bunch of details about the previous submission",
                    "NewPIANumberAssigned": "new_pia_existing_reference_number",
                    "NewPIAAllReferenceNumbersAssigned":"V13R5t9O"
                };

                // Load the initial state
                loadStateLocally(_survey, this.storageName_PIA, JSON.stringify(defaultData));

                saveStateLocally(_survey, this.storageName_PIA);

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
