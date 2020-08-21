import Vue from "vue";
import * as Survey from "survey-vue";
import {
    saveStateLocally,
    storageName_PA,
    loadStateLocally
} from "./SurveyLocalStorage";
import {
    initSurvey,
    initSurveyModelEvents,
    initSurveyModelProperties,
    onCurrentPageChanged_updateNavButtons
} from "./SurveyInit";
import { initSurveyFile, initSurveyFileModelEvents } from "./surveyFile";
import { printProblemDetails, getTranslation } from "./surveyHelper";

declare global {
    //  This is required for the buttons in the html page to work
    var survey: Survey.SurveyModel;
}

declare function exportToPDF(s: string, s2: string, s3: string): void;

// This is the total file sizes
const multipleFileMaxSize = 26214400;

export class PaSurvey {
    // This function is to build the html for the question of type html documentation_info.
    // I have tried to get this working in a more elegant way but no success
    private buildDocumentationInfoSection(survey, options) {
        options.question.html = "<section class='alert alert-info col-md-12'>";

        if (survey.locale === "fr") {
            options.question.html +=
                "<p>D’après les réponses que vous avez fournies jusqu’à présent, vous devez joindre les documents suivants à votre plainte :</p>";
        } else {
            options.question.html +=
                "<p>Based on your responses so far, you should attach the following documents with your complaint:</p>";
        }

        options.question.html += "<ul>";

        options.question.html += "<li class='mrgn-bttm-sm'>";
        if (survey.locale === "fr") {
            options.question.html +=
                "une copie des réponses par écrit que vous avez reçues de l’organisation au sujet de vos préoccupations en matière de protection de la vie privée (le cas échéant)";
        } else {
            options.question.html +=
                "a copy of any written responses you received from the organization about your privacy concerns";
        }
        options.question.html += "</li>";

        options.question.html += "<li class='mrgn-bttm-sm'>";
        if (survey.locale === "fr") {
            options.question.html +=
                "une copie de votre demande à l’institution";
        } else {
            options.question.html +=
                "a copy of your request to the institution";
        }
        options.question.html += "</li>";

        if (survey.data["RaisedPrivacyToAtipCoordinator"] === "yes") {
            options.question.html += "<li class='mrgn-bttm-sm'>";
            if (survey.locale === "fr") {
                options.question.html +=
                    "une copie de votre correspondance avec l’institution au sujet de vos préoccupations en matière de protection de la vie privée, y compris vos tentatives de faire part de vos préoccupations au coordonnateur de l’accès à l’information et de la protection des renseignements personnels (AIPRP) de l’institution";
            } else {
                options.question.html +=
                    "a copy of your correspondence with the institution about your privacy concerns, including your attempts to escalate your concerns to the institution’s Access to Information and Privacy (ATIP) Coordinator";
            }
            options.question.html += "</li>";
        }

        if (survey.data["FilingComplaintOnOwnBehalf"] === "someone_else") {
            options.question.html += "<li class='mrgn-bttm-sm'>";
            if (survey.locale === "fr") {
                options.question.html +=
                    "votre formulaire d’autorisation de représentation signé par le plaignant";
            } else {
                options.question.html +=
                    "your representative authorization form signed by the complainant";
            }
            options.question.html += "</li>";
        }

        if (
            survey.data["NatureOfComplaint"].filter(
                x => x === "NatureOfComplaintDenialOfAccess"
            ).length > 0
        ) {
            options.question.html += "<li class='mrgn-bttm-sm'>";
            if (survey.locale === "fr") {
                options.question.html +=
                    "vos demandes d’accès et toute réponse reçue de l’institution";
            } else {
                options.question.html +=
                    "your access request(s) and any reply(ies) received from the institution";
            }
            options.question.html += "</li>";
        }

        options.question.html += "</ul>";

        if (survey.locale === "fr") {
            options.question.html +=
                "<p>Vous pouvez joindre les documents à l’appui à cette plainte en ligne ou les envoyer par la poste séparément.</p>";
        } else {
            options.question.html +=
                "<p>You can either attach supporting documents to this online complaint or you can mail documents separately.</p>";
        }

        options.question.html += "</section>";
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
            totalBytes = totalBytes + parseInt(fileItem.content);
        });

        // We need to calculate the total size of all files for both file upload

        if (options.question.name === "documentation_file_upload") {
            const rep_file_upload = survey.getQuestionByName(
                "documentation_file_upload_rep"
            );

            if (rep_file_upload && rep_file_upload.value) {
                rep_file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content);
                });
            }
        } else if (options.question.name === "documentation_file_upload_rep") {
            const file_upload = survey.getQuestionByName(
                "documentation_file_upload"
            );

            if (file_upload && file_upload.value) {
                file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content);
                });
            }
        }

        return totalBytes;
    }

    private showPDF() {
        //  TODO: Find a good pdf file name
        const filename = "surveyResultToPDF.pdf";
        const json_pdf = "/sample-data/survey_pa_complaint.json";
        const lang =
            "@System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName";

        exportToPDF(filename, json_pdf, lang);
    }

    public init(jsonUrl: string, lang: string, token: string): void {
        initSurvey();
        initSurveyFile();

        fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {
                //globalThis.survey = new Survey.Model(json);
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
                            printProblemDetails(validationResponse);
                        }
                    };
                    xhr.send(data);
                });

                // //survey
                // //    .onCompleting
                // //    .add(function (sender, options) {

                // //        options.allowComplete = true;

                // //        //let data = JSON.stringify(sender.data, null, 3);

                // //        var params = { 'complaintId': sender.complaintId }
                // //        let query = Object.keys(params).map(k => encodeURIComponent(k) + '=' + encodeURIComponent(params[k])).join('&');
                // //        var uri = "/api/PASurvey/Validate?" + query;

                // //        const validate = async () => {

                // //            const response = await fetch(uri, {
                // //                method: 'POST',
                // //                headers: {
                // //                    'Accept': 'application/json',
                // //                    'Content-Type': 'application/json; charset=utf-8'
                // //                },
                // //                body: JSON.stringify(sender.data)

                // //            });

                // //            const validationResponse = await response;

                // //            if (validationResponse.status === 200) {
                // //                options.allowComplete = false;

                // //            }
                // //            else {
                // //                if (validationResponse.json) {
                // //                    validationResponse.json().then(function (error) {
                // //                        printProblemDetails(error);
                // //                    });
                // //                }
                // //                else {
                // //                    alert("oopsy");
                // //                }

                // //                //return false;
                // //            }
                // //        }

                // //        //    options.allowComplete = validate() === true;

                // //        validate();
                // //        //options.allowComplete = allowComplete;
                // //        //validate().catch(function (error) {
                // //        //    console.warn(error);
                // //        //});
                // //    });

                survey.onComplete.add((sender, options) => {
                    const params = { complaintId: sender.complaintId };
                    const query = Object.keys(params)
                        .map(
                            k =>
                                `${encodeURIComponent(k)}=${encodeURIComponent(
                                    params[k]
                                )}`
                        )
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
                                    $("#div_navigation").hide();

                                    // //var response = JSON.parse(completeResponse);

                                    // ////  Update the file reference number
                                    // //completeResponse.json().then(function (response) {
                                    // //    $("#sp_survey_file_number").html(response.referenceNumber);
                                    // //});

                                    // //survey.clear(true, true);
                                    // //clearLocalStorage(storageName_PA);
                                    saveStateLocally(survey, storageName_PA);

                                    console.log(sender.data);
                                    break;
                                case 400:
                                case 500:
                                    if (response.json) {
                                        response.json().then(problem => {
                                            printProblemDetails(problem);
                                        });
                                    }

                                    return response;

                                default:
                                    return response;
                            }
                        })
                        .catch(error => {
                            // console.warn("Could not upload the file");
                            console.warn(error);
                        });

                    // //let data = JSON.stringify(sender.data, null, 3);

                    // //var xhr = new XMLHttpRequest();
                    // //xhr.open("POST", "/api/PASurvey/Complete?complaintId=" + sender.complaintId);
                    // //xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
                    // //xhr.onload = xhr.onerror = function (result) {

                    // //    if (xhr.status === 200) {

                    // //        //  Hide the navigation buttons
                    // //        $("#div_navigation").hide();

                    // //        //  Update the file reference number
                    // //        var response = JSON.parse(xhr.response);
                    // //        $("#sp_survey_file_number").html(response.referenceNumber);

                    // //        //survey.clear(true, true);
                    // //        //clearLocalStorage(storageName_PA);
                    // //        saveStateLocally(survey, storageName_PA);

                    // //        console.log(data);

                    // //    } else if (xhr.status === 400) {

                    // //        var response = JSON.parse(xhr.response);
                    // //        alert(response.detail);
                    // //    }
                    // //    else {
                    // //        //Error
                    // //        options.showDataSavingError(); // you may pass a text parameter to show your own text
                    // //    }
                    // //};
                    // //xhr.send(data);
                });

                survey.onAfterRenderQuestion.add((survey, options) => {
                    if (
                        options.question.getType() === "html" &&
                        options.question.name === "documentation_info"
                    ) {
                        this.buildDocumentationInfoSection(survey, options);
                    } else if (
                        options.question.getType() === "file" &&
                        options.question.value
                    ) {
                        // Getting the total size of all uploaded files
                        const totalBytes = this.getTotalFileSize(
                            survey,
                            options
                        );

                        const sizeInMB = (totalBytes / 1000000).toFixed(2);

                        // Setting up the <meter> values
                        $("#sp_total").html(sizeInMB);
                        $("#meter_upload_total_mb").val(totalBytes);
                    }
                });

                // Adding particular event for this page only
                survey.onCurrentPageChanged.add((survey, options) => {
                    this.onCurrentPageChanged_saveState(survey);
                });

                survey.onValidateQuestion.add((sender, options) => {
                    if (
                        options.question.getType() === "file" &&
                        options.question.value
                    ) {
                        // Getting the total size of all uploaded files
                        const totalBytes = this.getTotalFileSize(
                            survey,
                            options
                        );

                        if (
                            multipleFileMaxSize > 0 &&
                            totalBytes > multipleFileMaxSize
                        ) {
                            options.error = getTranslation(
                                options.question.multipleFileMaxSizeErrorMessage
                            );
                            // return false;
                        }

                        const sizeInMB = (totalBytes / 1000000).toFixed(2);

                        // Setting up the <meter> values
                        $("#sp_total").html(sizeInMB);
                        $("#meter_upload_total_mb").val(totalBytes);
                    }
                });

                survey.onServerValidateQuestions.add((sender, options) => {
                    if (
                        options.data["documentation_type"] &&
                        (options.data["documentation_type"] === "upload" ||
                            options.data["documentation_type"] === "both")
                    ) {
                        //  Validating the documentation page if and only if there is documents to be validated

                        const params = { complaintId: sender.complaintId };
                        const query = Object.keys(params)
                            .map(
                                k =>
                                    `${encodeURIComponent(
                                        k
                                    )}=${encodeURIComponent(params[k])}`
                            )
                            .join("&");
                        const uri =
                            "/api/PASurvey/ValidateAttachments?" + query;

                        fetch(uri, {
                            method: "POST",
                            headers: {
                                Accept: "application/json",
                                "Content-Type":
                                    "application/json; charset=utf-8"
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
                                                printProblemDetails(problem);
                                            });
                                        } else {
                                            alert("oopsy");
                                        }

                                        break;
                                    default:
                                        alert("oopsy");
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
                    FilingComplaintOnOwnBehalf: "yourself",
                    RaisedPrivacyToAtipCoordinator: "yes",
                    WhichFederalGovernementInstitutionComplaintAgainst: "3",
                    NatureOfComplaint: [
                        "NatureOfComplaintOther",
                        "NatureOfComplaintDelay",
                        "NatureOfComplaintExtensionOfTime",
                        "NatureOfComplaintCollection"
                    ],
                    IsEmployeeChoice: "general_public",
                    AdditionalComments: "iuyiuyuiyiuy",
                    complainant_HaveYouSubmittedBeforeChoice: "no",
                    complainant_FormOfAddress: "Mr.",
                    complainant_FirstName: "jf",
                    complainant_LastName: "brouillette",
                    complainant_Email: "jf@hotmail.com",
                    complainant_MailingAddress: "66",
                    complainant_City: "gat",
                    complainant_PostalCode: "J9A2V5",
                    complainant_DayTimeNumber: "6135656667",
                    NeedsDisabilityAccommodationChoice: "yes",
                    DisabilityAccommodation: "iuyuiyiuyiuy",
                    complainant_Country: "CA",
                    complainant_ProvinceOrState: "2",
                    reprensentative_FormOfAddress: "Mr.",
                    reprensentative_FirstName: "jf",
                    reprensentative_LastName: "brouillette",
                    reprensentative_Email: "jf@hotmail.com",
                    reprensentative_MailingAddress: "66",
                    reprensentative_City: "gat",
                    reprensentative_PostalCode: "J9A2V5",
                    reprensentative_DayTimeNumber: "6135656667",
                    documentation_type: "none",
                    WhatWouldResolveYourComplaint: "gsdgdfgsdf",
                    SummarizeAttemptsToResolvePrivacyMatter:
                        "gdfghjvbcvbcxvbxcvbxbcvb",
                    DateSentRequests: "qwerewr",
                    WordingOfRequest: "hgdffgh",
                    MoreDetailsOfRequest: "oiuyoiuo",
                    DateOfFinalAnswer: "gfhfjgj",
                    DidNoRecordExistChoice: "yes",
                    InstitutionAgreedRequestOnInformalBasis: "not_sure",
                    SummarizeYourConcernsAndAnyStepsTaken: "poiuiop"
                };

                // Load the initial state
                loadStateLocally(survey, storageName_PA, defaultData);

                // Save the state back to local storage
                this.onCurrentPageChanged_saveState(survey);

                // Call the event to set the navigation buttons on page load
                onCurrentPageChanged_updateNavButtons(survey);

                // DICTIONNARY - this is just to show how to use localization
                // survey.setVariable("part_a_2_title", "Part A: Preliminary information (Identify institution)-Privacy complaint form (federal institution)");
                // survey.setVariable("part_b_1_title", "Part B: Steps taken (Writing to the ATIP Coordinator)-Privacy complaints form (federal institution)");

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: survey
                    }
                });
            });
    }
}
