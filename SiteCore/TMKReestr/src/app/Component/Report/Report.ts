
import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit } from "@angular/core";
import { IRepository } from "../../API/Repository";
import { UserINFO } from "../../API/UserINFO";
import { ReportParamModel, TMKReportTableModel } from "../../API/ReportModel";
import {NMIC_VID_NHISTORY, SPRModel} from "../../API/SPRModel"
import {FileAPI} from "../../API/FileAPI"
@Component({ selector: "Report", templateUrl: "Report.html" })
export class ReportComponent implements OnInit {
    @Input() userInfo:UserINFO;
    SPR:SPRModel;

    model:TMKReportTableModel = new TMKReportTableModel(null);
    param:ReportParamModel= new ReportParamModel();

    loading:boolean = false;

    constructor(public repo: IRepository) {
        this.SPR = new SPRModel(repo);
    }
    FILL_SPR = async () => {
        this.param.Date2 = new Date();
        this.param.Date1 = new Date(this.param.Date2.getFullYear(), 1, 1);
        await this.SPR.refreshNMIC_VID_NHISTORYAsync();
        this.NMIC_VID_NHISTORYSelect = [this.SPR.NMIC_VID_NHISTORY.SPR.getItem("1"),this.SPR.NMIC_VID_NHISTORY.SPR.getItem("4")];
    }

    ngOnInit(): void {        
        this.FILL_SPR();
    }

    GetData = async () => {
        try {
            this.loading = true;
            this.model = await this.repo.GetReportAsync(this.param);
        }
        catch (err) {
            alert(err.toString())
        }
        finally {
            this.loading = false;
        }
    }


     DownloadXLS = async () => {
        try {
          let file = await this.repo.GetReportXLSAsync();
          FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        }
        catch (err) {
            alert(err.toString())
        }
    }

    ConvertToDate(obj: any): Date {
        if(obj)
            return new Date(obj);
        return null;
    }




    _NMIC_VID_NHISTORYSelect: NMIC_VID_NHISTORY[] = [];
    get NMIC_VID_NHISTORYSelect(): NMIC_VID_NHISTORY[] {
        return this._NMIC_VID_NHISTORYSelect;
    }
    set NMIC_VID_NHISTORYSelect(val: NMIC_VID_NHISTORY[]) {
        this.param.VID_NHISTORY = val.map(x=>x.ID_VID_NHISTORY);
        this._NMIC_VID_NHISTORYSelect = val;
    }

    NMIC_VID_NHISTORYFiltered:NMIC_VID_NHISTORY[] = [];
    searchNMIC_VID_NHISTORY(event) {
       this.NMIC_VID_NHISTORYFiltered = this.SPR.NMIC_VID_NHISTORY.SPR.values().filter(x=> x.FULL_NAME.indexOf(event.query)!=-1)      
    }
}