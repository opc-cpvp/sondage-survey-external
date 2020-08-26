import Vue from "vue";
import * as Survey from "survey-vue";
import {
    saveStateLocally,
    storageName_PA,
    loadStateLocally
} from "../surveyLocalStorage";
import {
    initSurvey,
    initSurveyModelEvents,
    initSurveyModelProperties,
    onCurrentPageChanged_updateNavButtons
} from "../surveyInit";
import { initSurveyFile, initSurveyFileModelEvents } from "../surveyFile";
import { printProblemDetails, getTranslation } from "../surveyHelper";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

// This is the total file sizes
const multipleFileMaxSize = 26214400;

export class PaSurvey {
    // This function is to update the html for the question of type html named 'documentation_info'.
    // It will removed the hidden css on some of the <li> depending on some conditions
    private updateDocumentationInfoSection(survey, options) {

        const ul_documentation_info = document.getElementById("ul_documentation_info");
        if (ul_documentation_info == null) {
            return;
        }

        if (survey.data["RaisedPrivacyToAtipCoordinator"] === "yes") {

            const liNode = ul_documentation_info.querySelector(".raisedPrivacyToAtipCoordinator");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (survey.data["FilingComplaintOnOwnBehalf"] === "someone_else") {
            const liNode = ul_documentation_info.querySelector(".filingComplaintOnOwnBehalf");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (survey.data["NatureOfComplaint"].filter(x => x === "NatureOfComplaintDenialOfAccess").length > 0) {
            const liNode = ul_documentation_info.querySelector(".natureOfComplaint");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }
    }

    private onCurrentPageChanged_saveState(survey) {
        saveStateLocally(survey, storageName_PA);
    }

    // This is to get the total number of bytes for both file uploads.
    // We will compare with the max value later.
    // Note that the file size is being stored in file.content
    private getTotalFileSize(survey, options) {
        let totalBytes = 0;

        options.question.value.forEach(fileItem => {
            totalBytes = totalBytes + parseInt(fileItem.content, 10);
        });

        // We need to calculate the total size of all files for both file upload

        if (options.question.name === "documentation_file_upload") {
            const rep_file_upload = survey.getQuestionByName("documentation_file_upload_rep");

            if (rep_file_upload && rep_file_upload.value) {
                rep_file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content, 10);
                });
            }
        } else if (options.question.name === "documentation_file_upload_rep") {

            const file_upload = survey.getQuestionByName("documentation_file_upload");

            if (file_upload && file_upload.value) {
                file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content, 10);
                });
            }
        }

        return totalBytes;
    }

    public init(jsonUrl: string, lang: string, token: string): void {
        initSurvey();
        initSurveyFile();

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {

                const survey = new Survey.Model(json);
                globalThis.survey = survey;

                survey.complaintId = token;

                //  This needs to be here
                survey.locale = lang;

                // Add events only applicable to this page **********************

                // ISSUE WITH THE FLOW: When calling survey.onComplete the completed page shows right away. This is
                // not desired. If the validation doesn't pas, we don't want to go to the completed page. So to do that,
                // we call survey.onCompleting.
                // The event onCompleting is fired before the survey is completed and the onComplete event is fired.
                // You can prevent the survey from completing by setting options.allowComplete to false
                // sender - the survey object that fires the event.
                // NOTE:   The api call needs to be done synchronously for the onComplete event to fire.
                //          This is because the call is waiting for options.allowComplete = true
                survey.onCompleting.add((sender, options) => {
                    options.allowComplete = false;

                    const data = JSON.stringify(sender.data, null, 3);

                    const xhr = new XMLHttpRequest();
                    xhr.open(
                        "POST",
                        "/api/PASurvey/Validate?complaintId=" +
                        sender.complaintId,
                        false
                    );
                    xhr.setRequestHeader(
                        "Content-Type",
                        "application/json; charset=utf-8"
                    );
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
                                case 200:
                                    //  Hide the navigation buttons
                                    const div_navigation = document.getElementById("div_navigation");
                                    if (div_navigation) {
                                        div_navigation.style.display = "none";
                                    }

                                    //  Update the file reference number
                                    response.json().then(responseData => {

                                        const sp_survey_file_number = document.getElementById("sp_survey_file_number");
                                        if (sp_survey_file_number) {
                                            sp_survey_file_number.innerHTML = responseData.referenceNumber;
                                        }
                                    });

                                    saveStateLocally(survey, storageName_PA);

                                    console.log(sender.data);
                                    break;
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

                survey.onAfterRenderQuestion.add((sender, options) => {
                    if (options.question.getType() === "html" && options.question.name === "documentation_info") {

                        this.updateDocumentationInfoSection(sender, options);
                    } else if (options.question.getType() === "file" && options.question.value) {

                        // Getting the total size of all uploaded files
                        const totalBytes = this.getTotalFileSize(sender, options);

                        const sizeInMB = (totalBytes / 1000000).toFixed(2);

                        // Setting up the <meter> values
                        const spanTotal = document.getElementById("sp_total");
                        if (spanTotal != null) {
                            spanTotal.innerHTML = sizeInMB;
                        }

                        const meterElement = document.getElementById("meter_upload_total_mb") as HTMLMeterElement;
                        if (meterElement != null) {
                            meterElement.value = totalBytes;
                        }
                    }
                });

                // Adding particular event for this page only
                survey.onCurrentPageChanged.add((sender, options) => {
                    this.onCurrentPageChanged_saveState(sender);
                });

                survey.onValidateQuestion.add((sender, options) => {
                    if (options.question.getType() === "file" && options.question.value) {
                        // Getting the total size of all uploaded files
                        const totalBytes = this.getTotalFileSize(survey, options);

                        if (multipleFileMaxSize > 0 && totalBytes > multipleFileMaxSize) {
                            options.error = getTranslation(options.question.multipleFileMaxSizeErrorMessage, survey.locale);
                            // return false;
                        }

                        const sizeInMB = (totalBytes / 1000000).toFixed(2);

                        // Setting up the <meter> values
                        const spanTotal = document.getElementById("sp_total");
                        if (spanTotal != null) {
                            spanTotal.innerHTML = sizeInMB;
                        }

                        const meterElement = document.getElementById("meter_upload_total_mb") as HTMLMeterElement;
                        if (meterElement != null) {
                            meterElement.value = totalBytes;
                        }
                    }
                });

                survey.onServerValidateQuestions.add((sender, options) => {

                    if (options.data["documentation_type"] === "upload" || options.data["documentation_type"] === "both") {

                        //  Validating the documentation page if and only if there is documents to be validated

                        const params = { complaintId: sender.complaintId };
                        const query = Object.keys(params).map(k => `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`).join("&");
                        const uri = "/api/PASurvey/ValidateAttachments?" + query;

                        fetch(uri, {
                            method: "POST",
                            headers: {
                                Accept: "application/json", "Content-Type": "application/json; charset=utf-8"
                            },
                            body: JSON.stringify(options.data)
                        })
                            .then(response => {
                                switch (response.status) {
                                    case 200:
                                        //  This will allowed the validation to pass and go to the next page
                                        options.complete();
                                        break;
                                    case 400:
                                    case 500:
                                        if (response.json) {
                                            response.json().then(problem => {
                                                printProblemDetails(problem, sender.locale);
                                            });
                                        } else {
                                            console.warn(response);
                                        }

                                        break;
                                    default:
                                        console.warn(response);
                                }
                            })
                            .catch(error => {
                                console.warn(error);
                            });
                    } else {
                        options.complete();
                    }
                });

                // ****Event *****************************************************

                initSurveyModelEvents(survey);

                initSurveyModelProperties(survey);

                initSurveyFileModelEvents(survey);

                //  TODO:   for now, in order to be able to use the checkboxes with addiotional htnl info, we
                //          need to specify the array object type_complaint. There has to be a more elegant way of doing this.
                // var defaultData  = { };

                // var defaultData = {
                //    "type_complaint": []
                // };

                const defaultData = {
                    "FilingComplaintOnOwnBehalf": "yourself",
                    "RaisedPrivacyToAtipCoordinator": "yes",
                    "WhichFederalGovernementInstitutionComplaintAgainst": "3",
                    "NatureOfComplaint": [
                        "NatureOfComplaintOther",
                        "NatureOfComplaintDelay",
                        "NatureOfComplaintExtensionOfTime",
                        "NatureOfComplaintCollection"
                    ],
                    "IsEmployeeChoice": "general_public",
                    "AdditionalComments": "iuyiuyuiyiuy",
                    "complainant_HaveYouSubmittedBeforeChoice": "no",
                    "complainant_FormOfAddress": "Mr.",
                    "complainant_FirstName": "jf",
                    "complainant_LastName": "brouillette",
                    "complainant_Email": "jf@hotmail.com",
                    "complainant_MailingAddress": "66",
                    "complainant_City": "gat",
                    "complainant_PostalCode": "J9A2V5",
                    "complainant_DayTimeNumber": "6135656667",
                    "NeedsDisabilityAccommodationChoice": "yes",
                    "DisabilityAccommodation": "iuyuiyiuyiuy",
                    "complainant_Country": "CA",
                    "complainant_ProvinceOrState": "2",
                    "reprensentative_FormOfAddress": "Mr.",
                    "reprensentative_FirstName": "jf",
                    "reprensentative_LastName": "brouillette",
                    "reprensentative_Email": "jf@hotmail.com",
                    "reprensentative_MailingAddress": "66",
                    "reprensentative_City": "gat",
                    "reprensentative_PostalCode": "J9A2V5",
                    "reprensentative_DayTimeNumber": "6135656667",
                    "documentation_type": "none",
                    "WhatWouldResolveYourComplaint": "gsdgdfgsdf",
                    "SummarizeAttemptsToResolvePrivacyMatter":
                        "gdfghjvbcvbcxvbxcvbxbcvb",
                    "DateSentRequests": "qwerewr",
                    "WordingOfRequest": "hgdffgh",
                    "MoreDetailsOfRequest": "oiuyoiuo",
                    "DateOfFinalAnswer": "gfhfjgj",
                    "DidNoRecordExistChoice": "yes",
                    "InstitutionAgreedRequestOnInformalBasis": "not_sure",
                    "SummarizeYourConcernsAndAnyStepsTaken": "poiuiop"
                };

                // Load the initial state
                loadStateLocally(survey, storageName_PA, defaultData);

                // Save the state back to local storage
                this.onCurrentPageChanged_saveState(survey);

                // Call the event to set the navigation buttons on page load
                onCurrentPageChanged_updateNavButtons(survey);

                // DICTIONNARY - this is just to show how to use localization
                // survey.setVariable("part_a_2_title", "Part A: Preliminary information (Identify institution)...");
                // survey.setVariable("part_b_1_title", "Part B: Steps taken (Writing to the ATIP Coordinator)-...");

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey
                    }
                });
            });
    }
}
