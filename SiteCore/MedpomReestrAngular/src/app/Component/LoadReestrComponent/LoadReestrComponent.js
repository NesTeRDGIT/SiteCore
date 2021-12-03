var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Output, EventEmitter, ViewChild } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { LoadReestViewModel, ErrorTypeEnum, STATUS_FILE, TYPEFILE, ErrorItem } from "../../API/LoadReestViewModel";
import { InstructionDialog } from '../../Component/InstructionDialog/InstructionDialog';
let LoadReestrComponent = class LoadReestrComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.onSend = new EventEmitter();
        this.model = new LoadReestViewModel(null);
        this.ErrorTypeEnum = ErrorTypeEnum;
        this.STATUS_FILE = STATUS_FILE;
        this.TYPEFILE = TYPEFILE;
        this.getModel = async () => {
            try {
                this.isLoad = true;
                this.model = await this.repo.getLoadReestViewModelAsync();
            }
            catch (err) {
                this.addError(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.LoadFiles = async (items) => {
            try {
                this.clearError();
                this.ProgressValue = 0;
                this.ShowProgress = true;
                const list = [];
                let count = 0;
                for (let i = 0; i < items.length; i++) {
                    let ext = items[i].name.substr(items[i].name.lastIndexOf('.') + 1);
                    ext = ext.toUpperCase();
                    if (ext !== "ZIP" && ext !== "XML") {
                        alert(`Файл ${items[i].name} имеет не допустимый формат`);
                        continue;
                    }
                    list.push(items[i]);
                    count++;
                }
                if (count !== 0) {
                    this.ShowProgress = true;
                    const result = await this.repo.SendFiles(list, this.onProgressLoadFile);
                    this.ListError = result;
                    this.getModel();
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.ShowProgress = false;
            }
        };
        this.ShowProgress = false;
        this.ProgressValue = 0;
        this.onProgressLoadFile = (progress) => {
            this.ProgressValue = Math.round(progress.loaded / progress.total * 100);
        };
        this.ClearProcess = false;
        this.ClearList = async () => {
            try {
                if (!confirm("Вы уверены что хотите очистить список?"))
                    return;
                this.clearError();
                this.ClearProcess = true;
                await this.repo.ClearFiles();
            }
            catch (err) {
                this.addError(err.toString());
            }
            finally {
                await this.getModel();
                this.ClearProcess = false;
            }
        };
        this.RemoveProcess = false;
        this.RemoveFile = async (file) => {
            try {
                if (!confirm("Вы уверены что хотите удалить файл?"))
                    return;
                this.clearError();
                this.RemoveProcess = true;
                await this.repo.DeleteFile(file.ID);
            }
            catch (err) {
                this.addError(err.toString());
            }
            finally {
                await this.getModel();
                this.RemoveProcess = false;
            }
        };
        this.SendToServiceProcess = false;
        this.SendToService = async () => {
            try {
                this.SendToServiceProcess = true;
                if (!confirm("Вы уверены что хотите отправить файлы?"))
                    return;
                this.clearError();
                this.RemoveProcess = true;
                const result = await this.repo.Send();
                if (result.length === 0) {
                    this.onSend.emit();
                }
                else {
                    this.ListError = result;
                }
            }
            catch (err) {
                this.addError(err.toString());
            }
            finally {
                this.SendToServiceProcess = false;
            }
        };
        this.ListError = [];
        this.ShowInstruction = () => {
            this.instructionDialog.ShowDialog();
        };
        this.getModel();
    }
    addError(message) {
        const error = new ErrorItem(null);
        error.Error = message;
        error.ErrorT = ErrorTypeEnum.Error;
        this.ListError.push(error);
    }
    clearError() {
        this.ListError = [];
    }
    addMessage(message) {
        const mes = new ErrorItem(null);
        mes.Error = message;
        mes.ErrorT = ErrorTypeEnum.Text;
        this.ListError.push(mes);
    }
};
__decorate([
    Output()
], LoadReestrComponent.prototype, "onSend", void 0);
__decorate([
    ViewChild(InstructionDialog)
], LoadReestrComponent.prototype, "instructionDialog", void 0);
LoadReestrComponent = __decorate([
    Component({ selector: "load-reestr", templateUrl: "LoadReestrComponent.html" })
], LoadReestrComponent);
export { LoadReestrComponent };
//# sourceMappingURL=LoadReestrComponent.js.map