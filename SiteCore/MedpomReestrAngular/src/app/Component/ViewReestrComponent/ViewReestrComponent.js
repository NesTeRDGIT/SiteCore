var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { FileAPI } from "../../API/FileAPI";
import { HubConnect } from "../../API/HUBConnect";
import { ViewReestViewModel, StatusFilePack, StepsProcess, STATUS_FILE, TYPEFILE } from "../../API/LoadReestViewModel";
let ViewReestrComponent = class ViewReestrComponent extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.model = new ViewReestViewModel(null);
        this.StatusFilePack = StatusFilePack;
        this.StepsProcess = StepsProcess;
        this.STATUS_FILE = STATUS_FILE;
        this.TYPEFILE = TYPEFILE;
        this.hubConnect = new HubConnect();
        this.connectHub = async () => {
            try {
                await this.hubConnect.Connect();
                this.hubConnect.NewPackState(() => {
                    this.getModel();
                });
            }
            catch (err) {
                alert(`Ошибка подключения к интерфейсу обратного вызова: ${err.toString()}`);
            }
        };
        this.getModel = async () => {
            try {
                this.isLoad = true;
                this.model = await this.repo.getViewReestViewModelAsync();
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.isLoadFile = false;
        this.getProtocol = async () => {
            try {
                this.isLoadFile = true;
                const file = await this.repo.getProtocolAsync();
                FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoadFile = false;
            }
        };
        this.displayDialog = false;
        this.ShowInstruction = () => {
            this.displayDialog = true;
        };
        this.getModel();
        this.connectHub();
    }
    ngOnDestroy() {
        this.hubConnect.Disconnect();
    }
};
ViewReestrComponent = __decorate([
    Component({ selector: "view-reestr", templateUrl: "ViewReestrComponent.html" })
], ViewReestrComponent);
export { ViewReestrComponent };
//# sourceMappingURL=ViewReestrComponent.js.map