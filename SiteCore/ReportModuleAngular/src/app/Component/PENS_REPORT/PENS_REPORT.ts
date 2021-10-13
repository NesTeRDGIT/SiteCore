import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { PensRow } from "../../API/PensRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "pens-report", templateUrl: "PENS_REPORT.html" })
export class PensReportComponent {
    report: PensRow[] = [];
    year: number;
    isLoad=false;
    
    constructor(public repo: IRepository) {
        this.year = new Date().getFullYear();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getPensReportAsync(this.year);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getPensXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
