export class TMKFilter {
    ENP: string = null;
    FAM: string = null;
    IM: string = null;
    OT: string = null;
    DR: Date = null;
    DATE_B_BEGIN: Date = null;
    DATE_B_END: Date = null;
    DATE_PROTOKOL_BEGIN: Date = null;
    DATE_PROTOKOL_END: Date = null;
    DATE_QUERY_BEGIN: Date = null;
    DATE_QUERY_END: Date = null;
    DATE_TMK_BEGIN: Date = null;
    DATE_TMK_END: Date = null;
    CODE_MO: string[] = [];
    SMO: string[] = [];
    VID_NHISTORY: number[] = [];
    OPLATA: number[] = [];

    Clear = () => {
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
    }
    get HasValue(): boolean{
        return this.ENP!=null || this.FAM!=null || this.IM!=null || this.OT!=null || this.DR!=null || this.DATE_B_BEGIN!=null || this.DATE_B_END!=null || this.DATE_PROTOKOL_BEGIN!=null ||
        this.DATE_PROTOKOL_END!=null || this.DATE_QUERY_BEGIN!=null || this.DATE_QUERY_END!=null || this.DATE_TMK_BEGIN!=null || this.DATE_TMK_END!=null || 
        this.CODE_MO.length!=0 || this.SMO.length!=0 || this.VID_NHISTORY.length!=0 || this.OPLATA.length!=0;
    }

}