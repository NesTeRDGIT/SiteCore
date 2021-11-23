import { Component, Output, EventEmitter, OnChanges, SimpleChanges } from "@angular/core";


@Component({ selector: 'base-report', template: `<div></div>` })
export class BaseReportComponent {
    @Output() onLoading = new EventEmitter<Boolean>();
    _isLoad: boolean;

    get isLoad() {
        return this._isLoad;
    }

    set isLoad(val) {
        this._isLoad = val;
        this.onLoading.emit(this.isLoad);
    }
}