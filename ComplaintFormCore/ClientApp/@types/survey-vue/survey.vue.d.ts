import { IQuestion } from "survey-vue"
declare module "survey-vue" {
    interface IQuestion {
        renderAs: string
    }
}