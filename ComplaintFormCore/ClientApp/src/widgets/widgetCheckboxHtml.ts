import * as Survey from "survey-vue";
import { saveStateLocally, storageName_PA } from "../surveyLocalStorage";

export class WidgetCheckboxHtml {

    public init(): void {

        const widget = {
            //  the widget name. It should be unique and written in lowcase.
            name: "checkboxwithhtmlinfo",
            //  the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder
            title: "Checkbox list with addtional Html info",
            iconName: "",

            widgetIsLoaded() {
                //  If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded
                return true;
            },

            isFit(question) {
                //  This is a match for checkboxes that have addtionnal Html information to show when an item is clicked and not in 'preview' mode
                return question.getType() === "checkbox" && question.hasHtmlAddtionalInfo && !question.isReadOnly;
            },

            activatedByChanged() {

                //  Add the new property 'hasHtmlInfo' at the checkbox level to indication that
                //  this is a kind of checkbox with additionnal information displayed as html
                Survey.JsonObject.metaData.addProperties("checkbox", [
                    {
                        name: "hasHtmlAddtionalInfo", default: false
                    }
                ]);
            },

            isDefaultRender: true,
            //htmlTemplate: "",

            //  The main function, rendering and two-way binding
            afterRender(question: Survey.QuestionCheckboxModel, el) {
                let outputHTML = `<fieldset class=sv_qcbc sv_qcbx form-group"><legend aria-label="<span class='sv_q_required_text'>&amp;ast;</span>${question.title}"></legend>`;
                const allCheckboxes = question.choices;

                //  This is where each checkbox item is being created

                allCheckboxes.forEach((row: Survey.ItemValue) => {

                    //  the checked flag is based on incoming (or existing) json data
                    let checked = "";
                    let styleDisplay = "none";

                    if (question.data) {
                        const surveyData = question.data as Survey.VueSurveyModel;

                        if (surveyData && surveyData.data && surveyData.data[question.name]
                            && surveyData.data[question.name].includes(row.value)) {
                            checked = "checked";
                            styleDisplay = "block";
                            //this.addSelectedItems(row.value as string);
                        }
                    }

                    let divInfoPopup = "";

                    //  question.isReadOnly means we are in 'Preview' mode so we don't want to display the additional information in preview
                    if (!question.isReadOnly && row.htmlAdditionalInfo) {

                        //  This is where we are adding the additionnal information.
                        //  Putting the <div> wrapper here is probably better than putting it in the json

                        let rowText = "";

                        if (question.survey.getLocale() === "fr" && row.htmlAdditionalInfo.fr) {
                            rowText = row.htmlAdditionalInfo.fr;
                        } else if (row.htmlAdditionalInfo.en) {
                            rowText = row.htmlAdditionalInfo.en;
                        } else {
                            rowText = row.htmlAdditionalInfo;
                        }

                        divInfoPopup = `<div class="info-popup alert alert-info" style="display:${styleDisplay}";">${rowText}</div>`;
                    }

                    let chk_id = `chk${question.name}_${row.value as string}`;

                    outputHTML +=
                        `<div class="sv_q_checkbox sv-q-col-1 ${checked}">
                        <label class="sv_q_checkbox_label">
                        <input type = "checkbox" name = "${question.name}" value = "${row.value as string}"
                        aria-required="true" aria-label="${row.text}" id=${chk_id} ${checked}
                        class="sv_q_checkbox_control_item chk_${question.name}" onchange = "checkBoxInfoPopup(this)"/>
                        <span class="checkbox-material"><span class="check"></span></span>
                        <span class="sv_q_checkbox_control_label"><span style="position: static;">
                        <span style="position: static;">${row.text}</span></span></span>
                        </label>${divInfoPopup}</div>`;


                    //row.onPropertyChanged.add((sender, opt) => {

                    //    const test = sender;
                    //});
                });

                outputHTML += `</fieldset>`;
                el.innerHTML = outputHTML;



                //question.onAnyValueChanged(question.name);

                //question.onItemValuePropertyChanged.add((sender, opt) => {

                //    const test = sender;
                //});

                question.onPropertyChanged.add((sender, opt) => {

                    const checkboxQuestion = sender as Survey.QuestionCheckboxModel;
                    const thisSurvey = checkboxQuestion.setsurvey as Survey.VueSurveyModel;
                    let data = thisSurvey.data[checkboxQuestion.name];

                    const allChoices = Array.prototype.slice.call(document.getElementsByClassName(`chk_${checkboxQuestion.name}`));

                    data = [];

                    allChoices.forEach((selectedItem: HTMLInputElement) => {

                        if (selectedItem.checked) {
                            data.push(selectedItem.value);
                        }
                    });

                    thisSurvey.data[checkboxQuestion.name] = data;
                    saveStateLocally(thisSurvey, storageName_PA);
                });

                //function onValueChangedCallback() {

                //    const allChoices = Array.prototype.slice.call(document.getElementsByClassName(`chk_${question.name}`));

                //    allChoices.forEach((selectedItem: HTMLInputElement) => {

                //        let test = selectedItem.checked;
                //    });

                //    const test = question.value;

                //    question.value.forEach((selectedItem: string) => {

                //        let choice = question.choices.filter(x => x.value == selectedItem)[0];
                //    });

                //    //question.choices.forEach((choice: Survey.ItemValue) => {

                //    //});
                //}

                ////if the question value changed in the code, for example you have changed it in JavaScript
                //question.valueChangedCallback = onValueChangedCallback;
                //////set initial value
                //onValueChangedCallback();


            }
        };

        //  Register our widget in singleton custom widget collection
        Survey.CustomWidgetCollection.Instance.addCustomWidget(widget, "customtype");
    }

    //  This is to open the additional information div when a checkbox is being checked or hide it when the checkbox is un-checked.
    //  It will also remove or add the item being chekced or unchecked from the json data
    public checkBoxInfoPopup(checkbox: HTMLInputElement): void {

        //  Getting the <div> with css class info-popup from the parent <div>
        const inputCheckbox = checkbox.closest(".sv_q_checkbox") as HTMLDivElement;
        const infoPopupDiv = inputCheckbox.querySelector(".info-popup") as HTMLDivElement;
        //const data = this.survey.data;

        if (checkbox.checked) {
            //  If infoPopupDiv is undefined it means there is no popup for this checkbox item
            if (infoPopupDiv) {
                infoPopupDiv.style.display = "block";
            }

            //  If the array object of the checkbox list is not set the create it
            //if (!data[checkbox.name]) {
            //    data[checkbox.name] = [];
            //}

            ////  push the selected value
            //data[checkbox.name].push(checkbox.value);

        } else {
            //  If infoPopupDiv is undefined it means there is no popup for this checkbox item
            if (infoPopupDiv) {
                infoPopupDiv.style.display = "none";
            }

            //  removing the un-checked item from the json object
            //for (let i = 0; i < data[checkbox.name].length; i++) {
            //    if (data[checkbox.name][i] === checkbox.value) {
            //        data[checkbox.name].splice(i, 1);
            //    }
            //}
        }

        //this.survey.data = data;
    }
}
