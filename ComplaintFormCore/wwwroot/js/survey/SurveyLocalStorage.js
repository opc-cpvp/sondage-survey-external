var storageName_PA = "SurveyJS_LoadState_PA";
var storageName_Test = "SurveyJS_LoadState_Test";

function saveStateLocally(survey, storageName) {
    var res = {
        currentPageNo: survey.currentPageNo,
        data: survey.data
    };

    if (survey.isDisplayMode == true) {
        res.currentPageNo = 999;
    }
    else if (survey.state == "completed") {
        res.currentPageNo = 1000;
    }

    //Here should be the code to save the data into your database

    window
        .localStorage
        .setItem(storageName, JSON.stringify(res));
}


function loadStateLocally(survey, storageName, defaultData) {

    //Here should be the code to load the data from your database

    var storageSt = window
        .localStorage
        .getItem(storageName) || "";

    var res = {};
    if (storageSt)
        res = JSON.parse(storageSt); //Create the survey state for the demo. This line should be deleted in the real app.
    else {

         //  If nothing was found we set the default values for the json as well as set the current page to 0
        res = {
            currentPageNo: 0,
            data : defaultData
        };
    }

    if (res.data)
        survey.data = res.data;

    //Set the loaded data into the survey.
    if (res.currentPageNo == 999) {
        survey.showPreview();
    }
    else if (res.currentPageNo == 1000) {
        //go to completed page
    }
    else {
        survey.currentPageNo = res.currentPageNo;
    }     
}

function clearLocalStorage(storageName) {

    window
        .localStorage
        .setItem(storageName, "");
}

