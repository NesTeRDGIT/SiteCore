import {FindPacientModel} from '../API/FindPacientModel'

export class TMKItem {
    get IsAuto(): boolean {
        return this.PacAuto != null || this.PredAuto != null;
    }
    private PacAuto: FindPacientModel = null;
    private PredAuto: FindPacientModel = null;
    public SetAuto(val: FindPacientModel, isPred: boolean) {
       
        this.PacAuto = null;
        this.PredAuto = null;
        if (isPred) {
            this.PredAuto = val;
        }
        else {
            this.PacAuto = val;
        }
    }
  

    TMK_ID: number = null;
    NMIC: number = null;
    TMIS: number = null;
    PROFIL: number = null;
    DATE_TMK: Date = null;
    DATE_PROTOKOL: Date = null;
    DATE_QUERY: Date = null;
    DATE_B: Date = null;    
    NHISTORY: string = null;
    VID_NHISTORY: number = null;   
    DATE_INVITE: Date = null;
    _ISNOTSMO: boolean = false;
    get ISNOTSMO(): boolean { 
        if (!this.IsAuto)
            return this._ISNOTSMO;
        return false;
    }
    set ISNOTSMO(val: boolean) {
        this._ISNOTSMO = val;
    }
    _ENP: string = null;
    get ENP(): string {
        if (!this.IsAuto) {
            return this._ENP;
        }
        if (this.PacAuto != null) {
            return this.PacAuto?.POLIS;
        }
        else {
            return this.PredAuto?.POLIS;
        }
    }
    set ENP(val: string) {
        this._ENP = val;
    }
    _FAM: string = null;
    get FAM(): string {
        if (!this.IsAuto) {
            return this._FAM;
        }
        return this.PacAuto?.FAM;
    }
    set FAM(val: string) {
        this._FAM = val;
    }
    _IM: string = null;
    get IM(): string {
        if (!this.IsAuto) {
            return this._IM;
        }
        return this.PacAuto?.IM;
    }
    set IM(val: string) {
        this._IM = val;
    }
    _OT: string = null;
    get OT(): string {
        if (!this.IsAuto) {
            return this._OT;
        }
        return this.PacAuto?.OT;
    }
    set OT(val: string) {
        this._OT = val;
    }
    _DR: Date = null;
    get DR(): Date {
        if (!this.IsAuto) {
            return this._DR;
        }
        return this.PacAuto?.DR;
    }
    set DR(val: Date) {
        this._DR = val;
    }
    _NOVOR: boolean = false;
    get NOVOR(): boolean {
        if (this.PredAuto!=null) {
            return true;
        }
        return this._NOVOR;
    }
    set NOVOR(val: boolean) {
        this._NOVOR = val;
    }
    _FAM_P: string = null;
    get FAM_P(): string {
        if (!this.IsAuto) {
            return this._FAM_P;
        }
        return this.PredAuto?.FAM;
    }
    set FAM_P(val: string) {
        this._FAM_P = val;
    }
    _IM_P: string = null;
    get IM_P(): string {
        if (!this.IsAuto) {
            return this._IM_P;
        }
        return this.PredAuto?.IM;
    }
    set IM_P(val: string) {
        this._IM_P = val;
    }
    _OT_P: string = null;
    get OT_P(): string {
        if (!this.IsAuto) {
            return this._OT_P;
        }
        return this.PredAuto?.OT;
    }
    set OT_P(val: string) {
        this._OT_P = val;
    }
    _DR_P: Date = null;
    get DR_P(): Date {
        if (!this.IsAuto) {
            return this._DR_P;
        }
        return this.PredAuto?.DR;
    }
    set DR_P(val: Date) {
        this._DR_P = val;
    }
    CODE_MO: string = null;
    STATUS: StatusTMKRow = null;
    STATUS_COM: string = null;
    SMO: string = null;
    OPLATA: number = null;
    SMO_COM: string = null;
    DATE_EDIT:Date=null;
    Expertize:Expertize[] = [];
    constructor(obj: any) {
        if (obj != null) {
            this.TMK_ID = obj.TMK_ID;
            this.NMIC = obj.NMIC;
            this.TMIS = obj.TMIS;
            this.PROFIL = obj.PROFIL;
            if (obj.DATE_TMK)
                this.DATE_TMK = new Date(obj.DATE_TMK);
            if (obj.DATE_PROTOKOL)
                this.DATE_PROTOKOL = new Date(obj.DATE_PROTOKOL);
            if (obj.DATE_QUERY)
                this.DATE_QUERY = new Date(obj.DATE_QUERY);
            if (obj.DATE_B)
                this.DATE_B = new Date(obj.DATE_B);
            this.NHISTORY = obj.NHISTORY;
            this.VID_NHISTORY = obj.VID_NHISTORY;
            if (obj.DATE_INVITE)
                this.DATE_INVITE = new Date(obj.DATE_INVITE);
            this.ISNOTSMO = obj.ISNOTSMO;
            this.ENP = obj.ENP;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            if (obj.DR)
                this.DR = new Date(obj.DR);
            this.NOVOR = obj.NOVOR;
            this.FAM_P = obj.FAM_P;
            this.IM_P = obj.IM_P;
            this.OT_P = obj.OT_P;
            if (obj.DR_P)
                this.DR_P = new Date(obj.DR_P);
            this.CODE_MO = obj.CODE_MO;
            this.STATUS = obj.STATUS;
            this.STATUS_COM = obj.STATUS_COM;
            this.SMO = obj.SMO;
            this.OPLATA = obj.OPLATA;
            this.SMO_COM = obj.SMO_COM;
            this.Expertize = obj.Expertize.map((x: any) => new Expertize(x));
        }
    }
}


export enum StatusTMKRow {
    Open = 0,
    Closed = 1,
    Error = -1
}


export class Expertize {
    TMK_ID: number = null;
    EXPERTIZE_ID: number = null;
    S_TIP: ExpType = null;       
    DATEACT: Date = null;
    NUMACT: string = null;
    ISCOROLLARY: boolean = false;
    CELL: number = null;
   
    ISRECOMMENDMEDDOC: boolean = false;
    ISNOTRECOMMEND: boolean = false;
    
    FULL: number = null;
    NOTPERFORM: string = null;
    ISOSN: boolean = false;
    FIO: string = null;
    N_EXP: string = null;   
   
    OSN: ExpertiseOSN[] = [];
    constructor(obj: any) {
        if (obj != null) {
            this.NUMACT = obj.NUMACT;
            if(obj.DATEACT)
                this.DATEACT = new Date(obj.DATEACT);
            this.FIO = obj.FIO;
            this.FULL = obj.FULL;
            this.ISCOROLLARY = obj.ISCOROLLARY;
            this.ISNOTRECOMMEND = obj.ISNOTRECOMMEND;
            this.CELL = obj.CELL;
            this.ISOSN = obj.ISOSN;
            this.ISRECOMMENDMEDDOC = obj.ISRECOMMENDMEDDOC;
            this.NOTPERFORM = obj.NOTPERFORM;
            this.N_EXP = obj.N_EXP;
            this.S_TIP = obj.S_TIP;
            this.EXPERTIZE_ID = obj.EXPERTIZE_ID;
            this.TMK_ID = obj.TMK_ID;
            this.OSN = obj.OSN.map((x:any) => new ExpertiseOSN(x));
        }
    }
}

export enum ExpType {
    MEK = 1,
    MEE = 2,
    EKMP = 3
}


export class ExpertiseOSN {
    OSN_ID: number = null;
    S_OSN: number = null;
    S_COM: string = null;
    S_SUM: number = 0;
    S_FINE: number = 0;
    constructor(obj: any) {
        if (obj != null) {
            this.OSN_ID = obj.OSN_ID;
            this.S_OSN = obj.S_OSN;
            this.S_COM = obj.S_COM;
            this.S_SUM = obj.S_SUM;
            this.S_FINE = obj.S_FINE;
        }
    }
}