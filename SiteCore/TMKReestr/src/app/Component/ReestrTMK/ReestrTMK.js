var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, ViewChild, Input } from "@angular/core";
import { TMKListModelStatusEnum } from "../../API/TMKList";
import { SPRModel } from "../../API/SPRModel";
import { EditTMKComponent } from "../../Component/EditTMK/EditTMKComponent";
import { FileAPI } from "../../API/FileAPI";
import { TMKItem } from "src/app/API/TMKItem";
let ReestrTMKComponent = class ReestrTMKComponent {
    constructor(repo) {
        this.repo = repo;
        this.StatusENUM = TMKListModelStatusEnum;
        this.TMKList = [];
        this.first = 0;
        this.rows = 100;
        this.loading = false;
        this.selectedTMKItems = [];
        this.contextMenuSelect = null;
        this.contextTMKListModel = null;
        this.CurrentFilter = null;
        this.ShowTMK = (item) => {
            if (item != null) {
                this.EditTMKWin.Show(item.TMK_ID);
            }
        };
        this.EditTMK = (item) => {
            if (item != null) {
                this.EditTMKWin.Edit(item.TMK_ID);
            }
        };
        this.NewTMK = () => {
            this.EditTMKWin.New();
        };
        this.SPR = new SPRModel(repo);
    }
    onContextMenuSelect(event, contextMenu) {
        if (this.contextMenuSelect != null) {
            if (this.selectedTMKItems == null) {
                this.selectedTMKItems = [];
            }
            if (this.selectedTMKItems.filter(x => x.TMK_ID == this.contextMenuSelect.TMK_ID).length === 0) {
                this.selectedTMKItems = [this.contextMenuSelect];
            }
        }
        if (this.selectedTMKItems != null && this.selectedTMKItems.length != 0) {
            let firstItem = this.selectedTMKItems[0];
            let IsMy = firstItem.CODE_MO === this.userInfo.CodeMO;
            let isOpen = firstItem.STATUS === TMKListModelStatusEnum.Open;
            let isOpenALL = true;
            let IsMyALL = true;
            this.selectedTMKItems.forEach(item => {
                if (item.STATUS !== TMKListModelStatusEnum.Open)
                    isOpenALL = false;
                if (item.CODE_MO !== this.userInfo.CodeMO)
                    IsMyALL = false;
            });
            let EditEnabled = IsMy && isOpen;
            let DeleteEnabled = isOpenALL && IsMyALL;
            this.contextMenuItems = [];
            this.contextMenuItems.push({ label: 'Просмотр', icon: 'pi pi-fw pi-search', styleClass: 'bold-menuitem', command: () => { this.ShowTMK(firstItem); } });
            if (this.userInfo.IsTMKUser) {
                this.contextMenuItems.push({ label: 'Новая запись', icon: 'pi pi-fw pi-file', command: () => { this.NewTMK(); } });
                this.contextMenuItems.push({ separator: true });
                this.contextMenuItems.push({ label: 'Редактировать', disabled: !EditEnabled, icon: 'pi pi-fw pi-user-edit', command: () => { this.EditTMK(firstItem); } });
                this.contextMenuItems.push({ label: 'Удалить', disabled: !DeleteEnabled, icon: 'pi pi-fw pi-times', styleClass: 'red-menuitem', command: () => { this.RemoveItems(this.selectedTMKItems); } });
            }
            if (this.userInfo.IsTMKAdmin) {
                this.contextMenuItems.push({ separator: true });
                this.contextMenuItems.push({ label: 'Изменить статус', icon: 'pi pi-fw pi-send', command: () => { this.ChangeStatus(this.selectedTMKItems); } });
            }
            this.contextMenuItems.push({ separator: true });
            this.contextMenuItems.push({ label: 'Обновить', icon: 'pi pi-fw pi-refresh', command: () => { this.LoadData(this.first, this.rows); } });
        }
    }
    async onLazyLoad(event) {
        setTimeout(async () => {
            await this.LoadData(event.first, event.rows);
        });
    }
    async LoadData(first, rows) {
        try {
            await this.FILL_SPR();
            this.loading = true;
            var data = await this.repo.getTMKListAsync(first, rows, this.CurrentFilter);
            this.TMKList = data.Items;
            this.totalRecords = data.Count;
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    async RemoveItems(items) {
        try {
            var newItems = items.map(x => {
                let newItem = new TMKItem(null);
                newItem.TMK_ID = x.TMK_ID;
                return newItem;
            });
            let result = await this.repo.DeleteTMKItemAsync(newItems);
            if (!result.Result)
                alert(result.Error);
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.LoadData(this.first, this.rows);
        }
    }
    async ChangeStatus(items) {
        try {
            var newItems = items.map(x => {
                let newItem = new TMKItem(null);
                newItem.TMK_ID = x.TMK_ID;
                return newItem;
            });
            await this.repo.ChangeTmkReestrStatusAsync(newItems);
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.LoadData(this.first, this.rows);
        }
    }
    async DownloadXLS() {
        try {
            var file = await this.repo.getTMKListFileAsync(this.first, this.rows, this.CurrentFilter);
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        }
        catch (err) {
            alert(err.toString());
        }
    }
    async onChangeItems(TMK_ID) {
        if (TMK_ID) {
            debugger;
        }
        else {
            this.LoadData(this.first, this.totalRecords);
        }
    }
    onCurrentFilterChange(val) {
        try {
            this.CurrentFilter = val;
            this.LoadData(0, this.totalRecords);
        }
        catch (err) {
            alert(err.toString());
        }
    }
    ngOnInit() {
    }
    async FILL_SPR() {
        try {
            await this.SPR.refreshStaticSPR(false);
            await this.SPR.refreshVariableSPR(false);
        }
        catch (err) {
            alert(`Ошибка получение справочников: ${err.toString()}`);
        }
    }
};
__decorate([
    Input()
], ReestrTMKComponent.prototype, "userInfo", void 0);
__decorate([
    ViewChild(EditTMKComponent)
], ReestrTMKComponent.prototype, "EditTMKWin", void 0);
ReestrTMKComponent = __decorate([
    Component({ selector: "ReestrTMK", templateUrl: "ReestrTMK.html", styleUrls: ['ReestrTMK.scss'] })
], ReestrTMKComponent);
export { ReestrTMKComponent };
//# sourceMappingURL=ReestrTMK.js.map