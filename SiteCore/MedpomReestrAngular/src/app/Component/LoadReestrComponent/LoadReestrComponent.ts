import { Component, Output, EventEmitter, OnChanges, SimpleChanges} from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";

import { FileAPI } from "../../API/FileAPI";
import { LoadReestViewModel, ErrorTypeEnum, STATUS_FILE, TYPEFILE, ErrorItem, FileItemBase } from "../../API/LoadReestViewModel";


@Component({ selector: "load-reestr", templateUrl: "LoadReestrComponent.html" })
export class LoadReestrComponent extends BaseReportComponent  {
    @Output() onSend = new EventEmitter();


    model: LoadReestViewModel = new LoadReestViewModel(null);
    ErrorTypeEnum = ErrorTypeEnum;
    STATUS_FILE = STATUS_FILE;
    TYPEFILE = TYPEFILE;
    constructor(public repo: IRepository) {
        super();
        this.getModel();
    }

    getModel = async () => {
        try {
            this.isLoad = true;
            this.model = await this.repo.getLoadReestViewModelAsync();
        } catch (err) {
            this.addError(err.toString());
        } finally {
            this.isLoad = false;
        }
    }

    LoadFiles = async (items: FileList) => {
        try {
           this.clearError();
            this.ProgressValue = 0;
            this.ShowProgress = true;
            const list: File[] = [];
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

        } catch (err) {
            alert(err.toString());
        } finally {
            this.ShowProgress = false;
        }
    }

    ShowProgress = false;
    ProgressValue = 0;
    onProgressLoadFile = (progress:ProgressEvent) => {
        this.ProgressValue = Math.round(progress.loaded / progress.total * 100);
    }


    ClearProcess= false;
    ClearList = async () => {
        try {
            if (!confirm("Вы уверены что хотите очистить список?"))
                return;
            this.clearError();
            this.ClearProcess = true;
            await this.repo.ClearFiles();
        } catch (err) {
            this.addError(err.toString());
        } finally {
            await this.getModel();
            this.ClearProcess = false;
        }
    }
    RemoveProcess = false;
    RemoveFile = async (file: FileItemBase) => {
        try {
            if (!confirm("Вы уверены что хотите удалить файл?"))
                return;
            this.clearError();
            this.RemoveProcess = true;
            await this.repo.DeleteFile(file.ID);
        } catch (err) {
            this.addError(err.toString());
        } finally {
            await this.getModel();
            this.RemoveProcess = false;
        }
    }


    SendToServiceProcess= false;
    SendToService = async () => {
        try {
            this.SendToServiceProcess = true;
            if (!confirm("Вы уверены что хотите отправить файлы?"))
                return;
            this.clearError();
            this.RemoveProcess = true;
            const result = await this.repo.Send();
            if (result.length === 0) {
                this.onSend.emit();
            } else {
                this.ListError = result;
            }
            
        } catch (err) {
            this.addError(err.toString());
        } finally {
            
            this.SendToServiceProcess = false;
        }
    }



    ListError:ErrorItem[]=[];
    private addError(message: string): void {
        const error = new ErrorItem(null);
        error.Error = message;
        error.ErrorT = ErrorTypeEnum.Error;
        this.ListError.push(error);
    }
    private clearError(): void {
        this.ListError = [];
    }
    private addMessage(message: string): void {
        const mes = new ErrorItem(null);
        mes.Error = message;
        mes.ErrorT = ErrorTypeEnum.Text;
        this.ListError.push(mes);
    }





    displayDialog = false;
    ShowInstruction = () => {
        this.displayDialog = true;
    }
}



