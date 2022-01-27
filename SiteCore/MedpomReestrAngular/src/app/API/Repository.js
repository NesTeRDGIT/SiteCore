var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from '@angular/core';
import { LoadReestViewModel, ErrorItem, ViewReestViewModel } from "./LoadReestViewModel";
import { EditErrorSPRViewModel, ErrorSpr, SectionSpr } from "./ErrorSPRModel";
let IRepository = class IRepository {
};
IRepository = __decorate([
    Injectable()
], IRepository);
export { IRepository };
let Repository = class Repository {
    constructor() {
        this.defaultFetchParam = { credentials: "same-origin" };
        this.FileListToFormData = (items) => {
            const formData = new FormData();
            for (let i = 0; i < items.length; i++) {
                formData.append(`file_${i}`, items[i]);
            }
            return formData;
        };
        this.makeRequest = (method, url, formData, progress) => {
            return new Promise((resolve, reject) => {
                const xhr = new XMLHttpRequest();
                xhr.upload.onprogress = (e) => {
                    progress(e);
                };
                xhr.open(method, url);
                xhr.onload = function () {
                    if (this.status >= 200 && this.status < 300) {
                        resolve(JSON.parse(xhr.response));
                    }
                    else {
                        reject(new Error(`${this.status}-${xhr.statusText}`));
                    }
                };
                xhr.onerror = function () {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                };
                xhr.send(formData);
            });
        };
    }
    async RemoveErrorSPR(ID_ERR) {
        const form = new FormData();
        form.append("ID_ERR", ID_ERR.toString());
        ;
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
    convertToString(val) {
        return `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }
    ErrorSprToFormData(item) {
        const value = new FormData();
        if (item.ID_ERR != null)
            value.append("ID_ERR", item.ID_ERR.toString());
        value.append("OSN_TFOMS", item.OSN_TFOMS);
        if (item.ID_SECTION != null)
            value.append("ID_SECTION", item.ID_SECTION.toString());
        value.append("D_EDIT", this.convertToString(new Date()));
        value.append("EXAMPLE", item.EXAMPLE);
        value.append("TEXT", item.TEXT);
        if (item.D_BEGIN !== null)
            value.append("D_BEGIN", this.convertToString(item.D_BEGIN));
        if (item.D_END !== null)
            value.append("D_END", this.convertToString(item.D_END));
        value.append("ISMEK", item.IsMEK ? "True" : "False");
        return value;
    }
    async EditErrorSPR(item) {
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
    async AddErrorSPR(item) {
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
    async getSections() {
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
    async getError(ID_ERR) {
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
    async getErrorSPR(isShowClose) {
<<<<<<< HEAD
=======
        debugger;
>>>>>>> 6c6c044a498abee325c306a40cc061a8e0a09d91
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
    async getLoadReestViewModelAsync() {
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
    async SendFiles(items, progress) {
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
    async DeleteFile(id) {
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
    async Send() {
        const response = await this.createFetch(`../Send`, "POST");
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return [];
            }
            else {
                return data.Value.map(x => new ErrorItem(x));
            }
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    createFetch(url, method = "GET", formData = null) {
        const FetchParam = { credentials: "same-origin", method: method, body: formData };
        return fetch(url, FetchParam);
    }
    async getViewReestViewModelAsync() {
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
    async getProtocolAsync() {
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
};
Repository = __decorate([
    Injectable()
], Repository);
export { Repository };
//# sourceMappingURL=Repository.js.map