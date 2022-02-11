
import { Injectable } from '@angular/core';
import { FileASP } from "./FileASP";
import {SPRContactModel} from "./SPRContactModel"
import { TMKList } from "./TMKList";
import { NMIC_OPLATA,NMIC_VID_NHISTORY,SMO,CODE_MO,TMIS,NMIC,V002,EXPERTS,F014,NMIC_CELL,NMIC_FULL } from "./SPRModel";
import { TMKFilter } from "./TMKFilter";
import { TMKItem,Expertize } from "./TMKItem";
import { stringify } from 'querystring';




@Injectable()
export abstract class IRepository {

    abstract getTMKListAsync(first:number, rows:number,filter:TMKFilter): Promise<TMKList>;

    abstract GetNMIC_OPLATAAsync(): Promise<NMIC_OPLATA[]>;
    abstract GetNMIC_VID_NHISTORYAsync(): Promise<NMIC_VID_NHISTORY[]>;
    abstract GetCODE_SMOAsync(): Promise<SMO[]>;
    abstract GetCODE_MOAsync(): Promise<CODE_MO[]>;
    abstract GetCODE_MO_ReestrAsync(): Promise<CODE_MO[]>;
    abstract GetTMISAsync(): Promise<TMIS[]>;
    abstract GetNMICAsync(): Promise<NMIC[]>;
    abstract GetV002Async(): Promise<V002[]>;
    abstract GetF014Async(): Promise<F014[]>;
    abstract GetNMIC_CELLAsync(): Promise<NMIC_CELL[]>;
    abstract GetEXPERTSAsync(): Promise<EXPERTS[]>;
    abstract GetNMIC_FULLAsync(): Promise<NMIC_FULL[]>;

    abstract getTMKItemAsync(TMK_ID:number): Promise<TMKItem>;


    abstract AddTMKItemAsync(item:TMKItem): Promise<CustomResult>;
    abstract EditTMKItemAsync(item:TMKItem): Promise<CustomResult>;
    abstract DeleteTMKItemAsync(items: TMKItem[]): Promise<CustomResult>;
    abstract SetAsMtrTMKItemAsync(item:TMKItem): Promise<CustomResult>;
    abstract SaveSmoDataTMKItemAsync(item:TMKItem): Promise<CustomResult>;
    abstract EditExpertizeAsync(exp:Expertize): Promise<CustomResult>;
    abstract DeleteExpertizeAsync(items:Expertize[]): Promise<CustomResult>;

    abstract GetSPRContactAsync(): Promise<SPRContactModel[]>;
    abstract EditSPRContactAsync(item:SPRContactModel): Promise<CustomResult>;
    abstract DeleteSPRContactAsync(items:SPRContactModel[]): Promise<CustomResult>;
    
    
}



@Injectable()
export class Repository implements IRepository {
    async DeleteSPRContactAsync(items: SPRContactModel[]): Promise<CustomResult> {
        let formData: FormData = new FormData();
        items.forEach(item => {
            formData.append("ID_CONTACT_INFO", item.ID_CONTACT_INFO.toString());
        });


        const response = await Helper.createFetch(`DeleteSPRContact`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async EditSPRContactAsync(item: SPRContactModel): Promise<CustomResult> {
        const response = await Helper.createFetch(`EditSPRContact`, "POST", Helper.SPRContactModelToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async GetSPRContactAsync(): Promise<SPRContactModel[]> {
        const response = await Helper.createFetch(`GetSPRContact`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new SPRContactModel(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async DeleteExpertizeAsync(items: Expertize[]): Promise<CustomResult> {
        let formData: FormData = new FormData();
        items.forEach(item => {
            formData.append("EXPERTIZE_ID", item.EXPERTIZE_ID.toString());
        });


        const response = await Helper.createFetch(`DeleteExpertize`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async EditExpertizeAsync(exp: Expertize): Promise<CustomResult> {
        const response = await Helper.createFetch(`EditExpertize`, "POST", Helper.ExpertizeToFormData(exp));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async GetF014Async(): Promise<F014[]> {
        const response = await Helper.createFetch(`GetF014`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new F014(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetNMIC_CELLAsync(): Promise<NMIC_CELL[]> {
        const response = await Helper.createFetch(`GetNMIC_CELL`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC_CELL(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetEXPERTSAsync(): Promise<EXPERTS[]> {
        const response = await Helper.createFetch(`GetEXPERTS`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new EXPERTS(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetNMIC_FULLAsync(): Promise<NMIC_FULL[]> {
        const response = await Helper.createFetch(`GetNMIC_FULL`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC_FULL(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

   

    
    async AddTMKItemAsync(item: TMKItem): Promise<CustomResult> {
        const response = await Helper.createFetch(`EditTmkReestr`, "POST", Helper.convertModelToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async EditTMKItemAsync(item: TMKItem): Promise<CustomResult> {
        const response = await Helper.createFetch(`EditTmkReestr`, "POST", Helper.TMKItemToFormData(item));
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async DeleteTMKItemAsync(items: TMKItem[]): Promise<CustomResult> {
        let formData: FormData = new FormData();
        items.forEach(item => {
            formData.append("TMK_ID", item.TMK_ID.toString());
        });


        const response = await Helper.createFetch(`DeleteTmkReestr`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async SetAsMtrTMKItemAsync(item: TMKItem): Promise<CustomResult> {
        let formData: FormData = new FormData();
        formData.append("TMK_ID", item.TMK_ID.toString());
        const response = await Helper.createFetch(`SetAsMTR`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async SaveSmoDataTMKItemAsync(item: TMKItem): Promise<CustomResult> {
        
        let formData: FormData = new FormData();
        formData.append("TMK_ID", item.TMK_ID.toString());
        formData.append("VID_NHISTORY", item.VID_NHISTORY.toString());      
        formData.append("OPLATA", item.OPLATA.toString());
        if(item.SMO_COM)
            formData.append("SMO_COM", item.SMO_COM.toString());
        const response = await Helper.createFetch(`SetSmoData`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new CustomResult();
            } else {
                if (Array.isArray(data.Value)) {
                    return new CustomResult(false, data.Value);
                } else {
                    return new CustomResult(false, [data.Value]);
                }
            }
        }
    }
    async getTMKListAsync(first:number, rows:number,filter:TMKFilter): Promise<TMKList>
    {
        let str = Helper.serializeToUrlParam(filter, null);
        const response = await Helper.createFetch(`GetTMKListNew?first=${first}&rows=${rows}${str!=""?`&${str}`: ''}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKList(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    


    async GetNMIC_OPLATAAsync(): Promise<NMIC_OPLATA[]> {
        const response = await Helper.createFetch(`GetNMIC_OPLATA`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC_OPLATA(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetNMIC_VID_NHISTORYAsync(): Promise<NMIC_VID_NHISTORY[]> {
        const response = await Helper.createFetch(`GetNMIC_VID_NHISTORY`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC_VID_NHISTORY(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetCODE_SMOAsync(): Promise<SMO[]> {
        const response = await Helper.createFetch(`GetCODE_SMO`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new SMO(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetCODE_MOAsync(): Promise<CODE_MO[]> {
        const response = await Helper.createFetch(`GetCODE_MO`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new CODE_MO(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetCODE_MO_ReestrAsync(): Promise<CODE_MO[]> {
        const response = await Helper.createFetch(`GetCODE_MO_Reestr`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new CODE_MO(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetTMISAsync(): Promise<TMIS[]> {
        const response = await Helper.createFetch(`GetTMIS`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new TMIS(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetNMICAsync(): Promise<NMIC[]> {
        const response = await Helper.createFetch(`GetNMIC`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetV002Async(): Promise<V002[]> {
        const response = await Helper.createFetch(`GetV002`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new V002(x));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getTMKItemAsync(TMK_ID: number): Promise<TMKItem> {
        const response = await Helper.createFetch(`GetTmkReestr?TMK_ID=${TMK_ID}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKItem(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    
}




export class Helper
{
    static convertToString(val: Date): string {
        return `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }

    static createFetch(url: string, method: string = "GET", formData: FormData=null): Promise<Response> {
        const FetchParam: RequestInit = { credentials: "same-origin", method: method, body:formData };
        return fetch(url, FetchParam);
    }


    static convertModelToFormData(val:any, formData:FormData = new FormData, namespace = ''):FormData {
        if ((typeof val !== 'undefined') && val !== null) {
          if (val instanceof Date) {
            formData.append(namespace, Helper.convertToString(val));
          } else if (val instanceof Array) {
            for (let i = 0; i < val.length; i++) {
              this.convertModelToFormData(val[i], formData, namespace + '[' + i + ']');
            }
          } else if (typeof val === 'object' && !(val instanceof File)) {
            for (let propertyName in val) {
              if (val.hasOwnProperty(propertyName)) {
                this.convertModelToFormData(val[propertyName], formData, namespace ? `${namespace}[${propertyName}]` : propertyName);
              }
            }
          } else if (val instanceof File) {
            formData.append(namespace, val);
          } else {
            formData.append(namespace, val.toString());
          }
        }
        return formData;
      }

    static TMKItemToFormData(item: TMKItem): FormData {
        let data: FormData = new FormData();
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

    static ExpertizeToFormData(item: Expertize): FormData {
        return this.convertModelToFormData(item);
    }
    static SPRContactModelToFormData(item: SPRContactModel): FormData {
        return this.convertModelToFormData(item);
    }

    

    static serializeToUrlParam = (obj, prefix) => {
        let str = [];
        let p: any;
        for (p in obj) {
            if (obj.hasOwnProperty(p)) {
                let k = prefix ? prefix + "[" + p + "]" : p,
                    v = obj[p] instanceof Date ? Helper.convertToString(obj[p]) : obj[p];      
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
    }


}

export class CustomResult {
    Result: boolean;
    ErrMessage: string[] = [];
    get Error():string
    {
        return this.ErrMessage.join();
    }
    constructor(_result: boolean = true, _ErrMessage: string[] = []) {
        this.Result = _result;
        this.ErrMessage = _ErrMessage;
    }


}
