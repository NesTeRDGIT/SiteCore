import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { MenuItem, LazyLoadEvent, PrimeNGConfig } from 'primeng/api';
import { IRepository } from "../../API/Repository";

import { UserINFO } from "../../API/UserINFO";
import { FindPacientModel, FindPacientSelected } from "../../API/FindPacientModel";
import { SPRModel } from "../../API/SPRModel";
@Component({ selector: "FindPacient", templateUrl: "FindPacient.html", styleUrls:['FindPacient.scss'] })
export class FindPacientComponent {
    @Input() userInfo:UserINFO;
    @Input() SPR:SPRModel;
    @Output() onChangeSelected = new EventEmitter<FindPacientSelected>();
    isResult:boolean = null;
    ShowElement = false;
    _isPred:boolean = false;
    get isPred():boolean{
        return this._isPred;
    }
    set isPred(val:boolean){
        this._isPred = val;
        this.RaiseSelectChange()
    }    
    _isActive:boolean = false;
    get isActive():boolean{
        return this._isActive;
    }
    set isActive(val:boolean){
        this._isActive = val;
        this.RaiseSelectChange();
    }
    Pacients: FindPacientModel[] = [];
    CurrPacient:FindPacientModel = null;
    onLoading:boolean = false;

    _ActualDt: Date = null;
    @Input() get ActualDt(): Date {
        return this._ActualDt;
    }
    set ActualDt(value: Date) {
        if (value != null) {
            this._ActualDt = new Date(value.getFullYear(), value.getMonth(), value.getDate());
        }
        else {
            this._ActualDt = null;
        }
        this.FindCurrPacient();
    }
   
  
    constructor(public repo: IRepository) {        
        
    }


    public Clear(isHide:boolean = false) {
        this.isResult = null;
        this.CurrPacient = null;
        this._isPred = false;
        this._isActive = false; 
        if (isHide) {
            this.ShowElement = false;
        }     
        this.RaiseSelectChange();
    }

    private FindCurrPacient() {
        this.CurrPacient = null;
        this.Pacients.forEach(p => {           
            if (p.DBEG <= this.ActualDt && (p.DSTOP >= this.ActualDt || p.DSTOP == null) || p.DSTOP == null && this.ActualDt == null) {
                this.CurrPacient = p;
            }
        });
        this.RaiseSelectChange();
    }
    public FindPacient = async (ENP: string) => {
        try {
           this.ShowElement = true;
           this.onLoading = true;
           this.Clear();
            this.Pacients = await this.repo.FindPacientAsync(ENP);
            this.isResult = this.Pacients.length !== 0;
            if (this.isResult) {
                this.isActive = true;
                this.FindCurrPacient();
            } else{
                this.isActive = false;
                this.RaiseSelectChange();
            }            
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onLoading = false;
        }
    }

    private RaiseSelectChange() {       
        this.onChangeSelected?.emit(new FindPacientSelected(this.isPred, this.isActive? this.CurrPacient:null));
    }



}




