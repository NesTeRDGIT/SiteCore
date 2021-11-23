import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { EcoRecord} from "../../API/EcoRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "eco-report", templateUrl: "ECO_REPORT.html" })
export class EcoReportComponent extends BaseReportComponent{
    report = new EcoRecord(null);
    period:Date;
    constructor(public repo: IRepository) {
        super();
        this.period = new Date().addMonths(-1);
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getEcoReport(this.period.getFullYear(), this.period.getMonth()+1);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getEcoXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
