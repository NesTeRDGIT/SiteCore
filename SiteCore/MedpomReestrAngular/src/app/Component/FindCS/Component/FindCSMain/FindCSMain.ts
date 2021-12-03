import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";
import { BaseReportComponent } from 'src/app/Component/BaseReportComponent'
import { HubConnect }  from "src/app/API/HUBConnect";

@Component({ selector: "Find-CS-Main", templateUrl: "FindCSMain.html" })
export class FindCSMain extends  BaseReportComponent {
    @Input() AdminMode:boolean;


    hubConnect: HubConnect = new HubConnect();
    constructor() {
        super();
    }

}


