import { ProblemDetails } from "./models/problemDetails";
import { MultiLanguagePropery } from "./models/multiLanguageProperty";
import * as Survey from "survey-vue";

export function getTranslation(questionProperty: MultiLanguagePropery, lang: string): string {
    if (lang === "fr" && questionProperty.fr) {
        return questionProperty.fr;
    } else if (questionProperty.en) {
        return questionProperty.en;
    } else {
        return questionProperty.default;
    }
}

// Will return true if a dropdown has a selected item otherwise false.
export function HasSelectedItem(params: any[]): boolean {
    const value = params[0];
    return value !== "";
}

// This function will build a <section> with a list of errors to be displayed at the top of the page
export function printProblemDetails(problem: ProblemDetails, lang: string): void {
    const errorSection = document.getElementById("div_errors_list");

    if (errorSection && problem) {
        errorSection.innerHTML = "";

        const section = document.createElement("section");
        section.classList.add("alert");
        section.classList.add("alert-danger");

        const h2Title = document.createElement("H2");
        let textTitle;
        if (lang === "fr") {
            textTitle = document.createTextNode("Le formulaire ne pouvait pas être soumis parce que des erreurs ont été trouvée");
        } else {
            textTitle = document.createTextNode("The form could not be submitted because error(s) was found");
        }
        h2Title.appendChild(textTitle);
        section.appendChild(h2Title);

        if (problem.title) {
            const title = document.createElement("p");
            const titleText = document.createTextNode(problem.title);
            title.appendChild(titleText);
            section.appendChild(title);
        }

        if (problem.detail) {
            const details = document.createElement("p");
            const detailsText = document.createTextNode(problem.detail);
            details.appendChild(detailsText);
            section.appendChild(details);
        }

        const list = document.createElement("ol");

        if (problem.errors) {
            Object.keys(problem.errors).forEach(key => {
                const valueError = problem.errors[key];

                if (valueError.errorOwner) {
                    const question = valueError.errorOwner as Survey.Question;

                    //  This is a validation error from survey.js
                    const li = document.createElement("li");
                    const a = document.createElement("a");

                    if (question.getType() === "radiogroup") {
                        //  We are selecting the first option to href to
                        a.setAttribute("href", "#" + question.inputId + "_0");
                    } else {
                        a.setAttribute("href", "#" + question.inputId);
                    }

                    a.innerHTML = `${question.title} - ${valueError.getText() as string}`;

                    li.appendChild(a);
                    list.appendChild(li);
                } else if (Array.isArray(valueError)) {
                    //  This is ProblemDetails.Errors with multiple value for the same key
                    valueError.forEach((item: string) => {
                        const li = document.createElement("li");
                        li.innerHTML = `${key} - ${item}`;
                        list.appendChild(li);
                    });
                } else if (valueError.type) {
                    //  This is an unhandled exception
                    const li = document.createElement("li");
                    li.innerHTML = valueError.message;
                    list.appendChild(li);
                } else {
                    //  This is anything else coming the server that is of type ProblemDetails
                    const li = document.createElement("li");
                    li.innerHTML = valueError;
                    list.appendChild(li);
                }
            });
        }

        section.appendChild(list);

        errorSection.appendChild(section);
        errorSection.classList.remove("hidden");
        errorSection.classList.add("show");

        window.scrollTo(0, 0);
    }
}

export function printWarningMessage(messageEn: string, messageFr: string, lang: string): void {
    const errorSection = document.getElementById("div_errors_list");

    if (errorSection) {
        errorSection.innerHTML = "";

        const section = document.createElement("section");
        section.classList.add("alert");
        section.classList.add("alert-warning");

        const h2Title = document.createElement("H2");

        if (lang === "fr") {
            h2Title.innerText = "Message";
        } else {
            h2Title.innerText = "Message";
        }

        section.appendChild(h2Title);

        const messageParagraphe = document.createElement("p");

        if (lang === "fr") {
            messageParagraphe.innerText = messageFr;
        } else {
            messageParagraphe.innerText = messageEn;
        }

        section.appendChild(messageParagraphe);

        errorSection.appendChild(section);
        errorSection.classList.remove("hidden");
        errorSection.classList.add("show");

        window.scrollTo(0, 0);
    }
}

export function clearProblemDetails(): void {
    const errorSection = document.getElementById("div_errors_list");

    if (errorSection) {
        errorSection.innerHTML = "";
        errorSection.classList.remove("show");
        errorSection.classList.add("hidden");
    }
}

export function getProvinceFrenchPrefix_au(provinceId: number): string {

    switch (provinceId) {
        case 1: //  Ontario
        case 3: //  Nouvelle Ecosse
        case 6: //  Columbie Britanique
        case 8: //  Sasketchewan
        case 9: //  Alberta
        {
            return "en ";
        }
        case 2: //  Quebec
        case 4: //  Nouveau-Brunswick
        case 5: //  Manitoba
        case 11: //  Nunavut
        case 12: //  Yukon
        {
            return "au ";
        }
        case 7: {
            //  Ile du Prince Edouard
            return "à l'";
        }
        case 10: {
            //  Terre Neuve
            return "à ";
        }
        case 13: {
            //  Territoires Nord Ouest
            return "aux ";
        }
        case 14: {
            //  Autre
            return "à l'";
        }
        default:
            return "";
    }
}

export function getProvinceFrenchPrefix_du(provinceId: number): string {

    switch (provinceId) {
        case 1: //  Ontario
        case 9: //  Alberta
        case 7: //  Ile du Prince Edouard
        case 14: //  Autre
        {
            return "de l'";
        }
        case 2: //  Quebec
        case 4: //  Nouveau-Brunswick
        case 5: //  Manitoba
        case 11:    //  Nunavut
        case 12:    //  Yukon
        {
            return "du ";
        }
        case 8: //  Sasketchewan
        case 6: //  Columbie Britanique
        case 3: //  Nouvelle Ecosse
        {
            return "de la ";
        }
        case 10: {
            //  Terre Neuve
            return "de ";
        }

        case 13: {
            //  Territoires Nord Ouest
            return "des ";
        }
        default:
            return "";
    }
}
