import { Component, Output, EventEmitter, OnChanges, SimpleChanges, OnDestroy ,ViewChild} from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { IRepository } from "../../API/Repository";

import { FileAPI } from "../../API/FileAPI";
import { HubConnect } from "../../API/HUBConnect";
import { ViewReestViewModel, StatusFilePack, StepsProcess, STATUS_FILE, TYPEFILE } from "../../API/LoadReestViewModel";
import {InstructionDialog} from '../../Component/InstructionDialog/InstructionDialog'

@Component({ selector: "view-reestr", templateUrl: "ViewReestrComponent.html" })
export class ViewReestrComponent extends BaseReportComponent implements OnDestroy {
    ngOnDestroy(): void {
        try {
            this.hubConnect.Disconnect();
        }
        catch (err) {
            alert(err.toString());
        }
    }

    model: ViewReestViewModel = new ViewReestViewModel(null);
    StatusFilePack = StatusFilePack;
    StepsProcess = StepsProcess;
    STATUS_FILE = STATUS_FILE;
    TYPEFILE = TYPEFILE;
    hubConnect: HubConnect = new HubConnect();
    constructor(public repo: IRepository) {
        super();
        this.getModel();
        this.connectHub();
    }

    private connectHub = async () => {
        try {
            await this.hubConnect.Connect();
            this.hubConnect.RegisterNewPackState(() => {
                this.getModel(true);
            });
        }
        catch (err) {
            alert(`Ошибка подключения к интерфейсу обратного вызова: ${err.toString()}`);
        }
    }

    getModel = async (hide_mode:boolean = false) => {
        try {
            if(!hide_mode)
                this.isLoad = true;
            this.model = await this.repo.getViewReestViewModelAsync();
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }
    isLoadFile = false;
    getProtocol = async () => {
        try {
            this.isLoadFile = true;
            const file = await this.repo.getProtocolAsync();
            FileAPI.downloadBase64File(file.FileContents, file.ContentType, file.FileDownloadName);
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoadFile = false;
        }
    }

    @ViewChild(InstructionDialog) instructionDialog: InstructionDialog;
    ShowInstruction = () => {
        this.instructionDialog.ShowDialog();
    }


}


