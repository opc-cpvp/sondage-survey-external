export class ProblemDetails {
    detail: string;
    extensions: string;
    title: string;
    status: string;
    type: string;
    instance: string;
    errors: any



    constructor(private problem?: ProblemDetails) {
        this.detail = "";
        this.extensions = "";
        this.title = "";
        this.status = "";
        this.type = "";
        this.instance = "";
    }
}