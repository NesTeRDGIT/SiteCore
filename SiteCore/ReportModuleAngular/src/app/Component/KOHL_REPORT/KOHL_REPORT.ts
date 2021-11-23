import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { KohlRow } from "../../API/KOHLRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "kohl-report", templateUrl: "KOHL_REPORT.html" })
export class KohlReportComponent extends BaseReportComponent {
    report : KohlRow[] = [];
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
            this.report = await this.repo.getKohlReport(this.dateB, this.dateE);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getKohlXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
