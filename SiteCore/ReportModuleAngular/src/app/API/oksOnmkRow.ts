export class oksOnmkRow {
    SLUCH_ID: number;
    SMO: string;
    CODE_MO: string;
    NAM_MOK: string;
    LEV: string;
    PROFIL: string;
    FORMA: string;
    FAM: string;
    IM: string;
    OT: string;
    DR: Date;
    SPOLIS: string;
    NPOLIS: string;
    DATE_1: Date;
    DATE_2: Date;
    DS1: string;
    DS1_NAME: string;
    N_KSG: string;
    NAME_KSG: string;
    RSLT: string;
    ISHOD: string;
    SUMV: number;
    SUMP: number;

    constructor(obj: any) {
        this.SLUCH_ID = obj.SLUCH_ID;
        this.SMO = obj.SMO;
        this.CODE_MO = obj.CODE_MO;
        this.LEV = obj.LEV;
        this.PROFIL = obj.PROFIL;
        this.FORMA = obj.FORMA;
        this.FAM = obj.FAM;
        this.IM = obj.IM;
        this.OT = obj.OT;
        this.DR = obj.DR;
        this.SPOLIS = obj.SPOLIS;
        this.NPOLIS = obj.NPOLIS;
        this.DATE_1 = obj.DATE_1;
        this.DATE_2 = obj.DATE_2;
        this.DS1 = obj.DS1;
        this.DS1_NAME = obj.DS1_NAME;
        this.N_KSG = obj.N_KSG;
        this.NAME_KSG = obj.NAME_KSG;
        this.RSLT = obj.RSLT;
        this.ISHOD = obj.ISHOD;
        this.SUMV = obj.SUMV;
        this.SUMP = obj.SUMP;
       

    }
}