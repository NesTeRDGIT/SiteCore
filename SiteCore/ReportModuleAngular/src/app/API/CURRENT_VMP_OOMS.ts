export class CURRENT_VMP_OOMS {
    ID: number;
    CODE_MO: string;
    SMO: string;
    FIO: string;
    VID_HMP: string;
    METOD_HMP: string;
    GRP: string;
    DS1: string;
    DATE_1: Date;
    DATE_2: Date;
    TAL_P: Date;
    TAL_D: Date;
    OS_SLUCH: string;
    SUMM: number;
    constructor(obj: any) {
        if (obj != null) {
            this.ID = obj.ID;
            this.CODE_MO = obj.CODE_MO;
            this.SMO = obj.SMO;
            this.FIO = obj.FIO;
            this.VID_HMP = obj.VID_HMP;
            this.METOD_HMP = obj.METOD_HMP;
            this.GRP = obj.GRP;
            this.DS1 = obj.DS1;
            this.DATE_1 = obj.DATE_1;
            this.DATE_2 = obj.DATE_2;
            this.TAL_P = obj.TAL_P;
            this.TAL_D = obj.TAL_D;
            this.OS_SLUCH = obj.OS_SLUCH;
            this.SUMM = obj.SUMM;
        }
    }
}

export class VMP_OOMS {
    SMO: string;
    SLUCH_ID: number;
    CODE_MO: string;
    NAME_MO: string;
    YEAR: number;
    MONTH: number;
    FIO: string;
    W: number;
    VPOLIS: number;
    SPOLIS: string;
    NPOLIS: string;
    AGE: number;
    VID_HMP: number;
    METOD_HMP: number;
    GRP_HMP: string;
    DAYS: number;
    MKB: string;
    SUMP: number;

    constructor(obj: any) {
        if (obj != null) {
            this.SMO = obj.SMO;
            this.SLUCH_ID = obj.SLUCH_ID;
            this.CODE_MO = obj.CODE_MO;
            this.NAME_MO = obj.NAME_MO;
            this.YEAR = obj.YEAR;
            this.MONTH = obj.MONTH;
            this.FIO = obj.FIO;
            this.W = obj.W;
            this.VPOLIS = obj.VPOLIS;
            this.SPOLIS = obj.SPOLIS;
            this.NPOLIS = obj.NPOLIS;
            this.AGE = obj.AGE;
            this.VID_HMP = obj.VID_HMP;
            this.METOD_HMP = obj.METOD_HMP;
            this.GRP_HMP = obj.GRP_HMP;
            this.DAYS = obj.DAYS;
            this.MKB = obj.MKB;
            this.SUMP = obj.SUMP;

        }
    }
}