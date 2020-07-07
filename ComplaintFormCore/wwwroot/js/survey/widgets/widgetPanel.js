var widget = {
    //the widget name. It should be unique and written in lowcase.
    name: "panelinvisibleatpreview",
    //the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder
    title: "Make any 'panel' type question invisible in the Preview page",
    //the name of the icon on the toolbox. We will leave it empty to use the standard one
    iconName: "",
    //If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded
    widgetIsLoaded: function () {
        //return typeof $ == "function" && !!$.fn.select2; //return true if jQuery and select2 widget are loaded on the page
        return true; //we do not require anything so we just return true. 
    },
    //SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior
    isFit: function (question) {
        return question.getType() === 'panel' && !question.showOnPreview && !question.isReadOnly;
    },

    //If you want to use the default question rendering then set this property to true. We do not need any default rendering, we will use our our htmlTemplate
    isDefaultRender: true,
    afterRender: function (question, el) {

        debugger;
    }
}

//Register our widget in singleton custom widget collection
Survey.CustomWidgetCollection.Instance.addCustomWidget(widget, "customtype");
