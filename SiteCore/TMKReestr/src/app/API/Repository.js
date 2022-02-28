var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from '@angular/core';
import { FileASP } from "./FileASP";
import { SPRContactModel } from "./SPRContactModel";
import { TMKList } from "./TMKList";
import { NMIC_OPLATA, NMIC_VID_NHISTORY, SMO, CODE_MO, TMIS, NMIC, V002, EXPERTS, F014, NMIC_CELL, NMIC_FULL, CONTACT_SPRModel } from "./SPRModel";
import { TMKItem } from "./TMKItem";
import { TMKReportTableModel } from "./ReportModel";
import { FindPacientModel } from './FindPacientModel';
import { FindExpertizeModel } from './FindExpertizeModel';
let IRepository = class IRepository {
};
IRepository = __decorate([
    Injectable()
], IRepository);
export { IRepository };
let Repository = class Repository {
    async FindExpertizeAsync(TMK_ID) {
        const response = await Helper.createFetch(`FindExpertize?TMK_ID=${TMK_ID}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new FindExpertizeModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async FindPacientAsync(ENP) {
        const response = await Helper.createFetch(`FindPacient?ENP=${ENP}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new FindPacientModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetCONTACT_SPRAsync() {
        const response = await Helper.createFetch(`GetCONTACT_SPR`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new CONTACT_SPRModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async ChangeTmkReestrStatusAsync(items) {
        let formData = new FormData();
        items.forEach(item => {
            formData.append("TMK_ID", item.TMK_ID.toString());
        });
        const response = await Helper.createFetch(`ChangeTmkReestrStatus`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return true;
            }
            else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }
    async getTMKListFileAsync(first, rows, filter) {
        let str = Helper.serializeToUrlParam(filter, null);
        const response = await Helper.createFetch(`GetTMKReestrFile?first=${first}&rows=${rows}${str != "" ? `&${str}` : ''}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }
    ErrorResponse(response) {
        throw new Error(`Ошибка запроса: ${response.statusText}(${response.status})`);
    }
    async GetReportXLSAsync() {
        const response = await Helper.createFetch(`GetReportXLSXFile`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }
    async GetReportAsync(param) {
        const response = await Helper.createFetch(`GetReport?${Helper.serializeToUrlParam(param, null)}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKReportTableModel(data.Value);
            }
            else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }
    async DeleteSPRContactAsync(items) {
        let formData = new FormData();
        items.forEach(item => {
            formData.append("ID_CONTACT_INFO", item.ID_CONTACT_INFO.toString());
        });
        const response = await Helper.createFetch(`DeleteSPRContact`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async EditSPRContactAsync(item) {
        const response = await Helper.createFetch(`EditSPRContact`, "POST", Helper.SPRContactModelToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async GetSPRContactAsync() {
        const response = await Helper.createFetch(`GetSPRContact`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new SPRContactModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async DeleteExpertizeAsync(items) {
        let formData = new FormData();
        items.forEach(item => {
            formData.append("EXPERTIZE_ID", item.EXPERTIZE_ID.toString());
        });
        const response = await Helper.createFetch(`DeleteExpertize`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async EditExpertizeAsync(exp) {
        const response = await Helper.createFetch(`EditExpertize`, "POST", Helper.ExpertizeToFormData(exp));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async GetF014Async() {
        const response = await Helper.createFetch(`GetF014`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new F014(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetNMIC_CELLAsync() {
        const response = await Helper.createFetch(`GetNMIC_CELL`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new NMIC_CELL(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetEXPERTSAsync() {
        const response = await Helper.createFetch(`GetEXPERTS`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new EXPERTS(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetNMIC_FULLAsync() {
        const response = await Helper.createFetch(`GetNMIC_FULL`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new NMIC_FULL(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async AddTMKItemAsync(item) {
        const response = await Helper.createFetch(`EditTmkReestr`, "POST", Helper.TMKItemToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async EditTMKItemAsync(item) {
        const response = await Helper.createFetch(`EditTmkReestr`, "POST", Helper.TMKItemToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async DeleteTMKItemAsync(items) {
        let formData = new FormData();
        items.forEach(item => {
            formData.append("TMK_ID", item.TMK_ID.toString());
        });
        const response = await Helper.createFetch(`DeleteTmkReestr`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async SetAsMtrTMKItemAsync(item) {
        let formData = new FormData();
        formData.append("TMK_ID", item.TMK_ID.toString());
        const response = await Helper.createFetch(`SetAsMTR`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async SaveSmoDataTMKItemAsync(item) {
        let formData = new FormData();
        formData.append("TMK_ID", item.TMK_ID.toString());
        formData.append("VID_NHISTORY", item.VID_NHISTORY.toString());
        formData.append("OPLATA", item.OPLATA.toString());
        if (item.SMO_COM)
            formData.append("SMO_COM", item.SMO_COM.toString());
        const response = await Helper.createFetch(`SetSmoData`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            }
            else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                }
                else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
        this.ErrorResponse(response);
    }
    async getTMKListAsync(first, rows, filter) {
        let str = Helper.serializeToUrlParam(filter, null);
        const response = await Helper.createFetch(`GetTMKList?first=${first}&rows=${rows}${str != "" ? `&${str}` : ''}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKList(data.Value);
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetNMIC_OPLATAAsync() {
        const response = await Helper.createFetch(`GetNMIC_OPLATA`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new NMIC_OPLATA(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetNMIC_VID_NHISTORYAsync() {
        const response = await Helper.createFetch(`GetNMIC_VID_NHISTORY`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new NMIC_VID_NHISTORY(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetCODE_SMOAsync() {
        const response = await Helper.createFetch(`GetCODE_SMO`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new SMO(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetCODE_SMO_ReestrAsync() {
        const response = await Helper.createFetch(`GetCODE_SMO_Reestr`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new SMO(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetCODE_MOAsync() {
        const response = await Helper.createFetch(`GetCODE_MO`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new CODE_MO(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetCODE_MO_ReestrAsync() {
        const response = await Helper.createFetch(`GetCODE_MO_Reestr`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new CODE_MO(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetTMISAsync() {
        const response = await Helper.createFetch(`GetTMIS`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new TMIS(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetNMICAsync() {
        const response = await Helper.createFetch(`GetNMIC`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new NMIC(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async GetV002Async() {
        const response = await Helper.createFetch(`GetV002`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x) => new V002(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    async getTMKItemAsync(TMK_ID) {
        const response = await Helper.createFetch(`GetTmkReestr?TMK_ID=${TMK_ID}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKItem(data.Value);
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
};
Repository = __decorate([
    Injectable()
], Repository);
export { Repository };
export class Helper {
    static convertToString(val) {
        return `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }
    static createFetch(url, method = "GET", formData = null) {
        const FetchParam = { credentials: "same-origin", method: method, body: formData };
        return fetch(url, FetchParam);
    }
    static convertModelToFormData(val, formData = new FormData, namespace = '') {
        if ((typeof val !== 'undefined') && val !== null) {
            if (val instanceof Date) {
                formData.append(namespace, Helper.convertToString(val));
            }
            else if (val instanceof Array) {
                for (let i = 0; i < val.length; i++) {
                    this.convertModelToFormData(val[i], formData, namespace + '[' + i + ']');
                }
            }
            else if (typeof val === 'object' && !(val instanceof File)) {
                for (let propertyName in val) {
                    if (val.hasOwnProperty(propertyName)) {
                        this.convertModelToFormData(val[propertyName], formData, namespace ? `${namespace}[${propertyName}]` : propertyName);
                    }
                }
            }
            else if (val instanceof File) {
                formData.append(namespace, val);
            }
            else {
                let txt = val.toString();
                if (typeof val === 'number') {
                    txt = txt.replace(".", ',');
                }
                formData.append(namespace, txt);
            }
        }
        return formData;
    }
    static TMKItemToFormData(item) {
        let data = new FormData();
        if (item.TMK_ID)
            data.append("TMK_ID", item.TMK_ID.toString());
        if (item.NMIC)
            data.append("NMIC", item.NMIC.toString());
        if (item.TMIS)
            data.append("TMIS", item.TMIS.toString());
        if (item.PROFIL)
            data.append("PROFIL", item.PROFIL.toString());
        if (item.DATE_TMK)
            data.append("DATE_TMK", Helper.convertToString(item.DATE_TMK));
        if (item.DATE_PROTOKOL)
            data.append("DATE_PROTOKOL", Helper.convertToString(item.DATE_PROTOKOL));
        if (item.DATE_QUERY)
            data.append("DATE_QUERY", Helper.convertToString(item.DATE_QUERY));
        if (item.DATE_B)
            data.append("DATE_B", Helper.convertToString(item.DATE_B));
        if (item.NHISTORY)
            data.append("NHISTORY", item.NHISTORY);
        if (item.VID_NHISTORY)
            data.append("VID_NHISTORY", item.VID_NHISTORY.toString());
        if (item.ISNOTSMO)
            data.append("ISNOTSMO", item.ISNOTSMO.toString());
        if (item.ENP)
            data.append("ENP", item.ENP);
        if (item.FAM)
            data.append("FAM", item.FAM);
        if (item.IM)
            data.append("IM", item.IM);
        if (item.OT)
            data.append("OT", item.OT);
        if (item.DR)
            data.append("DR", Helper.convertToString(item.DR));
        if (item.NOVOR)
            data.append("NOVOR", item.NOVOR.toString());
        if (item.FAM_P)
            data.append("FAM_P", item.FAM_P);
        if (item.IM_P)
            data.append("IM_P", item.IM_P);
        if (item.OT_P)
            data.append("OT_P", item.OT_P);
        if (item.DR_P)
            data.append("DR_P", Helper.convertToString(item.DR_P));
        if (item.CODE_MO)
            data.append("CODE_MO", item.CODE_MO);
        return data;
    }
    static ExpertizeToFormData(item) {
        return this.convertModelToFormData(item);
    }
    static SPRContactModelToFormData(item) {
        return this.convertModelToFormData(item);
    }
}
Helper.serializeToUrlParam = (obj, prefix) => {
    let str = [];
    let p;
    for (p in obj) {
        if (obj.hasOwnProperty(p)) {
            let k = prefix ? prefix + "[" + p + "]" : p, v = obj[p] instanceof Date ? Helper.convertToString(obj[p]) : obj[p];
            let type = typeof v;
            if (type === "function")
                continue;
            if (type === "string")
                v = v.replace(" ", "%20");
            if (v !== null && v !== "" && !(isNaN(v) && type === "number")) {
                let s = (type === "object") ? Helper.serializeToUrlParam(v, k) : encodeURIComponent(k) + "=" + v;
                if (s !== "")
                    str.push(s);
            }
        }
    }
    return str.join("&");
};
export class CustomResult {
    constructor(_result = true, _ErrMessage = []) {
        this.ErrMessage = [];
        this.Result = _result;
        this.ErrMessage = _ErrMessage;
    }
    get Error() {
        return this.ErrMessage.join();
    }
}
//# sourceMappingURL=Repository.js.map