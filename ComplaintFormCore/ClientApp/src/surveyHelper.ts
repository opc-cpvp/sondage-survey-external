declare let $: any;

export function getTranslation(questionProperty, lang:string) {
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
export function buildValidationErrorMessage(problem, lang: string) {
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
        message += problem.title + "\n";
    }

    if (problem.detail) {
        message += problem.detail + "\n";
    }

    message += "<ul>";

    let errorIndex = 1;

    if (problem.errors) {
        problem.errors.forEach(function (key, value) {
            message += "<li>";
            if (lang == "fr") {
                message += "Erreur " + errorIndex + ": ";
            }
            else {
                message += "Error " + errorIndex + ": ";
            }
            message += key + " - " + value;
            message += "</li>";
            errorIndex = errorIndex + 1;
        });
    }

    message += "</ul>";
    message += "</section>";

    return message;
}

export function printProblemDetails(problem, lang:string) {
    $("#div_errors_list").html(buildValidationErrorMessage(problem, lang));
    $("#div_errors_list").show();
}
