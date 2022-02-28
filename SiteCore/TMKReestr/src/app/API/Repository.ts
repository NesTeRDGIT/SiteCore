
import { Injectable } from '@angular/core';
import { FileASP } from "./FileASP";
import {SPRContactModel} from "./SPRContactModel"
import { TMKList } from "./TMKList";
import { NMIC_OPLATA,NMIC_VID_NHISTORY,SMO,CODE_MO,TMIS,NMIC,V002,EXPERTS,F014,NMIC_CELL,NMIC_FULL,CONTACT_SPRModel } from "./SPRModel";
import { TMKFilter } from "./TMKFilter";
import { TMKItem,Expertize } from "./TMKItem";
import { ReportParamModel, TMKReportTableModel } from "./ReportModel";
import { URLHelper } from './URLHelper';
import { FindPacientModel } from './FindPacientModel';
import { FindExpertizeModel } from './FindExpertizeModel';


@Injectable()
export abstract class IRepository {


    abstract FindPacientAsync(ENP:string): Promise<FindPacientModel[]>;
    abstract FindExpertizeAsync(TMK_ID:number): Promise<FindExpertizeModel[]>;


    abstract getTMKListAsync(first:number, rows:number,filter:TMKFilter): Promise<TMKList>;
    abstract getTMKListFileAsync(first:number, rows:number,filter:TMKFilter): Promise<FileASP>;

    abstract GetNMIC_OPLATAAsync(): Promise<NMIC_OPLATA[]>;
    abstract GetNMIC_VID_NHISTORYAsync(): Promise<NMIC_VID_NHISTORY[]>;
    abstract GetCODE_SMOAsync(): Promise<SMO[]>
    abstract GetCODE_SMO_ReestrAsync(): Promise<SMO[]>;
    abstract GetCODE_MOAsync(): Promise<CODE_MO[]>;
    abstract GetCODE_MO_ReestrAsync(): Promise<CODE_MO[]>;
    abstract GetTMISAsync(): Promise<TMIS[]>;
    abstract GetNMICAsync(): Promise<NMIC[]>;
    abstract GetV002Async(): Promise<V002[]>;
    abstract GetF014Async(): Promise<F014[]>;
    abstract GetNMIC_CELLAsync(): Promise<NMIC_CELL[]>;
    abstract GetEXPERTSAsync(): Promise<EXPERTS[]>;
    abstract GetNMIC_FULLAsync(): Promise<NMIC_FULL[]>;
    abstract GetCONTACT_SPRAsync(): Promise<CONTACT_SPRModel[]>;

    

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

    abstract GetReportAsync(param:ReportParamModel): Promise<TMKReportTableModel>;
    abstract GetReportXLSAsync(): Promise<FileASP>;

    abstract ChangeTmkReestrStatusAsync(items: TMKItem[]): Promise<boolean>;
    
    
}



@Injectable()
export class Repository implements IRepository {
    async FindExpertizeAsync(TMK_ID: number): Promise<FindExpertizeModel[]> {
        const response = await Helper.createFetch(`FindExpertize?TMK_ID=${TMK_ID}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new FindExpertizeModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }

    async FindPacientAsync(ENP: string): Promise<FindPacientModel[]> {
       const response = await Helper.createFetch(`FindPacient?ENP=${ENP}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new FindPacientModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
   async GetCONTACT_SPRAsync(): Promise<CONTACT_SPRModel[]> {
        const response = await Helper.createFetch(`GetCONTACT_SPR`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new CONTACT_SPRModel(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
    }
    
    async ChangeTmkReestrStatusAsync(items: TMKItem[]): Promise<boolean> {
        let formData: FormData = new FormData();
        items.forEach(item => {
            formData.append("TMK_ID", item.TMK_ID.toString());
        });
        const response = await Helper.createFetch(`ChangeTmkReestrStatus`, "POST", formData);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return true;
            } else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }

    async getTMKListFileAsync(first: number, rows: number, filter: TMKFilter): Promise<FileASP> {
        let str = Helper.serializeToUrlParam(filter, null);
        const response = await Helper.createFetch(`GetTMKReestrFile?first=${first}&rows=${rows}${str!=""?`&${str}`: ''}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            } else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }


    private ErrorResponse(response: Response) {
        throw new Error(`Ошибка запроса: ${response.statusText}(${response.status})`);
    }




    async GetReportXLSAsync(): Promise<FileASP> {
        const response = await Helper.createFetch(`GetReportXLSXFile`);
   
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            } else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }

    async GetReportAsync(param: ReportParamModel): Promise<TMKReportTableModel> {
        const response = await Helper.createFetch(`GetReport?${Helper.serializeToUrlParam(param, null)}`);
     
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new TMKReportTableModel(data.Value);
            } else {
                throw new Error(data.Value);
            }
        }
        this.ErrorResponse(response);
    }
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
    }

   

    
    async AddTMKItemAsync(item: TMKItem): Promise<CustomResult> {
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
    }
    async getTMKListAsync(first: number, rows: number, filter: TMKFilter): Promise<TMKList> {
       
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
    


    async GetNMIC_OPLATAAsync(): Promise<NMIC_OPLATA[]> {
        const response = await Helper.createFetch(`GetNMIC_OPLATA`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new NMIC_OPLATA(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
    }
   
    async GetCODE_SMO_ReestrAsync(): Promise<SMO[]> {
        const response = await Helper.createFetch(`GetCODE_SMO_Reestr`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((x: any) => new SMO(x));
            }
            throw new Error(data.Value);
        }
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
        this.ErrorResponse(response);
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
              let txt = val.toString();
              if (typeof val === 'number') {
                  txt = txt.replace(".", ',');
              }
              formData.append(namespace, txt);
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
