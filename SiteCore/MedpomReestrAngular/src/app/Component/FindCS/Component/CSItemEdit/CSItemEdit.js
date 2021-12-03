var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, EventEmitter } from "@angular/core";
import { PersonItemModel, SPRModel } from 'src/app/API/CSModel';
let CSItemEdit = class CSItemEdit {
    constructor(repo, cdr) {
        this.repo = repo;
        this.cdr = cdr;
        this.mode = EditMode.New;
        this.EditMode = EditMode;
        this.currentCS_LIST_IN_ID = null;
        //#region Input Output
        this._display = false;
        this.onUpdateData = new EventEmitter();
        //#endregion
        this.model = new PersonItemModel(null);
        this.SPR = new SPRModel(null);
        this.isLoad = false;
        //#region Getter Setter
        this._selectW = null;
        this._selectF011 = null;
        this._selectVPOLIS = null;
        //#endregion
        //#region Mask
        this.maskNPOLIS = "";
        this.maskDOC_S = "";
        this.maskDOC_N = "";
        this.ErrorModelList = [];
    }
    get display() {
        return this._display;
    }
    set display(value) {
        this._display = value;
        if (!value) {
            this.ClearSPR();
            this.ClearModel();
        }
    }
    async ShowDialog(mode, CS_LIST_IN_ID = null) {
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
                    await this.GetModel(CS_LIST_IN_ID);
                    break;
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoad = false;
        }
    }
    ClearModel() {
        this.currentCS_LIST_IN_ID = null;
        this.model = new PersonItemModel(null);
        this.selectW = null;
        this.selectF011 = null;
        this.selectVPOLIS = null;
        this.ErrorModelList = [];
    }
    get selectW() {
        return this._selectW;
    }
    set selectW(value) {
        this._selectW = value;
        this.model.W = value === null ? null : value.ID;
    }
    get selectF011() {
        return this._selectF011;
    }
    set selectF011(value) {
        this._selectF011 = value;
        this.model.DOC_TYPE = value === null ? "" : value.ID;
        this.SetMaskNDoc(this.model.DOC_TYPE);
    }
    get selectVPOLIS() {
        return this._selectVPOLIS;
    }
    set selectVPOLIS(value) {
        this._selectVPOLIS = value;
        this.model.VPOLIS = value === null ? null : value.ID;
        this.SetMaskNPolis(this.model.VPOLIS);
    }
    SetMaskNPolis(VPOLIS) {
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
    SetMaskNDoc(DOC_TYPE) {
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
    ClearSPR() {
        this.SPR = new SPRModel(null);
    }
    async SaveModel() {
        var _a;
        try {
            this.isLoad = true;
            let result;
            if (this.mode === EditMode.Edit) {
                result = await this.repo.UpdatePerson(this.model);
            }
            else {
                result = await this.repo.AddPerson(this.model);
            }
            if (result.Result) {
                this.display = false;
                (_a = this.onUpdateData) === null || _a === void 0 ? void 0 : _a.emit();
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
    async GetModel(CS_LIST_IN_ID) {
        var item = await this.repo.GetPerson(CS_LIST_IN_ID);
        this.selectW = this.SPR.W.find(x => x.ID == item.W);
        this.selectF011 = this.SPR.F011.find(x => x.ID == item.DOC_TYPE);
        this.selectVPOLIS = this.SPR.VPOLIS.find(x => x.ID == item.VPOLIS);
        this.cdr.detectChanges();
        this.model = item;
    }
};
__decorate([
    Output()
], CSItemEdit.prototype, "onUpdateData", void 0);
CSItemEdit = __decorate([
    Component({ selector: "CSItemEdit", templateUrl: "CSItemEdit.html", styleUrls: ["CSItemEdit.css"] })
], CSItemEdit);
export { CSItemEdit };
export var EditMode;
(function (EditMode) {
    EditMode[EditMode["New"] = 1] = "New";
    EditMode[EditMode["Edit"] = 2] = "Edit";
    EditMode[EditMode["Double"] = 3] = "Double";
})(EditMode || (EditMode = {}));
//# sourceMappingURL=CSItemEdit.js.map