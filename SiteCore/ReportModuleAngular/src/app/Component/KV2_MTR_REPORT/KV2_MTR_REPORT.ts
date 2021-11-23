import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { Kv2MtrRow } from "../../API/Kv2MtrRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "kv2-mtr-report", templateUrl: "KV2_MTR_REPORT.html" })
export class Kv2MtrReportComponent extends BaseReportComponent {
    report: Kv2MtrRow[] = [];
    period: Date;
   
    
    constructor(public repo: IRepository) {
        super();
        this.period = new Date().addMonths(-1);
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getKv2MtrReportAsync(this.period.getFullYear(), this.period.getMonth() + 1);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getKv2MtrXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
