export class FindExpertizeModel {
    constructor(obj) {
        this.S_TIP = null;
        this.N_ACT = null;
        this.D_ACT = null;
        this.OSN = [];
        this.N_EXP = null;
        if (obj != null) {
            this.S_TIP = obj.S_TIP;
            this.N_ACT = obj.N_ACT;
            this.N_EXP = obj.N_EXP;
            this.D_ACT = new Date(obj.D_ACT);
            this.OSN = obj.OSN.map((x) => new FindExpertizeOSNModel(x));
        }
    }
}
export class FindExpertizeOSNModel {
    constructor(obj) {
        this.S_OSN = null;
        this.S_SUM = null;
        this.S_FINE = null;
        if (obj != null) {
            this.S_OSN = obj.S_OSN;
            this.S_SUM = obj.S_SUM;
            this.S_FINE = obj.S_FINE;
        }
    }
}
//# sourceMappingURL=FindExpertizeModel.js.map