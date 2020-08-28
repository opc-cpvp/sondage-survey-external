import Vue from "vue";
import * as Survey from "survey-vue";
declare let $: any; // import $ from "jquery";

import * as SurveyInit from "../surveyInit";
import * as SurveyHelper from "../surveyHelper";
import * as SurveyLocalStorage from "../surveyLocalStorage";

declare let SurveyPDF: any;

//declare global {
//    // TODO: get rid of this global variable
//    var survey: Survey.SurveyModel; // eslint-disable-line no-var
//}

export class TestSurvey {
    public init(jsonUrl: string, lang: string, token: string): void {
        function onCurrentPageChanged_saveState(survey) {
            SurveyLocalStorage.saveStateLocally(
                survey,
                SurveyLocalStorage.storageName_Test
            );
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
                SurveyLocalStorage.loadStateLocally(
                    survey,
                    SurveyLocalStorage.storageName_Test,
                    defaultData
                );

                // Save the state back to local storage
                onCurrentPageChanged_saveState(survey);

                // Call the event to set the navigation buttons on page load
                SurveyInit.onCurrentPageChanged_updateNavButtons(survey);

                // //onCurrentPageChanged_saveState(survey);

                // Add events only applicable to this page **********************
                survey.onCompleting.add((sender, options) => {
                    options.allowComplete = false;

                    const data2 = {
                        FilingComplaintOnOwnBehalf: "yourself",
                        RaisedPrivacyToAtipCoordinator: "fsdafasd",
                        WhichFederalGovernementInstitutionComplaintAgainst: "3",
                        NatureOfComplaint: [
                            "NatureOfComplaintOther",
                            "NatureOfComplaintDelay",
                            "NatureOfComplaintExtensionOfTime",
                            "NatureOfComplaintCollection"
                        ],
                        IsEmployeeChoice: "general_public",
                        AdditionalComments: "iuyiuyuiyiuy",
                        Complainant_HaveYouSubmittedBeforeChoice: "no",
                        Complainant_FormOfAddress: "Mr.",
                        Complainant_FirstName: "jf",
                        Complainant_LastName: "brouillette",
                        Complainant_Email: "jf@hotmail.com",
                        Complainant_MailingAddress: "66",
                        Complainant_City: "gat",
                        Complainant_PostalCode: "J9A2V5",
                        Complainant_DayTimeNumber: "6135656667",
                        NeedsDisabilityAccommodationChoice: "yes",
                        DisabilityAccommodation: "iuyuiyiuyiuy",
                        Complainant_Country: "CA",
                        Complainant_ProvinceOrState: "2",
                        Reprensentative_FormOfAddress: "Mr.",
                        Reprensentative_FirstName: "",
                        Reprensentative_LastName: "brouillette",
                        Reprensentative_Email: "jf@hotmail.com",
                        Reprensentative_MailingAddress: "66",
                        Reprensentative_City: "gat",
                        Reprensentative_PostalCode: "J9A2V5",
                        Reprensentative_DayTimeNumber: "6135656667",
                        Documentation_type: "none",
                        WhatWouldResolveYourComplaint: "gsdgdfgsdf",
                        SummarizeAttemptsToResolvePrivacyMatter:
                            "gdfghjvbcvbcxvbxcvbxbcvb",
                        DateSentRequests: "gfdgfd",
                        WordingOfRequest: "gdsfg",
                        MoreDetailsOfRequest: "oiuyoiuo",
                        DateOfFinalAnswer: "gfhfjgj",
                        DidNoRecordExistChoice: "yes",
                        InstitutionAgreedRequestOnInformalBasis: "not_sure",
                        SummarizeYourConcernsAndAnyStepsTaken: "poiuiop",
                        InformationIsTrue: ["yesff"],
                        documentation_file_upload: [
                            {
                                name: "test.txt",
                                type: "text/plain",
                                content: "10921680"
                            },
                            {
                                name: "test.txt",
                                type: "text/plain",
                                content: "10921680"
                            },
                            {
                                name: "layout.txt",
                                type: "text/plain",
                                content: "2735"
                            },
                            {
                                name: "New Text Document_one.txt",
                                type: "text/plain",
                                content: "4"
                            }
                        ]
                    };

                    const params = { complaintId: sender.complaintId };
                    const query = Object.keys(params)
                        .map(
                            k =>
                                `${encodeURIComponent(k)}=${encodeURIComponent(
                                    params[k]
                                )}`
                        )
                        .join("&");
                    const uri = `/api/PASurvey/Validate?${query}`;

                    fetch(uri, {
                        method: "POST",
                        headers: {
                            Accept: "application/json",
                            "Content-Type": "application/json; charset=utf-8"
                        },
                        body: JSON.stringify(data2)
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
                                        response.json().then(error => {
                                            let message = "";

                                            if (error.title) {
                                                message += error.title + "\n";
                                            }

                                            if (error.detail) {
                                                message += error.detail + "\n";
                                            }

                                            if (error.errors) {
                                                SurveyHelper.printProblemDetails(error, survey.locale);
                                            }

                                            alert(message);
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
                });

                survey.onComplete.add((sender, options) => {
                    // //const data = JSON.stringify(result.data, null, 3);

                    // //const data = {
                    // //    "FilingComplaintOnOwnBehalf": "yourself",
                    // //    "RaisedPrivacyToAtipCoordinator": "yes"
                    // //};

                    const data = {
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
                        Complainant_HaveYouSubmittedBeforeChoice: "no",
                        Complainant_title: "Mr.",
                        Complainant_FirstName: "jf",
                        Complainant_LastName: "brouillette",
                        Complainant_Email: "jf@hotmail.com",
                        Complainant_MailingAddress: "66",
                        Complainant_City: "gat",
                        Complainant_PostalCode: "J9A2V5",
                        Complainant_DayTimeNumber: "6135656667",
                        NeedsDisabilityAccommodationChoice: "yes",
                        DisabilityAccommodation: "iuyuiyiuyiuy",
                        Complainant_Country: "CA",
                        Complainant_ProvinceOrState: "2",
                        Reprensentative_title: "Mr.",
                        Reprensentative_FirstName: "jf",
                        Reprensentative_LastName: "brouillette",
                        Reprensentative_Email: "jf@hotmail.com",
                        Reprensentative_MailingAddress: "66",
                        Reprensentative_City: "gat",
                        Reprensentative_PostalCode: "J9A2V5",
                        Reprensentative_DayTimeNumber: "6135656667",
                        Documentation_type: "none",
                        WhatWouldResolveYourComplaint: "gsdgdfgsdf",
                        SummarizeAttemptsToResolvePrivacyMatter:
                            "gdfghjvbcvbcxvbxcvbxbcvb",
                        DateSentRequests: "qwerewr",
                        WordingOfRequest: "hgdffgh",
                        MoreDetailsOfRequest: "oiuyoiuo",
                        DateOfFinalAnswer: "gfhfjgj",
                        DidNoRecordExistChoice: "yes",
                        InstitutionAgreedRequestOnInformalBasis: "not_sure",
                        SummarizeYourConcernsAndAnyStepsTaken: "poiuiop",
                        documentation_file_upload: [
                            {
                                name: "test.txt",
                                type: "text/plain",
                                content: "10921680"
                            },
                            {
                                name: "test.txt",
                                type: "text/plain",
                                content: "10921680"
                            },
                            {
                                name: "layout.txt",
                                type: "text/plain",
                                content: "2735"
                            },
                            {
                                name: "New Text Document_one.txt",
                                type: "text/plain",
                                content: "4"
                            }
                        ]
                    };

                    const dataToSend = JSON.stringify(data, null, 3);

                    const xhr = new XMLHttpRequest();
                    xhr.open(
                        "POST",
                        `/api/CompletedSurvey/SurveyPA?complaintId=${sender.complaintId}`
                    );
                    xhr.setRequestHeader(
                        "Content-Type",
                        "application/json; charset=utf-8"
                    );
                    xhr.onload = xhr.onerror = () => {
                        if (xhr.status === 200) {
                            options.showDataSavingSuccess(); // you may pass a text parameter to show your own text
                            // Or you may clear all messages:
                            // options.showDataSavingClear();

                            //  Hide the navigation buttons
                            $("#div_navigation").hide();

                            //  Update the file reference number
                            // // $("#sp_survey_file_number").html(result.referenceNumber);

                            // //survey.clear(true, true);
                            // //clearLocalStorage(storageName_PA);
                            // // saveStateLocally(survey, storageName_PA);
                        } else {
                            // Error
                            options.showDataSavingError(); // you may pass a text parameter to show your own text
                        }
                    };
                    xhr.send(dataToSend);

                    console.log(dataToSend);

                    //  Hide the navigation buttons
                    $("#div_navigation").hide();

                    survey.clear(true, true);
                    SurveyLocalStorage.clearLocalStorage(
                        SurveyLocalStorage.storageName_Test
                    );
                });

                survey.onCurrentPageChanged.add(onCurrentPageChanged_saveState);

                $("#pdfPreviewBtn").click(() => {
                    const filename = "surveyResult.pdf";

                    const options = {
                        fontSize: 14,
                        margins: {
                            left: 10,
                            right: 10,
                            top: 10,
                            bot: 10
                        },
                        format: "a4",
                        fontName: "times",
                        fontStyle: "normal"
                    };

                    const surveyPDF = new SurveyPDF.SurveyPDF(json, options);
                    surveyPDF.data = survey.data;
                    surveyPDF.save(filename);
                });

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
