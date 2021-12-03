import { Component, OnInit, OnDestroy, ViewChild, Input } from "@angular/core";
import { MenuItem, LazyLoadEvent } from 'primeng/api';
import { IRepositoryCS } from 'src/app/API/IRepositoryCS';
import { PersonItem, TitleResult, StatusCS_LIST } from 'src/app/API/CSModel';
import { HubConnect } from "src/app/API/HUBConnect";
import { CSItemEdit, EditMode } from "src/app/Component/FindCS/Component/CSItemEdit/CSItemEdit"
import { CSItemView } from "src/app/Component/FindCS/Component/CSItemView/CSItemView"
import { InstructionDialogCS } from "src/app/Component/FindCS/Component/InstructionDialogCS/InstructionDialogCS"


@Component({ selector: "Main-Table", templateUrl: "MainTable.html", styleUrls:['MainTable.scss'] })
export class MainTable implements OnInit, OnDestroy {
    @Input() AdminMode: boolean = false;
    ListPerson: PersonItem[] = [];
    totalRecords: number;
    first: number = 0;
    countOnPage: number = 25;
    contextMenuItems: MenuItem[];
    contextPerson: PersonItem = null;

    selectedPersons: PersonItem[] = [];

    StatusCS_LIST = StatusCS_LIST;

    get firstsSelectedPersons() {
        if (this.selectedPersons.length !== 0)
            return this.selectedPersons[0];
        return null;
    }
    constructor(private repo: IRepositoryCS) {
        this.connectHub();
    }
    ngOnInit(): void {
    }

    public onContextMenuSelect(event, contextMenu) {
        if (this.contextPerson != null) {
            let find = this.selectedPersons.filter(x => x.CS_LIST_IN_ID == this.contextPerson.CS_LIST_IN_ID);
            if (find.length === 0) {
                this.selectedPersons = [this.contextPerson];
            }
        };

        const EditEnabled = this.firstsSelectedPersons != null && this.firstsSelectedPersons.STATUS_SEND === StatusCS_LIST.New;
        const SendEnabled = this.selectedPersons.length != 0 && this.selectedPersons.filter(x => x.STATUS_SEND !== StatusCS_LIST.New).length == 0;

      
        this.contextMenuItems = [
            { label: 'Просмотр', icon: 'pi pi-fw pi-search', styleClass:'bold-menuitem', command: () => { this.showItemDialog() } },
            { label: 'Новый', icon: 'pi pi-fw pi-file', command: () => { this.showNewItemDialog() } },
            { separator: true },
            { label: 'Редактировать', disabled: !EditEnabled, icon: 'pi pi-fw pi-user-edit', command: () => { this.EditItemDialog() } },
            { label: 'Дублировать', icon: 'pi pi-fw pi-copy', command: () => { this.DoubleItemDialog() } },
            { label: 'Установить статус "Новый"', icon: 'pi pi-fw pi-book', command: () => { this.SetNewStatusItems() } },
            { separator: true },
            { label: 'Удалить', icon: 'pi pi-fw pi-times', styleClass:'red-menuitem', command: () => { this.RemoveItems() } },
            { separator: true },
            { label: 'Обновить', icon: 'pi pi-fw pi-refresh', command: () => { this.LoadData(this.first, this.countOnPage) } },
            { label: 'Отправить', disabled: !SendEnabled, icon: 'pi pi-fw pi-send', command: () => { this.SendItems() } },
        ];
    }



    loading: boolean = false;


    async loadPerson(event: LazyLoadEvent) {
        setTimeout(async () => {
            await this.LoadData(event.first, event.rows);
        });
    }

    async LoadData(first: number, rows: number) {
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
    @ViewChild(CSItemEdit) editDialog: CSItemEdit;

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



    async onUpdateDataEditDialog(ID: number[]) {
        if (ID === null) {
            this.LoadData(this.first, this.countOnPage);
        }
        else {
            this.UpdateItems(ID);
        }
    }

    private async UpdateItems(ID: number[]) {
        const itemForUpdate = this.ListPerson.filter(x => ID.includes(x.CS_LIST_IN_ID));
        if (itemForUpdate.length != null) {
            var items = await this.repo.GetTitleByID(itemForUpdate.map(x => x.CS_LIST_IN_ID));
            items.forEach(x => {
                const item = itemForUpdate.find(y => y.CS_LIST_IN_ID === x.CS_LIST_IN_ID);
                if (item !== null) {
                    item.Merge(x);
                }
            });
            this.ListPerson = [...this.ListPerson]
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

    @ViewChild(CSItemView) viewDialog: CSItemView;

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


    //#region Hub
    hubConnect: HubConnect = new HubConnect();
    private connectHub = async () => {
        try {
            await this.hubConnect.Connect();
            this.hubConnect.RegisterNewCSListState((data) => {
                this.onUpdateDataEditDialog(data);
            });
        }
        catch (err) {
            alert(`Ошибка подключения к интерфейсу обратного вызова: ${err.toString()}`);
        }
    }
    ngOnDestroy(): void {
        try {
            this.hubConnect.Disconnect();

        }
        catch (err) {
            alert(err.toString());
        }
        this.hubConnect.Disconnect();
    }

    @ViewChild(InstructionDialogCS) instructionDialogCS: InstructionDialogCS;
    showInstructionDialogCS() {
        this.instructionDialogCS.ShowDialog();
    }

    //#endregion


}


