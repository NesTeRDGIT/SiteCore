export class TMKFilter {
    constructor() {
        this.ENP = null;
        this.FAM = null;
        this.IM = null;
        this.OT = null;
        this.DR = null;
        this.DATE_B_BEGIN = null;
        this.DATE_B_END = null;
        this.DATE_PROTOKOL_BEGIN = null;
        this.DATE_PROTOKOL_END = null;
        this.DATE_QUERY_BEGIN = null;
        this.DATE_QUERY_END = null;
        this.DATE_TMK_BEGIN = null;
        this.DATE_TMK_END = null;
        this.CODE_MO = [];
        this.SMO = [];
        this.VID_NHISTORY = [];
        this.OPLATA = [];
        this.Clear = () => {
            this.ENP = null;
            this.FAM = null;
            this.IM = null;
            this.OT = null;
            this.DR = null;
            this.DATE_B_BEGIN = null;
            this.DATE_B_END = null;
            this.DATE_PROTOKOL_BEGIN = null;
            this.DATE_PROTOKOL_END = null;
            this.DATE_QUERY_BEGIN = null;
            this.DATE_QUERY_END = null;
            this.DATE_TMK_BEGIN = null;
            this.DATE_TMK_END = null;
            this.CODE_MO = [];
            this.SMO = [];
            this.VID_NHISTORY = [];
            this.OPLATA = [];
        };
    }
    get HasValue() {
        return this.ENP != null || this.FAM != null || this.IM != null || this.OT != null || this.DR != null || this.DATE_B_BEGIN != null || this.DATE_B_END != null || this.DATE_PROTOKOL_BEGIN != null ||
            this.DATE_PROTOKOL_END != null || this.DATE_QUERY_BEGIN != null || this.DATE_QUERY_END != null || this.DATE_TMK_BEGIN != null || this.DATE_TMK_END != null ||
            this.CODE_MO.length != 0 || this.SMO.length != 0 || this.VID_NHISTORY.length != 0 || this.OPLATA.length != 0;
    }
}
//# sourceMappingURL=TMKFilter.js.map