export class FindPacientModel {
    POLIS: string = null;
    FAM: string = null;
    IM: string = null;
    OT: string = null;
    DR: Date = null;
    SMO: string = null;
    DBEG: Date = null;
    DSTOP: Date = null;
    get FIO(): string {
        return `${this.FAM} ${this.IM} ${this.OT}`.trim()
    }
    constructor(obj: any) {
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
}
export class FindPacientSelected {
    isPred: boolean;
    Pacient: FindPacientModel;
    constructor(_isPred: boolean, _Pacient: FindPacientModel) {
        this.isPred = _isPred;
        this.Pacient = _Pacient;
    }
}
