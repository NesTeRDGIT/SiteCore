
import { Component, ViewChild, ElementRef, AfterViewInit, Input,Output, EventEmitter } from "@angular/core";
import { IRepository,CustomResult } from "../../API/Repository";
import { SPRModel} from "../../API/SPRModel";
import { TMKItem,StatusTMKRow,ExpType, Expertize } from "../../API/TMKItem";
import { UserINFO } from "../../API/UserINFO";
import {ExpertizeEditComponent} from "../../Component/ExpertizeEdit/ExpertizeEditComponent"


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
            this.onLoading = true;
            await this.SPR.refreshStaticSPR(false);
            await this.SPR.refreshVariableSPR(false);
            this.onLoading = false;
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
    AddExpertize = async (type:ExpType) => {
        try {
            this.ExpertizeEditWin.ShowCreateNewExpertize(type, this.CurrentTMK.TMK_ID);
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
        this.TypeOpen = EditTMKType.Read;
        this.IsReadOnly = true;
        this.Display = true;
        await this.FILL_SPR();
        this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);
    }

    public async Edit(TMK_ID: number) {
        this.TypeOpen = EditTMKType.Edit;
        this.IsReadOnly = false;      
        this.Display = true;
        await this.FILL_SPR();
        this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);        
    }
    public async New() {
        this.TypeOpen = EditTMKType.New;
        this.IsReadOnly = false; 
        this.Display = true;
        await this.FILL_SPR();
        this.CurrentTMK = new TMKItem(null);      
        this.CurrentTMK.DATE_EDIT = this.CurrentTMK.DATE_INVITE = new Date();
        this.CurrentTMK.STATUS = StatusTMKRow.Open;  
    }

    public Close() {
        this.Display = false;
        this.CurrentTMK = new TMKItem(null);
        this.ErrMessage = [];
    }


    IsTFOMS = (): boolean => {
        return this.CurrentTMK.SMO == "75";
    }

    refreshModel= async ()=>
    {
        this.CurrentTMK = await this.repo.getTMKItemAsync(this.CurrentTMK.TMK_ID);
    }
}

enum EditTMKType {
    New = 0,
    Edit = 1,
    Read = 2
}
