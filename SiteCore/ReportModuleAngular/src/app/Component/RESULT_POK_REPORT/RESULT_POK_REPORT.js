var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { FileAPI } from "../../API/FileAPI";
let ResultPokReportComponent = class ResultPokReportComponent {
    constructor(repo) {
        this.repo = repo;
        this.report = [];
        this.isLoad = false;
        this.getReport = async () => {
            try {
                this.isLoad = true;
                this.report = await this.repo.GetEffectivenessReport(this.dateB, this.dateE);
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
                const file = await this.repo.GetEffectivenessXls();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.dateB = this.dateE = new Date();
    }
};
ResultPokReportComponent = __decorate([
    Component({ selector: "result-pok-report", templateUrl: "RESULT_POK_REPORT.html" })
], ResultPokReportComponent);
export { ResultPokReportComponent };
//# sourceMappingURL=RESULT_POK_REPORT.js.map