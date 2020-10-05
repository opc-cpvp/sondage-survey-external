# online-complaint-form-pa

### Survey librairy update
When updating the survey package (survey-vue) because these guys are not super careful when updating. 
I am not 100% sure of how they operate but it seems like code get moved around or deleted on their end.

- Before 1.7.26... they have removed the property 'owner' in survey.onGetQuestionTitle. This was breaking our project.
- From 1.7.26 to 1.7.28: our file upload module was missing the choose file button. This got fixed at their end.
- From 1.7.28 to 1.8.3: the choose file button is gone

### Where to find Documentation

- https://surveyjs.io/Examples/Library is very complete in terms of examples
- https://surveyjs.io/Documentation/Library is where you can get property, event or method details
- The latest code of Survey.js lives here https://github.com/surveyjs/survey-library
- I didn't find much on StackOverflow but here, I have found a lot of questions/answers https://github.com/surveyjs/survey-library/issues?q=
- Here is the link to the question I've asked regarding hiding a page on preview: https://github.com/surveyjs/survey-library/issues/2268

### Special libs & polyfills used

a) showdown.js found at https://cdnjs.cloudflare.com/ajax/libs/showdown/1.9.1/showdown.min.js. 
	It is used;
		- to convert markdown text to html 
		- convert to html description found in 'comment' type questions
		- the * at the beginning of required questions

b) Ladda: https://github.com/hakimel/Ladda -> npm install ladda
	It is used for a loading/saving spinner

### Added Nuget packages
- Hellang.Middleware.ProblemDetails for error handling in Web Apis. This is to standardize the erro message format coming from API's
- libphonenumber-csharp already in used in the original project
- FluentValidator

### Survey.js Custom Widget

Located in the code in ~\wwwroot\js\survey\

1) widgetCheckboxHtml
- This is to display additional information under a checkbox when it is being checked. Plain text or html can be used.
- To use it, add the property 'hasHtmlAddtionalInfo' : 'true' to a checkbox question AND for the desired items (choices), add the property 'htmlAdditionalInfo':'your html'
- Each check box item is being constructed in the function 'afterRender'.
- There is a function checkBoxInfoPopup(checkbox) needed for it to work

* All other widgets are only for testing features. They are not being used.

### Notes on CSS classes

- When Survey.js builds the pages, it sets a lot of css classes on html elements. These classes are not used for styling in our case. In
other words, we are not using the style provided by Survey.js but rather the css for GoC. There is some custom css in ~\wwwroot\css\site.css

### Preview Problem

In order for survey to show the preview at the end we must set survey.showPreviewBeforeComplete property. The options are showAnsweredQuestions or showAllQuestions. 
	- showAllQuestions will show all questions including 'html' type question. I don't think it make sens to display html question.
	- showAnsweredQuestions will only show the question that we answered. Problem is if we want the user to see the empty answers. 

Also, for all questions, the 'description' property is being hidden in the survey event onAfterRenderQuestion
Also, for 'html' questions, during the preview, the actual html gets wiped out in survey event onUpdateQuestionCssClasses

I have asked the guy at Survey.js. Here is the link to the question: https://github.com/surveyjs/survey-library/issues/2268
Hopefully he can implement something soon. I finally found a solution to hide whole pages on preview using onUpdatePanelCssClasses


### Error handling

1) The error messages can be updated at the survey level like this...
	Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
	... and also at the question level by adding the property "requiredErrorText": "This field is required at the question level"
	The question level overwrites the survey level

2)	The code to build the error box at the top of the page when thre is errors is in SurveyHelper.js in function printProblemDetails. 
	There is probably a better way to handle the construction of the "section", specially for multilanguage sites but for now it is working.

### Localization notes

a) Files needs to be saved as UTF-8 in order for the accents to be displayed properly

### TODO 

1) Replace the div id="div_errors_list" in body
2) WCAG compliance, talk to Stephanie
3) Complete page -> Page refresh problem
4) Find a strategy to clear local storage. Put a timestamp on local storage?
5) Make sure the css classes are the same on the checkboxes & the radio buttons
6) Log JS exceptions using a web api. console.write
7) (PA) There is a bug with the logic of the checkboxes with html and the textarea to show in the next page after. Something
		to do with isAnyFirstFourSelected && isAnyLastFiveSelected
8) (PA) Fix this logic in the json -> Phone number should be required only for the complainant if no representative and only the 
		representative if there's a representative
9) PDF: 
	- Some text was screwed up by using french apostrophe. Using single quote fixes it. Maybe saving in utf-8

10) Add "maxLength": value to all the property that have a maximum in the json
11) Get rid of the 'survey' global variable. That causes an issues with lint
		-	There is a problem with the navigation
		-	There is a problem with the checkboxes with html (Fixed)

12) Matrixdynamic 
		-	Other box need some style
		-	Figure out the logic for duplicate selection

13) The bundle.js is getting to 40Mb. Figure out what makes it so big.
14) Build the preview page using the same logic as for pdf, e.g. creating a new survey. There is too much blank wasted spaces
		with the existing one because we are hidding divs instead of not having them

### Mode details required or help required
 
1) The alert-label-error when a question is not answered is not displaying properly. It is just CSS.

### Fixed todos

1) French accents in json files are not rendered properly. The one found in Survey.js are ok. That json file needed to be saved as UTF-8
2) On alert-label-error, make a box on the top of the page showing all errors
3) Validation not working for checkboxes with additional html. Plan B: always show the html additional information and use the 'comment' as is (DONE with plan A)
4) When selecting "Are you filing this complaint on your own behalf (or for a minor child you are guardian of) or on behalf of someone else?" -> Someone else,
then the section "Representative" info is missing
5) Match the property names with the original project 
6) Survey.StylesManager.Enabled = false
7) The style on the 'comment' has crapped after disabling the native style (Survey.StylesManager.Enabled)
8) The page & panel title h tag have been hard coded in Survey.vue.OPC.js. Need to find a way to not do that. 
	[Hint: look for SurveyTemplateText() but it only looks like it is avaialble for knockout]
9) When selecting "Are you filing this complaint on your own behalf (or for a minor child you are guardian of) or on behalf of someone else?" -> Someone else,
then the section "Authorization form attachment(s)" info is missing when uploading files
10) Return to the same question on page refresh or on language switching (DONE -> but need to implement logic to access/store data to the database)
11) Fr & en property of elements (Stephanie). This is started in we are now using only 1 file (survey_pa_complaint.json).
12) Some of the links/urls have not been set for french in html "sections" when I add the "en" + "fr" parts
13) Part-C, at the question 'Did the institution agree to process your request on an informal basis?', the 'htmldescription' is missing. 
	I need to create another widget for radiobuttons, just like for the checkboxHtml. No need to custom widget.
14) The logic for showing the information section in Part C section 4 needs to be checked to reproduce exactly the same behaviour
15) Update the page title for every page
16) Hide navigation bar in the 'Preview', except the Complete button
17) Make a proper completed page
18) Complete page -> { survey_file_number }
19) Problem with refresh or language switching during the 'Preview'. We are going back to the start page because currentPageNo gets reset to 0. 
		Fixed using currentPageNo = 999
20) Put the certify page with checkbox the last page
21) Total MB downloaded. I need to know if we are going to 'storeDataAsText' or save the file data to the database. LOCALSTORAGE.PROBLEMS!
		The local storage quota limit is 5MB. Files are stored in the file system for now.
22) prevent files with same name. We are pre-fixing the file name with a timestamps in survey.onUploadFiles. It is way easier then 
		setting up errors on the question and asking the user to do an action.
23) Survey Id generation coming from the email. We are not using surveyId because the surveyId has a different purposes. We are using tokens.
24) Create a C# object from JSON to be sent to CRM
25) Rename survey parameter in function, use sender instead. It can lead to confusion with the survey object
26) Server side validation - work with Data annotation for SurveyPAModel
27) Javascript fetch is not working, ask PL for solution. Replace ajax calls by fetch. Used polyfill & fetch.js
28) PDF;
	- English is working, french not working (property mode had to be set on SurveyPDF object).
	- Why do we have blank spaces (whole page) see first page. Options.compress: true seems to not include the answers? (Fix with updated/modified json)
	- Fit more questions by page, now it looks like pages are broken down by pages (Fix with updated/modified json)
	- Hide html type questions (Fix with updated/modified json)
	- Hide whole pages (Fix with updated/modified json)
	- Not exporting pdf in francais (property locale had to be set on SurveyPDF object)
	- Make it not editable (property mode had to be set on SurveyPDF object)
	- Remove question numbers (property showQuestionNumbers had to be set on SurveyPDF object)
	- Markdown is not working (using surveyPDF.save(filename))
	- Attachments are missing
	- Remove menu button when finished with the bugs
	- Some text was screwed up by using french apostrophe. Using single quote fixes it. Maybe saving in utf-8
	- Try to have only one json to manage
	- question not visible if still showing

29) Set the navigation buttons to be invisble by default
30) IE;
	- Details-summary tags not working. (fixed using a polyfill > details-polyfill)
	- Chekcboxes with html don't work. 
		i) Element.closest not supported by IE (fixed using the polyfill -> element-closest-polyfill)
		2) Use onchange event instead of onclick on the input
	- - the whole file preview thing is not working as well as the html "meter" object. Fixed it by adding a div inside the meter	

31) Put a spinner when completing the survey since there is 2 api calls. Bootstrap. Talk to Josh. Vue component. PBR client project
32) Using fetch instead of ajax or XMLHttpRequest 
	- Is not working syncro oncompleting and it cannot reach onComplete. Work around: use XMLHttpRequest async = false in oncompleting.
	- Also, I cannot get the ReferenceNumber property from the response. Work around: use XMLHttpRequest (FIXED)
33) Refactor widget comment with html
34) JS Error: 
	Uncaught TypeError: Cannot read property 'length' of undefined
    at Function.ChoicesRestfull.unregisterSameRequests (choicesRestfull.ts:76)
    at ChoicesRestfull.onLoad (choicesRestfull.ts:359)
    at XMLHttpRequest.xhr.onload (choicesRestfull.ts:197)
	This is exception seems to have dissapeared.
35) localization in the model, inject IStringLocalizer didn't work. Maybe I need to have an interface like ISurveyPAModel
	Localization is done in the Validator classes and it is working.

### Postponed todo
a) Explore survey Creator (POSTPONED)
b) The navigation panel is the native of and should be replaced (POSTPONED)
c) Add a modal popup to confirm when completing the survey. There is too many ways to do this and I'm not sure which one is best for you.
d) Create a queryable javascript object that contains the json data. To be able to show the preview button based on some logic. There is
	no need for this just now.
e) Backend error logging - need to ask soemone

### Won't fix or implement'

a) Try to make use of the start properties of survey (survey.firstPageIsStarted = true; OR survey.startSurveyText = "Start";). It's not going
	to work with our custom navigation AND also on page refresh or on language switching it returns to the start page which is not
	what we want.

### How to use the Survey widgets
1) npm install surveyjs-widgets
2) In *.cshtml file add if using select2 or tagbox 
	```	    
	<script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.4/js/select2.js"></script>
	<link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.4/css/select2.min.css" rel="stylesheet" />
	```

3) In *.ts file
	```
	import * as widgets from "surveyjs-widgets";
	```
	...then
	```
	widgets.select2tagbox(Survey);
	```


### Editable strings in Survey

```
 Survey.surveyLocalization.locales["en"].otherItemText = "Other";
 Survey.surveyLocalization.locales["fr"].otherItemText = "Autre";	
```

- pagePrevText: "Previous",
- pageNextText: "Next",
- completeText: "Complete",
- previewText: "Preview",
- editText: "Edit",
- startSurveyText: "Start",
- otherItemText: "Other (describe)",
- noneItemText: "None",
- selectAllItemText: "Select All",
- progressText: "Page {0} of {1}",
- panelDynamicProgressText: "Record {0} of {1}",
- questionsProgressText: "Answered {0}/{1} questions",
- emptySurvey: "There is no visible page or question in the survey.",
- completingSurvey: "Thank you for completing the survey!",
- completingSurveyBefore: "Our records show that you have already completed this survey.",
- loadingSurvey: "Loading Survey...",
- optionsCaption: "Choose...",
- value: "value",
- requiredError: "Please answer the question.",
- requiredErrorInPanel: "Please answer at least one question.",
- requiredInAllRowsError: "Please answer questions in all rows.",
- numericError: "The value should be numeric.",
- textMinLength: "Please enter at least {0} characters.",
- textMaxLength: "Please enter less than {0} characters.",
- textMinMaxLength: "Please enter more than {0} and less than {1} characters.",
- minRowCountError: "Please fill in at least {0} rows.",
- minSelectError: "Please select at least {0} variants.",
- maxSelectError: "Please select no more than {0} variants.",
- numericMinMax: "The '{0}' should be equal or more than {1} and equal or less than {2}",
- numericMin: "The '{0}' should be equal or more than {1}",
- numericMax: "The '{0}' should be equal or less than {1}",
- invalidEmail: "Please enter a valid e-mail address.",
- invalidExpression: "The expression: {0} should return 'true'.",
- urlRequestError: "The request returned error '{0}'. {1}",
- urlGetChoicesError: "The request returned empty data or the 'path' property is incorrect",
- exceedMaxSize: "The file size should not exceed {0}.",
- otherRequiredError: "Please enter the other value.",
- uploadingFile: "Your file is uploading. Please wait several seconds and try again.",
- loadingFile: "Loading...",
- chooseFile: "Choose file(s)...",
- noFileChosen: "No file chosen",
- confirmDelete: "Do you want to delete the record?",
- keyDuplicationError: "This value should be unique.",
- addColumn: "Add column",
- addRow: "Add row",
- removeRow: "Remove",
- addPanel: "Add new",
- removePanel: "Remove",
- choices_Item: "item",
- matrix_column: "Column",
- matrix_row: "Row",
- savingData: "The results are saving on the server...",
- savingDataError: "An error occurred and we could not save the results.",
- savingDataSuccess: "The results were saved successfully!",
- saveAgainButton: "Try again",
- timerMin: "min",
- timerSec: "sec",
- timerSpentAll: "You have spent {0} on this page and {1} in total.",
- timerSpentPage: "You have spent {0} on this page.",
- timerSpentSurvey: "You have spent {0} in total.",
- timerLimitAll: "You have spent {0} of {1} on this page and {2} of {3} in total.",
- timerLimitPage: "You have spent {0} of {1} on this page.",
- timerLimitSurvey: "You have spent {0} of {1} in total.",
- cleanCaption: "Clean",
- clearCaption: "Clear",
- chooseFileCaption: "Choose file",
- removeFileCaption: "Remove this file",
- booleanCheckedLabel: "Yes",
- booleanUncheckedLabel: "No",
- confirmRemoveFile: "Are you sure that you want to remove this file: {0}?",
- confirmRemoveAllFiles: "Are you sure that you want to remove all files?",
- questionTitlePatternText: "Question Title"
