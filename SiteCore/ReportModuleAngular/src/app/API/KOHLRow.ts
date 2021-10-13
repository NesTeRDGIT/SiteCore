export class KohlRow {
    SLUCH_ID: number;
    C_OKATO: string;
    NAME_TFK: string;
    LPU: string;
    NAM_MOK: string;
    FAM: string;
    IM: string;
    OT: string;
    DR: Date;
    DATE_1: Date;
    DATE_2: Date;
    DS1: string;
    DS1_NAME: string;
    SUMV: number;
    COMENTSL: string;
    SUMP: number;
    USL: string;

    constructor(obj: any) {
        this.SLUCH_ID = obj.SLUCH_ID;
        this.C_OKATO = obj.C_OKATO;
        this.NAME_TFK = obj.NAME_TFK;
        this.LPU = obj.LPU;
        this.NAM_MOK = obj.NAM_MOK;
        this.FAM = obj.FAM;
        this.IM = obj.IM;
        this.OT = obj.OT;
        this.DR = obj.DR;
        this.DATE_1 = obj.DATE_1;
        this.DATE_2 = obj.DATE_2;
        this.DS1 = obj.DS1;
        this.DS1_NAME = obj.DS1_NAME;
        this.SUMV = obj.SUMV;
        this.COMENTSL = obj.COMENTSL;
        this.SUMP = obj.SUMP;
        this.USL = obj.USL;

    }
}
