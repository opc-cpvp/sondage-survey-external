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

                const value = problem.errors[key];

                if (value.errorOwner) {
                    //  This is a validation error from survey.js
                    const li = document.createElement("li");
                    const a = document.createElement("a");

                    if (value.errorOwner.getType() === "radiogroup") {
                        //  We are selecting the first option to href to
                        a.setAttribute("href", "#" + value.errorOwner.inputId + "_0");
                    } else {
                        a.setAttribute("href", "#" + value.errorOwner.inputId);
                    }

                    a.innerHTML = value.errorOwner.title + " - " + value.getText();

                    li.appendChild(a);
                    list.appendChild(li);
                } else if (Array.isArray(value)) {
                    value.forEach(item => {
                        const li = document.createElement("li");
                        li.innerHTML = key + " - " + item;
                        list.appendChild(li);
                    });
                } else if (value.type) {
                    //  This is an unhandled exception
                    const li = document.createElement("li");
                    li.innerHTML = value.message;
                    list.appendChild(li);
                } else {
                    //  This is anything else coming the server that is of type ProblemDetails
                    const li = document.createElement("li");
                    li.innerHTML = value;
                    list.appendChild(li);
                }
            });
        }

        section.appendChild(list);

        errorSection.appendChild(section);
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
