import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { DispRecord, DispVzrRow, DispDetRow, ProfDetRow, ProfVzrRow} from "../../API/DispEntity";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "disp-report", templateUrl: "DISP_REPORT.html" })
export class DispReportComponent extends BaseReportComponent{
    report = new DispRecord(null);
    period:Date;

    constructor(public repo: IRepository) {
        super();
        this.period = new Date().addMonths(-1);
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getDispReport(this.period.getFullYear(), this.period.getMonth()+1);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getDispXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
