
import { NgIf } from '@angular/common';
import { Injectable } from '@angular/core';
import { Module } from 'module';
import { TitleResult,PersonItem, SPRModel, PersonItemModel, PersonView } from "./CSModel";
import { LogEntries } from "./LogEntries";

@Injectable()
export abstract class IRepositoryCS {
    abstract GetTitle(first: number, rows: number): Promise<TitleResult>;
    abstract GetTitleByID(CS_LIST_IN_ID:number[]): Promise<PersonItem[]>;
    abstract GetSPR(): Promise<SPRModel>;
    abstract AddPerson(item: PersonItemModel): Promise<ResultOperation>;
    abstract UpdatePerson(item: PersonItemModel): Promise<ResultOperation>;
    abstract GetPerson(CS_LIST_IN_ID: number): Promise<PersonItemModel>;
    abstract RemovePerson(CS_LIST_IN_ID: number[]): Promise<boolean>;
    abstract SendPerson(CS_LIST_IN_ID: number[]): Promise<boolean>;
    abstract GetPersonView(CS_LIST_IN_ID: number): Promise<PersonView>;
    abstract GetServiceState(): Promise<boolean>;
    abstract GetServiceState(): Promise<boolean>;
    abstract GetLogService(): Promise<LogEntries[]>;
    abstract SetNewStatus(CS_LIST_IN_ID: number[]): Promise<boolean>
}

@Injectable()
export class RepositoryCS implements IRepositoryCS {

    defaultFetchParam: RequestInit = { credentials: "same-origin" };
    createFetch(url: string, method: string = "GET", formData: FormData = null): Promise<Response> {
        const FetchParam: RequestInit = { credentials: "same-origin", method: method, body: formData };
        return fetch(url, FetchParam);
    }

    async GetTitleByID(CS_LIST_IN_ID:number[]): Promise<PersonItem[]> {

        const q = CS_LIST_IN_ID.map(x=>`CS_LIST_IN_ID=${x}`).join("&");
     
        const response = await this.createFetch(`../../Identification/GetCSItemTitleByID?${q}`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map(x=>new PersonItem(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetTitle(first: number, rows: number): Promise<TitleResult> {

        const response = await this.createFetch(`../../Identification/GetCSItemTitle?first=${first}&rows=${rows}`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TitleResult(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetSPR(): Promise<SPRModel> {

        const response = await this.createFetch(`../../Identification/GetSPR`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new SPRModel(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    private PersonItemModelToFormData(item: PersonItemModel, withID: boolean) {
        const fd = new FormData();

        if (withID) {
            if (item.CS_LIST_IN_ID != null)
                fd.append("CS_LIST_IN_ID", item.CS_LIST_IN_ID.toString());
        }
        if (item.FAM)
            fd.append("FAM", item.FAM);
        if (item.IM)
            fd.append("IM", item.IM);
        if (item.OT)
            fd.append("OT", item.OT);
        if (item.DR != null)
            fd.append("DR", item.DR.toASPstring());
        if (item.W != null)
            fd.append("W", item.W.toString());
        if (item.DOC_TYPE != null)
            fd.append("DOC_TYPE", item.DOC_TYPE);
        if (item.DOC_NUM)
            fd.append("DOC_NUM", item.DOC_NUM);
        if (item.DOC_SER)
            fd.append("DOC_SER", item.DOC_SER);
        if (item.VPOLIS != null)
            fd.append("VPOLIS", item.VPOLIS.toString());
        if (item.SPOLIS)
            fd.append("SPOLIS", item.SPOLIS);
        if (item.NPOLIS)
            fd.append("NPOLIS", item.NPOLIS);
        if (item.SNILS)
            fd.append("SNILS", item.SNILS);
        return fd;
    }
    async AddPerson(item: PersonItemModel): Promise<ResultOperation> {
        const response = await this.createFetch(`../../Identification/AddPerson`, 'POST', this.PersonItemModelToFormData(item, false));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new ResultOperation(data.Result, null);
            }
            else {
                return new ResultOperation(data.Result, data.Value);
            }
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async UpdatePerson(item: PersonItemModel): Promise<ResultOperation> {

        const response = await this.createFetch(`../../Identification/UpdatePerson`, 'POST', this.PersonItemModelToFormData(item, true));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new ResultOperation(data.Result, null);
            }
            else {
                return new ResultOperation(data.Result, data.Value);
            }
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetPerson(CS_LIST_IN_ID: number): Promise<PersonItemModel> {
        const response = await this.createFetch(`../../Identification/GetPerson?CS_LIST_IN_ID=${CS_LIST_IN_ID}`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new PersonItemModel(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async RemovePerson(CS_LIST_IN_ID: number[]): Promise<boolean> {
        let form = new FormData();
        CS_LIST_IN_ID.forEach(x => {
            form.append('CS_LIST_IN_ID', x.toString());
        })
        const response = await this.createFetch(`../../Identification/RemovePerson`, 'POST', form);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return true;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async SendPerson(CS_LIST_IN_ID: number[]): Promise<boolean> {
        let form = new FormData();
        CS_LIST_IN_ID.forEach(x => {
            form.append('CS_LIST_IN_ID', x.toString());
        })
        const response = await this.createFetch(`../../Identification/SendPerson`, 'POST', form);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return true;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async SetNewStatus(CS_LIST_IN_ID: number[]): Promise<boolean> {
        let form = new FormData();
        CS_LIST_IN_ID.forEach(x => {
            form.append('CS_LIST_IN_ID', x.toString());
        })
        const response = await this.createFetch(`../../Identification/SetNewStatus`, 'POST', form);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return true;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async GetPersonView(CS_LIST_IN_ID: number): Promise<PersonView> {

        const response = await this.createFetch(`../../Identification/GetPersonView?CS_LIST_IN_ID=${CS_LIST_IN_ID}`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new PersonView(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetServiceState(): Promise<boolean> {

        const response = await this.createFetch(`../../Identification/GetServiceState`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetLogService(): Promise<LogEntries[]> {
        const response = await this.createFetch(`../../Identification/GetServiceLog`, 'GET');
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map(x => new LogEntries(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
}

export class ResultOperation {
    Result: boolean;
    Error: string[] = [];
    constructor(result: any, Error: any) {
        this.Result = result;
        if (Error != null) {
            if (Array.isArray(Error))
                this.Error = Error.map(x => x.toString());
            else
                this.Error.push(Error.toString());
        }
    }
}





