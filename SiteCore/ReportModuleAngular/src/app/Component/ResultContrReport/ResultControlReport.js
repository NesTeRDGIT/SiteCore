var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { FileAPI } from "../../API/FileAPI";
let ResultControlComponent = class ResultControlComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.reportVZR = [];
        this.reportDET = [];
        this.getReport = async () => {
            try {
                this.isLoad = true;
                const result = await this.repo.GetResultControlReport(this.dateB, this.dateE);
                this.reportVZR = result.VZR;
                this.reportDET = result.DET;
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.getXls = async () => {
            try {
                const file = await this.repo.GetResultControlXls();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.dateB = this.dateE = new Date().addMonths(-1);
    }
};
ResultControlComponent = __decorate([
    Component({ selector: "result-control-report", templateUrl: "ResultControlReport.html" })
], ResultControlComponent);
export { ResultControlComponent };
//# sourceMappingURL=ResultControlReport.js.map