import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";



@Component({ selector: "ContentLoad", templateUrl: "ContentLoad.html", styleUrls: ['./ContentLoad.css'] })
export class ContentLoad  {
   @Input() isLoad = false;

}