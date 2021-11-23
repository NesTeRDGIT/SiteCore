export class Kv2MtrRow {
    NAIM: string;
    ED: string;
    NN: string;
    KOL: number;
    SUM: number;

    get isBold() {
       
        return this.NN.indexOf(".") === -1;
    }

    constructor(obj: any) {
        if (obj != null) {
            this.NAIM = obj.NAIM;
            this.ED = obj.ED;
            this.NN = obj.NN;
            this.KOL = obj.KOL;
            this.SUM = obj.SUM;
        }
    }
}