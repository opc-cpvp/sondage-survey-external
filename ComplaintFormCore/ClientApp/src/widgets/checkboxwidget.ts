import { Widget } from "./widget";
import * as Survey from "survey-vue";

export class CheckboxWidget extends Widget {
    private activatedBy = "property";

    constructor() {
        super("checkboxwithhtmlinfo", "Checkbox list with addtional Html info");
    }

    static init(): void {
        const widget: CheckboxWidget = new CheckboxWidget();

        // If activatedBy isn't passed, it will default to property.
        Survey.CustomWidgetCollection.Instance.addCustomWidget(widget);
    }

    /**
     * SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior.
     *
     * @param question
     */
    isFit(question: Survey.IQuestion): boolean {
        if (this.activatedBy === "property") {
            return question.renderAs === "checkboxwithhtmlinfo" && question.getType() === "checkbox";
        } else if (this.activatedBy === "type") {
            return question.getType() === "checkbox";
        } else if (this.activatedBy === "customtype") {
            return question.getType() === "checkboxwithhtmlinfo";
        }
        return false;
    }

    /**
     * Use this function to create a new class or add new properties or remove unneeded properties from your widget.
     *
     * @param activatedBy Tells how your widget has been activated by: property, type or customType.
     * property - It means that it will activated if a property of the existing question type is set to particular value, for example inputType = "date".
     * type - You are changing the behaviour of entire question type. For example render radiogroup question differently, have a fancy radio buttons.
     * customType - You are creating a new type, like in our example "textwithbutton".
     */
    activatedByChanged(activatedBy: string): void {
        this.activatedBy = activatedBy;
        Survey.JsonObject.metaData.removeProperty("checkbox", "renderAs");
        if (activatedBy === "property") {
            Survey.JsonObject.metaData.addProperty("checkbox", {
                name: "renderAs",
                category: "general",
                default: "standard",
                choices: ["standard", "checkboxwithhtmlinfo"]
            });
        } else if (activatedBy === "customtype") {
            Survey.JsonObject.metaData.addClass("checkboxwithhtmlinfo", [], undefined, "checkbox");
        }
    }

    /**
     * The main function, rendering and two-way binding.
     *
     * @param question
     * @param el
     */
    afterRender(question: Survey.IQuestion, el: HTMLElement): void {
        const checkboxQuestion: Survey.QuestionCheckboxModel = (question as unknown) as Survey.QuestionCheckboxModel;
        const locale = question.getLocale();

        for (const choice of checkboxQuestion.choices) {
            const item: Survey.ItemValue = choice as Survey.ItemValue;

            // ignore choices without the additional info
            if (!choice.htmlAdditionalInfo) {
                continue;
            }

            const input: HTMLInputElement | null = el.querySelector(`input[value="${item.value as string}"]`);
            const container = input?.closest("div");

            if (!input || !container) {
                continue;
            }

            const additionalInfo = locale === "fr" ? choice.htmlAdditionalInfo.fr : choice.htmlAdditionalInfo.en;

            // create / append the alert to the choice
            const alert = document.createElement("div");
            alert.className = "alert alert-info";
            alert.innerHTML = additionalInfo;
            alert.classList.add(input.checked ? "show" : "hidden");

            input.onchange = (ev: Event) => {
                alert.classList.toggle("hidden");
                alert.classList.toggle("show");
            };

            container?.appendChild(alert);
        }
    }
}
