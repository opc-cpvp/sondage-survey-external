# online-complaint-form-pa_jf

### Where to find Documentation

- https://surveyjs.io/Examples/Library is very complete in terms of examples
- https://surveyjs.io/Documentation/Library is where you can get property, event or method details
- The latest code of Survey.js lives here https://github.com/surveyjs/survey-library
- I didn't find much on StackOverflow but here, I have found a lot of questions/answers https://github.com/surveyjs/survey-library/issues?q=
- Here is the link to the question I've asked regarding hiding a page on preview: https://github.com/surveyjs/survey-library/issues/2268

### Special lib used

a) showdown.js found at https://cdnjs.cloudflare.com/ajax/libs/showdown/1.9.1/showdown.min.js. It is used ton convert markdown to html.
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

All other widgets are only for testing features. They are not being used.

### Notes on CSS classes

- When Survey.js builds the pages, it sets a lot of css classes on html elements. These classes are not used for styling in our case. In
other words, we are not using the style provided by Survey.js but rather the css for GoC. There is some custom css in ~\wwwroot\css\site.css

### TODO

a) The alert-label-error when a question is not answered is not displaying properly.
	a1) Also on this, make a box on the top of the page showing all errors (DONE)

b) The navigation panel is the native of and should be replaced
c) Validation not working for checkboxes with additional html. Plan B: always show the html additional information and use the 'comment' as is. 
d) Make a french PA.json and set local=fr (that created problem g)
e) The logic for showing the information section in Part C section 4 needs to be checked to reproduce exactly the same behaviour
f) Hide navigation bar in the 'Preview'
g) Return to the same question on page refresh or on language switching (DONE -> but need to implement logic to access/store data to the database)
h) Total MB downloaded
i) Try to make use of the start properties of survey (survey.firstPageIsStarted = true; OR survey.startSurveyText = "Start";)
j) Match the property names with the original project 
k) French accents in json files are not rendered properly. The one found in Survey.js are ok.

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