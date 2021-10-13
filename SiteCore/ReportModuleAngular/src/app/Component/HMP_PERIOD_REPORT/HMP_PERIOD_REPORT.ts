
import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { VMP_OOMS } from "../../API/CURRENT_VMP_OOMS";
import { FileAPI } from "../../API/FileAPI";

@Component({ selector: "hmp-period-report", templateUrl: "HMP_PERIOD_REPORT.html" })
export class HmpReportPeriodComponent {
    report: VMP_OOMS[] = [];
    dateB: Date;
    dateE: Date;
    isLoad=false;

    constructor(public repo: IRepository) {
        this.dateB = this.dateE = new Date();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getVmpPeriodReportAsync(this.dateB, this.dateE);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getVmpPeriodReportXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        } 
    }



}
