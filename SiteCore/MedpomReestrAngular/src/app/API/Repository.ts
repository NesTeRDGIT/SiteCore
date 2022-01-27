
import { Injectable, EventEmitter } from '@angular/core';
import { FileASP } from "./FileASP";
import { LoadReestViewModel, ErrorItem, ViewReestViewModel } from "./LoadReestViewModel";
import { EditErrorSPRViewModel,ErrorSpr,SectionSpr } from "./ErrorSPRModel";






@Injectable()
export abstract class IRepository {
    abstract getLoadReestViewModelAsync(): Promise<LoadReestViewModel>;
    abstract SendFiles(items: File[], progress: (e: ProgressEvent) => void): Promise<ErrorItem[]>;
    abstract ClearFiles();
    abstract DeleteFile(id: number);
    abstract Send(): Promise<ErrorItem[]>;

    abstract getViewReestViewModelAsync(): Promise<ViewReestViewModel>;
    abstract getProtocolAsync(): Promise<FileASP>;

    abstract getErrorSPR(isShowClose:boolean): Promise<EditErrorSPRViewModel>;
    abstract getError(ID_ERR: number): Promise<ErrorSpr>;
    abstract getSections(): Promise<SectionSpr[]>;

    abstract AddErrorSPR(item: ErrorSpr): Promise<string[]>;
    abstract EditErrorSPR(item: ErrorSpr): Promise<string[]>;
    abstract RemoveErrorSPR(ID_ERR: number): Promise<void>;
    
}


@Injectable()
export class Repository implements IRepository {


    async RemoveErrorSPR(ID_ERR: number): Promise<void> {
        const form = new FormData();
        form.append("ID_ERR", ID_ERR.toString());;
        const response = await this.createFetch(`../RemoveErrorSPR`, 'POST', form);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    defaultFetchParam: RequestInit = { credentials: "same-origin" };

    convertToString(val: Date): string {
        return `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }
    ErrorSprToFormData(item: ErrorSpr): FormData {
              const value = new FormData();
        if (item.ID_ERR!=null)
            value.append("ID_ERR", item.ID_ERR.toString());
        value.append("OSN_TFOMS", item.OSN_TFOMS);
        if (item.ID_SECTION!=null)
            value.append("ID_SECTION", item.ID_SECTION.toString());
        value.append("D_EDIT", this.convertToString(new Date()));
        value.append("EXAMPLE", item.EXAMPLE);
        value.append("TEXT", item.TEXT);
        if(item.D_BEGIN !== null)
            value.append("D_BEGIN", this.convertToString(item.D_BEGIN));
        if(item.D_END !== null)
            value.append("D_END", this.convertToString(item.D_END));
        value.append("ISMEK", item.IsMEK? "True": "False");
        return value;
    }


    async EditErrorSPR(item: ErrorSpr): Promise<string[]> {
        const response = await this.createFetch(`../EditErrorSPR`, 'POST', this.ErrorSprToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return [];
            }
            return data.Value;
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async AddErrorSPR(item: ErrorSpr): Promise<string[]> {
        const response = await this.createFetch(`../AddErrorSPR`, 'POST', this.ErrorSprToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return [];
            }
            return data.Value;
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }




    async getSections(): Promise<SectionSpr[]> {
        const response = await this.createFetch(`../GetSections`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map(x => new SectionSpr(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getError(ID_ERR: number): Promise<ErrorSpr> {
        const response = await this.createFetch(`../GetError?ID_ERR=${ID_ERR}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new ErrorSpr(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getErrorSPR(isShowClose:boolean): Promise<EditErrorSPRViewModel> {
       
        const response = await this.createFetch(`../ErrorList?isShowClose=${isShowClose}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new EditErrorSPRViewModel(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getLoadReestViewModelAsync(): Promise<LoadReestViewModel> {
        const response = await this.createFetch(`../GetLoadReestrModel`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
               
                return new LoadReestViewModel(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    FileListToFormData = (items: File[])=> {
        const formData = new FormData();
        for (let i = 0; i < items.length; i++) {
            formData.append(`file_${i}`, items[i]);
        }
        return formData;

    }
    makeRequest = (method: string, url: string, formData: FormData, progress: (e:ProgressEvent)=>void) => {
        return new Promise<any>((resolve, reject) => {
            const xhr = new XMLHttpRequest();
            xhr.upload.onprogress = (e: ProgressEvent) => {
                progress(e);
            }
            xhr.open(method, url);
            xhr.onload = function (this: XMLHttpRequest) {
                if (this.status >= 200 && this.status < 300) {
                    resolve(JSON.parse(xhr.response));
                } else {
                    reject(new Error(`${this.status}-${xhr.statusText}`));
                }
            };
            xhr.onerror = function (this: XMLHttpRequest) {
                reject({
                    status: this.status,
                    statusText: xhr.statusText
                });
            };
            xhr.send(formData);
        });
    }



    async SendFiles(items: File[], progress: (e: ProgressEvent) => void):Promise<ErrorItem[]> {
        const result = await this.makeRequest("POST", "../Upload", this.FileListToFormData(items), progress);
        return result.Value.map(x => new ErrorItem(x));

    }

    async ClearFiles() {
        const response = await this.createFetch(`../Clear`, "POST");
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async DeleteFile(id: number) {
        const formData = new FormData();
        formData.append("ID", id.toString());
        const response = await this.createFetch(`../DeleteFile`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async Send(): Promise<ErrorItem[]> {
        const response = await this.createFetch(`../Send`, "POST");
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return [];
            } else {
                return data.Value.map(x => new ErrorItem(x));
            }
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    createFetch(url: string, method: string = "GET", formData: FormData=null): Promise<Response> {
        const FetchParam: RequestInit = { credentials: "same-origin", method: method, body:formData };
        return fetch(url, FetchParam);
    }


  
    async getViewReestViewModelAsync(): Promise<ViewReestViewModel> {
     
        const response = await this.createFetch(`../GetViewReestrModel`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new ViewReestViewModel(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getProtocolAsync(): Promise<FileASP> {
        const response = await this.createFetch(`../DownloadProtocol`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    
    


}





