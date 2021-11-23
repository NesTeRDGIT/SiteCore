import { Component, Output,Input, EventEmitter, OnChanges, SimpleChanges, ViewChild, ElementRef} from "@angular/core";



@Component({ selector: "File-Loader", templateUrl: "FileLoader.html", styleUrls: ['./FileLoader.css'] })
export class FileLoader  {
    @ViewChild('fileControl') fileControl: ElementRef<HTMLElement>;
    @Output() onSelect = new EventEmitter<FileList>();
    @Input() filter: string;

    isDrag:boolean;
    DragOverFileDialog = (e: DragEvent) => {
        this.isDrag = true;
        e.preventDefault();
        e.stopPropagation();
    }
    DragEnterFileDialog = (e: DragEvent) => {
      
        e.preventDefault();
        e.stopPropagation();
    }
    DragLeaveFileDialog = (e) => {
       
        this.isDrag = false;
    }
    DropFileDialog = (e:DragEvent) => {
        this.isDrag = false;
        if (e.dataTransfer) {
            if (e.dataTransfer.files.length) {
                e.preventDefault();
                e.stopPropagation();
                this.onSelect.emit(e.dataTransfer.files);
            }
        }
    }
    
    ShowFileDialog = () => {
        const el: HTMLElement = this.fileControl.nativeElement;
        el.click();
    }

    onChange = (e:Event) => {
        const target = event.target as HTMLInputElement;
        if (target.files.length) {
            this.onSelect.emit(target.files);
            target.value="";
        }
    }

}


