import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { oksOnmkRow } from "../../API/oksOnmkRow";
import { FileAPI } from "../../API/FileAPI";
@Component({ selector: "oksonmk-report", templateUrl: "OKSONMK_REPORT.html" })
export class OksOnmkReportComponent {
    report : oksOnmkRow[] = [];
    year: number;
    isLoad=false;

    constructor(public repo: IRepository) {
        const now = new Date();
        this.year = now.getFullYear();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getOksOnmkReport(this.year);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getOksOnmkXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}
