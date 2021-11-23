import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { DliRecord} from "../../API/DliRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "dli-report", templateUrl: "DLI_REPORT.html" })
export class DliReportComponent extends BaseReportComponent {
    report = new DliRecord(null);
    year:number;
   

    constructor(public repo: IRepository) {
        super();
        this.year = new Date().addMonths(-1).getFullYear();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getDliReportAsync(this.year);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getDliXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
