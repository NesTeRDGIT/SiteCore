var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { DliRecord } from "../../API/DliRow";
import { FileAPI } from "../../API/FileAPI";
let DliReportComponent = class DliReportComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.report = new DliRecord(null);
        this.getReport = async () => {
            try {
                this.isLoad = true;
                this.report = await this.repo.getDliReportAsync(this.year);
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
                const file = await this.repo.getDliXlsAsync();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.year = new Date().addMonths(-1).getFullYear();
    }
};
DliReportComponent = __decorate([
    Component({ selector: "dli-report", templateUrl: "DLI_REPORT.html" })
], DliReportComponent);
export { DliReportComponent };
//# sourceMappingURL=DLI_REPORT.js.map