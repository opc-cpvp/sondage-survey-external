import { Widget } from "./widget";
import { File } from "./file";
import { CustomWidgetCollection, JsonObject, Question, QuestionFileModel, SurveyModel } from "survey-vue";

export class FileMeterWidget extends Widget {
    private activatedBy = "property";
    private questionFiles: Map<Question, File[]> = new Map();

    constructor() {
        super("filemeter", "Meter used to display total amount of space used by files");
    }

    static register(): void {
        const widget: FileMeterWidget = new FileMeterWidget();

        // If activatedBy isn't passed, it will default to property.
        CustomWidgetCollection.Instance.addCustomWidget(widget);
    }

    /**
     * Use this function to create a new class or add new properties or remove unneeded properties from your widget.
     *
     * @param activatedBy Tells how your widget has been activated by: property, type or customType.
     * property - It means that it will activated if a property of the existing question type is set to particular value, for example inputType = "date".
     * type - You are changing the behaviour of entire question type. For example render radiogroup question differently, have a fancy radio buttons.
     * customType - You are creating a new type, like in our example "textwithbutton".
     */
    activatedByChanged(activatedBy: string): void {
        this.activatedBy = activatedBy;
        if (activatedBy === "property") {
            JsonObject.metaData.addProperty("file", {
                name: "showMeter:boolean",
                category: "general",
                default: false
            });
            JsonObject.metaData.addProperty("file", {
                name: "totalSize:number",
                category: "general",
                default: 0
            });
        }
    }

    /**
     * The main function, rendering and two-way binding.
     *
     * @param question
     * @param el
     */
    afterRender(question: Question, el: HTMLElement): void {
        const fileQuestion: QuestionFileModel = (question as unknown) as QuestionFileModel;
        const survey = fileQuestion.survey as SurveyModel;

        const placeholder = el.querySelector(".sv_q_file");
        if (!placeholder) {
            return;
        }

        this.loadQuestionFiles(fileQuestion);

        const container = document.createElement("section");
        container.className = "alert alert-info";

        const header = document.createElement("h4");
        this.updateHeader(header, question);

        const meter = document.createElement("meter");
        meter.className = "full-width";
        meter.min = 0;
        meter.max = question.totalSize || 0;
        meter.value = this.getQuestionSize(question);

        container.appendChild(header);
        container.appendChild(meter);

        placeholder.insertAdjacentElement("beforebegin", container);

        survey.onValidateQuestion.add((sender: SurveyModel, options: any) => {
            if (options.question !== question) {
                return;
            }

            const totalSize = question.totalSize || 0;
            const size = this.getQuestionSize(question);

            if (size > totalSize) {
                options.error = "The size of the files exceeds the total size allowed.";
            }
        });

        survey.onUploadFiles.add((sender: SurveyModel, options: any) => {
            if (options.question !== question) {
                return;
            }

            const files = this.questionFiles.get(question) || [];

            options.files.forEach(file => {
                if (!files.some(f => f.name === file.name)) {
                    files.push(file);
                }
            });

            this.questionFiles.set(question, files);

            meter.value = this.getQuestionSize(question);

            this.updateHeader(header, question);
        });

        survey.onClearFiles.add((sender: SurveyModel, options: any) => {
            if (options.question !== question) {
                return;
            }

            let files = this.questionFiles.get(question) || [];

            if (options.fileName === null) {
                files = [];
            } else {
                const index = files.findIndex(f => f.name === options.fileName);
                if (index < 0) {
                    return;
                }
                files.splice(index, 1);
            }

            this.questionFiles.set(question, files);

            meter.value = this.getQuestionSize(question);

            this.updateHeader(header, question);
        });

        question.onStateChanged.add((sender: QuestionFileModel, options: any) => {
            if (options.state !== "loaded") {
                return;
            }

            const files = question.value || [];
            const questionFiles = this.questionFiles.get(question) || [];

            for (const file of files) {
                const name = file.name;
                const questionFile = questionFiles.find(f => f.name === name);

                if (!questionFile) {
                    continue;
                }

                const size = questionFile.size;
                file.size = size;
            }
        });
    }

    /**
     * SurveyJS library calls this function for every question to check, if it should use this widget instead of default rendering/behavior.
     *
     * @param question
     */
    isFit(question: Question): boolean {
        if (this.activatedBy === "property") {
            return question.showMeter && question.totalSize && question.getType() === "file";
        }
        return false;
    }

    private loadQuestionFiles(question: QuestionFileModel): void {
        if (this.questionFiles.get(question)) {
            return;
        }

        const files = question.value || [];
        const questionFiles = files.map(
            f =>
                ({
                    name: f.name,
                    type: f.type,
                    size: f.size
                } as File)
        ) as File[];
        this.questionFiles.set(question, questionFiles);
    }

    private updateHeader(header: HTMLHeadingElement, question: Question): void {
        let totalSize: number = question.totalSize || 0;
        let size = this.getQuestionSize(question);

        // Convert bytes to megabytes
        totalSize = totalSize / 1048576;
        size = size / 1048576;

        header.innerText = `You have uploaded ${size.toFixed(1)} MB of files out of ${totalSize.toFixed(1)} MB allowed.`;
    }

    private getQuestionSize(question: Question): number {
        const files = this.questionFiles.get(question) || [];
        let total = 0;
        if (files.length > 0) {
            files.forEach(file => {
                if (file.size) {
                    total += file.size;
                }
            });

            return total;
            // return files.reduce((previous, current) => previous + current.size, 0);
        } else {
            return 0;
        }
    }
}
