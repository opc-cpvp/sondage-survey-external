import * as Survey from "survey-vue";
import * as SurveyCore from "survey-core"; //  SurveyPDF is using survey-core
import { storageName_PA } from "./SurveyLocalStorage";
import * as SurveyPDF from "survey-pdf";
import * as showdown from "showdown";

const options = {
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
    //htmlRenderAs: "image",
  
    //pagebreak: { mode: 'avoid-all' }      // this option avoid breaking the survey on an element
    //compress: true
};

export function exportToPDF(filename: string, jsonUrl: string, lang: string) {

    fetch(jsonUrl)
        .then(response => response.json())
        .then(json_pdf => {

            //  The idea is to convert each survey pages into survey panels
            let root = {
                pages : [] as any
            };

            //  TODO: somehow the titles (en + fr) must come from the parameter because we can re-use this method
            let singlePage = {
                name: "single_page",
                title: {
                    en: "Review and send Privacy complaint form (federal institution)",
                    fr: "FR-Review and send—Privacy complaint form (federal institution)"
                },
                elements: [] as any
            };

            for (var key in json_pdf) {
                if (key == 'pages') {
                    for (var i = 0; i < json_pdf[key].length; i++) {

                        let page = json_pdf[key][i];

                        if (!page.hideOnPDF) {

                            //  Create a panel for each page
                            let panel = {
                                name: page.name,
                                type: 'panel',
                                title: {
                                    en: page.title.en,
                                    fr: page.title.fr
                                },
                                elements: [] as any
                            };

                            let tempArray = [] as any;

                            for (var j = 0; j < page.elements.length; j++) {

                                var element = page.elements[j];

                                var elements = getPanelElements(element);

                                if (elements.length > 0) {
                                    tempArray.push(elements);
                                }
                            }

                            panel.elements = tempArray;

                            singlePage.elements.push(panel);
                        }
                    }
                }
            }

            root.pages.push(singlePage);

            let newJson = JSON.stringify(root);

            var survey_pdf = new Survey.Model(newJson);

            //  Getting the data from browser local storage
            var storageSt = window.localStorage.getItem(storageName_PA) || "";

            if (storageSt) {
                var res = JSON.parse(storageSt);

                if (res.data) {
                    survey_pdf.data = res.data;

                    saveSurveyPDF(newJson, survey_pdf, lang, filename);
                }
            }
        });
}

function saveSurveyPDF(json, surveyModel, lang, filename) {

    //  From: https://embed.plnkr.co/qoxpmWp2XOUFlRDsk6ta/

    var surveyPDF = new SurveyPDF.SurveyPDF(json, options);
    surveyPDF.locale = lang;
    surveyPDF.data = surveyModel.data;
    surveyPDF.showQuestionNumbers = "off";

    //  This is to avoid the pdf to be editable
    surveyPDF.mode = "display";

    //  Adding the markdown
    const converter = new showdown.Converter();
    converter.setOption('simpleLineBreaks', true);
    converter.setOption('tasklists', true);

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

                let htmlQuestion: SurveyCore.Question = SurveyCore.QuestionFactory.Instance.createQuestion("html", "html_question");

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
                            size = Math.round(fileSizeInBytes / 1000);
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

                //  TODO: jf
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

function getPanelElements(element) {

    var elements = [] as any;

    if (!element.hideOnPDF) {

        if (element.type == 'panel') {
            var panelElements = [] as any;

            for (var j = 0; j < element.elements.length; j++) {
                var tempElement = getPanelElements(element.elements[j]);
                if (tempElement.length > 0) {
                    panelElements.push(tempElement);
                }
            }

            element.elements = panelElements;
            elements.push(element);
        }
        else if (element.type == 'html') {
            //  do nothing
        }
        else {
            //  just add the element to the array
            elements.push(element);
        }
    }

    return elements;
}
