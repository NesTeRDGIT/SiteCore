import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";



@Component({ selector: "ProgressBar", templateUrl: "ProgressBar.html", styleUrls: ['./ProgressBar.css'] })
export class ProgressBar  {
    @Input() Caption: string = "";
    @Input() Progress: number = 0;
}


