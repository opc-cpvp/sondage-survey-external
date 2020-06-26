var widget = {
    //the widget name. It should be unique and written in lowcase.
    name: "checkboxwithhtmlinfo",
    //the widget title. It is how it will appear on the toolbox of the SurveyJS Editor/Builder
    title: "Check box with Html info",
    iconName: "",
    //If the widgets depends on third-party library(s) then here you may check if this library(s) is loaded
    widgetIsLoaded: function () {
        return true; //we do not require anything so we just return true. 
    },
    //SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior
    isFit: function (question) {
        //we return true if the type of question is commentwithhtml
        return question.getType() === 'checkbox' && question.asHtmlInfo == true;
       // return ['checkbox'].indexOf(question.getType()) > -1;
    },
    //Use this function to create a new class or add new properties or remove unneeded properties from your widget
    //activatedBy tells how your widget has been activated by: property, type or customType
    //property - it means that it will activated if a property of the existing question type is set to particular value, for example inputType = "date" 
    //type - you are changing the behaviour of entire question type. For example render radiogroup question differently, have a fancy radio buttons
    //customType - you are creating a new type, like in our example "textwithbutton"
    activatedByChanged: function (activatedBy) {
        //we do not need to check acticatedBy parameter, since we will use our widget for customType only
        //We are creating a new class and derived it from text question type. It means that text model (properties and fuctions) will be available to us
        //Survey.JsonObject.metaData.addClass("commentwithhtmldescription", [], null, "comment");
        //signaturepad is derived from "empty" class - basic question class
        //Survey.JsonObject.metaData.addClass("signaturepad", [], null, "empty");

        //Add new property(s)
        //For more information go to https://surveyjs.io/Examples/Builder/?id=addproperties#content-docs
        Survey.JsonObject.metaData.addProperties("commentwithhtmldescription", [
            {
                name: "htmlInfo", default: "",
                name: "asHtmlInfo", default: false
            }
        ]);
    },
    //If you want to use the default question rendering then set this property to true. We do not need any default rendering, we will use our our htmlTemplate
    isDefaultRender: false,
    //You should use it if your set the isDefaultRender to false
    //htmlTemplate: "<div><input /><button></button></div>",
    //The main function, rendering and two-way binding
    afterRender: function (question, el) {

        var outputHTML = '';
        var allCheckboxes = question.choices;

        allCheckboxes.forEach(function (row, index, rows) {
            outputHTML = outputHTML +
                '<div>' +
                '   <label data - index="1" role = "checkbox" >' +
                '   <input type="checkbox" name="' + row.value + '" value="' + row.value + '" class="checkbox-info-popup-trigger"/>' + row.text + 
                '   </label>' +
                '   <div class="info-popup alert alert-info col-md-12 mrgn-tp-md" style="display: none;"><p>The institution has not responded to your access or correction request within the statutory time limits. Under the <i>Privacy Act</i> institutions have thirty (30) days after your request is received to respond (or 60 days if an extension has been properly invoked for an access request).</p></div>' +
                '</div >'
        });

        el.innerHTML = outputHTML;
    },
    //Use it to destroy the widget. It is typically needed by jQuery widgets
    willUnmount: function (question, el) {
        //We do not need to clear anything in our simple example
        //Here is the example to destroy the image picker
        //var $el = $(el).find("select");
        //$el.data('picker').destroy();
    }
}

//Register our widget in singleton custom widget collection
Survey.CustomWidgetCollection.Instance.addCustomWidget(widget, "customtype");
