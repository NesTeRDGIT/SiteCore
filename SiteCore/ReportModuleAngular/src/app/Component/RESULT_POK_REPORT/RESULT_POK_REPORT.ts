import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { ZpzEffectiveness } from "../../API/zpzEffectiveness";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "result-pok-report", templateUrl: "RESULT_POK_REPORT.html" })
export class ResultPokReportComponent extends BaseReportComponent {
    report: ZpzEffectiveness[] = [];
    dateB: Date;
    dateE:Date;
   
    
    constructor(public repo: IRepository) {
        super();
        this.dateB = this.dateE = new Date().addMonths(-1);
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.GetEffectivenessReport(this.dateB, this.dateE);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.GetEffectivenessXls();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
