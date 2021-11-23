var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { DispRecord } from "../../API/DispEntity";
import { FileAPI } from "../../API/FileAPI";
let DispReportComponent = class DispReportComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.report = new DispRecord(null);
        this.getReport = async () => {
            try {
                this.isLoad = true;
                this.report = await this.repo.getDispReport(this.period.getFullYear(), this.period.getMonth() + 1);
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
                const file = await this.repo.getDispXlsAsync();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.period = new Date().addMonths(-1);
    }
};
DispReportComponent = __decorate([
    Component({ selector: "disp-report", templateUrl: "DISP_REPORT.html" })
], DispReportComponent);
export { DispReportComponent };
//# sourceMappingURL=DISP_REPORT.js.map