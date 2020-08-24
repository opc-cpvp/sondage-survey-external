declare let $: any;
import { ProblemDetails } from "./problemDetails";

export function getTranslation(questionProperty, lang: string) {
    if (lang === "fr" && questionProperty.fr) {
        return questionProperty.fr;
    } else if (questionProperty.en) {
        return questionProperty.en;
    } else {
        return questionProperty;
    }
}

// Will return true if a dropdown has a selected item otherwise false.
export function HasSelectedItem(params) {
    const value = params[0];
    return value !== "";
}

// This function will build a <section> with a list of errors to be displayed at the top of the page
export function buildValidationErrorMessage(problem: ProblemDetails, lang: string) {
    let message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (lang === "fr") {
        message +=
            "Le formulaire ne pouvait pas être soumis parce que des erreurs ont été trouvée";
    } else {
        message += "The form could not be submitted because error(s) was found";
    }

    message += "</h2>";

    if (problem.title) {
        message += problem.title + "</br>";
    }

    if (problem.detail) {
        message += problem.detail + "</br>";
    }

    message += "<ol>";

    if (problem.errors) {

        Object.keys(problem.errors).forEach(function (key) {

            let value = problem.errors[key];

            message += "<li>";

            if (value.errorOwner) {
                //  This is a validation error

                if (value.errorOwner.getType() === "radiogroup") {
                    //  We are selecting the first option to href to
                    message += "<a href='#" + value.errorOwner.inputId + "_0'>";
                } else {
                    message += "<a href='#" + value.errorOwner.inputId + "'>";
                }

                message += value.errorOwner.title;
                message += " - " + value.getText();
                message += "</a>";
            }
            else if (value.type) {
                //  This is an unhandled exception
                message += value.message;
            }
            else {
                //  This is anything else coming the server that is of type ProblemDetails
                message += value;
            }

            message += "</li>";
        });
    }

    message += "</ol>";
    message += "</section>";

    return message;
}

export function printProblemDetails(problem: ProblemDetails, lang: string) {

    const errorSection = document.getElementById("div_errors_list");

    if (errorSection && problem) {
        errorSection.innerHTML = buildValidationErrorMessage(problem, lang);
        errorSection.style.display = 'block';
    }
}

export function clearProblemDetails() {

    const errorSection = document.getElementById("div_errors_list");

    if (errorSection) {
        errorSection.innerHTML = "";
        errorSection.style.display = 'none';
    }
}

