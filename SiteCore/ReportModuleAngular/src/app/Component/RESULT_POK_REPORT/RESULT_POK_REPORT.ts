import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { ZpzEffectiveness } from "../../API/zpzEffectiveness";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "result-pok-report", templateUrl: "RESULT_POK_REPORT.html" })
export class ResultPokReportComponent {
    report: ZpzEffectiveness[] = [];
    dateB: Date;
    dateE:Date;
    isLoad=false;
    
    constructor(public repo: IRepository) {
        this.dateB = this.dateE = new Date();
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
