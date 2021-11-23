var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, Input, EventEmitter } from "@angular/core";
let InstructionDialog = class InstructionDialog {
    constructor() {
        this._display = false;
        this.displayChange = new EventEmitter();
    }
    get display() {
        return this._display;
    }
    set display(value) {
        if (this._display !== value)
            this.displayChange.emit(value);
        this._display = value;
    }
};
__decorate([
    Input()
], InstructionDialog.prototype, "display", null);
__decorate([
    Output()
], InstructionDialog.prototype, "displayChange", void 0);
InstructionDialog = __decorate([
    Component({ selector: "Instruction-Dialog", templateUrl: "InstructionDialog.html" })
], InstructionDialog);
export { InstructionDialog };
//# sourceMappingURL=InstructionDialog.js.map