export class TMKReportTableModel {
    Report: ReportTMKRow[] = [];
    Report2: Report2TMKRow[] = [];
    ReportItog: ReportTMKRow = new ReportTMKRow(null);
    Report2Itog: Report2TMKRow = new Report2TMKRow(null);
    constructor(obj: any) {
        if (obj != null) {
            this.Report = obj.Report.map((x: any) => new ReportTMKRow(x));
            this.Report2 = obj.Report2.map((x: any) => new Report2TMKRow(x));
            this.ReportItog = new ReportTMKRow(obj.ReportItog);
            this.Report2Itog = new Report2TMKRow(obj.Report2Itog);
        }
    }
    get IsSMO1():boolean{
       return this.Report.find(r=>r.SMO)!==undefined;
    }
    get IsMO1():boolean{
        return this.Report.find(r=>r.MO)!==undefined;
     }
     get IsNMIC1():boolean{
        return this.Report.find(r=>r.NMIC)!==undefined;
     }

     get IsSMO2():boolean{
        return this.Report2.find(r=>r.SMO)!==undefined;
     }
     get IsMO2():boolean{
         return this.Report2.find(r=>r.MO)!==undefined;
      }
      get IsNMIC2():boolean{
         return this.Report2.find(r=>r.NMIC)!==undefined;
      }
}

export class ReportTMKRow {
    SUB: string = null;
    SMO: string = null;
    NMIC: string = null;
    NAM_SMOK: string = null;
    MO: string = null;
    NAM_MOK: string = null;
    C: number = null;
    C_V: number = null;
    C_P: number = null;
    C_MEK_TFOMS: number = null;
    C_MEE_TFOMS: number = null;
    C_EKMP_TFOMS: number = null;
    C_MEK_SMO: number = null;
    C_MEE_SMO: number = null;
    C_EKMP_SMO: number = null;
    C_MEK_D_TFOMS: number = null;
    C_MEE_D_TFOMS: number = null;
    C_EKMP_D_TFOMS: number = null;
    C_MEK_D_SMO: number = null;
    C_MEE_D_SMO: number = null;
    C_EKMP_D_SMO: number = null;
    S_MEK_D_TFOMS: number = null;
    S_MEE_D_TFOMS: number = null;
    S_EKMP_D_TFOMS: number = null;
    S_MEK_D_SMO: number = null;
    S_MEE_D_SMO: number = null;
    S_EKMP_D_SMO: number = null;
    S_SUM_TFOMS: number = null;
    S_FINE_TFOMS: number = null;
    S_SUM_SMO: number = null;
    S_FINE_SMO: number = null;
    S_ALL: number = null;
    S_1_1_3: number = null;
    S_1_2_2: number = null;
    S_1_3_2: number = null;
    S_1_4: number = null;
    S_3_1: number = null;
    S_3_2_2: number = null;
    S_3_2_3: number = null;
    S_3_2_4: number = null;
    S_3_2_5: number = null;
    S_3_2_6: number = null;
    S_3_3_1: number = null;
    S_3_4: number = null;
    S_3_5: number = null;
    S_3_6: number = null;
    S_3_7: number = null;
    S_3_8: number = null;
    S_3_10: number = null;
    S_4_2: number = null;
    S_5_1_3: number = null;
    S_5_3_1: number = null;
    S_5_4: number = null;
    S_5_5: number = null;
    S_5_6: number = null;
    S_5_7: number = null;
    S_5_8: number = null;
    constructor(obj: any) {
        if (obj != null) {
            this.SUB = obj.SUB;
            this.SMO = obj.SMO;
            this.NMIC = obj.NMIC;
            this.NAM_SMOK = obj.NAM_SMOK;
            this.MO = obj.MO;
            this.NAM_MOK = obj.NAM_MOK;
            this.C = obj.C;
            this.C_V = obj.C_V;
            this.C_P = obj.C_P;
            this.C_MEK_TFOMS = obj.C_MEK_TFOMS;
            this.C_MEE_TFOMS = obj.C_MEE_TFOMS;
            this.C_EKMP_TFOMS = obj.C_EKMP_TFOMS;
            this.C_MEK_SMO = obj.C_MEK_SMO;
            this.C_MEE_SMO = obj.C_MEE_SMO;
            this.C_EKMP_SMO = obj.C_EKMP_SMO;
            this.C_MEK_D_TFOMS = obj.C_MEK_D_TFOMS;
            this.C_MEE_D_TFOMS = obj.C_MEE_D_TFOMS;
            this.C_EKMP_D_TFOMS = obj.C_EKMP_D_TFOMS;
            this.C_MEK_D_SMO = obj.C_MEK_D_SMO;
            this.C_MEE_D_SMO = obj.C_MEE_D_SMO;
            this.C_EKMP_D_SMO = obj.C_EKMP_D_SMO;
            this.S_MEK_D_TFOMS = obj.S_MEK_D_TFOMS;
            this.S_MEE_D_TFOMS = obj.S_MEE_D_TFOMS;
            this.S_EKMP_D_TFOMS = obj.S_EKMP_D_TFOMS;
            this.S_MEK_D_SMO = obj.S_MEK_D_SMO;
            this.S_MEE_D_SMO = obj.S_MEE_D_SMO;
            this.S_EKMP_D_SMO = obj.S_EKMP_D_SMO;
            this.S_SUM_TFOMS = obj.S_SUM_TFOMS;
            this.S_FINE_TFOMS = obj.S_FINE_TFOMS;
            this.S_SUM_SMO = obj.S_SUM_SMO;
            this.S_FINE_SMO = obj.S_FINE_SMO;
            this.S_ALL = obj.S_ALL;
            this.S_1_1_3 = obj.S_1_1_3;
            this.S_1_2_2 = obj.S_1_2_2;
            this.S_1_3_2 = obj.S_1_3_2;
            this.S_1_4 = obj.S_1_4;
            this.S_3_1 = obj.S_3_1;
            this.S_3_2_2 = obj.S_3_2_2;
            this.S_3_2_3 = obj.S_3_2_3;
            this.S_3_2_4 = obj.S_3_2_4;
            this.S_3_2_5 = obj.S_3_2_5;
            this.S_3_2_6 = obj.S_3_2_6;
            this.S_3_3_1 = obj.S_3_3_1;
            this.S_3_4 = obj.S_3_4;
            this.S_3_5 = obj.S_3_5;
            this.S_3_6 = obj.S_3_6;
            this.S_3_7 = obj.S_3_7;
            this.S_3_8 = obj.S_3_8;
            this.S_3_10 = obj.S_3_10;
            this.S_4_2 = obj.S_4_2;
            this.S_5_1_3 = obj.S_5_1_3;
            this.S_5_3_1 = obj.S_5_3_1;
            this.S_5_4 = obj.S_5_4;
            this.S_5_5 = obj.S_5_5;
            this.S_5_6 = obj.S_5_6;
            this.S_5_7 = obj.S_5_7;
            this.S_5_8 = obj.S_5_8;
        }
    }
}
export class Report2TMKRow {
    SUB: string = null;
    SMO: string = null;
    NMIC: string = null;
    NAM_SMOK: string = null;
    MO: string = null;
    NAM_MOK: string = null;
    C: number = null;
    C_V: number = null;
    C_P: number = null;
    C_MEE_SMO: number = null;
    C_EKMP_SMO: number = null;
    C_EKMP_SMO_PROC: number = null;
    C_MEE_D_SMO: number = null;
    C_EKMP_D_SMO: number = null;
    S_MEE_D_SMO: number = null;
    S_EKMP_D_SMO: number = null;
    S_SUM_SMO: number = null;
    S_FINE_SMO: number = null;
    S_ALL: number = null;
    S_1_4_3: number = null;
    S_1_6_1: number = null;
    S_1_9: number = null;
    S_1_10: number = null;
    S_2_1: number = null;
    S_2_17: number = null;
    S_3_1_2: number = null;
    S_3_1_3: number = null;
    S_3_1_4: number = null;
    S_3_1_5: number = null;
    S_3_2_2: number = null;
    S_3_2_3: number = null;
    S_3_2_4: number = null;
    S_3_2_5: number = null;
    S_3_3: number = null;
    S_3_4: number = null;
    S_3_5: number = null;
    S_3_6: number = null;
    S_3_8: number = null;
    S_3_7: number = null;
    S_3_10: number = null;
    S_3_11: number = null;
    S_3_13: number = null;

    constructor(obj: any) {
        if (obj != null) {
            this.SUB = obj.SUB;
            this.SMO = obj.SMO;
            this.NMIC = obj.NMIC;
            this.NAM_SMOK = obj.NAM_SMOK;
            this.MO = obj.MO;
            this.NAM_MOK = obj.NAM_MOK;
            this.C = obj.C;
            this.C_V = obj.C_V;
            this.C_P = obj.C_P;
            this.C_MEE_SMO = obj.C_MEE_SMO;
            this.C_EKMP_SMO = obj.C_EKMP_SMO;
            this.C_EKMP_SMO_PROC = obj.C_EKMP_SMO_PROC;
            this.C_MEE_D_SMO = obj.C_MEE_D_SMO;
            this.C_EKMP_D_SMO = obj.C_EKMP_D_SMO;
            this.S_MEE_D_SMO = obj.S_MEE_D_SMO;
            this.S_EKMP_D_SMO = obj.S_EKMP_D_SMO;
            this.S_SUM_SMO = obj.S_SUM_SMO;
            this.S_FINE_SMO = obj.S_FINE_SMO;
            this.S_ALL = obj.S_ALL;
            this.S_1_4_3 = obj.S_1_4_3;
            this.S_1_6_1 = obj.S_1_6_1;
            this.S_1_9 = obj.S_1_9;
            this.S_1_10 = obj.S_1_10;
            this.S_2_1 = obj.S_2_1;
            this.S_2_17 = obj.S_2_17;
            this.S_3_1_2 = obj.S_3_1_2;
            this.S_3_1_3 = obj.S_3_1_3;
            this.S_3_1_4 = obj.S_3_1_4;
            this.S_3_1_5 = obj.S_3_1_5;
            this.S_3_2_2 = obj.S_3_2_2;
            this.S_3_2_3 = obj.S_3_2_3;
            this.S_3_2_4 = obj.S_3_2_4;
            this.S_3_2_5 = obj.S_3_2_5;
            this.S_3_3 = obj.S_3_3;
            this.S_3_4 = obj.S_3_4;
            this.S_3_5 = obj.S_3_5;
            this.S_3_6 = obj.S_3_6;
            this.S_3_8 = obj.S_3_8;
            this.S_3_7 = obj.S_3_7;
            this.S_3_10 = obj.S_3_10;
            this.S_3_11 = obj.S_3_11;
            this.S_3_13 = obj.S_3_13;
            debugger;
        }
    }
}


export class ReportParamModel {
    Date1: Date = null;
    Date2: Date = null;
    IsSMO: boolean = null;
    IsMO: boolean = null;
    IsNMIC: boolean = null;
    VID_NHISTORY: number[] = [];


}