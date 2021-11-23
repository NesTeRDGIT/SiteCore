import { Component, Output, EventEmitter, OnChanges, SimpleChanges} from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { AbortRow } from "../../API/AbortRow";
import { FileAPI } from "../../API/FileAPI";

@Component({ selector: "abort-report", templateUrl: "ABORT_REPORT.html" })
export class AbortReportComponent extends BaseReportComponent  {


    report: AbortRow[] = [];
    year: number;

    constructor(public repo: IRepository) {
        super();
        this.year = new Date().addMonths(-1).getFullYear();
    }

    getReport = async () => {
        try {
            this.isLoad = true;
           
            this.report = await this.repo.getGetAbortReportAsync(this.year);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    getXls = async () => {
        try {
            const file = await this.repo.getAbortXlsAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        }
    }


}


