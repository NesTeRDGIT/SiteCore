
import { Component, ViewChild, ElementRef, AfterViewInit, Input, OnInit } from "@angular/core";
import { MenuItem } from 'primeng/api';

import { IRepository } from "../../API/Repository";
import { SPRModel } from "../../API/SPRModel";
import { UserINFO } from "../../API/UserINFO";
import { SPRContactModel } from "../../API/SPRContactModel";
import { SPRContactEditComponent } from "../../Component/SPRContactEdit/SPRContactEdit";

@Component({ selector: "SPRContact", templateUrl: "SPRContact.html", styleUrls:["SPRContact.scss"] })
export class SPRContactComponent implements OnInit {
    @Input() userInfo:UserINFO;
    SPR: SPRModel;
    model:SPRContactModel[] = [];
    loading:boolean = false;
    constructor(public repo: IRepository) {
        this.SPR = new SPRModel(this.repo);
    }
    ngOnInit(): void {
        this.GetData();
        
    }

    GetData = async () => {
        try {
            this.loading = true;
            await this.SPR.refreshCODE_MOAsync();
            this.model = await this.repo.GetSPRContactAsync();
        }
        catch (err) {
            alert(err.toString())
        }
        finally {
            this.loading = false;
        }
    }


    selectedTMKItems: SPRContactModel[] = [];

    contextMenuSelect: SPRContactModel = null;



    contextMenuItems: MenuItem[] ;
    contextTMKListModel: SPRContactModel = null;


   

    public onContextMenuSelect(event, contextMenu) {
        if (this.contextMenuSelect != null) {
            if (this.selectedTMKItems == null) {
                this.selectedTMKItems = [];
            }
            if (this.selectedTMKItems.filter(x => x.ID_CONTACT_INFO == this.contextMenuSelect.ID_CONTACT_INFO).length === 0) {
                this.selectedTMKItems = [this.contextMenuSelect];
            }
        }


        this.contextMenuItems = [];
        this.contextMenuItems.push({ label: 'Новая запись', icon: 'pi pi-fw pi-file', command: () => {  this.New();} });
        this.contextMenuItems.push({ separator: true });
        this.contextMenuItems.push({ label: 'Редактировать',  icon: 'pi pi-fw pi-user-edit', command: () => { this.Edit(this.selectedTMKItems[0]);  } });
        this.contextMenuItems.push({ label: 'Удалить', icon: 'pi pi-fw pi-times', styleClass: 'red-menuitem', command: () => { this.Delete(this.selectedTMKItems); } });
        this.contextMenuItems.push({ separator: true });
        this.contextMenuItems.push({ label: 'Обновить', icon: 'pi pi-fw pi-refresh',  command: () => {this.GetData() } });
    }

    @ViewChild(SPRContactEditComponent) SPRContactEditWin: SPRContactEditComponent;
    New = () => {
        try {
            this.SPRContactEditWin.New();
        }
        catch (err) {
            alert(err.toString())
        }
    }

    Edit = (item: SPRContactModel) => {
        try {
            this.SPRContactEditWin.Edit(item);
        }
        catch (err) {
            alert(err.toString())
        }
    }


     Delete = async (items: SPRContactModel[]) => {
        try {
            if(confirm("Вы уверены, что хотите удалить записи?"))
            {
                await this.repo.DeleteSPRContactAsync(items);
                this.GetData();
            }            
        }
        catch (err) {
            alert(err.toString())
        }
    }

}