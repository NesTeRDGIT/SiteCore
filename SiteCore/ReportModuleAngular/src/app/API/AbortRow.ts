export class AbortRow {
    Text: string;
    DS: string;
    USL: number;
    C: number;
    SUMV: number;
    constructor(obj: any) {
        this.Text = obj.Text;
        this.DS = obj.DS;
        this.USL = obj.USL;
        this.C = obj.C;
        this.SUMV = obj.SUMV;
    }
}