import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "core-js/es";
import "details-polyfill"; //  Polyfill to open/close the <details> tags
import "element-closest-polyfill"; //  Polyfill to use Element.closest
import { SurveyModel } from "survey-vue";
import "whatwg-fetch";

import { BreachPaSurvey } from "./breach_pa/breachPaSurvey";
import { PaSurvey } from "./pa/paSurvey";
import { PbrSurvey } from "./pbr/pbrSurvey";
import { PiaSurvey } from "./pia/piaSurvey";
import { PipedaSurvey } from "./pipeda/pipedaSurvey";
import * as SurveyNavigation from "./surveyNavigation";
import { surveyPdfExport } from "./surveyPDF";

import { ContactInfoSurvey } from "./contact_info_centre/contactInfoSurvey";
import { TellOPCSurvey } from "./other/tellOPCSurvey";
import { MultiLanguageProperty } from "./models/multiLanguageProperty";

declare global {
    function startSurvey(survey: SurveyModel): void;
    function prevPage(survey: SurveyModel): void;
    function nextPage(survey: SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: SurveyModel): void;

    function initBreachPaSurvey(lang: "en" | "fr", token: string): void;
    function initPaSurvey(lang: "en" | "fr", token: string): void;
    function initPiaETool(lang: "en" | "fr", token: string): void;
    function initPbr(lang: "en" | "fr", token: string): void;
    function initPipeda(lang: "en" | "fr", token: string): void;
    function initContactInfo(lang: "en" | "fr", token: string): void;
    function initTellOPC(lang: "en" | "fr", token: string): void;

    function exportToPDF(): void;
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

        const storageName_BREACH_PA = "SurveyJS_LoadState_BREACH_PA";
        const storageName_PA = "SurveyJS_LoadState_PA";
        const storageName_PBR = "SurveyJS_LoadState_PBR";
        const storageName_PIA = "SurveyJS_LoadState_PIA";
        const storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";
        const storageName_CONTACTINFO = "SurveyJS_LoadState_CONTACTINFO";
        const storageName_TELLOPC = "SurveyJS_LoadState_TELLOPC";

        globalThis.initContactInfo = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_contact.json";

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

            const contactInfoSurvey = new ContactInfoSurvey(lang, token, storageName_CONTACTINFO);
            await contactInfoSurvey.loadSurveyFromUrl(jsonUrl);
            contactInfoSurvey.renderSurvey();
        };

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

        globalThis.initBreachPaSurvey = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_breach_pa.json";

            const breachPaSurvey = new BreachPaSurvey(lang, token, storageName_BREACH_PA);
            await breachPaSurvey.loadSurveyFromUrl(jsonUrl);
            breachPaSurvey.renderSurvey();
        };

        globalThis.initPaSurvey = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            // await import("./pa/pa_test_data").then(testData => {
            //   const storage = new LocalStorage();

            //   const storageData = {
            //       currentPageNo: 0,
            //       data: testData.paTestData2
            //   } as SurveyState;

            //   storage.save(storageName_PA, storageData);
            // });

            const paSurvey = new PaSurvey(lang, token, storageName_PA);
            await paSurvey.loadSurveyFromUrl(jsonUrl);
            paSurvey.renderSurvey();

            globalThis.exportToPDF = function () {
                const filename = "survey_export_pa";
                const pdfClass = new surveyPdfExport();
                const page_title: MultiLanguageProperty = {
                    en: "PA Review and send Privacy complaint form (federal institution)",
                    fr: "FR-Review and send—Privacy complaint form (federal institution)",
                    default: ""
                };

                pdfClass.exportToPDF(filename, jsonUrl, lang, paSurvey.getSurveyModel(), page_title);
            };
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
            piaSurvey.renderSurveySideMenu();

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
            //    const storage = new LocalStorage();

            //    const storageData = {
            //        currentPageNo: 0,
            //        data: testData.testData_pipeda
            //    } as SurveyState;

            //    storage.save(storageName_PIPEDA, storageData);
            // });

            const pipedaSurvey = new PipedaSurvey(lang, token, storageName_PIPEDA);
            await pipedaSurvey.loadSurveyFromUrl(jsonUrl);
            pipedaSurvey.renderSurvey();

            globalThis.exportToPDF = function () {
                const filename = "survey_export_pipeda";
                const pdfClass = new surveyPdfExport();

                const page_title: MultiLanguageProperty = {
                    en: "PIPDEA Review and send Privacy complaint form (federal institution)",
                    fr: "FR-Review and send—Privacy complaint form (federal institution)",
                    default: ""
                };

                pdfClass.exportToPDF(filename, jsonUrl, lang, pipedaSurvey.getSurveyModel(), page_title);
            };
        };

        globalThis.initTellOPC = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_tell_opc.json";
            const tellOPCSurvey = new TellOPCSurvey(lang, token, storageName_TELLOPC);
            await tellOPCSurvey.loadSurveyFromUrl(jsonUrl);
            tellOPCSurvey.renderSurvey();
        };
    }

    surveyPolyfill();
    main();
})();
