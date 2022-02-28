
import { Component, ViewChild, ElementRef, AfterViewInit, Input,Output, EventEmitter } from "@angular/core";
import { IRepository,CustomResult } from "../../API/Repository";
import { SPRModel} from "../../API/SPRModel";
import { TMKItem,StatusTMKRow,ExpType, Expertize } from "../../API/TMKItem";
import { UserINFO } from "../../API/UserINFO";
import {ExpertizeEditComponent} from "../../Component/ExpertizeEdit/ExpertizeEditComponent"

import { FindPacientComponent } from "../FindPacient/FindPacient";
import { FindExpertizeComponent } from "../FindExpertize/FindExpertize";
import { FindPacientSelected } from "../../API/FindPacientModel";
import {FindExpertizeModel} from '../../API/FindExpertizeModel';
@Component({ selector: "EditTMK", templateUrl: "EditTMKComponent.html", styleUrls:["EditTMKComponent.scss"]})
export class EditTMKComponent  {
    @Input() userInfo:UserINFO;
    @Output() onChange: EventEmitter<number> = new EventEmitter();
    ExpType = ExpType;
    EditTMKType = EditTMKType;
    TypeOpen:EditTMKType = EditTMKType.New;
    StatusTMKRow = StatusTMKRow;
    ErrMessage:string[]= [];
    CurrentTMK:TMKItem = new TMKItem(null);
    onLoading:boolean = true;
    SPR:SPRModel = new SPRModel(this.repo);
    Display: boolean = false;
    IsReadOnly:boolean = true;
    constructor(public repo: IRepository) {

    }
   
    async FILL_SPR() {
        try {
            await this.SPR.refreshStaticSPR(false);
            await this.SPR.refreshVariableSPR(false);
        }
        catch (err) {
            alert(`Ошибка получение справочников: ${err.toString()}`);
        }
    }

    
    onOperationProgress: boolean = false;
    onSaveProgress:boolean = false;
    Save = async () => {
        try {
            this.onOperationProgress = this.onSaveProgress = true;
            let result: CustomResult;
            if (this.TypeOpen == EditTMKType.New) {
                result = await this.repo.AddTMKItemAsync(this.CurrentTMK);
            } else {
                result = await this.repo.EditTMKItemAsync(this.CurrentTMK);
            }            
            if (result.Result) {
                this.onChange.emit(null);
                this.Close();
            }
            else {
                this.ErrMessage = result.ErrMessage;
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onOperationProgress = this.onSaveProgress = false;
        }
    }

    onDeleteProgress:boolean = false;
    Delete = async () => {
        try {
            if (confirm("Вы уверены, что хотите удалить запись?")) {
                this.onOperationProgress = this.onDeleteProgress = true;
                let result = await this.repo.DeleteTMKItemAsync([this.CurrentTMK]);
                if (result.Result) {
                    this.onChange.emit(null);
                    this.Close();
                }
                else {
                    this.ErrMessage = result.ErrMessage;
                }
            }

        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onOperationProgress = this.onDeleteProgress = false;
        }
    }

    onSetAsMTRProgress:boolean = false;
    SetAsMTR = async () => {
        try {
            if (confirm("Вы уверены, что хотите отметить запись как МТР?")) {
                this.onOperationProgress = this.onSetAsMTRProgress = true;
                let result = await this.repo.SetAsMtrTMKItemAsync(this.CurrentTMK);
                if (result.Result) {
                    this.CurrentTMK = await this.repo.getTMKItemAsync(this.CurrentTMK.TMK_ID);
                    this.onChange.emit(this.CurrentTMK.TMK_ID);
                }
                else {
                    this.ErrMessage = result.ErrMessage;
                }
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onOperationProgress = this.onSetAsMTRProgress= false;
        }
    }

   
    @ViewChild(ExpertizeEditComponent) ExpertizeEditWin: ExpertizeEditComponent;
    AddExpertize = async (type:ExpType, exp:FindExpertizeModel = null) => {
        try {
            this.ExpertizeEditWin.ShowCreateNewExpertize(type, this.CurrentTMK.TMK_ID,exp);
            this.onChange.emit(this.CurrentTMK.TMK_ID);
        }
        catch (err) {
            alert(err.toString());
        }
    }

    EditExpertize = async (item: Expertize) => {
        try {
            this.ExpertizeEditWin.EditCreateNewExpertize(item);
            this.onChange.emit(this.CurrentTMK.TMK_ID);
        }
        catch (err) {
            alert(err.toString());
        }
    }

    DelExpertize = async (item: Expertize) => {
        try {
            if (confirm("Вы уверены, что хотите удалить экспертизу'?")) {
                let result = await this.repo.DeleteExpertizeAsync([item]);
                if (result) {
                    this.refreshModel();
                    this.onChange.emit(this.CurrentTMK.TMK_ID);
                }
                else {
                    alert(result.Error);
                }
            }
        }
        catch (err) {
            alert(err.toString());
        }
    }

    ConvertToDate(obj: any): Date {
        if(obj)
            return new Date(obj);
        return null;
    }

    public async Show(TMK_ID: number) {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.Read;
            this.IsReadOnly = true;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }

    public async Edit(TMK_ID: number) {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.Edit;
            this.IsReadOnly = false;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }
    public async New() {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.New;
            this.IsReadOnly = false;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = new TMKItem(null);
            this.CurrentTMK.CODE_MO = this.userInfo.CodeMO;
            this.CurrentTMK.DATE_EDIT = this.CurrentTMK.DATE_INVITE = new Date();
            this.CurrentTMK.STATUS = StatusTMKRow.Open;
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }

    public Close() {
        this.Display = false;
        this.CurrentTMK = new TMKItem(null);
        this.ErrMessage = [];
        this.FindPacientPanel?.Clear(true);
        this.FindExpertizePanel?.Clear(true);
    }


    IsTFOMS = (): boolean => {
        return this.CurrentTMK.SMO == "75";
    }

    refreshModel = async () => {
        this.CurrentTMK = await this.repo.getTMKItemAsync(this.CurrentTMK.TMK_ID);
    }

    @ViewChild(FindPacientComponent) FindPacientPanel: FindPacientComponent;
    FindPacient = async () => {
        this.FindPacientPanel.FindPacient(this.CurrentTMK.ENP);
    }


    FindPacient_onChangeSelected(item: FindPacientSelected) {
        setTimeout(()=>{this.CurrentTMK.SetAuto(item.Pacient, item.isPred)});
     
    }


    ENP_KeyDown(event: any) {
        if (event.key === "Enter" && !this.IsReadOnly) {
            event.preventDefault();
            this.FindPacient();
        }
    }


    @ViewChild(FindExpertizeComponent)  FindExpertizePanel: FindExpertizeComponent;
    FindExpertize = async () => {
        this.FindExpertizePanel.FindExpertize(this.CurrentTMK.TMK_ID);
    }


    AddFindExpertize(exp: FindExpertizeModel) {
        if (exp != null) {
            this.AddExpertize(exp.S_TIP, exp);
        }
    }
}

enum EditTMKType {
    New = 0,
    Edit = 1,
    Read = 2
}
