export class DliRecord {
    tbl1: DliTbl1Row[] = [];
    tbl2: DliTb21Row[] =[];
    constructor(obj: any) {
        if (obj != null) {
            this.tbl1 = obj.tbl1.map(v => new DliTbl1Row(v));
            this.tbl2 = obj.tbl2.map(v => new DliTb21Row(v));
        }
    }
    get length() {
        return this.tbl1.length + this.tbl2.length;
    }
}



export class DliTbl1Row {
    SMO: string;
    NAME: string;
    VID: string;
    K: number;
    ENP: number;
    S: number;
    K_MTR: number;
    ENP_MTR: number;
    S_MTR: number;
    constructor(obj: any) {
        if (obj != null) {
            this.SMO = obj.SMO;
            this.NAME = obj.NAME;
            this.K = obj.K;
            this.ENP = obj.ENP;
            this.S = obj.S;
            this.K_MTR = obj.K_MTR;
            this.ENP_MTR = obj.ENP_MTR;
            this.S_MTR = obj.S_MTR;
        }
    }
}

export class DliTb21Row {
    NAME_TFK: string;
    OKRUG: number;
    C_KT: string;
    E_KT: number;
    S_KT: number;

    C_MRT: string;
    E_MRT: number;
    S_MRT: number;

    C_USI: string;
    E_USI: number;
    S_USI: number;

    C_ENDO: string;
    E_ENDO: number;
    S_ENDO: number;

    C_MOL: string;
    E_MOL: number;
    S_MOL: number;

    C_GIST: string;
    E_GIST: number;
    S_GIST: number;

    constructor(obj: any) {
        if (obj != null) {
            this.NAME_TFK = obj.NAME_TFK;
            this.OKRUG = obj.OKRUG;

            this.C_KT = obj.C_KT;
            this.E_KT = obj.E_KT;
            this.S_KT = obj.S_KT;

            this.C_MRT = obj.C_MRT;
            this.E_MRT = obj.E_MRT;
            this.S_MRT = obj.S_MRT;

            this.C_USI = obj.C_USI;
            this.E_USI = obj.E_USI;
            this.S_USI = obj.S_USI;

            this.C_ENDO = obj.C_ENDO;
            this.E_ENDO = obj.E_ENDO;
            this.S_ENDO = obj.S_ENDO;

            this.C_MOL = obj.C_MOL;
            this.E_MOL = obj.E_MOL;
            this.S_MOL = obj.S_MOL;

            this.C_GIST = obj.C_GIST;
            this.E_GIST = obj.E_GIST;
            this.S_GIST = obj.S_GIST;

        }
    }
}