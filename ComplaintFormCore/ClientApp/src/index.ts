/// <reference path="../@types/survey-vue/survey.vue.d.ts" />

import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "details-polyfill";  //  Polyfill to open/close the <details> tags
import "element-closest-polyfill";  //  Polyfill to use Element.closest

import * as Survey from "survey-vue";
import { TestSurvey } from "./tests/testSurvey";
import { PaSurvey } from "./pa/PaSurvey";
import { CheckboxWidget } from "./widgets/checkboxwidget";
import { WidgetCommentHtml } from "./widgets/widgetCommentHtml";
import { surveyPdfExport } from "./surveyPDF";
import * as SurveyNavigation from "./surveyNavigation";
import { PiaETool } from "./pia/piaE-ToolSurvey";
import { WidgetRadioButtonHtml } from "./widgets/widgetRadioButtonHtml";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: string, token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function initPiaETool(lang: string, token: string): void;
    function exportToPDF(lang: string): void;
    function checkBoxInfoPopupEvent(checkbox): void;
}

declare let Symbol;

(() => {
    function IsIE() {
        if (/MSIE \d|Trident.*rv:/.test(navigator.userAgent)) {
            return true;
        }

        return false;
    }

    function surveyPolyfill() {
        // IE11 has somme issue about setter with deep recursion
        // with Symbol polyfill. We need to use the useSimple
        // option from core-js
        if (IsIE()) {
            Symbol.useSimple();
        }
    }

    function main() {
        globalThis.startSurvey = SurveyNavigation.startSurvey;
        globalThis.endSession = SurveyNavigation.endSession;
        globalThis.showPreview = SurveyNavigation.showPreview;
        globalThis.completeSurvey = SurveyNavigation.completeSurvey;

        globalThis.initPaSurvey = (lang, token) => {

            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            CheckboxWidget.init();
            WidgetCommentHtml.init();

            const paSurvey = new PaSurvey();
            paSurvey.init(jsonUrl, lang, token);
        };

        globalThis.initPiaETool = (lang, token) => {

            const jsonUrl = "/sample-data/survey_pia_e_tool.json";

            WidgetCommentHtml.init();
            WidgetRadioButtonHtml.init();

            const piaETool = new PiaETool();
            piaETool.init(jsonUrl, lang, token);
        };

        globalThis.exportToPDF = lang => {
            //  TODO: somehow the json url must come from the parameter because we can re-use this method
            const jsonUrl = "/sample-data/survey_pa_complaint.json";
            const filename = "survey_export";
            const pdfClass = new surveyPdfExport();
            pdfClass.exportToPDF(filename, jsonUrl, lang);
        };

        globalThis.initTestSurvey = (lang, token) => {
            const sampleSurvey = new TestSurvey();
            const jsonUrl = "/sample-data/survey_test_2.json";

            sampleSurvey.init(jsonUrl, lang, token);
        };
    }

    surveyPolyfill();
    main();
})();
