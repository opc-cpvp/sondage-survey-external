import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";

import * as SurveyInit from "./SurveyInit";
import * as Survey from "survey-vue";
import { TestSurvey } from "./testSurvey";
import { PaSurvey } from "./PaSurvey";
import * as SurveyPDF from "./surveyPDF";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: string, token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function exportToPDF(lang: string): void;
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
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            paSurvey.init(jsonUrl, lang, token);
        };

        globalThis.exportToPDF = (lang) => {

            //  TODO: somehow the json url must come from the parameter because we can re-use this method
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            SurveyPDF.exportToPDF("survey_export", jsonUrl, lang);
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
