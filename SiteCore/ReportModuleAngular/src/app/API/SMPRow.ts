export class SMPRow {

    POK: string;
    NN: string;
    KOL: number;
    SUM: number;
    KOL_DET: number;
    SUM_DET: number;

    constructor(obj: any) {
        this.POK = obj.POK;
        this.NN = obj.NN;
        this.KOL = obj.KOL;
        this.SUM = obj.SUM;
        this.KOL_DET = obj.KOL_DET;
        this.SUM_DET = obj.SUM_DET;
    }
}