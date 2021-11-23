import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";



@Component({ selector: "Instruction-Dialog", templateUrl: "InstructionDialog.html" })
export class InstructionDialog  {
    _display = false;
    @Input()
    get display(): any {
        return this._display;
    }
    set display(value: any) {
        if (this._display !== value)
            this.displayChange.emit(value);
        this._display = value;
    }

    @Output() displayChange: EventEmitter<any> = new EventEmitter();


}


