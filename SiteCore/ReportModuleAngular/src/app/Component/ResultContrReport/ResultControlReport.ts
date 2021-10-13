import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { ResultControlDET, ResultControlVZR } from "../../API/ResultControlReportRows";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "result-control-report", templateUrl: "ResultControlReport.html" })
export class ResultControlComponent {
    reportVZR: ResultControlVZR[] = [];
    reportDET: ResultControlDET[] = [];
    dateB: Date;
    dateE:Date;
    isLoad=false;
    
    constructor(public repo: IRepository) {
        this.dateB = this.dateE = new Date();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            const result = await this.repo.GetResultControlReport(this.dateB, this.dateE);
            this.reportVZR = result.VZR;
            this.reportDET = result.DET;
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.GetResultControlXls();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
