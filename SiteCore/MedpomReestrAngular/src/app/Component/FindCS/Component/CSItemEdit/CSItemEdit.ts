import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef } from "@angular/core";
import { PersonItemModel, SPRModel, SprF011ItemModel, SprVPOLISItemModel, SprWItemModel } from 'src/app/API/CSModel';
import { IRepositoryCS, ResultOperation } from 'src/app/API/IRepositoryCS';
import { ChangeDetectorRef } from '@angular/core';
@Component({ selector: "CSItemEdit", templateUrl: "CSItemEdit.html", styleUrls: ["CSItemEdit.css"] })
export class CSItemEdit {
    mode: EditMode = EditMode.New;
    EditMode = EditMode;
    currentCS_LIST_IN_ID: number = null;
    //#region Input Output
    _display = false;
    get display(): any {
        return this._display;
    }
    set display(value: any) {
        this._display = value;
        if (!value) {
            this.ClearSPR();
            this.ClearModel();
        }
    }
    @Output() onUpdateData: EventEmitter<any> = new EventEmitter();
    //#endregion

    model = new PersonItemModel(null);
    SPR: SPRModel = new SPRModel(null);
    isLoad: boolean = false;
    constructor(private repo: IRepositoryCS, private cdr: ChangeDetectorRef) {

    }

    public async ShowDialog(mode: EditMode, CS_LIST_IN_ID: number = null) {
        try {
            this.currentCS_LIST_IN_ID = CS_LIST_IN_ID;
            this.mode = mode;
            this.isLoad = true;
            this.display = true;
            this.SPR = await this.repo.GetSPR();
            switch (mode) {
                case EditMode.New:
                    this.model = new PersonItemModel(null);
                    break;
                case EditMode.Double:
                case EditMode.Edit:
                    await this.GetModel(CS_LIST_IN_ID); break;
            }

        } catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoad = false;
        }
    }

    private ClearModel() {

        this.currentCS_LIST_IN_ID = null;
        this.model = new PersonItemModel(null);
        this.selectW = null;
        this.selectF011 = null;
        this.selectVPOLIS = null;
        this.ErrorModelList = []
    }

    //#region Getter Setter
    _selectW: SprWItemModel = null;
    get selectW(): SprWItemModel | null {
        return this._selectW;
    }
    set selectW(value: SprWItemModel | null) {
        this._selectW = value;
        this.model.W = value === null ? null : value.ID;
    }

    _selectF011: SprF011ItemModel = null;
    get selectF011(): SprF011ItemModel | null {
        return this._selectF011;
    }
    set selectF011(value: SprF011ItemModel | null) {
        this._selectF011 = value;
        this.model.DOC_TYPE = value === null ? "" : value.ID;
        this.SetMaskNDoc(this.model.DOC_TYPE);
    }

    _selectVPOLIS: SprVPOLISItemModel = null;
    get selectVPOLIS(): SprVPOLISItemModel | null {
        return this._selectVPOLIS;
    }
    set selectVPOLIS(value: SprVPOLISItemModel | null) {
        this._selectVPOLIS = value;
        this.model.VPOLIS = value === null ? null : value.ID;
        this.SetMaskNPolis(this.model.VPOLIS);
    }

    //#endregion
    //#region Mask
    maskNPOLIS: string = "";
    private SetMaskNPolis(VPOLIS: number): void {
        let mask = "";
        if (VPOLIS === 2) {
            mask = "999999999";
            var regExpNPOLIS = new RegExp("^\\d{9}$");
            if (!regExpNPOLIS.test(this.model.NPOLIS)) {
                this.model.NPOLIS = '';
            }
        }
        if (VPOLIS === 4) {
            mask = "9999999999999999";
            var regExpNPOLIS = new RegExp("^\\d{16}$");
            if (!regExpNPOLIS.test(this.model.NPOLIS)) {
                this.model.NPOLIS = '';
            }
        }
        this.maskNPOLIS = mask;
    }

    maskDOC_S: string = "";
    maskDOC_N: string = "";
    private SetMaskNDoc(DOC_TYPE: string): void {
        let maskDOC_S = "";
        let maskDOC_N = "";
        if (DOC_TYPE === "14") {
            maskDOC_N = "999999";
            maskDOC_S = "99 99";

            var regExpDOC_S = new RegExp("^\\d{2} \\d{2}$");
            if (!regExpDOC_S.test(this.model.DOC_SER)) {
                this.model.DOC_SER = '';
            }
            var regExpDOC_N = new RegExp("^\\d{6}$");
            if (!regExpDOC_N.test(this.model.DOC_NUM)) {
                this.model.DOC_NUM = '';
            }
        }
        this.maskDOC_S = maskDOC_S;
        this.maskDOC_N = maskDOC_N;
    }
    //#endregion

    private ClearSPR() {
        this.SPR = new SPRModel(null);
    }
    ErrorModelList: string[] = [];

    async SaveModel() {
        try {
            this.isLoad = true;
            let result: ResultOperation;
            if (this.mode === EditMode.Edit) {
                result = await this.repo.UpdatePerson(this.model);
            }
            else {
                result = await this.repo.AddPerson(this.model);
            }
            if (result.Result) {
                this.display = false;
                this.onUpdateData?.emit();
            }
            else {
                this.ErrorModelList = result.Error;
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoad = false;
        }
    }

    async GetModel(CS_LIST_IN_ID: number) {
        var item = await this.repo.GetPerson(CS_LIST_IN_ID);
        this.selectW = this.SPR.W.find(x => x.ID == item.W);
        this.selectF011 = this.SPR.F011.find(x => x.ID == item.DOC_TYPE);
        this.selectVPOLIS = this.SPR.VPOLIS.find(x => x.ID == item.VPOLIS);
        this.cdr.detectChanges();
        this.model = item;
    }

}




export enum EditMode {
    New = 1,
    Edit = 2,
    Double = 3
}

