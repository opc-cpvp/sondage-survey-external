import Vue from "vue"; // https://vuejs.org/v2/guide/typescript.html
import * as Survey from "survey-vue";

export class SampleSurvey {
    private _surveyModelUrl: string;

    constructor(surveyModelUrl: string) {
        this._surveyModelUrl = surveyModelUrl;
    }

    public init(): void {
        void fetch(this._surveyModelUrl)
            .then(response => response.json())
            .then(json => {
                const survey = new Survey.Model(json);

                const myCss = {
                    navigationButton: "btn btn-primary",
                    html: "sq-html"
                };

                survey.cssType = "bootstrap";
                survey.css = myCss;

                const app = new Vue({
                    el: "#surveyElement",
                    data: {
                        survey
                    }
                });

                return app;
            });
    }
}
