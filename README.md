# online-complaint-form-pa_jf

### Where to find Documentation

- https://surveyjs.io/Examples/Library is very complete in terms of examples
- https://surveyjs.io/Documentation/Library is where you can get property, event or method details
- The latest code of Survey.js lives here https://github.com/surveyjs/survey-library
- I didn't find much on StackOverflow but here, I have found a lot of questions/answers https://github.com/surveyjs/survey-library/issues?q=
- Here is the link to the question I've asked regarding hiding a page on preview: https://github.com/surveyjs/survey-library/issues/2268

### Special lib used

a) showdown.js found at https://cdnjs.cloudflare.com/ajax/libs/showdown/1.9.1/showdown.min.js. 
	It is used;
		- to convert markdown text to html 
		- convert to html description found in 'comment' type questions
		- the * at the beginning of required questions

b) inputmask.js found at https://unpkg.com/inputmask@5.0.3/dist/inputmask.js. it is used for input mask such phone number or postal code (NOT USED YET)

### On the Code 

- All code for Survey.js is located in ~\wwwroot\js\survey\
- I made a copy of survey.vue.js ~\wwwroot\js\survey\survey.vue.OPC.js because I made a few changes and I wanted to keed the original so you can compare with the original.

### Survey.js Custom Widget

Located in the code in ~\wwwroot\js\survey\

1) widgetCommentHtml
- This is to be able to have the 'description' parsed as html.
- Simply add the property 'htmldescription':'your html' to a 'comment' type question

2) widgetCheckboxHtml
- This is to display additional information under a checkbox when it is being checked. Plain text or html can be used.
- To use it, add the property 'hasHtmlAddtionalInfo' : 'true' to a checkbox question AND for the desired items (choices), add the property 'htmlAdditionalInfo':'your html'
- Each check box item is being constructed in the function 'afterRender'.
- There is a function checkBoxInfoPopup(checkbox) needed for it to work

* All other widgets are only for testing features. They are not being used.

### Notes on CSS classes

- When Survey.js builds the pages, it sets a lot of css classes on html elements. These classes are not used for styling in our case. In
other words, we are not using the style provided by Survey.js but rather the css for GoC. There is some custom css in ~\wwwroot\css\site.css

### Preview Problem
	Still not fixed!!!

	In order for survey to show the preview at the end we must set survey.showPreviewBeforeComplete property. The options are showAnsweredQuestions or showAllQuestions. 
		- showAllQuestions will show all questions including 'html' type question. I don't think it make sens to display html question.
		- showAnsweredQuestions will only show the question that we answered. Problem is if we want the user to see the empty answers. 
			For this to work, I am adding a period (.) for any question of type 'comment' in the box. It is not pretty but it's the best I could find for now. 
				(Adding an blank spaces is not working because survey treats the blank spaces as empty e.g. question not answered). THAT DIDN'T WORK.
			For other types of non required questions (such as radiogroup, dropdown or checkbox) it a TODO

	Also, for all questions, the 'description' property is being hidden.
	Also, for 'html' questions, during the preview, the actual html gets wiped out. 

	I have asked the guy at Survey.js. Here is the link to the question: https://github.com/surveyjs/survey-library/issues/2268
	Hopefully he can implement something soon.


### Error handling

1) The error messages can be updated at the survey level like this...
	Survey.surveyLocalization.locales["en"].requiredError = "This field is required";
	... and also at the question level by adding the property "requiredErrorText": "This field is required at the question level"
	The question level overwrites the survey level

2)	The code to build the error box at the top of the page when thre is errors is in SurveyInit.js in function buildErrorMessage. 
	There is probably a better to handle the construction of the <section>, specially for multilanguage sites but for now it is working.

### Localization notes

a) Files needs to be saved as UTF-8 in order for the accents to be displayed properly

### TODO

a) The alert-label-error when a question is not answered is not displaying properly. It is just CSS.
b) The logic for showing the information section in Part C section 4 needs to be checked to reproduce exactly the same behaviour
c) Hide navigation bar in the 'Preview'
d) Total MB downloaded 
e) Try to make use of the start properties of survey (survey.firstPageIsStarted = true; OR survey.startSurveyText = "Start";)
f) Add max width on text fields. Need to find out the max width for first name, last name and all those.
h) Update the page title for every page
i) Replace <div id="div_errors_list" style="display:none"></div> in body
j) WCAG compliance, talk to Stephanie

### Fixed todos

1) French accents in json files are not rendered properly. The one found in Survey.js are ok. (DONE)
2) On alert-label-error, make a box on the top of the page showing all errors (DONE)
3) Validation not working for checkboxes with additional html. Plan B: always show the html additional information and use the 'comment' as is (DONE with plan A)
4) When selecting "Are you filing this complaint on your own behalf (or for a minor child you are guardian of) or on behalf of someone else?" -> Someone else,
then the section "Representative" info is missing
5) Match the property names with the original project 
6) Survey.StylesManager.Enabled = false
7) The style on the 'comment' has crapped after disabling the native style (Survey.StylesManager.Enabled)
8) The page & panel title <h> tag have been hard coded in Survey.vue.OPC.js. Need to find a way to not do that. 
	[Hint: look for SurveyTemplateText() but it only looks like it is avaialble for knockout]
9) When selecting "Are you filing this complaint on your own behalf (or for a minor child you are guardian of) or on behalf of someone else?" -> Someone else,
then the section "Authorization form attachment(s)" info is missing when uploading files
10) Return to the same question on page refresh or on language switching (DONE -> but need to implement logic to access/store data to the database)
11) Fr & en property of elements (Stephanie). This is started in we are now using only 1 file (survey_pa_complaint.json).
12) Some of the links/urls have not been set for french in <sections> when I add the "en" + "fr" parts
13) Part-C, at the question 'Did the institution agree to process your request on an informal basis?', the 'htmldescription' is missing. 
	I need to create another widget for radiobuttons, just like for the checkboxHtml. No need to custom widget.

### Postponed todo
A) Explore survey Creator (POSTPONED)
B) The navigation panel is the native of and should be replaced (POSTPONED)