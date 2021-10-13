import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { EcoRecord} from "../../API/EcoRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "eco-report", templateUrl: "ECO_REPORT.html" })
export class EcoReportComponent {
    report = new EcoRecord(null);
    period:Date;
    isLoad=false;

    constructor(public repo: IRepository) {
        this.period = new Date();
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
