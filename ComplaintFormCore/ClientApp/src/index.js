"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
require("abortcontroller-polyfill/dist/polyfill-patch-fetch");
require("core-js/es");
require("details-polyfill"); //  Polyfill to open/close the <details> tags
require("element-closest-polyfill"); //  Polyfill to use Element.closest
require("whatwg-fetch");
var paSurvey_1 = require("./pa/paSurvey");
var pbrSurvey_1 = require("./pbr/pbrSurvey");
var piaSurvey_1 = require("./pia/piaSurvey");
var pidSurvey_1 = require("./pid/pidSurvey");
var pipedaSurvey_1 = require("./pipeda/pipedaSurvey");
var SurveyNavigation = require("./surveyNavigation");
var surveyPDF_1 = require("./surveyPDF");
var testSurvey_1 = require("./tests/testSurvey");
var tellOPCSurvey_1 = require("./other/tellOPCSurvey");
(function () {
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
        var _this = this;
        globalThis.startSurvey = SurveyNavigation.startSurvey;
        globalThis.prevPage = SurveyNavigation.prevPage;
        globalThis.nextPage = SurveyNavigation.nextPage;
        globalThis.endSession = SurveyNavigation.endSession;
        globalThis.showPreview = SurveyNavigation.showPreview;
        globalThis.completeSurvey = SurveyNavigation.completeSurvey;
        var storageName_PA = "SurveyJS_LoadState_PA";
        var storageName_PBR = "SurveyJS_LoadState_PBR";
        var storageName_PIA = "SurveyJS_LoadState_PIA";
        var storageName_PIPEDA = "SurveyJS_LoadState_PIPEDA";
        var storageName_PID = "SurveyJS_LoadState_PID";
        var storageName_TELLOPC = "SurveyJS_LoadState_TELLOPC";
        globalThis.initPbr = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, pbrSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_pbr.json";
                        pbrSurvey = new pbrSurvey_1.PbrSurvey(lang, token, storageName_PBR);
                        return [4 /*yield*/, pbrSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        pbrSurvey.renderSurvey();
                        return [2 /*return*/];
                }
            });
        }); };
        globalThis.initPaSurvey = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, paSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_pa_complaint.json";
                        paSurvey = new paSurvey_1.PaSurvey(lang, token, storageName_PA);
                        return [4 /*yield*/, paSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        paSurvey.renderSurvey();
                        globalThis.exportToPDF = function () {
                            var filename = "survey_export_pa";
                            var pdfClass = new surveyPDF_1.surveyPdfExport();
                            var page_title = {
                                en: "PA Review and send Privacy complaint form (federal institution)",
                                fr: "FR-Review and send—Privacy complaint form (federal institution)",
                                default: ""
                            };
                            var surveyData = JSON.stringify(paSurvey.surveyData);
                            pdfClass.exportToPDFAlt(filename, jsonUrl, lang, surveyData, page_title);
                        };
                        return [2 /*return*/];
                }
            });
        }); };
        globalThis.initPidSurvey = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, pidSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_pid.json";
                        pidSurvey = new pidSurvey_1.PidSurvey(lang, token, storageName_PID);
                        return [4 /*yield*/, pidSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        pidSurvey.renderSurvey();
                        globalThis.exportToPDF = function () {
                            var filename = "survey_export_pid";
                            var pdfClass = new surveyPDF_1.surveyPdfExport();
                            var page_title = {
                                en: "PID export",
                                fr: "PID export",
                                default: ""
                            };
                            var surveyData = JSON.stringify(pidSurvey.surveyData);
                            pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PID, page_title);
                        };
                        return [2 /*return*/];
                }
            });
        }); };
        globalThis.initPiaETool = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, piaSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_pia_e_tool.json";
                        piaSurvey = new piaSurvey_1.PiaSurvey(lang, token, storageName_PIA);
                        return [4 /*yield*/, piaSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        piaSurvey.renderSurvey();
                        globalThis.gotoSection = function (section) {
                            piaSurvey.gotoSection(section);
                        };
                        globalThis.gotoPage = function (pageName) {
                            piaSurvey.gotoPage(pageName);
                        };
                        return [2 /*return*/];
                }
            });
        }); };
        globalThis.initPipeda = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, pipedaSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_pipeda_complaint.json";
                        pipedaSurvey = new pipedaSurvey_1.PipedaSurvey(lang, token, storageName_PIPEDA);
                        return [4 /*yield*/, pipedaSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        pipedaSurvey.renderSurvey();
                        globalThis.exportToPDF = function () {
                            var filename = "survey_export_pipeda";
                            var pdfClass = new surveyPDF_1.surveyPdfExport();
                            var page_title = {
                                en: "PIPDEA Review and send Privacy complaint form (federal institution)",
                                fr: "FR-Review and send—Privacy complaint form (federal institution)",
                                default: ""
                            };
                            var surveyData = JSON.stringify(pipedaSurvey.surveyData);
                            pdfClass.exportToPDFAlt(filename, jsonUrl, lang, surveyData, page_title);
                        };
                        return [2 /*return*/];
                }
            });
        }); };
        globalThis.initTellOPC = function (lang, token) { return __awaiter(_this, void 0, void 0, function () {
            var jsonUrl, tellOPCSurvey;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        jsonUrl = "/sample-data/survey_tell_opc.json";
                        tellOPCSurvey = new tellOPCSurvey_1.TellOPCSurvey(lang, token, storageName_TELLOPC);
                        return [4 /*yield*/, tellOPCSurvey.loadSurveyFromUrl(jsonUrl)];
                    case 1:
                        _a.sent();
                        tellOPCSurvey.renderSurvey();
                        return [2 /*return*/];
                }
            });
        }); };
        //globalThis.exportToPDF = (lang, complaintType) => {
        //    let jsonUrl = "";
        //    let filename = "";
        //    const pdfClass = new surveyPdfExport();
        //        const page_title: MultiLanguageProperty = {
        //            en: "PIPDEA Review and send Privacy complaint form (federal institution)",
        //            fr: "FR-Review and send—Privacy complaint form (federal institution)",
        //            default: ""
        //        };
        //        pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PIPEDA, page_title);
        //    } else if (complaintType === "pa") {
        //        jsonUrl = "/sample-data/survey_pa_complaint.json";
        //        filename = "survey_export_pa";
        //        const page_title: MultiLanguageProperty = {
        //            en: "PA Review and send Privacy complaint form (federal institution)",
        //            fr: "FR-Review and send—Privacy complaint form (federal institution)",
        //            default: ""
        //        };
        //        pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PA, page_title);
        //    } else if (complaintType === "pid") {
        //        jsonUrl = "/sample-data/survey_pid.json";
        //        filename = "survey_export_pid";
        //        const page_title: MultiLanguageProperty = {
        //            en: "PID PDF title",
        //            fr: "FR-PID PDF title",
        //            default: ""
        //        };
        //        pdfClass.exportToPDF(filename, jsonUrl, lang, storageName_PID, page_title);
        //    }
        //        const surveyData: string = JSON.stringify(pipedaSurvey.surveyData);
        //        pdfClass.exportToPDFAlt(filename, jsonUrl, lang, surveyData, page_title);
        //    };
        //};
        globalThis.initTestSurvey = function (lang, token) {
            var sampleSurvey = new testSurvey_1.TestSurvey();
            var jsonUrl = "/sample-data/survey_test_2.json";
            sampleSurvey.init(jsonUrl, lang, token);
        };
    }
    surveyPolyfill();
    main();
})();
//# sourceMappingURL=index.js.map