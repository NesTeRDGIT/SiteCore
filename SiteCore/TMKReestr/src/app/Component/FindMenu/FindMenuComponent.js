var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, EventEmitter } from "@angular/core";
import { SPRModel } from "../../API/SPRModel";
import { TMKFilter } from "../../API/TMKFilter";
let FindMenuComponent = class FindMenuComponent {
    constructor(repo) {
        this.repo = repo;
        this.onSearch = new EventEmitter();
        this.SPR = new SPRModel(this.repo);
        this.Filter = new TMKFilter();
        this.onLoading = true;
        this.ShowEmptyFilter = false;
        //#region Filter Select
        this._CODE_MOSelect = [];
        this.CODE_MOFiltered = [];
        this._SMOSelect = [];
        this.SMOFiltered = [];
        this._NMIC_VID_NHISTORYSelect = [];
        this.NMIC_VID_NHISTORYFiltered = [];
        this.NMIC_OPLATAFiltered = [];
    }
    ngOnInit() {
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
        var _a;
        this.SMOSelect = [];
        this.CODE_MOSelect = [];
        this.NMIC_VID_NHISTORYSelect = [];
        this.NMIC_OPLATASelect = [];
        this.Filter.Clear();
        (_a = this.onSearch) === null || _a === void 0 ? void 0 : _a.emit(this.Filter);
    }
    Find() {
        var _a;
        (_a = this.onSearch) === null || _a === void 0 ? void 0 : _a.emit(this.Filter);
    }
    ConvertToDate(obj) {
        if (obj != null && obj != '')
            return new Date(obj);
        return null;
    }
    get CODE_MOSelect() {
        return this._CODE_MOSelect;
    }
    set CODE_MOSelect(val) {
        this.Filter.CODE_MO = val.map(x => x.MCOD);
        this._CODE_MOSelect = val;
    }
    searchMO(event) {
        this.CODE_MOFiltered = this.SPR.CODE_MO_Reestr.SPR.values().filter(x => x.MCOD.indexOf(event.query) != -1 || x.NAM_MOK.indexOf(event.query) != -1);
    }
    get SMOSelect() {
        return this._SMOSelect;
    }
    set SMOSelect(val) {
        this.Filter.SMO = val.map(x => x.SMOCOD);
        this._SMOSelect = val;
    }
    searchSMO(event) {
        this.SMOFiltered = this.SPR.CODE_SMO_Reestr.SPR.values().filter(x => x.SMOCOD.indexOf(event.query) != -1 || x.NAM_SMOK.indexOf(event.query) != -1);
    }
    get NMIC_VID_NHISTORYSelect() {
        return this._NMIC_VID_NHISTORYSelect;
    }
    set NMIC_VID_NHISTORYSelect(val) {
        this.Filter.VID_NHISTORY = val.map(x => x.ID_VID_NHISTORY);
        this._NMIC_VID_NHISTORYSelect = val;
    }
    searchNMIC_VID_NHISTORY(event) {
        this.NMIC_VID_NHISTORYFiltered = this.SPR.NMIC_VID_NHISTORY.SPR.values().filter(x => x.VID_NHISTORY.indexOf(event.query) != -1);
    }
    get NMIC_OPLATASelect() {
        return this._NMIC_OPLATASelect;
    }
    set NMIC_OPLATASelect(val) {
        this.Filter.OPLATA = val.map(x => x.ID_OPLATA);
        this._NMIC_OPLATASelect = val;
    }
    searchNMIC_OPLATA(event) {
        this.NMIC_OPLATAFiltered = this.SPR.NMIC_OPLATA.SPR.values().filter(x => x.OPLATA.indexOf(event.query) != -1);
    }
};
__decorate([
    Output()
], FindMenuComponent.prototype, "onSearch", void 0);
FindMenuComponent = __decorate([
    Component({ selector: "FindMenu", templateUrl: "FindMenuComponent.html" })
], FindMenuComponent);
export { FindMenuComponent };
//# sourceMappingURL=FindMenuComponent.js.map