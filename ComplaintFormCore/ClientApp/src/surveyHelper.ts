// import $ from "jquery";
// import * as $ from 'jquery';
declare let $: any;

declare let survey: { locale: string };

export function getTranslation(questionProperty) {
    if (survey.locale === "fr" && questionProperty.fr) {
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
    // alert(value)
    // value is the id of the selected item
    return value !== "";
}

// This function will build a <section> with a list of errors to be displayed at the top of the page
export function buildValidationErrorMessage(problem) {

    let message = "<section role='alert' class='alert alert-danger'>";
    message += "<h2>";

    if (survey.locale === "fr") {
        message += "Le formulaire ne pouvait pas être soumis parce que des erreurs ont été trouvée";
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

    const errorIndex = 1;

    if (problem.errors) {
        // //for (const [key, value] of Object.entries(problem.errors)) {

        // //    message += "<li>";

        // //    if (survey.locale == "fr") {
        // //        message += "Erreur " + errorIndex + ": ";
        // //    }
        // //    else {
        // //        message += "Error " + errorIndex + ": ";
        // //    }

        // //    message += key + " - " + value;

        // //    message += "</li>";

        // //    errorIndex = errorIndex + 1;
        // //}
    }

    message += "</ul>";
    message += "</section>";

    return message;
}

export function printProblemDetails(problem) {
    $("#div_errors_list").html(buildValidationErrorMessage(problem));
    $("#div_errors_list").show();
}
