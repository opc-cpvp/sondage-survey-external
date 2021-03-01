import { Converter } from "showdown";
import { QuestionFactory } from "survey-core"; //  SurveyPDF is using survey-core
import { AdornersOptions, SurveyPDF } from "survey-pdf";
import {
    IElement,
    Model,
    PageModel,
    PanelModelBase,
    Question,
    QuestionHtmlModel,
    QuestionMatrixDynamicModel,
    QuestionPanelDynamicModel,
    QuestionSelectBase
} from "survey-vue";
import { MultiLanguageProperty } from "./models/multiLanguageProperty";

export class surveyPdfExport {
    private pdfOptions = {
        fontSize: 12,
        margins: {
            left: 10,
            right: 10,
            top: 10,
            bot: 10
        },
        format: "a4",
        fontName: "helvetica",
        fontStyle: "normal"
        // htmlRenderAs: "image",
        // pagebreak: { mode: 'avoid-all' }      // this option avoid breaking the survey on an element
        // compress: true
    };

    public exportToPDF(filename: string, jsonUrl: string, lang: string, storageName: string, pdf_page_title: MultiLanguageProperty): void {
        void fetch(jsonUrl)
            .then(response => response.json())
            .then(json_pdf => {
                //  Modify the json to strip out what we don't want
                const modifiedJson = this.modifySurveyJsonforPDF(json_pdf, lang, pdf_page_title);

                //  Getting the data from browser local storage
                const storageSt = window.localStorage.getItem(storageName) || "";

                if (storageSt) {
                    const res = JSON.parse(storageSt);

                    if (res.data) {
                        //  Then construct a new survey pdf object with the modified json
                        const survey_pdf = this.initSurveyPDF(modifiedJson, res.data, lang);

                        void survey_pdf.save(filename);
                    }
                }
            });
    }

    private modifySurveyJsonforPDF(json_pdf: any, lang: string, pdf_page_title: MultiLanguageProperty): string {
        const originalSurvey = new Model(json_pdf);
        originalSurvey.locale = lang;

        //  The idea is to convert each survey pages into survey panels
        const root = {
            pages: [] as any
        };

        //  TODO: somehow the titles (en + fr) must come from the parameter because we can re-use this method
        const singlePage = {
            name: "single_page",
            title: {
                en: pdf_page_title.en,
                fr: pdf_page_title.fr
            },
            elements: [] as any
        };

        originalSurvey.pages.forEach((page: PageModel) => {
            const hideOnPDF = page.getPropertyValue("hideOnPDF");

            if (!hideOnPDF && page.visible) {
                //  Create a panel for each page
                const panel = {
                    name: page.name,
                    type: "panel",
                    title: page.title,
                    elements: [] as any
                };

                page.elements.forEach((element: IElement) => {
                    this.setElements(panel, element);
                });

                singlePage.elements.push(panel);
            }
        });

        root.pages.push(singlePage);

        return JSON.stringify(root);
    }

    private setElements(panel: any, element: IElement): any {
        if (element instanceof PanelModelBase) {
            const panelBase = element;

            //  'hideOnPDF' is a custom property that the can be set in the json
            let hideOnPDF = panelBase.getPropertyValue("hideOnPDF");
            if (!hideOnPDF) {
                hideOnPDF = false;

                if (hideOnPDF === false) {
                    const innerPanel = {
                        name: panelBase.name,
                        type: "panel",
                        title: panelBase.title,
                        visibleIf: panelBase.visibleIf,
                        elements: [] as any
                    };

                    panelBase.elements.forEach((panelElement: IElement) => {
                        this.setElements(innerPanel, panelElement);
                    });

                    panel.elements.push(innerPanel);
                }
            }
        } else if (element.getType() === "paneldynamic") {
            const panelDynamicBase = element as QuestionPanelDynamicModel;

            const innerPanel = {
                name: panelDynamicBase.name,
                valueName: panelDynamicBase.valueName,
                type: "paneldynamic",
                title: panelDynamicBase.title,
                visibleIf: panelDynamicBase.visibleIf,
                templateElements: [] as any
            };

            panelDynamicBase.templateElements.forEach((panelElement: IElement) => {
                this.setElements(innerPanel, panelElement);
            });

            panel.elements.push(innerPanel);
        } else if (element.getType() === "matrixdynamic") {
            const matrixDynamicBase = element as QuestionMatrixDynamicModel;

            const innerPanel = {
                name: matrixDynamicBase.name,
                valueName: matrixDynamicBase.valueName,
                type: "matrixdynamic",
                title: matrixDynamicBase.title,
                visibleIf: matrixDynamicBase.visibleIf,
                columns: matrixDynamicBase.columns,
                choices: matrixDynamicBase.choices
            };

            panel.elements.push(innerPanel);
        } else {
            const question = element as Question;

            if (question instanceof QuestionHtmlModel) {
                //  Do nothing
            } else if (question instanceof QuestionSelectBase) {
                const newElement = {
                    name: question.name,
                    type: question.getType(),
                    title: question.title,
                    choices: question.choices,
                    choicesByUrl: question.choicesByUrl,
                    visibleIf: question.visibleIf,
                    hasOther: question.hasOther
                };

                if (panel.templateElements) {
                    panel.templateElements.push(newElement);
                } else {
                    panel.elements.push(newElement);
                }
            } else {
                const newElement = {
                    name: question.name,
                    type: question.getType(),
                    title: question.title,
                    visibleIf: question.visibleIf,
                    id: question.id
                };

                if (panel.templateElements) {
                    panel.templateElements.push(newElement);
                } else {
                    panel.elements.push(newElement);
                }
            }
        }

        return;
    }

    private buildFilePreview(survey: SurveyPDF, options: AdornersOptions, lang: string) {
        const htmlQuestion = QuestionFactory.Instance.createQuestion("html", "html_question");

        if (options.question.value && options.question.value.length > 0) {
            htmlQuestion.html = "<ol>";

            options.question.value.forEach((fileItem: Question) => {
                htmlQuestion.html += "<li>";

                const fileSizeInBytes = (fileItem.size as number) || 0;

                if (fileSizeInBytes < 1000) {
                    htmlQuestion.html += `${fileItem.name} (${fileSizeInBytes} B)`;
                } else if (fileSizeInBytes < 1000000) {
                    const size = Math.round(fileSizeInBytes / 1000);
                    htmlQuestion.html += `${fileItem.name} (${size} KB)`;
                } else {
                    const size = Math.round(fileSizeInBytes / 1000000);
                    htmlQuestion.html += `${fileItem.name} (${size} MB)`;
                }

                htmlQuestion.html += "</li>";
            });

            htmlQuestion.html += "</ol>";
        } else {
            if (lang === "fr") {
                htmlQuestion.html = "Aucune pièce jointe n’a encore été téléchargée.";
            } else {
                htmlQuestion.html = "No attachments have been uploaded yet.";
            }
        }

        //  TODO: jf
        const flatHtml = options.repository.create(survey, htmlQuestion, options.controller, "html");

        return new Promise<void>(resolve => {
            flatHtml
                .generateFlats(options.point)
                .then(htmlBricks => {
                    options.bricks = htmlBricks;
                    resolve();
                })
                .catch(error => {
                    console.warn(error);
                });
        });
    }

    private initSurveyPDF(json: string, data: any, lang: string): SurveyPDF {
        //  From: https://embed.plnkr.co/qoxpmWp2XOUFlRDsk6ta/

        const surveyPDF = new SurveyPDF(json, this.pdfOptions);
        surveyPDF.locale = lang;
        surveyPDF.data = data;
        surveyPDF.showQuestionNumbers = "off";

        //  This is to avoid the pdf to be editable
        surveyPDF.mode = "display";

        //  Adding the markdown
        const converter = new Converter();
        converter.setOption("simpleLineBreaks", true);
        converter.setOption("tasklists", true);

        surveyPDF.onTextMarkdown.add((sender, options) => {
            //  convert the mardown text to html
            let str = converter.makeHtml(options.text);

            //  remove root paragraphs <p></p>
            str = str.substring(3);
            str = str.substring(0, str.length - 4);

            //  set html
            options.html = str;
        });

        surveyPDF.onRenderPanel.add((survey, options) => {
            console.log(options.panel.name + ": " + options.panel.visibleIf);
            if (options.panel.isVisible === false) {
                options.panel.delete();
            }
        });

        surveyPDF.onRenderQuestion.add((survey, options) => {
            if (options.question.isVisible === false) {
                options.question.clearValue();
            }

            if (options.question.getType() === "file") {
                return this.buildFilePreview(survey, options, lang);
            }
        });

        return surveyPDF;
    }
}
