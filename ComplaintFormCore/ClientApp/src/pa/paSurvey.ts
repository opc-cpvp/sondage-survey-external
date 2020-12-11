import Vue from "vue";
import * as Survey from "survey-vue";
import { initSurvey, initSurveyModelEvents, initSurveyModelProperties } from "../surveyInit";
import { initSurveyFile, initSurveyFileModelEvents } from "../surveyFile";
import { printProblemDetails, getTranslation } from "../surveyHelper";
import * as SurveyNavigation from "../surveyNavigation";
import * as Ladda from "ladda";
import { paTestData } from "./pa_test_data";
import { SurveyLocalStorage } from "../surveyLocalStorage";

declare global {
    // TODO: get rid of this global variable
    var survey: Survey.SurveyModel; // eslint-disable-line no-var
}

// This is the total file sizes
const multipleFileMaxSize = 26214400;

export class PaSurvey {
    private storageName_PA = "SurveyJS_LoadState_PA";

    public init(jsonUrl: string, lang: string, token: string): void {
        initSurvey();
        initSurveyFile();

        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json => {
                const _survey = new Survey.Model(json);
                globalThis.survey = _survey;

                _survey.complaintId = token;

                //  This needs to be here
                _survey.locale = lang;

                // Add events only applicable to this page **********************

                // ISSUE WITH THE FLOW: When calling survey.onComplete the completed page shows right away. This is
                // not desired. If the validation doesn't pass, we don't want to go to the completed page. So to do that,
                // we call survey.onCompleting.
                // The event onCompleting is fired before the survey is completed and the onComplete event is fired after.
                // You can prevent the survey from completing by setting options.allowComplete to false
                // sender - the survey object that fires the event.

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
                                        printProblemDetails(problem, sender.locale);
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
                            if (response.ok) {
                                //  Hide the navigation buttons
                                const div_navigation = document.getElementById("div_navigation");
                                if (div_navigation) {
                                    div_navigation.classList.add("hidden");
                                }

                                //  Update the file reference number
                                void response
                                    .json()
                                    .then(responseData => {
                                        const sp_survey_file_number = document.getElementById("sp_survey_file_number");
                                        if (sp_survey_file_number) {
                                            sp_survey_file_number.innerText = responseData.referenceNumber;
                                        }
                                    })
                                    .catch(error => {
                                        console.warn(error);
                                    });

                                new SurveyLocalStorage().saveStateLocally(_survey, this.storageName_PA);

                                console.log(sender.data);
                                Ladda.stopAll();
                            } else {
                                if (response.json) {
                                    void response.json().then(problem => {
                                        printProblemDetails(problem, sender.locale);
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

                _survey.onAfterRenderQuestion.add((sender, options) => {
                    if (options.question.getType() === "html" && options.question.name === "documentation_info") {
                        this.updateDocumentationInfoSection(sender);
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

                        //  This is for IE to work.
                        //  See: https://css-tricks.com/html5-meter-element/
                        const div_meter_upload = document.getElementById("div_meter_upload") as HTMLDivElement;
                        if (div_meter_upload) {
                            const percentage = ((totalBytes / multipleFileMaxSize) * 100).toFixed(0);
                            div_meter_upload.style.width = `${percentage}%`;
                        }
                    }
                });

                // Adding particular event for this page only
                _survey.onCurrentPageChanged.add((sender, options) => {
                    this.onCurrentPageChanged_saveState(sender);
                });

                _survey.onValidateQuestion.add((sender, options) => {
                    if (options.question.getType() === "file" && options.question.value) {
                        // Getting the total size of all uploaded files
                        const totalBytes = this.getTotalFileSize(_survey, options);

                        if (multipleFileMaxSize > 0 && totalBytes > multipleFileMaxSize) {
                            options.error = getTranslation(options.question.multipleFileMaxSizeErrorMessage, _survey.locale);
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

                        //  This is for IE to work.
                        //  See: https://css-tricks.com/html5-meter-element/
                        const div_meter_upload = document.getElementById("div_meter_upload") as HTMLDivElement;
                        if (div_meter_upload) {
                            const percentage = ((totalBytes / multipleFileMaxSize) * 100).toFixed(0);
                            div_meter_upload.style.width = `${percentage}%`;
                        }
                    }
                });

                _survey.onServerValidateQuestions.add((sender, options) => {
                    if (options.data["documentation_type"] === "upload" || options.data["documentation_type"] === "both") {
                        //  Validating the documentation page if and only if there is documents to be validated

                        const params = { complaintId: sender.complaintId };
                        const query = Object.keys(params)
                            .map(k => `${encodeURIComponent(k)}=${encodeURIComponent(params[k])}`)
                            .join("&");
                        const uri = "/api/PASurvey/ValidateAttachments?" + query;

                        fetch(uri, {
                            method: "POST",
                            headers: {
                                Accept: "application/json",
                                "Content-Type": "application/json; charset=utf-8"
                            },
                            body: JSON.stringify(options.data)
                        })
                            .then(response => {
                                if (response.ok) {
                                    //  This will allowed the validation to pass and go to the next page
                                    options.complete();
                                } else {
                                    response
                                        .json()
                                        .then(problem => {
                                            printProblemDetails(problem, sender.locale);
                                        })
                                        .catch(error => {
                                            console.warn(error);
                                        });
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

                initSurveyModelEvents(_survey);

                initSurveyModelProperties(_survey);

                initSurveyFileModelEvents(_survey, "pa");

                //  TODO:   for now, in order to be able to use the checkboxes with addiotional htnl info, we
                //          need to specify the array object type_complaint. There has to be a more elegant way of doing this.
                // var defaultData  = { };

                const defaultData = {
                    "type_complaint": []
                };

                // Load the initial state
                new SurveyLocalStorage().loadStateLocally(_survey, this.storageName_PA, JSON.stringify(paTestData));

                // Save the state back to local storage
                this.onCurrentPageChanged_saveState(_survey);

                // Call the event to set the navigation buttons on page load
                SurveyNavigation.onCurrentPageChanged_updateNavButtons(_survey);

                // DICTIONNARY - this is just to show how to use localization
                // survey.setVariable("part_a_2_title", "Part A: Preliminary information (Identify institution)...");
                // survey.setVariable("part_b_1_title", "Part B: Steps taken (Writing to the ATIP Coordinator)-...");

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey: _survey
                    }
                });
            });
    }

    // This function is to update the html for the question of type html named 'documentation_info'.
    // It will removed the hidden css on some of the <li> depending on some conditions
    private updateDocumentationInfoSection(surveyObj) {
        const ul_documentation_info = document.getElementById("ul_documentation_info");
        if (ul_documentation_info == null) {
            return;
        }

        if (surveyObj.data["RaisedPrivacyToAtipCoordinator"] === "yes") {
            const liNode = ul_documentation_info.querySelector(".raisedPrivacyToAtipCoordinator");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (surveyObj.data["FilingComplaintOnOwnBehalf"] === "someone_else") {
            const liNode = ul_documentation_info.querySelector(".filingComplaintOnOwnBehalf");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }

        if (surveyObj.data["NatureOfComplaint"].filter(x => x === "NatureOfComplaintDenialOfAccess").length > 0) {
            const liNode = ul_documentation_info.querySelector(".natureOfComplaint");
            if (liNode != null) {
                liNode.classList.remove("sv-hidden");
            }
        }
    }

    private onCurrentPageChanged_saveState(surveyObj) {
        new SurveyLocalStorage().saveStateLocally(surveyObj, this.storageName_PA);
    }

    // This is to get the total number of bytes for both file uploads.
    // We will compare with the max value later.
    // Note that the file size is being stored in file.content
    private getTotalFileSize(surveyObj, options) {
        let totalBytes = 0;

        options.question.value.forEach(fileItem => {
            totalBytes = totalBytes + parseInt(fileItem.content, 10);
        });

        // We need to calculate the total size of all files for both file upload

        if (options.question.name === "documentation_file_upload") {
            const rep_file_upload = surveyObj.getQuestionByName("documentation_file_upload_rep");

            if (rep_file_upload && rep_file_upload.value) {
                rep_file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content, 10);
                });
            }
        } else if (options.question.name === "documentation_file_upload_rep") {
            const file_upload = surveyObj.getQuestionByName("documentation_file_upload");

            if (file_upload && file_upload.value) {
                file_upload.value.forEach(fileItem => {
                    totalBytes = totalBytes + parseInt(fileItem.content, 10);
                });
            }
        }

        return totalBytes;
    }
}
