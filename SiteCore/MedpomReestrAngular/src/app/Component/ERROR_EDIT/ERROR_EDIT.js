var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, Input, EventEmitter } from "@angular/core";
import { ErrorSpr } from '../../API/ErrorSPRModel';
import * as ClassicEditor from '../../ClassicEditor/ckeditor.js';
let ERROR_EDIT = class ERROR_EDIT {
    constructor(repo) {
        this.repo = repo;
        this.model = new ErrorSpr(null);
        this.Editor = ClassicEditor;
        this._display = false;
        this.displayChange = new EventEmitter();
        this.onSave = new EventEmitter();
        this.ReadOnly = true;
        this.SectionSpr = [];
        this.SectionSprFiltered = [];
        this.SelectedSection = null;
        this.isLoad = false;
        this.getModel = async () => {
            try {
                if (this.ID_ERR === null) {
                    this.model = new ErrorSpr(null);
                    return;
                }
                this.isLoad = true;
                this.model = await this.repo.getError(this.ID_ERR);
                this.SelectedSection = this.SectionSpr.find(x => x.ID_SECTION === this.model.ID_SECTION);
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.FillSpr = async () => {
            try {
                this.isLoad = true;
                this.SectionSpr = await this.repo.getSections();
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.errorList = [];
        this.SaveError = async () => {
            try {
                this.isLoad = true;
                if (this.SelectedSection != null) {
                    this.model.ID_SECTION = this.SelectedSection.ID_SECTION;
                }
                let result;
                if (this.ID_ERR === null) {
                    result = await this.repo.AddErrorSPR(this.model);
                }
                else {
                    result = await this.repo.EditErrorSPR(this.model);
                }
                this.errorList = result;
                if (result.length === 0) {
                    this.onSave.emit(true);
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
    }
    get display() {
        return this._display;
    }
    set display(value) {
        if (this._display !== value)
            this.displayChange.emit(value);
        this._display = value;
        if (value) {
            this.FillSpr();
            this.getModel();
        }
    }
    onReady(editor) {
        const toolbarElement = editor.ui.view.toolbar.element;
        editor.on('change:isReadOnly', (evt, propertyName, isReadOnly) => {
            if (isReadOnly) {
                toolbarElement.style.display = 'none';
            }
            else {
                toolbarElement.style.display = 'flex';
            }
        });
        if (editor.isReadOnly) {
            toolbarElement.style.display = 'none';
        }
    }
    filterSectionSpr(event) {
        const filtered = [];
        const query = event.query;
        for (let i = 0; i < this.SectionSpr.length; i++) {
            const item = this.SectionSpr[i];
            if (item.SECTION_NAME.toLowerCase().indexOf(query.toLowerCase()) === 0) {
                filtered.push(item);
            }
        }
        this.SectionSprFiltered = filtered;
    }
};
__decorate([
    Input()
], ERROR_EDIT.prototype, "display", null);
__decorate([
    Output()
], ERROR_EDIT.prototype, "displayChange", void 0);
__decorate([
    Output()
], ERROR_EDIT.prototype, "onSave", void 0);
__decorate([
    Input()
], ERROR_EDIT.prototype, "ID_ERR", void 0);
__decorate([
    Input()
], ERROR_EDIT.prototype, "ReadOnly", void 0);
ERROR_EDIT = __decorate([
    Component({ selector: "ERROR_EDIT", templateUrl: "ERROR_EDIT.html", styleUrls: ["ERROR_EDIT.css"] })
], ERROR_EDIT);
export { ERROR_EDIT };
//# sourceMappingURL=ERROR_EDIT.js.map