
import { Injectable } from '@angular/core';
import { CURRENT_VMP_OOMS, VMP_OOMS } from "./current_vmp_ooms";
import { FileASP } from "./FileASP";
import { AbortRow } from "./AbortRow";
import { EcoRecord } from "./EcoRow";
import { KohlRow } from "./KOHLRow"
import { oksOnmkRow } from "./oksOnmkRow"
import { ZpzEffectiveness } from "./ZpzEffectiveness";
import { ResultControlDET, ResultControlVZR, ResultControl } from "./ResultControlReportRows";
import { SMPRow } from "./SMPRow";
import { DataBaseStateRow } from "./DataBaseStateRow";
import { PensRow } from "./PensRow";
import { DispRecord } from "./DispEntity";
import { Kv2MtrRow } from "./Kv2MtrRow";
import { DliRecord } from "./DliRow";
@Injectable()
export abstract class IRepository {
    abstract getVmpReportAsync(): Promise<CURRENT_VMP_OOMS[]>;
    abstract getVmpReportXlsAsync(): Promise<FileASP>;
    abstract getGetAbortReportAsync(year: number): Promise<AbortRow[]>;
    abstract getAbortXlsAsync(): Promise<FileASP>;
    abstract getEcoReport(year: number, month: number): Promise<EcoRecord>;
    abstract getEcoXlsAsync(): Promise<FileASP>;
    abstract getKohlReport(dt1: Date, dt2: Date): Promise<KohlRow[]>;
    abstract getKohlXlsAsync(): Promise<FileASP>;
    abstract getOksOnmkReport(year: number): Promise<oksOnmkRow[]>;
    abstract getOksOnmkXlsAsync(): Promise<FileASP>;
    abstract GetEffectivenessReport(dt1: Date, dt2: Date): Promise<ZpzEffectiveness[]>;
    abstract GetEffectivenessXls(): Promise<FileASP>;
    abstract GetResultControlReport(dt1: Date, dt2: Date): Promise<ResultControl>;
    abstract GetResultControlXls(): Promise<FileASP>;

    abstract getSmpReportAsync(dt1: Date, dt2: Date): Promise<SMPRow[]>;
    abstract getSmpXlsAsync(): Promise<FileASP>;

    abstract getDBState(): Promise<DataBaseStateRow[]>;

    abstract getPensReportAsync(year: number): Promise<PensRow[]>;
    abstract getPensXlsAsync(): Promise<FileASP>;


    abstract getVmpPeriodReportAsync(dt1: Date, dt2: Date): Promise<VMP_OOMS[]>;
    abstract getVmpPeriodReportXlsAsync(): Promise<FileASP>;

    abstract getDispReport(year: number, month: number): Promise<DispRecord>;
    abstract getDispXlsAsync(): Promise<FileASP>;

    abstract getKv2MtrReportAsync(year: number, month: number): Promise<Kv2MtrRow[]>;
    abstract getKv2MtrXlsAsync(): Promise<FileASP>;

    abstract getDliReportAsync(year: number): Promise<DliRecord>;
    abstract getDliXlsAsync(): Promise<FileASP>;


}



@Injectable()
export class Repository implements IRepository {
    defaultFetchParam: RequestInit = { credentials: "same-origin" };


    async getDliReportAsync(year: number): Promise<DliRecord> {

        const response = await fetch(`GetDliReport?year=${year}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new DliRecord(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getDliXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetDliXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getKv2MtrReportAsync(year: number, month: number): Promise<Kv2MtrRow[]> {

        const response = await fetch(`GetKv2MtrReport?year=${year}&month=${month}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new Kv2MtrRow(value));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getKv2MtrXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetKv2MtrXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getDispReport(year: number, month: number): Promise<DispRecord> {

        const response = await fetch(`GetDispReport?year=${year}&month=${month}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new DispRecord(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getDispXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetDispXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getPensReportAsync(year: number): Promise<PensRow[]> {
       
        const response = await fetch(`GetPensReport?year=${year}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new PensRow(value));;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getPensXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetPensXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getDBState(): Promise<DataBaseStateRow[]> {
        const response = await fetch(`getDBState`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                
                return data.Value.map((value) => new DataBaseStateRow(value));;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getSmpReportAsync(dt1: Date, dt2: Date): Promise<SMPRow[]> {
        const response = await fetch(`GetSmpReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new SMPRow(value));;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getSmpXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetSmpXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetResultControlReport(dt1: Date, dt2: Date): Promise<ResultControl> {
        const response = await fetch(`GetResultControlReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                const res = new ResultControl();
                res.DET = data.Value.DET.map((value) => new ResultControlDET(value));
                res.VZR = data.Value.VZR.map((value) => new ResultControlVZR(value));
                return res;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async GetResultControlXls(): Promise<FileASP> {
        debugger;
        const response = await fetch("GetResultControlXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getVmpPeriodReportAsync(dt1: Date, dt2: Date): Promise<VMP_OOMS[]> {

        const response = await fetch(`GetVMPPeriodReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((obj) => new VMP_OOMS(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getVmpPeriodReportXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetVMPPeriodXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getVmpReportAsync(): Promise<CURRENT_VMP_OOMS[]> {
      
        const response = await fetch("GetVMPReport", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((obj) => new CURRENT_VMP_OOMS(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getVmpReportXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetVmpReportXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getGetAbortReportAsync(year: number): Promise<AbortRow[]> {
        const response = await fetch(`GetAbortReport?YEAR=${year}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((obj) => new AbortRow(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getAbortXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetAbortXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

 
    async getEcoReport(year: number, month: number): Promise<EcoRecord> {
        const response = await fetch(`GetEcoReport?year=${year}&month=${month}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {

                return new EcoRecord(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getEcoXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetEcoXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }


    async getKohlReport(dt1: Date, dt2: Date): Promise<KohlRow[]> {
        const response = await fetch(`GetKohlReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`, this.defaultFetchParam);
     
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {

                return data.Value.map((obj) => new KohlRow(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getKohlXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetKohlXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    convertToString(val: Date):string {
        return `${val.getFullYear()}-${(val.getMonth()+1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }

   
    async getOksOnmkReport(year: number): Promise<oksOnmkRow[]> {
        const response = await fetch(`GetOksOnmkReport?year=${year}`, this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {

                return data.Value.map((obj) => new oksOnmkRow(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async getOksOnmkXlsAsync(): Promise<FileASP> {
        const response = await fetch("GetOksOnmkXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }



    async GetEffectivenessReport(dt1: Date, dt2: Date): Promise<ZpzEffectiveness[]> {
        const response = await fetch(`GetEffectivenessReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`, this.defaultFetchParam);

        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((obj) => new ZpzEffectiveness(obj));
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }

    async GetEffectivenessXls(): Promise<FileASP> {
        const response = await fetch("GetEffectivenessXls", this.defaultFetchParam);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return new FileASP(data.Value);
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }



    
    


}




