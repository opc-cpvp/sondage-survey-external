(function () {
    var widget = {
        //the widget name. It should be unique and written in lowcase.
        name: "commentwithhtmldescription",

        //the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder
        title: "Comment box with Html description",

        //the name of the icon on the toolbox. We will leave it empty to use the standard one
        iconName: "",

        //If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded
        widgetIsLoaded: function () {
            return true; //we do not require anything so we just return true. 
        },

        //SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior
        isFit: function (question) {
            //we return true if the type of question is 'comment'
            return question.getType() === 'comment';
        },

        //Use this function to create a new class or add new properties or remove unneeded properties from your widget
        //activatedBy tells how your widget has been activated by: property, type or customType
        //property - it means that it will activated if a property of the existing question type is set to particular value, for example inputType = "date" 
        //type - you are changing the behaviour of entire question type. For example render radiogroup question differently, have a fancy radio buttons
        //customType - you are creating a new type, like in our example "textwithbutton"
        activatedByChanged: function (activatedBy) {

            //Add new property(s)
            Survey.JsonObject.metaData.addProperties("comment", [
                { name: "htmldescription", default: "" }
            ]);
        },

        //If you want to use the default question rendering then set this property to true. We do not need any default rendering, we will use our our htmlTemplate
        isDefaultRender: true,

        //You should use it if your set the isDefaultRender to false
        // htmlTemplate: "<div></div>",

        //The main function, rendering and two-way binding
        afterRender: function (question, el) {

            // NOTE:    This is where we are setting the "description" property from the htmldescription.
            //          At the moment I do not know why the html gets parse as html but it works! Magic.
            if (question.htmldescription) {

                var description = "";

                if (question.survey.locale == 'fr' && question.htmldescription.fr) {
                    description = question.htmldescription.fr;
                }
                else if (question.htmldescription.en) {
                    description = question.htmldescription.en;
                }
                else {
                    description = question.htmldescription;
                }

                question.description = description;
            }
        },
        //Use it to destroy the widget. It is typically needed by jQuery widgets
        willUnmount: function (question, el) {
            //We do not need to clear anything in our simple example
            //Here is the example to destroy the image picker
        }
    }

    //Register our widget in singleton custom widget collection
    Survey.CustomWidgetCollection.Instance.addCustomWidget(widget, "customtype");
})();