var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from '@angular/core';
import { CURRENT_VMP_OOMS, VMP_OOMS } from "./current_vmp_ooms";
import { FileASP } from "./FileASP";
import { AbortRow } from "./AbortRow";
import { EcoRecord } from "./EcoRow";
import { KohlRow } from "./KOHLRow";
import { oksOnmkRow } from "./oksOnmkRow";
import { ZpzEffectiveness } from "./ZpzEffectiveness";
import { ResultControlDET, ResultControlVZR, ResultControl } from "./ResultControlReportRows";
import { SMPRow } from "./SMPRow";
import { DataBaseStateRow } from "./DataBaseStateRow";
import { PensRow } from "./PensRow";
let IRepository = class IRepository {
};
IRepository = __decorate([
    Injectable()
], IRepository);
export { IRepository };
let Repository = class Repository {
    async getPensReportAsync(year) {
        const response = await fetch(`GetPensReport?year=${year}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new PensRow(value));
                ;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getPensXlsAsync() {
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
    async getDBState() {
        const response = await fetch(`getDBState`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new DataBaseStateRow(value));
                ;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getSmpReportAsync(dt1, dt2) {
        const response = await fetch(`GetSmpReport?dt1=${this.convertToString(dt1)}&dt2=${this.convertToString(dt2)}`);
        if (response.ok) {
            const data = await response.json();
            if (data.Result) {
                return data.Value.map((value) => new SMPRow(value));
                ;
            }
            throw new Error(data.Value);
        }
        throw new Error(`${response.status}: ${response.statusText}`);
    }
    async getSmpXlsAsync() {
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
    async GetResultControlReport(dt1, dt2) {
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
    async GetResultControlXls() {
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
    async getVmpPeriodReportAsync(dt1, dt2) {
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
    async getVmpPeriodReportXlsAsync() {
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
    async getVmpReportAsync() {
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
    async getVmpReportXlsAsync() {
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
    async getGetAbortReportAsync(year) {
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
    async getAbortXlsAsync() {
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
    async getEcoReport(year, month) {
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
    async getEcoXlsAsync() {
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
    async getKohlReport(dt1, dt2) {
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
    async getKohlXlsAsync() {
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
    convertToString(val) {
        return `${val.getFullYear()}-${(val.getMonth() + 1).toString().padStart(2, "0")}-${val.getDate().toString().padStart(2, "0")}`;
    }
    async getOksOnmkReport(year) {
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
    async getOksOnmkXlsAsync() {
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
    async GetEffectivenessReport(dt1, dt2) {
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
    async GetEffectivenessXls() {
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
};
Repository = __decorate([
    Injectable()
], Repository);
export { Repository };
//# sourceMappingURL=Repository.js.map