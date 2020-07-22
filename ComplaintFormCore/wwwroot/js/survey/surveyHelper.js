function getTranslation(questionProperty) {

    if (survey.locale == 'fr' && questionProperty.fr) {
        return questionProperty.fr;
    }
    else if (questionProperty.en) {
        return questionProperty.en;
    }
    else {
        return questionProperty;
    }
}

//  Will return true if a dropdown has a selected item otherwise false.
function HasSelectedItem(params) {
    var value = params[0];
    // alert(value)
    // value is the id of the selected item
    return value !== "";
}