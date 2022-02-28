import { Component, OnInit,Output,EventEmitter,ViewChild } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { SPRModel, CODE_MO, SMO, NMIC_VID_NHISTORY, NMIC_OPLATA} from "../../API/SPRModel";
import { TMKFilter } from "../../API/TMKFilter";


@Component({ selector: "FindMenu", templateUrl: "FindMenuComponent.html"})
export class FindMenuComponent implements OnInit {
    @Output() onSearch = new EventEmitter<TMKFilter>();

    SPR:SPRModel = new SPRModel(this.repo);
    Filter:TMKFilter = new TMKFilter();
    onLoading:boolean = true;
    constructor(public repo: IRepository) {

    }
    ngOnInit(): void {
      
    }


    async onTabOpen(e) {
        try {
            this.onLoading = true;
            await this.SPR.refreshStaticSPR(true);
            await this.SPR.refreshVariableSPR(true);
            this.onLoading = false;
        }
        catch (err) {
            alert(err.toString());
        }
    }
    

   

    ClearFilter() {
        this.SMOSelect = [];
        this.CODE_MOSelect = [];
        this.NMIC_VID_NHISTORYSelect = [];
        this.NMIC_OPLATASelect = [];
        this.Filter.Clear();
        this.onSearch?.emit(this.Filter);
    }

    ShowEmptyFilter = false;
    Find() {
        this.onSearch?.emit(this.Filter);
    }

    ConvertToDate(obj: any): Date {
        if (obj != null && obj != '')
            return new Date(obj);
        return null;
    }


    //#region Filter Select


    _CODE_MOSelect: CODE_MO[] = [];
    get CODE_MOSelect(): CODE_MO[] {
        return this._CODE_MOSelect;
    }
    set CODE_MOSelect(val: CODE_MO[]) {
        this.Filter.CODE_MO = val.map(x=>x.MCOD);
        this._CODE_MOSelect = val;
    }

    CODE_MOFiltered:CODE_MO[] = [];
    searchMO(event) {
       this.CODE_MOFiltered = this.SPR.CODE_MO_Reestr.SPR.values().filter(x=> x.MCOD.indexOf(event.query)!=-1 || x.NAM_MOK.indexOf(event.query)!=-1)      
    }

    _SMOSelect: SMO[] = [];
    get SMOSelect(): SMO[] {
        return this._SMOSelect;
    }
    set SMOSelect(val: SMO[]) {
        this.Filter.SMO = val.map(x=>x.SMOCOD);
        this._SMOSelect = val;
    }

    SMOFiltered:SMO[] = [];
    searchSMO(event) {
        this.SMOFiltered = this.SPR.CODE_SMO_Reestr.SPR.values().filter(x=> x.SMOCOD.indexOf(event.query)!=-1 || x.NAM_SMOK.indexOf(event.query)!=-1)      
    }

    _NMIC_VID_NHISTORYSelect: NMIC_VID_NHISTORY[] = [];
    get NMIC_VID_NHISTORYSelect(): NMIC_VID_NHISTORY[] {
        return this._NMIC_VID_NHISTORYSelect;
    }
    set NMIC_VID_NHISTORYSelect(val: NMIC_VID_NHISTORY[]) {
        this.Filter.VID_NHISTORY = val.map(x=>x.ID_VID_NHISTORY);
        this._NMIC_VID_NHISTORYSelect = val;
    }

    NMIC_VID_NHISTORYFiltered:NMIC_VID_NHISTORY[] = [];
    searchNMIC_VID_NHISTORY(event) {
        this.NMIC_VID_NHISTORYFiltered = this.SPR.NMIC_VID_NHISTORY.SPR.values().filter(x=> x.VID_NHISTORY.indexOf(event.query)!=-1)      
    }

    _NMIC_OPLATASelect: NMIC_OPLATA[];
    get NMIC_OPLATASelect(): NMIC_OPLATA[] {
        return this._NMIC_OPLATASelect;
    }
    set NMIC_OPLATASelect(val: NMIC_OPLATA[]) {
        this.Filter.OPLATA = val.map(x=>x.ID_OPLATA);
        this._NMIC_OPLATASelect = val;
    }
    NMIC_OPLATAFiltered:NMIC_OPLATA[] = [];
    searchNMIC_OPLATA(event) {
      this.NMIC_OPLATAFiltered = this.SPR.NMIC_OPLATA.SPR.values().filter(x=> x.OPLATA.indexOf(event.query)!=-1)      
    }
    //#endregion
}
