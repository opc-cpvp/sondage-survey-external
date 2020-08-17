var options = {
    fontSize: 12,
    margins: {
        left: 10,
        right: 10,
        top: 10,
        bot: 10
    },
    format: "a4",
    fontName: 'helvetica',
    fontStyle: 'normal',
    htmlRenderAs: "image",
  
    //pagebreak: { mode: 'avoid-all' }      // this option avoid breaking the survey on an element
    //compress: true
};

function saveSurveyPDF(json, surveyModel, lang, filename) {

    //  From: https://embed.plnkr.co/qoxpmWp2XOUFlRDsk6ta/

    var surveyPDF = new SurveyPDF.SurveyPDF(json, options);
    surveyPDF.locale = lang;
    surveyPDF.data = surveyModel.data;
    surveyPDF.showQuestionNumbers = "off";

    //  This is to avoid the pdf to be editable
    surveyPDF.mode = "display";

    //  Adding the markdown
    var converter = new showdown.Converter();
    converter.simpleLineBreaks = true;
    converter.tasklists = true;

    surveyPDF
        .onTextMarkdown
        .add(function (sender, options) {

            //convert the mardown text to html
            var str = converter.makeHtml(options.text);

            //remove root paragraphs <p></p>
            str = str.substring(3);
            str = str.substring(0, str.length - 4);

            //set html
            options.html = str;
        });

    surveyPDF
        .onRenderQuestion
        .add(function (survey, options) {

            if (options.question.getType() == "file") {

                var htmlQuestion = Survey.QuestionFactory.Instance.createQuestion("html", "html_question");

                if (options.question.value && options.question.value.length > 0) {

                    htmlQuestion.html = "<ol>";

                    (options.question.value).forEach(function (fileItem) {

                        htmlQuestion.html += "<li>";

                        var fileSizeInBytes = fileItem.content || 0;
                        var size = 0;

                        if (fileSizeInBytes < 1000) {
                            htmlQuestion.html += fileItem.name + " (" + fileSizeInBytes + " B)";
                        }
                        else {
                            size = Math.round(fileSizeInBytes / 1000, 0);
                            htmlQuestion.html += fileItem.name + " (" + size + " KB)";
                        }

                        htmlQuestion.html += "</li>";
                    });

                    htmlQuestion.html += "</ol>";                   
                }
                else {
                    if (lang == 'fr') {
                        htmlQuestion.html += "Aucune pièce jointe n’a encore été téléchargée.";
                    }
                    else {
                        htmlQuestion.html += "No attachments have been uploaded yet.";
                    }                   
                }

                var flatHtml = options
                    .repository
                    .create(survey, htmlQuestion, options.controller, "html");

                return new Promise(function (resolve) {
                    flatHtml
                        .generateFlats(options.point)
                        .then(function (htmlBricks) {
                            options.bricks = htmlBricks;
                            resolve();
                        });
                });
            }
        });

    surveyPDF.save(filename);
}
