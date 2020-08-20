import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";

import * as SurveyInit from "./SurveyInit";
import * as Survey from "survey-vue";
import { TestSurvey } from "./testSurvey";
import { PaSurvey } from "./PaSurvey";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;
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

        globalThis.startSurvey = SurveyInit.startSurvey;
        globalThis.endSession = SurveyInit.endSession;
        globalThis.showPreview = SurveyInit.showPreview;

        const sampleSurvey = new TestSurvey();
        sampleSurvey.init();

        const paSurvey = new PaSurvey();
        paSurvey.init();
    }

    surveyPolyfill();
})();
