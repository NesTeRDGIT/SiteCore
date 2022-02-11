
import { Component, ViewChild, ElementRef, AfterViewInit, Input,Output, EventEmitter } from "@angular/core";
import { IRepository,CustomResult } from "../../API/Repository";
import { SPRModel} from "../../API/SPRModel";
import { SPRContactModel } from "../../API/SPRContactModel";
import { UserINFO } from "../../API/UserINFO";



@Component({ selector: "SPRContactEdit", templateUrl: "SPRContactEdit.html"})
export class SPRContactEditComponent  {
    @Input() userInfo:UserINFO;
    @Output() onChange: EventEmitter<number> = new EventEmitter();
    @Input() SPR:SPRModel;
    onLoading:boolean = true;
    model:SPRContactModel = new SPRContactModel(null);
    Display:boolean =false;
    ErrMessage:string[]  = [];
    constructor(public repo: IRepository) {

    }
   
    async FILL_SPR() {
        try {
            this.onLoading = true;
            await this.SPR.refreshCODE_MOAsync(false);
            this.onLoading = false;
        }
        catch (err) {
            alert(`Ошибка получение справочников: ${err.toString()}`);
        }
    }

    Close = () => {
        this.Display = false;
        this.model = new SPRContactModel(null);
        this.ErrMessage = [];
    }

    
 
    onSaveProgress: boolean = false;
    Save = async () => {
        try {
             this.onSaveProgress = true;
            let result = await this.repo.EditSPRContactAsync(this.model);
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
            this.onSaveProgress = false;
        }
    }


    public async Edit(item: SPRContactModel) {    
        this.Display = true;
        await this.FILL_SPR();  
        this.model = new SPRContactModel(item);
    }

    public async New() {
        this.Display = true;
        await this.FILL_SPR();  
        this.model = new SPRContactModel(null);
        this.model.CODE_MO = this.userInfo.CodeMO;
    }
}
