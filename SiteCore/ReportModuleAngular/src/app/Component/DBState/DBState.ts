import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";
import { DataBaseStateRow } from "../../API/DataBaseStateRow";

@Component({ selector: "DB-State", templateUrl: "DBState.html" })
export class DBStateComponent extends BaseReportComponent {
    report: DataBaseStateRow[] = [];
    constructor(public repo: IRepository) {
        super();
    }



    getReport = async () => {
        try {
            this.isLoad = true;
            this.report = await this.repo.getDBState();
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }
    

}
