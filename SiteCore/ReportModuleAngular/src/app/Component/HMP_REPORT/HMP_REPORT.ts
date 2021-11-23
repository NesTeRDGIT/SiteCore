
import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { CURRENT_VMP_OOMS } from "../../API/CURRENT_VMP_OOMS";
import { FileAPI } from "../../API/FileAPI";

@Component({ selector: "hmp-report", templateUrl: "HMP_REPORT.html" })
export class HmpReportComponent extends BaseReportComponent {
    report: CURRENT_VMP_OOMS[] = [];
   

    constructor(public repo: IRepository) {
        super();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getVmpReportAsync();
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getVmpReportXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        } 
    }



}
