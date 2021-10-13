
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
    


}
@Injectable()
export class Repository implements IRepository {
    async getPensReportAsync(year: number): Promise<PensRow[]> {
       
        const response = await fetch(`GetPensReport?year=${year}`);
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
        const response = await fetch("GetPensXls");
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
        const response = await fetch(`getDBState`);
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
        const response = await fetch(`GetSmpReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`);
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
        const response = await fetch("GetSmpXls");
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
        const response = await fetch(`GetResultControlReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`);
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
        const response = await fetch("GetResultControlXls");
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

        const response = await fetch(`GetVMPPeriodReport?dt1=${this.convertToString(dt1)}&&dt2=${this.convertToString(dt2)}`);
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
        const response = await fetch("GetVMPPeriodXls");
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
      
        const response = await fetch("GetVMPReport");
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
        const response = await fetch("GetVmpReportXls");
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
        const response = await fetch(`GetAbortReport?YEAR=${year}`);
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
        const response = await fetch("GetAbortXls");
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
        const response = await fetch(`GetEcoReport?year=${year}&&month=${month}`);
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
        const response = await fetch("GetEcoXls");
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
        const response = await fetch(`GetKohlReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`);
     
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
        const response = await fetch("GetKohlXls");
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
        const response = await fetch(`GetOksOnmkReport?year=${year}`);
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
        const response = await fetch("GetOksOnmkXls");
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
        const response = await fetch(`GetEffectivenessReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`);

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
        const response = await fetch("GetEffectivenessXls");
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




