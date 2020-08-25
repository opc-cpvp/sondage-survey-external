import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";

import * as SurveyInit from "./surveyInit";
import * as Survey from "survey-vue";
import { TestSurvey } from "./tests/testSurvey";
import { PaSurvey } from "./pa/PaSurvey";
import * as SurveyPDF from "./surveyPDF";
import { WidgetCheckboxHtml } from "./widgets/widgetCheckboxHtml";
import { WidgetCommentHtml } from "./widgets/widgetCommentHtml";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: string, token: string): void;
    function initTestSurvey(lang: string, token: string): void;
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
        globalThis.startSurvey = SurveyInit.startSurvey;
        globalThis.endSession = SurveyInit.endSession;
        globalThis.showPreview = SurveyInit.showPreview;

        globalThis.initPaSurvey = (lang, token) => {
            const paSurvey = new PaSurvey();
            const widgetCheckboxHtml = new WidgetCheckboxHtml();
            const widgetCommentHtml = new WidgetCommentHtml();
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            paSurvey.init(jsonUrl, lang, token);
            widgetCheckboxHtml.init();
            widgetCommentHtml.init();
        };

        globalThis.exportToPDF = lang => {
            //  TODO: somehow the json url must come from the parameter because we can re-use this method
            const jsonUrl = "/sample-data/survey_pa_complaint.json";
            const filename = "survey_export";

            SurveyPDF.exportToPDF(filename, jsonUrl, lang);
        };

        globalThis.checkBoxInfoPopupEvent = checkbox => {
            SurveyInit.checkBoxInfoPopup(checkbox);
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
