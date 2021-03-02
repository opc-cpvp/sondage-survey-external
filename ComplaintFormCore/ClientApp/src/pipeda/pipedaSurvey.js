"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
exports.PipedaSurvey = void 0;
var survey_1 = require("../survey");
var survey_vue_1 = require("../../node_modules/survey-vue/survey.vue");
var pipedaProvinceData_1 = require("./pipedaProvinceData");
var filemeterwidget_1 = require("../widgets/filemeterwidget");
var PipedaSurvey = /** @class */ (function (_super) {
    __extends(PipedaSurvey, _super);
    function PipedaSurvey(locale, authToken, storageName) {
        if (locale === void 0) { locale = "en"; }
        var _this = _super.call(this, locale, storageName) || this;
        _this.surveyData = "";
        _this.authToken = authToken;
        // Since our completed page relies on a variable, we'll hide it until the variable is set.
        _this.survey.showCompletedPage = false;
        return _this;
    }
    PipedaSurvey.prototype.registerWidgets = function () {
        filemeterwidget_1.FileMeterWidget.register();
    };
    PipedaSurvey.prototype.registerEventHandlers = function () {
        var _this = this;
        _super.prototype.registerEventHandlers.call(this);
        this.survey.onServerValidateQuestions.add(function (sender, options) {
            _this.handleOnServerValidateQuestions(sender, options);
        });
        this.survey.onComplete.add(function (sender, options) {
            _this.handleOnComplete(sender, options);
        });
        this.survey.onValueChanged.add(function (sender, options) {
            _this.handleOnValueChanged(sender, options);
        });
        this.survey.onUploadFiles.add(function (sender, options) {
            _this.handleOnUploadFiles(sender, options);
        });
        this.survey.onClearFiles.add(function (sender, options) {
            _this.handleOnClearFiles(sender, options);
        });
    };
    PipedaSurvey.prototype.handleOnServerValidateQuestions = function (sender, options) {
        var _this = this;
        if (!this.survey.isLastPage) {
            options.complete();
            return;
        }
        var validationUrl = "/api/PipedaSurvey/Validate?complaintId=" + this.authToken;
        void (function () { return __awaiter(_this, void 0, void 0, function () {
            var response, questions, errors, problem_1, validationOptions;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, fetch(validationUrl, {
                            method: "POST",
                            headers: {
                                Accept: "application/json",
                                "Content-Type": "application/json; charset=utf-8"
                            },
                            body: JSON.stringify(sender.data)
                        })];
                    case 1:
                        response = _a.sent();
                        questions = [];
                        errors = [];
                        if (!!response.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, response.json()];
                    case 2:
                        problem_1 = _a.sent();
                        Object.keys(problem_1.errors).forEach(function (q) {
                            // options.errors in only able to set one error per question
                            options.errors[q] = problem_1.errors[q][0];
                            var question = sender.getQuestionByName(q);
                            if (question && question["errors"]) {
                                question.clearErrors();
                                questions.push(question);
                                for (var _i = 0, _a = problem_1.errors[q]; _i < _a.length; _i++) {
                                    var error = _a[_i];
                                    errors.push(new survey_vue_1.SurveyError(error, question));
                                }
                            }
                        });
                        _a.label = 3;
                    case 3:
                        options.complete();
                        // TODO: Remove the following lines after updating surveyjs >= v1.8.21 (Bug #2566)
                        if (this.survey.onValidatedErrorsOnCurrentPage.isEmpty) {
                            return [2 /*return*/];
                        }
                        validationOptions = {
                            page: sender.currentPage,
                            questions: questions,
                            errors: errors
                        };
                        this.survey.onValidatedErrorsOnCurrentPage.fire(sender, validationOptions);
                        return [2 /*return*/];
                }
            });
        }); })();
    };
    PipedaSurvey.prototype.handleOnComplete = function (sender, options) {
        var _this = this;
        void (function () { return __awaiter(_this, void 0, void 0, function () {
            var completeUrl, response, problem, responseData;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        completeUrl = "/api/PipedaSurvey/Complete?complaintId=" + this.authToken;
                        options.showDataSaving();
                        return [4 /*yield*/, fetch(completeUrl, {
                                method: "POST",
                                headers: {
                                    Accept: "application/json",
                                    "Content-Type": "application/json; charset=utf-8"
                                },
                                body: JSON.stringify(sender.data)
                            })];
                    case 1:
                        response = _a.sent();
                        if (!!response.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, response.json()];
                    case 2:
                        problem = _a.sent();
                        options.showDataSavingError();
                        return [2 /*return*/];
                    case 3: return [4 /*yield*/, response.json()];
                    case 4:
                        responseData = _a.sent();
                        this.survey.setVariable("referenceNumber", responseData.referenceNumber);
                        //  Store the json data in a public variable before clearing the storage.
                        //  This is for pdf export
                        this.surveyData = sender.data;
                        // Now that the variable is set, show the completed page.
                        this.survey.showCompletedPage = true;
                        this.storage.remove(this.storageName);
                        options.showDataSavingSuccess();
                        return [2 /*return*/];
                }
            });
        }); })();
    };
    PipedaSurvey.prototype.handleOnValueChanged = function (sender, options) {
        var question = options.question;
        var value = options.value;
        if (question.name !== "ProvinceIncidence" || value === null) {
            return;
        }
        var provinceId = Number(value);
        var province = pipedaProvinceData_1.PipedaProvincesData.get(provinceId);
        var provinceData = this.survey.locale === "fr" ? province.French : province.English;
        this.survey.setVariable("province_incidence_prefix_au", provinceData.FrenchPrefix_Au); // en, au, à...
        this.survey.setVariable("province_incidence_prefix_du", provinceData.FrenchPrefix_Du); // de, du, de la...
        this.survey.setVariable("province_link", provinceData.Province_link);
        this.survey.setVariable("link_province_opc", provinceData.Link_province_opc);
        this.survey.setVariable("link_more_info", provinceData.Link_more_info);
    };
    PipedaSurvey.prototype.handleOnUploadFiles = function (sender, options) {
        var _this = this;
        void (function () { return __awaiter(_this, void 0, void 0, function () {
            var questionName, uploadUrl, formData, response, problem_2, questionErrors_1, responseData;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        questionName = options.name;
                        uploadUrl = "/api/File/Upload?complaintId=" + this.authToken + "&questionName=" + questionName;
                        formData = new FormData();
                        options.files.forEach(function (file) {
                            formData.append(file.name, file);
                        });
                        return [4 /*yield*/, fetch(uploadUrl, {
                                method: "POST",
                                body: formData
                            })];
                    case 1:
                        response = _a.sent();
                        if (!!response.ok) return [3 /*break*/, 3];
                        return [4 /*yield*/, response.json()];
                    case 2:
                        problem_2 = _a.sent();
                        questionErrors_1 = new Map();
                        Object.keys(problem_2.errors).forEach(function (q) {
                            var errors = problem_2.errors[q].map(function (error) { return new survey_vue_1.SurveyError(error, options.question); });
                            questionErrors_1.set(options.question, errors);
                        });
                        this.displayErrorSummary(questionErrors_1);
                        return [2 /*return*/];
                    case 3: return [4 /*yield*/, response.json()];
                    case 4:
                        responseData = _a.sent();
                        options.callback("success", options.files.map(function (file) { return ({
                            file: file,
                            content: "/api/File/Get?complaintId=" + _this.authToken + "&fileUniqueId=" + responseData[file.name].content + "&filename=" + file.name
                        }); }));
                        return [2 /*return*/];
                }
            });
        }); })();
    };
    PipedaSurvey.prototype.handleOnClearFiles = function (sender, options) {
        options.callback("success");
    };
    return PipedaSurvey;
}(survey_1.SurveyBase));
exports.PipedaSurvey = PipedaSurvey;
//# sourceMappingURL=pipedaSurvey.js.map