import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";



@Component({ selector: "Content-Load", templateUrl: "ContentLoad.html", styleUrls: ['./ContentLoad.css'] })
export class ContentLoad  {
   @Input() isLoad = false;

}


