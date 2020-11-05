/// <reference path="../@types/survey-vue/survey.vue.d.ts" />

import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "details-polyfill"; //  Polyfill to open/close the <details> tags
import "element-closest-polyfill"; //  Polyfill to use Element.closest

import * as Survey from "survey-vue";
import { TestSurvey } from "./tests/testSurvey";
import { PaSurvey } from "./pa/PaSurvey";
import { CheckboxWidget } from "./widgets/checkboxwidget";
import { surveyPdfExport } from "./surveyPDF";
import * as SurveyNavigation from "./surveyNavigation";
import { PiaETool } from "./pia/piaE-ToolSurvey";
import { PipedaTool } from "./pipeda/pipedaSurvey";
import { PbrSurvey } from "./pbr/pbrSurvey";
import { storageName_PA, storageName_PIPEDA } from "./surveyLocalStorage";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function prevPage(survey: Survey.SurveyModel): void;
    function nextPage(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: string, token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function initPiaETool(lang: string, token: string): void;
    function initPipeda(lang: string, token: string): void;
    function initPbr(lang: string, token: string): void;

    function exportToPDF(lang: string): void;
    function checkBoxInfoPopupEvent(checkbox): void;

    function gotoSection(survey: Survey.SurveyModel, section: number): void;
    function gotoPage(survey: Survey.SurveyModel, pageName: string): void;
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
        globalThis.prevPage = SurveyNavigation.prevPage;
        globalThis.nextPage = SurveyNavigation.nextPage;
        globalThis.endSession = SurveyNavigation.endSession;
        globalThis.showPreview = SurveyNavigation.showPreview;
        globalThis.completeSurvey = SurveyNavigation.completeSurvey;

        globalThis.initPbr = (lang, token) => {
            const jsonUrl = "/sample-data/survey_pbr.json";
            const pbrSurvey = new PbrSurvey();
            pbrSurvey.init(jsonUrl, lang, token);
        };

        globalThis.initPaSurvey = (lang, token) => {
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            CheckboxWidget.init();

            const paSurvey = new PaSurvey();
            paSurvey.init(jsonUrl, lang, token);
        };

        globalThis.initPiaETool = (lang, token) => {
            const jsonUrl = "/sample-data/survey_pia_e_tool.json";
            const piaETool = new PiaETool();
            piaETool.init(jsonUrl, lang, token);

            globalThis.gotoSection = (survey, section) => {
                piaETool.gotoSection(survey, section);
            };

            globalThis.gotoPage = (survey, pageName) => {
                piaETool.gotoPage(survey, pageName);
            };

            globalThis.nextPage = survey => {
                piaETool.nextPage(survey);
            };
        };

        globalThis.initPipeda = (lang, token) => {
            const jsonUrl = "/sample-data/survey_pipeda_complaint.json";
            const pipedaTool = new PipedaTool();
            pipedaTool.init(jsonUrl, lang, token);
        };

        globalThis.exportToPDF = (lang, complaintType) => {
            let jsonUrl = "";
            let filename = "";
            const pdfClass = new surveyPdfExport();

            if (complaintType === "pipeda") {
                jsonUrl = "/sample-data/survey_pipeda_complaint.json";
                filename = "survey_export_pipeda";
                pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PIPEDA);
            } else if (complaintType === "pa") {
                jsonUrl = "/sample-data/survey_pa_complaint.json";
                filename = "survey_export_pa";
                pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PA);
            }
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
