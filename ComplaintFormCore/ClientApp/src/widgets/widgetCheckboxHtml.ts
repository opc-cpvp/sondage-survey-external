import * as Survey from "survey-vue";

export class WidgetCheckboxHtml {

    public init(): void {

        const widget = {
            //  the widget name. It should be unique and written in lowcase.
            name: "checkboxwithhtmlinfo",
            //  the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder
            title: "Checkbox list with addtional Html info",
            iconName: "",

            widgetIsLoaded () {
                //  If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded
                return true;
            },

            isFit (question) {
                //  This is a match for checkboxes that have addtionnal Html information to show when an item is clicked and not in 'preview' mode
                return question.getType() === "checkbox" && question.hasHtmlAddtionalInfo && !question.isReadOnly;
            },

            activatedByChanged (activatedBy) {

                //  Add the new property 'hasHtmlInfo' at the checkbox level to indication that
                //  this is a kind of checkbox with additionnal information displayed as html
                Survey.JsonObject.metaData.addProperties("checkbox", [
                    {
                        name: "hasHtmlAddtionalInfo", default: false
                    }
                ]);
            },

            isDefaultRender: false,
            //  htmlTemplate: "<div><input /><button></button></div>",

            //  The main function, rendering and two-way binding
            afterRender (question, el) {

                let outputHTML = "";
                const allCheckboxes = question.choices;

                //  This is where each checkbox item is being created

                allCheckboxes.forEach((row, index, rows) => {

                    //  the checked flag is based on incoming (or existing) json data
                    let isChecked = false;

                    if (question.data && question.data.data && question.data.data[question.name]
                        && question.data.data[question.name].includes(row.value)) {
                        isChecked = true;
                    }

                    outputHTML += "<div class='sv_q_checkbox sv-q-col-1'>";
                    outputHTML += "<label class='sv_q_checkbox_label'>";
                    outputHTML += "<input type = 'checkbox' name = '" + question.name + "' value = '" + row.value + "'";
                    outputHTML += " aria-required='true' aria-label='" + row.text + "'";
                    outputHTML += " class='sv_q_checkbox_control_item' ";
                    outputHTML += " onclick = 'checkBoxInfoPopup(this)' ";

                    if (isChecked) {
                        outputHTML += " checked ";
                    }

                    outputHTML += "/>"; // closing input tag

                    outputHTML += " <span class='checkbox-material'>";
                    outputHTML += "<span class='check'></span>";
                    outputHTML += "</span>";
                    outputHTML += "<span class='sv_q_checkbox_control_label'>";
                    outputHTML += "<span style='position: static;'>";
                    outputHTML += "<span style='position: static;'>" + row.text + "</span>";
                    outputHTML += "</span>";
                    outputHTML += "</span>";
                    outputHTML += "</label>";

                    //  question.isReadOnly means we are in 'Preview' mode so we don't want to display the additional information in preview
                    if (!question.isReadOnly && row.htmlAdditionalInfo) {

                        //  This is where we are adding the additionnal information.
                        //  Putting the <div> wrapper here is probably better than putting it in the json
                        outputHTML += "<div class='info-popup alert alert-info' style='display: " + (isChecked ? "block" : "none") + ";'>";

                        let rowText = "";

                        if (question.survey.locale === "fr" && row.htmlAdditionalInfo.fr) {
                            rowText = row.htmlAdditionalInfo.fr;
                        } else if (row.htmlAdditionalInfo.en) {
                            rowText = row.htmlAdditionalInfo.en;
                        } else {
                            rowText = row.htmlAdditionalInfo;
                        }

                        outputHTML += rowText;
                        outputHTML += "</div>";
                    }

                    outputHTML += "</div>";

                });

                el.innerHTML = outputHTML;
            }
        };

        //  Register our widget in singleton custom widget collection
        Survey.CustomWidgetCollection.Instance.addCustomWidget(widget, "customtype");
    }
}
