import { ProblemDetails } from "./problemDetails";

export function getTranslation(questionProperty, lang: string): string {
    if (lang === "fr" && questionProperty.fr) {
        return questionProperty.fr;
    } else if (questionProperty.en) {
        return questionProperty.en;
    } else {
        return questionProperty;
    }
}

// Will return true if a dropdown has a selected item otherwise false.
export function HasSelectedItem(params): boolean {
    const value = params[0];
    return value !== "";
}

// This function will build a <section> with a list of errors to be displayed at the top of the page
export function buildValidationErrorMessage(problem: ProblemDetails, lang: string): string {
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

            const value = problem.errors[key];

            if (value.errorOwner) {
                //  This is a validation error from survey.js
                message += "<li>";
                if (value.errorOwner.getType() === "radiogroup") {
                    //  We are selecting the first option to href to
                    message += "<a href='#" + value.errorOwner.inputId + "_0'>";
                } else {
                    message += "<a href='#" + value.errorOwner.inputId + "'>";
                }

                message += value.errorOwner.title;
                message += " - " + value.getText();
                message += "</a>";
                message += "</li>";
            }
            else if (Array.isArray(value)) {
                value.forEach(function (item) {
                    message += "<li>";
                    message += item;
                    message += "</li>";
                });
            }
            else if (value.type) {
                //  This is an unhandled exception
                message += "<li>";
                message += value.message;
                message += "</li>";
            }
            else {
                //  This is anything else coming the server that is of type ProblemDetails
                message += "<li>";
                message += value;
                message += "</li>";
            }
        });
    }

    message += "</ol>";
    message += "</section>";

    return message;
}

export function printProblemDetails(problem: ProblemDetails, lang: string): void {

    const errorSection = document.getElementById("div_errors_list");

    if (errorSection && problem) {
        const section = document.createElement("section") as HTMLElement;
        section.classList.add("alert alert-danger");

        const h2Title = document.createElement("H2");
        let textTitle;
        if (lang === "fr") {
            textTitle = document.createTextNode("Le formulaire ne pouvait pas être soumis parce que des erreurs ont été trouvée");
        }
        else {
            textTitle = document.createTextNode("The form could not be submitted because error(s) was found");
        }

        section.appendChild(h2Title);

        errorSection.innerHTML = buildValidationErrorMessage(problem, lang);
        errorSection.style.display = "block";

        window.scrollTo(0, 0);
    }
}

export function clearProblemDetails(): void {

    const errorSection = document.getElementById("div_errors_list");

    if (errorSection) {
        errorSection.innerHTML = "";
        errorSection.style.display = "none";
    }
}

export function addItemToPrint(item: string): string{
    if (item && item.length > 0) {
        return item + "</br>";
    }

    return "";
}

export function getLiValue(errors: any): string {

    if (errors) {
        Object.keys(errors).forEach(function (key) {
            const value = errors[key];

            if (value.errorOwner) {
                //  This is a validation error from survey.js
                if (value.errorOwner.getType() === "radiogroup") {
                    //  We are selecting the first option to href to
                    return "<a href='#" + value.errorOwner.inputId + "_0'>" + value.errorOwner.title + " - " + value.getText() + "</a>";
                } else {
                    return "<a href='#" + value.errorOwner.inputId + "'>" + value.errorOwner.title + " - " + value.getText() + "</a>";
                }
            }
            else if (Array.isArray(value)) {
                value.forEach(function (item) {
                    message += "<li>";
                    message += item;
                    message += "</li>";
                });
            }
            else if (value.type) {
                //  This is an unhandled exception
                return value.message;
            }
            else {
                //  This is anything else coming the server that is of type ProblemDetails
                return value;
            }
        });
    }
    else {
        return ""
    };



    
}