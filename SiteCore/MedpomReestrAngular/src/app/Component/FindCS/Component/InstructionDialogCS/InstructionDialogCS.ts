import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef } from "@angular/core";

@Component({ selector: "InstructionDialogCS", templateUrl: "InstructionDialogCS.html" })
export class InstructionDialogCS {
    display = false;
    public ShowDialog() {
        this.display = true;
    }
}


