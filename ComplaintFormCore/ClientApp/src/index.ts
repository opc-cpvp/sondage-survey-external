import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "core-js/es";
import "details-polyfill"; //  Polyfill to open/close the <details> tags
import "element-closest-polyfill"; //  Polyfill to use Element.closest
import { SurveyModel } from "survey-vue";
import "whatwg-fetch";
import { PaSurvey } from "./pa/paSurvey";
import { PbrSurvey } from "./pbr/pbrSurvey";
import { PiaSurvey } from "./pia/piaSurvey";
import { PidSurvey } from "./pid/pidSurvey";
import { PipedaSurvey } from "./pipeda/pipedaSurvey";
import * as SurveyNavigation from "./surveyNavigation";
import { surveyPdfExport } from "./surveyPDF";
import { TestSurvey } from "./tests/testSurvey";

import { LocalStorage } from "./localStorage";
import { SurveyState } from "./models/surveyState";
import { TellOPCSurvey } from "./other/tellOPCSurvey";
import { MultiLanguageProperty } from "./models/multiLanguageProperty";

declare global {
    function startSurvey(survey: SurveyModel): void;
    function prevPage(survey: SurveyModel): void;
    function nextPage(survey: SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: SurveyModel): void;

    function initPaSurvey(lang: "en" | "fr", token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function initPiaETool(lang: "en" | "fr", token: string): void;
    function initPbr(lang: "en" | "fr", token: string): void;
    function initPipeda(lang: "en" | "fr", token: string): void;
    function initPidSurvey(lang: "en" | "fr", token: string): void;
    function initTellOPC(lang: "en" | "fr", token: string): void;

    function exportToPDF(lang: string, complaintType: string): void;
    function checkBoxInfoPopupEvent(checkbox): void;

    function gotoSection(section: string): void;
    function gotoPage(pageName: string): void;
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
        const storageName_PBR = "SurveyJS_LoadState_PBR";
        const storageName_PIA = "SurveyJS_LoadState_PIA";
        const storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";
        const storageName_PID = "SurveyJS_LoadState_PID";
        const storageName_TELLOPC = "SurveyJS_LoadState_TELLOPC";

        globalThis.initPbr = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pbr.json";

            // await import("./pbr/pbr_test_data")
            //    .then(testData => testData.pbr_test_data)
            //    .then(testData => {
            //        const storage = new LocalStorage();

            //        const storageData = {
            //            currentPageNo: 0,
            //            data: testData
            //        } as SurveyState;

            //        storage.save(storageName_PBR, storageData);
            //    });

            const pbrSurvey = new PbrSurvey(lang, token, storageName_PBR);
            await pbrSurvey.loadSurveyFromUrl(jsonUrl);
            pbrSurvey.renderSurvey();
        };

        globalThis.initPaSurvey = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            // await import("./pa/pa_test_data").then(testData => {
            //     const storage = new LocalStorage();

            //     const storageData = {
            //         currentPageNo: 0,
            //         data: testData.paTestData2
            //     } as SurveyState;

            //     storage.save(storageName_PA, storageData);
            // });

            const paSurvey = new PaSurvey(lang, token, storageName_PA);
            await paSurvey.loadSurveyFromUrl(jsonUrl);
            paSurvey.renderSurvey();
        };

        globalThis.initPidSurvey = async (lang: "fr" | "en", token) => {
            const jsonUrl = "/sample-data/survey_pid.json";
            const pidSurvey = new PidSurvey(lang, token, storageName_PID);
            await pidSurvey.loadSurveyFromUrl(jsonUrl);
            pidSurvey.renderSurvey();
        };

        globalThis.initPiaETool = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pia_e_tool.json";

            // await import("./pia/pia_test_data").then(testData => {
            //    const storage = new LocalStorage();

            //    const storageData = {
            //        currentPageNo: 0,
            //        data: testData.piaTestData
            //    } as SurveyState;

            //    storage.save(storageName_PIA, storageData);
            // });

            const piaSurvey = new PiaSurvey(lang, token, storageName_PIA);
            await piaSurvey.loadSurveyFromUrl(jsonUrl);
            piaSurvey.renderSurvey();

            globalThis.gotoSection = section => {
                piaSurvey.gotoSection(section);
            };

            globalThis.gotoPage = pageName => {
                piaSurvey.gotoPage(pageName);
            };
        };

        globalThis.initPipeda = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pipeda_complaint.json";

            // await import("./pipeda/pipeda_test_data").then(testData => {
            //     const storage = new LocalStorage();

            //     const storageData = {
            //         currentPageNo: 0,
            //         data: testData.testData_pipeda
            //     } as SurveyState;

            //     storage.save(storageName_PIPEDA, storageData);
            // });

            const pipedaSurvey = new PipedaSurvey(lang, token, storageName_PIPEDA);
            await pipedaSurvey.loadSurveyFromUrl(jsonUrl);
            pipedaSurvey.renderSurvey();
        };

        globalThis.initTellOPC = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_tell_opc.json";
            const tellOPCSurvey = new TellOPCSurvey(lang, token, storageName_TELLOPC);
            await tellOPCSurvey.loadSurveyFromUrl(jsonUrl);
            tellOPCSurvey.renderSurvey();
        };

        globalThis.exportToPDF = (lang, complaintType) => {
            let jsonUrl = "";
            let filename = "";
            const pdfClass = new surveyPdfExport();

            if (complaintType === "pipeda") {
                jsonUrl = "/sample-data/survey_pipeda_complaint.json";
                filename = "survey_export_pipeda";

                const page_title: MultiLanguageProperty = {
                    en: "PIPDEA Review and send Privacy complaint form (federal institution)",
                    fr: "FR-Review and send—Privacy complaint form (federal institution)",
                    default: ""
                };

                pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PIPEDA, page_title);
            } else if (complaintType === "pa") {
                jsonUrl = "/sample-data/survey_pa_complaint.json";
                filename = "survey_export_pa";

                const page_title: MultiLanguageProperty = {
                    en: "PA Review and send Privacy complaint form (federal institution)",
                    fr: "FR-Review and send—Privacy complaint form (federal institution)",
                    default: ""
                };

                pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PA, page_title);
            } else if (complaintType === "pid") {
                jsonUrl = "/sample-data/survey_pid.json";
                filename = "survey_export_pid";

                const page_title: MultiLanguageProperty = {
                    en: "PID PDF title",
                    fr: "FR-PID PDF title",
                    default: ""
                };

                pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PID, page_title);
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
