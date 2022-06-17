export class DispRecord {
    DispDet: DispDetRow[] = [];
    DispVzr: DispVzrRow[] = [];
    ProfVzr: ProfVzrRow[] = [];
    ProfDet: ProfDetRow[] = [];
    constructor(obj: any) {
        if (obj !== null) {
            this.DispDet = obj.DispDet.map((v) => new DispDetRow(v));
            this.DispVzr = obj.DispVzr.map((v) => new DispVzrRow(v));
            this.ProfVzr = obj.ProfVzr.map((v) => new ProfVzrRow(v));
            this.ProfDet = obj.ProfDet.map((v) => new ProfDetRow(v));
        }
    }

    get length() {
        return this.DispDet.length + this.DispVzr.length + this.ProfVzr.length + this.ProfDet.length;
    }
}
export class DispDetRow {

    constructor(obj: any) {
        if (obj === null) return;
        this.SMO = obj.SMO;
        this.POK = obj.POK;
        this.SUMV = obj.SUMV;
        this.KOL = obj.KOL;
        this.SUM = obj.SUM;
        this.KOL_VBR = obj.KOL_VBR;
        this.SUM_VBR = obj.SUM_VBR;
        this.SUMP = obj.SUMP;
        this.KOL_P = obj.KOL_P;
        this.SUM_P = obj.SUM_P;
        this.KOL_VBR_P = obj.KOL_VBR_P;
        this.SUM_VBR_P = obj.SUM_VBR_P;
        this.C_GRP_1 = obj.C_GRP_1;
        this.C_GRP_2 = obj.C_GRP_2;
        this.C_GRP_3 = obj.C_GRP_3;
        this.C_GRP_4 = obj.C_GRP_4;
        this.C_GRP_5 = obj.C_GRP_5;
    }

    SMO: string;
    POK: string;
    SUMV: number;
    KOL: number;
    SUM: number;
    KOL_VBR: number;
    SUM_VBR: number;
    SUMP:number;
    KOL_P: number;
    SUM_P: number;
    KOL_VBR_P: number;
    SUM_VBR_P: number;
    C_GRP_1: number;
    C_GRP_2: number;
    C_GRP_3: number;
    C_GRP_4: number;
    C_GRP_5: number;
}
export class DispVzrRow {
    constructor(obj: any) {
        if (obj === null) return;
        this.SMO = obj.SMO;
        this.POK = obj.POK;
        this.ST3 = obj.ST3;
        this.ST4 = obj.ST4;
        this.ST5 = obj.ST5;
        this.ST6 = obj.ST6;
        this.ST7 = obj.ST7;
        this.ST8 = obj.ST8;
        this.ST9 = obj.ST9;
        this.ST10 = obj.ST10;
        this.ST11 = obj.ST11;
        this.ST12 = obj.ST12;
        this.ST13 = obj.ST13;
        this.ST14 = obj.ST14;
        this.ST15 = obj.ST15;
        this.ST16 = obj.ST16;
        this.ST17 = obj.ST17;
        this.ST18 = obj.ST18;
        this.ST19 = obj.ST19;
        this.ST20 = obj.ST20;
        this.ST21 = obj.ST21;
        this.ST22 = obj.ST22;
        this.ST23 = obj.ST23;
        this.ST24 = obj.ST24;
        this.ST25 = obj.ST25;
        this.ST26 = obj.ST26;
        this.ST27 = obj.ST27;
        this.ST28 = obj.ST28;
        this.ST29 = obj.ST29;
        this.ST30 = obj.ST30;
        this.ST31 = obj.ST31;
        this.ST32 = obj.ST32;
        this.ST33 = obj.ST33;
        this.ST34 = obj.ST34;
        this.ST35 = obj.ST35;
        this.ST36 = obj.ST36;
        this.ST37 = obj.ST37;
        this.ST38 = obj.ST38;
    }

    SMO: string;
    POK: string;
    ST3: number;
    ST4: number;
    ST5: number;
    ST6: number;
    ST7: number;
    ST8: number;
    ST9: number;
    ST10: number;
    ST11: number;
    ST12: number;
    ST13: number;
    ST14: number;
    ST15: number;
    ST16: number;
    ST17: number;
    ST18: number;
    ST19: number;
    ST20: number;
    ST21: number;
    ST22: number;
    ST23: number;
    ST24: number;
    ST25: number;
    ST26: number;
    ST27: number;
    ST28: number;
    ST29: number;
    ST30: number;
    ST31: number;
    ST32: number;
    ST33: number;
    ST34: number;
    ST35: number;
    ST36: number;
    ST37: number;
    ST38: number;
}
export class ProfVzrRow {
    constructor(obj: any) {
        if (obj === null) return;
        this.SMO = obj.SMO;
        this.NAM_SMO = obj.NAM_SMO;
        this.NN = obj.NN;
        this.GRP = obj.GRP;
        this.KOL = obj.KOL;
        this.SUM = obj.SUM;
        this.KOL_P = obj.KOL_P;
        this.SUM_P = obj.SUM_P;
    }

    SMO: string;
    NAM_SMO: string;
    NN: string;
    GRP: string;
    KOL: number;
    SUM: number;
    KOL_P: number;
    SUM_P: number;
}
export class ProfDetRow {
    constructor(obj: any) {
        if (obj === null) return;
        this.SMO = obj.SMO;
        this.ST2 = obj.ST2;
        this.ST3 = obj.ST3;
        this.ST4 = obj.ST4;
        this.ST5 = obj.ST5;
        this.ST6 = obj.ST6;
        this.ST7 = obj.ST7;
        this.ST8 = obj.ST8;
        this.ST9 = obj.ST9;
        this.ST10 = obj.ST10;
        this.ST11 = obj.ST11;
        this.ST12 = obj.ST12;
        this.ST13 = obj.ST13;
        this.ST14 = obj.ST14;
        this.ST15 = obj.ST15;
        this.ST16 = obj.ST16;
        this.ST17 = obj.ST17;
        this.ST18 = obj.ST18;
        this.ST19 = obj.ST19;
        this.ST20 = obj.ST20;
        this.ST21 = obj.ST21;
        this.ST22 = obj.ST22;
        this.ST23 = obj.ST23;
        this.ST24 = obj.ST24;
        this.ST25 = obj.ST25;
        this.ST26 = obj.ST26;
        this.ST27 = obj.ST27;
        this.ST28 = obj.ST28;
    }

    SMO: string;
    ST2: number;
    ST3: number;
    ST4: number;
    ST5: number;
    ST6: number;
    ST7: number;
    ST8: number;
    ST9: number;
    ST10: number;
    ST11: number;
    ST12: number;
    ST13: number;
    ST14: number;
    ST15: number;
    ST16: number;
    ST17: number;
    ST18: number;
    ST19: number;
    ST20: number;
    ST21: number;
    ST22: number;
    ST23: number;
    ST24: number;
    ST25: number;
    ST26: number;
    ST27: number;
    ST28: number;
}
