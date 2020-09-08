import { IQuestion } from "survey-vue";

export abstract class Widget {
    /**
     * The widget name. It should be unique and written in lowcase.
     */
    protected readonly name: string;

    /**
     * The widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder.
     */
    protected readonly title: string;

    /**
     * The name of the icon on the toolbox. We will leave it empty to use the standard one.
     */
    protected readonly iconName: string;

    /**
     * You should use it if your set the isDefaultRender to false.
     */
    protected htmlTemplate = "";

    /**
     * If you want to use the default question rendering then set this property to true. We do not need any default rendering, we will use our our htmlTemplate
     */
    protected isDefaultRender = true;

    /**
     * @param name the widget name. It should be unique and written in lowcase.
     * @param title the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder.
     * @param iconName the name of the icon on the toolbox. We will leave it empty to use the standard one.
     */
    constructor(name: string, title: string, iconName: string = "") {
        this.name = name;
        this.title = title;
        this.iconName = iconName;
    }

    /**
     * The main function, rendering and two-way binding.
     *
     * @param question
     * @param el
     */
    afterRender(question: IQuestion, el: HTMLElement): void {}

    /**
     * Use it to destroy the widget. It is typically needed by jQuery widgets.
     *
     * @param question
     * @param el
     */
    willUnmount(question: IQuestion, el: HTMLElement): void {}

    /**
     * If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded.
     */
    widgetIsLoaded(): boolean {
        return true;
    }

    /**
     * Use this function to create a new class or add new properties or remove unneeded properties from your widget.
     *
     * @param activatedBy Tells how your widget has been activated by: property, type or customType.
     * property - It means that it will activated if a property of the existing question type is set to particular value, for example inputType = "date".
     * type - You are changing the behaviour of entire question type. For example render radiogroup question differently, have a fancy radio buttons.
     * customType - You are creating a new type, like in our example "textwithbutton".
     */
    abstract activatedByChanged(activatedBy: string): void;

    /**
     * SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior.
     *
     * @param question
     */
    abstract isFit(question: IQuestion): boolean;
}
