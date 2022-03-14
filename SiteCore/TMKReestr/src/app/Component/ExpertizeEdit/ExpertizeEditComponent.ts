
import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit, Output,EventEmitter } from "@angular/core";
import { FindExpertizeModel } from "../../API/FindExpertizeModel";


import { IRepository } from "../../API/Repository";
import { SPRModel } from "../../API/SPRModel";
import { Expertize,ExpertiseOSN,ExpType } from "../../API/TMKItem";

@Component({ selector: "ExpertizeEdit", templateUrl: "ExpertizeEditComponent.html", styleUrls:['ExpertizeEditComponent.scss'] })
export class ExpertizeEditComponent {
    @Input() CurrentExpertize: Expertize = new Expertize(null);   
    @Input() SPR: SPRModel;
    @Output() onChange:EventEmitter<any> = new EventEmitter(); 
    ExpTypeEnum = ExpType;    
    OpenType:ExpertizeEditType = ExpertizeEditType.New;

    Display:boolean = false;

    ErrList:string[]=[];
    constructor(public repo: IRepository) {

    }

    ConvertToDate(obj: any): Date {
        if(obj)
        return new Date(obj);
    return null;
    }

    Close(): void {
         this.ErrList = [];
         this.Display = false;
         this.CurrentExpertize = new Expertize(null);
    }

    ShowCreateNewExpertize(type: ExpType, TMK_ID: number, exp:FindExpertizeModel = null) {
        this.CurrentExpertize = new Expertize(null);
        this.CurrentExpertize.S_TIP = type;
        if (exp != null) {
            this.CurrentExpertize.DATEACT = exp.D_ACT;
            this.CurrentExpertize.NUMACT = exp.N_ACT;          
            this.CurrentExpertize.N_EXP = exp.N_EXP;
            exp.OSN.forEach(osn => {
                let o = new ExpertiseOSN(null);
                o.S_OSN = osn.S_OSN;
                o.S_FINE = osn.S_FINE;
                o.S_SUM = osn.S_SUM;
                this.CurrentExpertize.OSN.push(o);
            });
        }
        this.CurrentExpertize.TMK_ID = TMK_ID;
        this.Display = true;
    }

    EditCreateNewExpertize(exp: Expertize) {
        this.CurrentExpertize = new Expertize(exp);    
        this.Display = true;
    }

    AddOsn = () => {
        this.CurrentExpertize.OSN.push(new ExpertiseOSN(null));
    }

    RemoveOsn = (osn: ExpertiseOSN) => {
        const index = this.CurrentExpertize.OSN.indexOf(osn);
        if (index > -1) {
            this.CurrentExpertize.OSN.splice(index, 1);
        }
    }

    onSaveProgress:boolean = false;
    Save = async () => {
        try {
            this.onSaveProgress = true;
            let result = await this.repo.EditExpertizeAsync(this.CurrentExpertize);
            if (result.Result) {
                this.onChange.emit();
                this.Close();
            }
            else {
                this.ErrList = result.ErrMessage;
            }           
        }
        catch (err) {
            alert(err.toString());
        }
        finally{
            this.onSaveProgress = false;
        }
    }
  
}


enum ExpertizeEditType {
    New = 0,
    Edit = 1,
    Read = 2
}
