import { Component, Output,Input, EventEmitter,OnInit} from "@angular/core";
import { MenuItem } from "primeng/api";
import { IRepositoryCS} from 'src/app/API/IRepositoryCS';
import { LogEntries, TypeEntries } from 'src/app/API/LogEntries';

@Component({ selector: "CSServiceStatus", templateUrl: "CSServiceStatus.html", styleUrls:["CSServiceStatus.scss"]})
export class CSServiceStatus  implements OnInit   {
    items: MenuItem[];
    isEnabled:boolean|null = null;
    @Input() 
    AdminMode:boolean = false;
    TypeEntries= TypeEntries;

    constructor(private repo:IRepositoryCS)
    {
        
    }

    ngOnInit() {
        this.items = [
            {label: 'Обновить', icon: 'pi pi-refresh', command: () => { this.updateStatus(); }}
        ];
        if(this.AdminMode)
            this.items.push({label: 'Показать лог',  command: () => { this.showLog(); }});

       this.updateStatus();
    }

    isLoad:boolean = false;
    async updateStatus() {
        try
        {
            this.isLoad = true;
            this.isEnabled = null;
            this.isEnabled = await this.repo.GetServiceState();
        }
        catch(err){
            alert(err.toString());
        }
        finally{
            this.isLoad = false;
        }
    }


    _isDisplayLogDialog:boolean = false;
    get isDisplayLogDialog(){
        return this._isDisplayLogDialog;
    }
    set isDisplayLogDialog(value){       
        this._isDisplayLogDialog = value;
        if(value===false)
            this.Log = [];
    }
    isLoadLog:boolean = false;
    Log:LogEntries[] = [];

    async showLog() {
        try
        {
            this.isLoadLog = true;
            this.isDisplayLogDialog = true;
            this.Log = await this.repo.GetLogService();
        }
        catch(err){
            alert(err.toString());
        }
        finally{
            this.isLoadLog = false;
        }
    }

}


