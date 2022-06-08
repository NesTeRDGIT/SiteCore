var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../BaseReportComponent';
import { FileAPI } from "../../API/FileAPI";
let KSGReportComponent = class KSGReportComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.report = [];
        this.getReport = async () => {
            try {
                this.isLoad = true;
                this.report = await this.repo.getKSGReportAsync(this.dateB, this.dateE);
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
                const file = await this.repo.getKSGXlsAsync();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        const now = new Date().addMonths(-1);
        this.dateB = new Date(now.getFullYear(), 0, 1);
        this.dateE = new Date(now.getFullYear(), 11, 31);
    }
};
KSGReportComponent = __decorate([
    Component({ selector: "ksg-report", templateUrl: "KSG_REPORT.html" })
], KSGReportComponent);
export { KSGReportComponent };
//# sourceMappingURL=KSG_REPORT.js.map