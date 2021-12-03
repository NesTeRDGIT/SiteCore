var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input } from "@angular/core";
import { TypeEntries } from 'src/app/API/LogEntries';
let CSServiceStatus = class CSServiceStatus {
    constructor(repo) {
        this.repo = repo;
        this.isEnabled = null;
        this.AdminMode = false;
        this.TypeEntries = TypeEntries;
        this.isLoad = false;
        this._isDisplayLogDialog = false;
        this.isLoadLog = false;
        this.Log = [];
    }
    ngOnInit() {
        this.items = [
            { label: 'Обновить', icon: 'pi pi-refresh', command: () => { this.updateStatus(); } }
        ];
        if (this.AdminMode)
            this.items.push({ label: 'Показать лог', command: () => { this.showLog(); } });
        this.updateStatus();
    }
    async updateStatus() {
        try {
            this.isLoad = true;
            this.isEnabled = null;
            this.isEnabled = await this.repo.GetServiceState();
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoad = false;
        }
    }
    get isDisplayLogDialog() {
        return this._isDisplayLogDialog;
    }
    set isDisplayLogDialog(value) {
        this._isDisplayLogDialog = value;
        if (value === false)
            this.Log = [];
    }
    async showLog() {
        try {
            this.isLoadLog = true;
            this.isDisplayLogDialog = true;
            this.Log = await this.repo.GetLogService();
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoadLog = false;
        }
    }
};
__decorate([
    Input()
], CSServiceStatus.prototype, "AdminMode", void 0);
CSServiceStatus = __decorate([
    Component({ selector: "CSServiceStatus", templateUrl: "CSServiceStatus.html", styleUrls: ["CSServiceStatus.css", "CSServiceStatus.scss"] })
], CSServiceStatus);
export { CSServiceStatus };
//# sourceMappingURL=CSServiceStatus.js.map