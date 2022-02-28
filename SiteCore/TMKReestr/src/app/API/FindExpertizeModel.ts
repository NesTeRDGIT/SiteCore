
export class FindExpertizeModel {
    S_TIP: number = null;
    N_ACT: string = null;
    D_ACT: Date = null;
    OSN: FindExpertizeOSNModel[] = [];
    N_EXP: string = null;
    constructor(obj: any) {
        if (obj != null) {
            this.S_TIP = obj.S_TIP;
            this.N_ACT = obj.N_ACT;
            this.N_EXP = obj.N_EXP;
            this.D_ACT = new Date(obj.D_ACT);
            this.OSN = obj.OSN.map((x: any) => new FindExpertizeOSNModel(x));            
        }
    }
}
export class FindExpertizeOSNModel {
    S_OSN: number = null;
    S_SUM: number = null;
    S_FINE: number = null;
    constructor(obj: any) {
        if (obj != null) {
            this.S_OSN = obj.S_OSN;
            this.S_SUM = obj.S_SUM;
            this.S_FINE = obj.S_FINE;
        }
    }
}