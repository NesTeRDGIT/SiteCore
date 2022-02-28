var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input } from "@angular/core";
import { ReportParamModel, TMKReportTableModel } from "../../API/ReportModel";
import { SPRModel } from "../../API/SPRModel";
import { FileAPI } from "../../API/FileAPI";
let ReportComponent = class ReportComponent {
    constructor(repo) {
        this.repo = repo;
        this.model = new TMKReportTableModel(null);
        this.param = new ReportParamModel();
        this.loading = false;
        this.FILL_SPR = async () => {
            this.param.Date2 = new Date();
            this.param.Date1 = new Date(this.param.Date2.getFullYear(), 1, 1);
            await this.SPR.refreshNMIC_VID_NHISTORYAsync();
            this.NMIC_VID_NHISTORYSelect = [this.SPR.NMIC_VID_NHISTORY.SPR.getItem("1"), this.SPR.NMIC_VID_NHISTORY.SPR.getItem("4")];
        };
        this.GetData = async () => {
            try {
                this.loading = true;
                this.model = await this.repo.GetReportAsync(this.param);
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.loading = false;
            }
        };
        this.DownloadXLS = async () => {
            try {
                let file = await this.repo.GetReportXLSAsync();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this._NMIC_VID_NHISTORYSelect = [];
        this.NMIC_VID_NHISTORYFiltered = [];
        this.SPR = new SPRModel(repo);
    }
    ngOnInit() {
        this.FILL_SPR();
    }
    ConvertToDate(obj) {
        if (obj)
            return new Date(obj);
        return null;
    }
    get NMIC_VID_NHISTORYSelect() {
        return this._NMIC_VID_NHISTORYSelect;
    }
    set NMIC_VID_NHISTORYSelect(val) {
        this.param.VID_NHISTORY = val.map(x => x.ID_VID_NHISTORY);
        this._NMIC_VID_NHISTORYSelect = val;
    }
    searchNMIC_VID_NHISTORY(event) {
        this.NMIC_VID_NHISTORYFiltered = this.SPR.NMIC_VID_NHISTORY.SPR.values().filter(x => x.FULL_NAME.indexOf(event.query) != -1);
    }
};
__decorate([
    Input()
], ReportComponent.prototype, "userInfo", void 0);
ReportComponent = __decorate([
    Component({ selector: "Report", templateUrl: "Report.html" })
], ReportComponent);
export { ReportComponent };
//# sourceMappingURL=Report.js.map