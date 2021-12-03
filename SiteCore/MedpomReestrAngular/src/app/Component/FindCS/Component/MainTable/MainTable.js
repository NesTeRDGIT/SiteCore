var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, ViewChild, Input } from "@angular/core";
import { StatusCS_LIST } from 'src/app/API/CSModel';
import { HubConnect } from "src/app/API/HUBConnect";
import { CSItemEdit, EditMode } from "src/app/Component/FindCS/Component/CSItemEdit/CSItemEdit";
import { CSItemView } from "src/app/Component/FindCS/Component/CSItemView/CSItemView";
import { InstructionDialogCS } from "src/app/Component/FindCS/Component/InstructionDialogCS/InstructionDialogCS";
let MainTable = class MainTable {
    constructor(repo) {
        this.repo = repo;
        this.AdminMode = false;
        this.ListPerson = [];
        this.first = 0;
        this.countOnPage = 25;
        this.contextPerson = null;
        this.selectedPersons = [];
        this.StatusCS_LIST = StatusCS_LIST;
        this.loading = false;
        //#region Hub
        this.hubConnect = new HubConnect();
        this.connectHub = async () => {
            try {
                await this.hubConnect.Connect();
                this.hubConnect.RegisterNewCSListState((data) => {
                    this.onUpdateDataEditDialog(data);
                });
            }
            catch (err) {
                alert(`Ошибка подключения к интерфейсу обратного вызова: ${err.toString()}`);
            }
        };
        this.connectHub();
    }
    get firstsSelectedPersons() {
        if (this.selectedPersons.length !== 0)
            return this.selectedPersons[0];
        return null;
    }
    ngOnInit() {
    }
    onContextMenuSelect(event, contextMenu) {
        if (this.contextPerson != null) {
            let find = this.selectedPersons.filter(x => x.CS_LIST_IN_ID == this.contextPerson.CS_LIST_IN_ID);
            if (find.length === 0) {
                this.selectedPersons = [this.contextPerson];
            }
        }
        ;
        const EditEnabled = this.firstsSelectedPersons != null && this.firstsSelectedPersons.STATUS_SEND === StatusCS_LIST.New;
        const SendEnabled = this.selectedPersons.length != 0 && this.selectedPersons.filter(x => x.STATUS_SEND !== StatusCS_LIST.New).length == 0;
        this.contextMenuItems = [
            { label: 'Просмотр', icon: 'pi pi-fw pi-search', styleClass: 'bold-menuitem', command: () => { this.showItemDialog(); } },
            { label: 'Новый', icon: 'pi pi-fw pi-file', command: () => { this.showNewItemDialog(); } },
            { separator: true },
            { label: 'Редактировать', disabled: !EditEnabled, icon: 'pi pi-fw pi-user-edit', command: () => { this.EditItemDialog(); } },
            { label: 'Дублировать', icon: 'pi pi-fw pi-copy', command: () => { this.DoubleItemDialog(); } },
            { label: 'Установить статус "Новый"', icon: 'pi pi-fw pi-book', command: () => { this.SetNewStatusItems(); } },
            { separator: true },
            { label: 'Удалить', icon: 'pi pi-fw pi-times', styleClass: 'red-menuitem', command: () => { this.RemoveItems(); } },
            { separator: true },
            { label: 'Обновить', icon: 'pi pi-fw pi-refresh', command: () => { this.LoadData(this.first, this.countOnPage); } },
            { label: 'Отправить', disabled: !SendEnabled, icon: 'pi pi-fw pi-send', command: () => { this.SendItems(); } },
        ];
    }
    async loadPerson(event) {
        setTimeout(async () => {
            await this.LoadData(event.first, event.rows);
        });
    }
    async LoadData(first, rows) {
        try {
            this.loading = true;
            var data = await this.repo.GetTitle(first, rows);
            this.contextPerson = null;
            this.selectedPersons = [];
            this.ListPerson = data.PersonItems;
            this.totalRecords = data.TotalRecord;
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    showNewItemDialog() {
        try {
            this.editDialog.ShowDialog(EditMode.New);
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    EditItemDialog() {
        try {
            if (this.selectedPersons.length !== 0) {
                let item = this.selectedPersons[0];
                this.editDialog.ShowDialog(EditMode.Edit, item.CS_LIST_IN_ID);
            }
        }
        catch (err) {
            alert(err.toString());
        }
    }
    DoubleItemDialog() {
        try {
            if (this.selectedPersons.length !== 0) {
                let item = this.selectedPersons[0];
                this.editDialog.ShowDialog(EditMode.Double, item.CS_LIST_IN_ID);
            }
        }
        catch (err) {
            alert(err.toString());
        }
    }
    async onUpdateDataEditDialog(ID) {
        if (ID === null) {
            this.LoadData(this.first, this.countOnPage);
        }
        else {
            this.UpdateItems(ID);
        }
    }
    async UpdateItems(ID) {
        const itemForUpdate = this.ListPerson.filter(x => ID.includes(x.CS_LIST_IN_ID));
        if (itemForUpdate.length != null) {
            var items = await this.repo.GetTitleByID(itemForUpdate.map(x => x.CS_LIST_IN_ID));
            items.forEach(x => {
                const item = itemForUpdate.find(y => y.CS_LIST_IN_ID === x.CS_LIST_IN_ID);
                if (item !== null) {
                    item.Merge(x);
                }
            });
            this.ListPerson = [...this.ListPerson];
        }
    }
    async RemoveItems() {
        try {
            if (this.selectedPersons.length !== 0) {
                if (confirm(`Вы уверены, что хотите удалить ${this.selectedPersons.length} записей?`)) {
                    this.loading = true;
                    await this.repo.RemovePerson(this.selectedPersons.map(x => x.CS_LIST_IN_ID));
                    await this.LoadData(this.first, this.countOnPage);
                }
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    async SendItems() {
        try {
            if (this.selectedPersons.length !== 0) {
                if (confirm(`Вы уверены, что хотите отправить ${this.selectedPersons.length} записей?`)) {
                    this.loading = true;
                    await this.repo.SendPerson(this.selectedPersons.map(x => x.CS_LIST_IN_ID));
                    await this.LoadData(this.first, this.countOnPage);
                }
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    async SetNewStatusItems() {
        try {
            if (this.selectedPersons.length !== 0) {
                if (confirm(`Вы уверены, что хотите установить статус "Новый" ${this.selectedPersons.length} записей?`)) {
                    this.loading = true;
                    await this.repo.SetNewStatus(this.selectedPersons.map(x => x.CS_LIST_IN_ID));
                    await this.LoadData(this.first, this.countOnPage);
                }
            }
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.loading = false;
        }
    }
    showItemDialog() {
        try {
            if (this.selectedPersons.length !== 0) {
                let item = this.selectedPersons[0];
                this.viewDialog.ShowDialog(item.CS_LIST_IN_ID);
            }
        }
        catch (err) {
            alert(err.toString());
        }
    }
    ngOnDestroy() {
        try {
            this.hubConnect.Disconnect();
        }
        catch (err) {
            alert(err.toString());
        }
        this.hubConnect.Disconnect();
    }
    showInstructionDialogCS() {
        this.instructionDialogCS.ShowDialog();
    }
};
__decorate([
    Input()
], MainTable.prototype, "AdminMode", void 0);
__decorate([
    ViewChild(CSItemEdit)
], MainTable.prototype, "editDialog", void 0);
__decorate([
    ViewChild(CSItemView)
], MainTable.prototype, "viewDialog", void 0);
__decorate([
    ViewChild(InstructionDialogCS)
], MainTable.prototype, "instructionDialogCS", void 0);
MainTable = __decorate([
    Component({ selector: "Main-Table", templateUrl: "MainTable.html", styleUrls: ['MainTable.scss'] })
], MainTable);
export { MainTable };
//# sourceMappingURL=MainTable.js.map