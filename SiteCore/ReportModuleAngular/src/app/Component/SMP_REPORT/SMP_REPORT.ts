import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { SMPRow } from "../../API/SMPRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "smp-report", templateUrl: "SMP_REPORT.html" })
export class SmpReportComponent {
    report: SMPRow[] = [];
    dateB: Date;
    dateE: Date;
    isLoad=false;
    
    constructor(public repo: IRepository) {
        this.dateB = this.dateE  = new Date();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getSmpReportAsync(this.dateB,this.dateE);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getSmpXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
