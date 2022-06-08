
import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { KSGRow } from "../../API/KSGRow";
import { FileAPI } from "../../API/FileAPI";

@Component({ selector: "ksg-report", templateUrl: "KSG_REPORT.html" })
export class KSGReportComponent extends BaseReportComponent {
    report : KSGRow[] = [];
    dateB: Date;
    dateE: Date;

    constructor(public repo: IRepository) {
        super();
        const now = new Date().addMonths(-1);
        this.dateB = new Date(now.getFullYear(), 0, 1);
        this.dateE = new Date(now.getFullYear(), 11, 31);
    }

    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getKSGReportAsync(this.dateB, this.dateE);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getKSGXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
