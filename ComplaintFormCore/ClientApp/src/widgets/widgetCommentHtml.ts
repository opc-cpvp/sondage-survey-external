import * as Survey from "survey-vue";
import { Widget } from "./widget";

export class WidgetCommentHtml extends Widget {
    private activatedBy = "property";

    constructor() {
        super(
            "commentwithhtmldescription",
            "Comment box with Html description"
        );
    }

    static init(): void {
        const widget: WidgetCommentHtml = new WidgetCommentHtml();

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
            return (
                question.renderAs === "commentwithhtmldescription" &&
                question.getType() === "comment"
            );
        } else if (this.activatedBy === "type") {
            return question.getType() === "comment";
        } else if (this.activatedBy === "customtype") {
            return question.getType() === "commentwithhtmldescription";
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
        Survey.JsonObject.metaData.removeProperty("comment", "renderAs");
        if (activatedBy === "property") {

            Survey.JsonObject.metaData.addProperty("comment", {
                name: "renderAs",
                category: "general",
                default: "standard",
                choices: ["standard", "commentwithhtmldescription"]
            });

            Survey.JsonObject.metaData.addProperties("comment", [
                { name: "htmldescription", default: "" }
            ]);

        } else if (activatedBy === "customtype") {
            Survey.JsonObject.metaData.addClass("commentwithhtmldescription", [], undefined, "comment");
        }
    }

    /**
     * The main function, rendering and two-way binding.
     *
     * @param question
     * @param el
     */
    afterRender(question: Survey.IQuestion, el: HTMLElement): void {
        // NOTE:    This is where we are setting the "description" property from the htmldescription.
        //          At the moment I do not know why the html gets parse as html but it works! Magic.

        const commentQuestion: Survey.QuestionCommentModel = question as unknown as Survey.QuestionCommentModel;
        const locale = question.getLocale();

        if (commentQuestion.htmldescription) {

            let description = "";

            if (locale === "fr" && commentQuestion.htmldescription.fr) {
                description = commentQuestion.htmldescription.fr;
            } else if (commentQuestion.htmldescription.en) {
                description = commentQuestion.htmldescription.en;
            } else {
                description = commentQuestion.htmldescription;
            }

            commentQuestion.description = description;
        }
    }
}
