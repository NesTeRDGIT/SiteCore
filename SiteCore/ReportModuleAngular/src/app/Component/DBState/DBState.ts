import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { DataBaseStateRow } from "../../API/DataBaseStateRow";

@Component({ selector: "DB-State", templateUrl: "DBState.html" })
export class DBStateComponent {
    report: DataBaseStateRow[] = [];
    isLoad=false;
    
    constructor(public repo: IRepository) {
        
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
