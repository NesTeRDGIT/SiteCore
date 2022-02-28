import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit, Output, EventEmitter } from "@angular/core";
import { MenuItem, LazyLoadEvent, PrimeNGConfig } from 'primeng/api';
import { IRepository } from "../../API/Repository";

import { UserINFO } from "../../API/UserINFO";
import { FindExpertizeModel } from "../../API/FindExpertizeModel";
import { SPRModel } from "../../API/SPRModel";
@Component({ selector: "FindExpertize", templateUrl: "FindExpertize.html" })
export class FindExpertizeComponent {
    @Input() userInfo:UserINFO;
    @Input() SPR:SPRModel;
    @Output() AddExpertize = new EventEmitter<FindExpertizeModel>();
    isResult:boolean = null;
    ShowElement = false;

  
  
    Expertizes: FindExpertizeModel[] = [];
    
    onLoading:boolean = false;

  
   
  
    constructor(public repo: IRepository) {        
        
    }


    public Clear(isHide:boolean = false) {
        this.isResult = null;
        if (isHide) {
            this.ShowElement = false;
        }
    }

    
    public FindExpertize = async (TMK_ID: number) => {
        try {
           this.ShowElement = true;
           this.onLoading = true;
           this.Clear();
            this.Expertizes = await this.repo.FindExpertizeAsync(TMK_ID);
            this.isResult = this.Expertizes.length !== 0;
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.onLoading = false;
        }
    }


    RaiseAddExpertize(exp: FindExpertizeModel) {
        this.AddExpertize?.emit(exp);
    }
}




