import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "details-polyfill"; //  Polyfill to open/close the <details> tags
import "element-closest-polyfill"; //  Polyfill to use Element.closest

import * as Survey from "survey-vue";
import { TestSurvey } from "./tests/testSurvey";
import { NewPaSurvey } from "./pa/newPaSurvey";
import { surveyPdfExport } from "./surveyPDF";
import * as SurveyNavigation from "./surveyNavigation";
import { PiaETool } from "./pia/piaE-ToolSurvey";
import { PbrSurvey } from "./pbr/pbrSurvey";
import { LocalStorage } from "./localStorage";
import { SurveyState } from "./models/surveyState";
import { NewPipedaSurvey } from "./pipeda/newPipedaSurvey";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function prevPage(survey: Survey.SurveyModel): void;
    function nextPage(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: "en" | "fr", token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function initPiaETool(lang: string, token: string): void;
    function initPipeda(lang: "en" | "fr", token: string): void;
    function initPbr(lang: string, token: string): void;

    function exportToPDF(lang: string, complaintType: string): void;
    function checkBoxInfoPopupEvent(checkbox): void;

    function gotoSection(survey: Survey.SurveyModel, section: string): void;
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

        const storageName_PA = "SurveyJS_LoadState_PA";
        const storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";
        // const storageName_PBR = "SurveyJS_LoadState_PBR";

        globalThis.initPbr = (lang, token) => {
            const jsonUrl = "/sample-data/survey_pbr.json";
            const pbrSurvey = new PbrSurvey();
            pbrSurvey.init(jsonUrl, lang, token);
        };

        globalThis.initPaSurvey = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            /*
            await import("./pa/pa_test_data").then(testData => {
                const storage = new LocalStorage();

                const storageData = {
                    currentPageNo: 0,
                    data: testData.paTestData2
                } as SurveyState;

                storage.save(storageName_PA, storageData);
            });
            */

            const paSurvey = new NewPaSurvey(lang, token, storageName_PA);
            await paSurvey.loadSurveyFromUrl(jsonUrl);
            paSurvey.renderSurvey();
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

        globalThis.initPipeda = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pipeda_complaint.json";

            await import("./pipeda/pipeda_test_data").then(testData => {
                const storage = new LocalStorage();

                const storageData = {
                    currentPageNo: 0,
                    data: testData.testData_pipeda
                } as SurveyState;

                storage.save(storageName_PIPEDA, storageData);
            });

            const pipedaSurvey = new NewPipedaSurvey(lang, token, storageName_PIPEDA);
            await pipedaSurvey.loadSurveyFromUrl(jsonUrl);
            pipedaSurvey.renderSurvey();
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
