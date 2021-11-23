import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { SMPRow } from "../../API/SMPRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "smp-report", templateUrl: "SMP_REPORT.html" })
export class SmpReportComponent extends BaseReportComponent {
    report: SMPRow[] = [];
    dateB: Date;
    dateE: Date;
   
    
    constructor(public repo: IRepository) {
        super();
        this.dateB = this.dateE  = new Date().addMonths(-1);
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
