export class EcoRecord {
    constructor(obj) {
        if (obj == null) {
            this.ecoMp = [];
            this.ecoMtr = [];
            this.svodEcoMo = [];
        }
        else {
            this.ecoMp = obj.ECO_MP.map((v) => new EcoMpRow(v));
            this.ecoMtr = obj.ECO_MP.map((v) => new EcoMtrRow(v));
            this.svodEcoMo = obj.SVOD_ECO_MO.map((v) => new EcoMpRow(v));
        }
    }
    get length() {
        return this.ecoMtr.length + this.ecoMp.length + this.svodEcoMo.length;
    }
}
export class EcoMpRow {
    constructor(obj) {
        this.SLUCH_ID = obj.SLUCH_ID;
        this.SMO = obj.SMO;
        this.YEAR = obj.YEAR;
        this.MONTH = obj.MONTH;
        this.CODE_MO = obj.CODE_MO;
        this.NAM_MOK = obj.NAM_MOK;
        this.FAM = obj.FAM;
        this.IM = obj.IM;
        this.OT = obj.OT;
        this.DR = obj.DR;
        this.DATE_1 = obj.DATE_1;
        this.DATE_2 = obj.DATE_2;
        this.N_KSG = obj.N_KSG;
        this.NAME_KSG = obj.NAME_KSG;
        this.SUMV = obj.SUMV;
        this.SUMP = obj.SUMP;
        this.KSLP = obj.KSLP;
        this.KSLP_NAME = obj.KSLP_NAME;
    }
}
export class EcoMtrRow {
    constructor(obj) {
        this.SLUCH_ID = obj.SLUCH_ID;
        this.FAM = obj.FAM;
        this.IM = obj.IM;
        this.OT = obj.OT;
        this.DR = obj.DR;
        this.DATE_1 = obj.DATE_1;
        this.DATE_2 = obj.DATE_2;
        this.SUMV = obj.SUMV;
        this.SUMP = obj.SUMP;
        this.DS1 = obj.DS1;
        this.DS_NAME = obj.DS_NAME;
        this.DPLAT = obj.DPLAT;
        this.NPLAT = obj.NPLAT;
        this.LPU = obj.LPU;
        this.NAM_MOK = obj.NAM_MOK;
        this.C_OKATO = obj.C_OKATO;
        this.NAME_TFK = obj.NAME_TFK;
        this.COMENTSL = obj.COMENTSL;
        this.USL = obj.USL;
    }
}
//# sourceMappingURL=EcoRow.js.map