import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit } from "@angular/core";
import { IRepository } from "../../API/Repository";
import { UserINFO } from "../../API/UserINFO";
import { URLHelper } from '../../API/URLHelper';

@Component({ selector: "app", templateUrl: "MainComponent.html" })
export class MainComponent implements OnInit {
    userInfo:UserINFO;
    TMKViewType=TMKViewType;
    CurrentTMKViewType:TMKViewType = TMKViewType.TMKReestr;
    constructor(public repo: IRepository, public urlHelper: URLHelper,private elementRef: ElementRef, ) {

        let IsTMKAdmin = this.elementRef.nativeElement.getAttribute("[IsTMKAdmin]") === "true";
        let IsTMKUser = this.elementRef.nativeElement.getAttribute("[IsTMKUser]") === "true";
        let IsTMKReader = this.elementRef.nativeElement.getAttribute("[IsTMKReader]") === "true";
        let IsTMKSmo = this.elementRef.nativeElement.getAttribute("[IsTMKSMO]") === "true";
        let CODE_MO = this.elementRef.nativeElement.getAttribute("[CODE_MO]");
        this.userInfo = new UserINFO(IsTMKAdmin,IsTMKUser,IsTMKReader,IsTMKSmo,CODE_MO);
    }
    
    ngOnInit(): void {
        let selectIndex = this.urlHelper.getParameter('selectIndex');
        switch(selectIndex)
        {
            case "1":this.CurrentTMKViewType = TMKViewType.TMKReport; break;
            case "2":this.CurrentTMKViewType = TMKViewType.SPRContact; break;
            
        }
    }
}



enum TMKViewType {
    TMKReestr = 0,
    TMKReport = 1,
    SPRContact =2
}
