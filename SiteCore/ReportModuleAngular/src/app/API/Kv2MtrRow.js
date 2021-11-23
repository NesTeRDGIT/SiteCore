export class Kv2MtrRow {
    constructor(obj) {
        if (obj != null) {
            this.NAIM = obj.NAIM;
            this.ED = obj.ED;
            this.NN = obj.NN;
            this.KOL = obj.KOL;
            this.SUM = obj.SUM;
        }
    }
    get isBold() {
        return this.NN.indexOf(".") === -1;
    }
}
//# sourceMappingURL=Kv2MtrRow.js.map