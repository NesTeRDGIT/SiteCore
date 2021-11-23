var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
let DBStateComponent = class DBStateComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.report = [];
        this.getReport = async () => {
            try {
                this.isLoad = true;
                this.report = await this.repo.getDBState();
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
    }
};
DBStateComponent = __decorate([
    Component({ selector: "DB-State", templateUrl: "DBState.html" })
], DBStateComponent);
export { DBStateComponent };
//# sourceMappingURL=DBState.js.map