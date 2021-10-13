export class ZpzEffectiveness {
    CODE_MO: string;
    NAM_MOK: string;
    COUNT: number;
    COUNT_OSN: number;
    DOL_OSN: number;
    BAL_OSN: number;
    C_MEE: number;
    C_MEE_ERR: number;
    DOL_MEE: number;
    BAL_MEE: number;
    C_EKMP: number;
    C_EKMP_ERR: number;
    DOL_EKMP: number;
    BAL_EKMP: number;
    SUM_BAL: number;
    constructor(obj: any) {
        this.CODE_MO = obj.CODE_MO;
        this.NAM_MOK = obj.NAM_MOK;
        this.COUNT = obj.COUNT;
        this.COUNT_OSN = obj.COUNT_OSN;
        this.DOL_OSN = obj.DOL_OSN;
        this.BAL_OSN = obj.BAL_OSN;
        this.C_MEE = obj.C_MEE;
        this.C_MEE_ERR = obj.C_MEE_ERR;
        this.DOL_MEE = obj.DOL_MEE;
        this.BAL_MEE = obj.BAL_MEE;
        this.C_EKMP = obj.C_EKMP;
        this.C_EKMP_ERR = obj.C_EKMP_ERR;
        this.DOL_EKMP = obj.DOL_EKMP;
        this.BAL_EKMP = obj.BAL_EKMP;
        this.SUM_BAL = obj.SUM_BAL;

    }
}