var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, ViewChild, Input } from "@angular/core";
import { SPRModel } from "../../API/SPRModel";
import { SPRContactEditComponent } from "../../Component/SPRContactEdit/SPRContactEdit";
let SPRContactComponent = class SPRContactComponent {
    constructor(repo) {
        this.repo = repo;
        this.model = [];
        this.loading = false;
        this.GetData = async () => {
            try {
                this.loading = true;
                await this.SPR.refreshCODE_MOAsync();
                this.model = await this.repo.GetSPRContactAsync();
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.loading = false;
            }
        };
        this.selectedTMKItems = [];
        this.contextMenuSelect = null;
        this.contextTMKListModel = null;
        this.New = () => {
            try {
                this.SPRContactEditWin.New();
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.Edit = (item) => {
            try {
                this.SPRContactEditWin.Edit(item);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.Delete = async (items) => {
            try {
                if (confirm("Вы уверены, что хотите удалить записи?")) {
                    await this.repo.DeleteSPRContactAsync(items);
                    this.GetData();
                }
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.SPR = new SPRModel(this.repo);
    }
    ngOnInit() {
        this.GetData();
    }
    onContextMenuSelect(event, contextMenu) {
        if (this.contextMenuSelect != null) {
            if (this.selectedTMKItems == null) {
                this.selectedTMKItems = [];
            }
            if (this.selectedTMKItems.filter(x => x.ID_CONTACT_INFO == this.contextMenuSelect.ID_CONTACT_INFO).length === 0) {
                this.selectedTMKItems = [this.contextMenuSelect];
            }
        }
        this.contextMenuItems = [];
        this.contextMenuItems.push({ label: 'Новая запись', icon: 'pi pi-fw pi-file', command: () => { this.New(); } });
        this.contextMenuItems.push({ separator: true });
        this.contextMenuItems.push({ label: 'Редактировать', icon: 'pi pi-fw pi-user-edit', command: () => { this.Edit(this.selectedTMKItems[0]); } });
        this.contextMenuItems.push({ label: 'Удалить', icon: 'pi pi-fw pi-times', styleClass: 'red-menuitem', command: () => { this.Delete(this.selectedTMKItems); } });
        this.contextMenuItems.push({ separator: true });
        this.contextMenuItems.push({ label: 'Обновить', icon: 'pi pi-fw pi-refresh', command: () => { this.GetData(); } });
    }
};
__decorate([
    Input()
], SPRContactComponent.prototype, "userInfo", void 0);
__decorate([
    ViewChild(SPRContactEditComponent)
], SPRContactComponent.prototype, "SPRContactEditWin", void 0);
SPRContactComponent = __decorate([
    Component({ selector: "SPRContact", templateUrl: "SPRContact.html", styleUrls: ["SPRContact.scss"] })
], SPRContactComponent);
export { SPRContactComponent };
//# sourceMappingURL=SPRContact.js.map