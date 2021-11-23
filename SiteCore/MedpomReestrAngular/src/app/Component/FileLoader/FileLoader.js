var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, Input, EventEmitter, ViewChild } from "@angular/core";
let FileLoader = class FileLoader {
    constructor() {
        this.onSelect = new EventEmitter();
        this.DragOverFileDialog = (e) => {
            this.isDrag = true;
            e.preventDefault();
            e.stopPropagation();
        };
        this.DragEnterFileDialog = (e) => {
            e.preventDefault();
            e.stopPropagation();
        };
        this.DragLeaveFileDialog = (e) => {
            this.isDrag = false;
        };
        this.DropFileDialog = (e) => {
            this.isDrag = false;
            if (e.dataTransfer) {
                if (e.dataTransfer.files.length) {
                    e.preventDefault();
                    e.stopPropagation();
                    this.onSelect.emit(e.dataTransfer.files);
                }
            }
        };
        this.ShowFileDialog = () => {
            const el = this.fileControl.nativeElement;
            el.click();
        };
        this.onChange = (e) => {
            const target = event.target;
            if (target.files.length) {
                this.onSelect.emit(target.files);
                target.value = "";
            }
        };
    }
};
__decorate([
    ViewChild('fileControl')
], FileLoader.prototype, "fileControl", void 0);
__decorate([
    Output()
], FileLoader.prototype, "onSelect", void 0);
__decorate([
    Input()
], FileLoader.prototype, "filter", void 0);
FileLoader = __decorate([
    Component({ selector: "File-Loader", templateUrl: "FileLoader.html", styleUrls: ['./FileLoader.css'] })
], FileLoader);
export { FileLoader };
//# sourceMappingURL=FileLoader.js.map