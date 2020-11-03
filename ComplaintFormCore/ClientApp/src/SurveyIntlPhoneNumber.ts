import * as Survey from "survey-vue";
import intlTelInput from "intl-tel-input";

export const intTelOptions: intlTelInput.Options = {
    separateDialCode: false,
    nationalMode: false,
    autoPlaceholder: "polite",
    initialCountry: "ca",
    preferredCountries: ["ca", "us"],
    utilsScript: "~/lib/intl-tel-input/build/js/utils.js"
};

export function initIntlPhoneNumber(): void {
    Survey.JsonObject.metaData.addProperty("text", {
        name: "isInternationalPhoneNumber:boolean"
    });
}

export function initIntlPhoneNumberEvents(survey: Survey.SurveyModel): void {
    survey.onUpdateQuestionCssClasses.add((sender, options) => {
        const classes = options.cssClasses;

        if (options.question.getType() === "text" && options.question.isInternationalPhoneNumber) {
            classes.root += " internationalPhoneNumber";
        }
    });

    survey.onValidateQuestion.add((sender, options) => {
        if (options.question.isInternationalPhoneNumber && options.value !== "") {
            const div = document.getElementById(options.question.id) as HTMLDivElement;
            if (div) {
                const textbox = document.getElementById(options.question.inputId) as HTMLInputElement;
                if (textbox) {
                    const iti = window.intlTelInput(textbox, intTelOptions);
                    // var number = iti.getNumber();
                    // var countryData = iti.getSelectedCountryData();

                    if (!iti.isValidNumber()) {
                        // alert("invalid");
                        // options.value = options.oldValue;
                        const title: string = options.question.title;
                        const error = iti.getValidationError();

                        if (error === intlTelInputUtils.validationError.TOO_SHORT) {
                            options.error = `${title} - TOO_SHORT`;
                        } else if (error === intlTelInputUtils.validationError.INVALID_COUNTRY_CODE) {
                            options.error = `${title} - INVALID_COUNTRY_CODE`;
                        } else if (error === intlTelInputUtils.validationError.NOT_A_NUMBER) {
                            options.error = `${title} - NOT_A_NUMBER`;
                        } else if (error === intlTelInputUtils.validationError.TOO_LONG) {
                            options.error = `${title} - TOO_LONG`;
                        } else if (error === intlTelInputUtils.validationError.IS_POSSIBLE) {
                            options.error = `${title} - IS_POSSIBLE`;
                        }
                    }
                }
            }
        }
    });

    survey.onAfterRenderQuestion.add((sender, options) => {
        if (options.question.getType() === "text" && options.question.isInternationalPhoneNumber) {
            setInternationalPhoneNumberTextboxes();
        }
    });
}

export function setInternationalPhoneNumberTextboxes(): void {
    const input = document.getElementsByClassName("internationalPhoneNumber");
    let i;

    for (i = 0; i < input.length; i++) {
        window.intlTelInput(input[i], intTelOptions);
    }
}
