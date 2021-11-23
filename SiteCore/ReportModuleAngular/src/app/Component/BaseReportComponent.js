var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, EventEmitter } from "@angular/core";
let BaseReportComponent = class BaseReportComponent {
    constructor() {
        this.onLoading = new EventEmitter();
    }
    get isLoad() {
        return this._isLoad;
    }
    set isLoad(val) {
        this._isLoad = val;
        this.onLoading.emit(this.isLoad);
    }
};
__decorate([
    Output()
], BaseReportComponent.prototype, "onLoading", void 0);
BaseReportComponent = __decorate([
    Component({ selector: 'base-report', template: `<div></div>` })
], BaseReportComponent);
export { BaseReportComponent };
//# sourceMappingURL=BaseReportComponent.js.map