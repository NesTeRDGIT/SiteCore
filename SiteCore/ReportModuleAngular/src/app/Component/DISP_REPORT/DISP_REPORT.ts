import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { DispRecord, DispVzrRow, DispDetRow, ProfDet, ProfVzr} from "../../API/DispEntity";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "disp-report", templateUrl: "DISP_REPORT.html" })
export class DispReportComponent {
    report = new DispRecord(null);
    period:Date;
    isLoad=false;

    constructor(public repo: IRepository) {
        this.period = new Date();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report.DispVzr.push(new DispVzrRow(null));
            this.report.DispDet.push(new DispDetRow(null));
            this.report.ProfVzr.push(new ProfVzr(null));
            this.report.ProfDet.push(new ProfDet(null));

            // this.report = await this.repo.getDispReport(this.period.getFullYear(), this.period.getMonth()+1);
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
