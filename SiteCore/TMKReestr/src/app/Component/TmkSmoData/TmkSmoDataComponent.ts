
import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit } from "@angular/core";


import { IRepository } from "../../API/Repository";
import { SPRModel } from "../../API/SPRModel";
import { TMKItem } from "../../API/TMKItem";

@Component({ selector: "TmkSmoData", templateUrl: "TmkSmoDataComponent.html" })
export class TmkSmoDataComponent {
    @Input() SPR: SPRModel;
    @Input() IsReadOnly: boolean = true;

    _CurrentTMK: TMKItem;
    get CurrentTMK(): TMKItem {
        return this._CurrentTMK;
    }
    @Input() set CurrentTMK(value: TMKItem) {
        this._CurrentTMK = value;
        this.ClearProgress();
    }


    constructor(public repo: IRepository) {

    }

    onSaveProgress: boolean = false;
    onSaveStatus: boolean = null;
    onSaveComment: string = "";

    Save = async () => {
        try {
            this.ClearProgress();
            this.onSaveProgress = true;
            let result = await this.repo.SaveSmoDataTMKItemAsync(this.CurrentTMK);
            if (result.Result) {
                this.onSaveStatus = true;
                this.onSaveComment = "Данные успешно сохранены"
                setTimeout(() => { this.onSaveStatus = null }, 4000);
            }
            else {
                this.onSaveStatus = false;
                this.onSaveComment = result.Error;
            }

        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onSaveProgress = false;
        }
    }

    ClearProgress = () => {
        this.onSaveProgress = false;
        this.onSaveStatus = null;
        this.onSaveComment = "";
    }
}