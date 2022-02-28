export class FindPacientModel {
    constructor(obj) {
        this.POLIS = null;
        this.FAM = null;
        this.IM = null;
        this.OT = null;
        this.DR = null;
        this.SMO = null;
        this.DBEG = null;
        this.DSTOP = null;
        if (obj != null) {
            this.POLIS = obj.POLIS;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            this.SMO = obj.SMO;
            if (obj.DR != null)
                this.DR = new Date(obj.DR);
            if (obj.DBEG != null)
                this.DBEG = new Date(obj.DBEG);
            if (obj.DSTOP != null)
                this.DSTOP = new Date(obj.DSTOP);
        }
    }
    get FIO() {
        return `${this.FAM} ${this.IM} ${this.OT}`.trim();
    }
}
export class FindPacientSelected {
    constructor(_isPred, _Pacient) {
        this.isPred = _isPred;
        this.Pacient = _Pacient;
    }
}
//# sourceMappingURL=FindPacientModel.js.map