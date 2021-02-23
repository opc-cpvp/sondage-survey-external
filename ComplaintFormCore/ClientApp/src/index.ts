import "core-js/es";
import "whatwg-fetch";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";
import "details-polyfill"; //  Polyfill to open/close the <details> tags
import "element-closest-polyfill"; //  Polyfill to use Element.closest

import * as Survey from "survey-vue";
import { NewPaSurvey } from "./pa/newPaSurvey";
import { NewPbrSurvey } from "./pbr/newPbrSurvey";
import { NewPiaToolSurvey } from "./pia/newPiaToolSurvey";
import { NewPipedaSurvey } from "./pipeda/newPipedaSurvey";
import { TestSurvey } from "./tests/testSurvey";
import * as SurveyNavigation from "./surveyNavigation";
import { surveyPdfExport } from "./surveyPDF";
import { LocalStorage } from "./localStorage";
import { SurveyState } from "./models/surveyState";
import { MultiLanguageProperty } from "./models/multiLanguageProperty";

declare global {
    function startSurvey(survey: Survey.SurveyModel): void;
    function prevPage(survey: Survey.SurveyModel): void;
    function nextPage(survey: Survey.SurveyModel): void;
    function endSession(): void;
    function showPreview(survey: Survey.SurveyModel): void;
    function completeSurvey(button: HTMLButtonElement, survey: Survey.SurveyModel): void;

    function initPaSurvey(lang: "en" | "fr", token: string): void;
    function initTestSurvey(lang: string, token: string): void;
    function initPiaETool(lang: "en" | "fr", token: string): void;
    function initPbr(lang: "en" | "fr", token: string): void;
    function initPipeda(lang: "en" | "fr", token: string): void;

    function exportToPDF(lang: string, complaintType: string, token: string): void;
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

            const pbrSurvey = new NewPbrSurvey(lang, token, storageName_PBR);
            await pbrSurvey.loadSurveyFromUrl(jsonUrl);
            pbrSurvey.renderSurvey();
        };

        globalThis.initPaSurvey = async (lang: "en" | "fr", token) => {
            const jsonUrl = "/sample-data/survey_pa_complaint.json";

            await import("./pa/pa_test_data").then(testData => {
                const storage = new LocalStorage();

                const storageData = {
                    currentPageNo: 0,
                    data: testData.paTestData2
                } as SurveyState;

                storage.save(storageName_PA, storageData);
            });

            const paSurvey = new NewPaSurvey(lang, token, storageName_PA);
            await paSurvey.loadSurveyFromUrl(jsonUrl);
            paSurvey.renderSurvey();
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

            const piaSurvey = new NewPiaToolSurvey(lang, token, storageName_PIA);
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

            const pipedaSurvey = new NewPipedaSurvey(lang, token, storageName_PIPEDA);
            await pipedaSurvey.loadSurveyFromUrl(jsonUrl);
            pipedaSurvey.renderSurvey();
        };

        globalThis.exportToPDF = (lang, complaintType, token) => {
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

                const surveyData: string = getCookie(token);
                pdfClass.exportToPDFAlt(filename, jsonUrl, lang, surveyData, page_title);
            }
        };

        globalThis.initTestSurvey = (lang, token) => {
            const sampleSurvey = new TestSurvey();
            const jsonUrl = "/sample-data/survey_test_2.json";

            sampleSurvey.init(jsonUrl, lang, token);
        };
    }

    function getCookie(name: string) {
        const nameEQ = `${name}=`;
        const ca = document.cookie.split(";");
        for (let c of ca) {
            while (c.charAt(0) === " ") {
                c = c.substring(1, c.length);
            }
            if (c.indexOf(nameEQ) === 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
        return "";
    }

    surveyPolyfill();
    main();
})();
