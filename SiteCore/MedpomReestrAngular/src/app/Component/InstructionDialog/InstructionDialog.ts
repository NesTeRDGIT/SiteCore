import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef } from "@angular/core";



@Component({ selector: "Instruction-Dialog", templateUrl: "InstructionDialog.html" })
export class InstructionDialog {
    display = false;
    public ShowDialog() {
        this.display = true;
    }
}


