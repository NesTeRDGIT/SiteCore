export class TMKReportTableModel {
    constructor(obj) {
        this.Report = [];
        this.Report2 = [];
        this.ReportItog = new ReportTMKRow(null);
        this.Report2Itog = new Report2TMKRow(null);
        if (obj != null) {
            this.Report = obj.Report.map((x) => new ReportTMKRow(x));
            this.Report2 = obj.Report2.map((x) => new Report2TMKRow(x));
            this.ReportItog = new ReportTMKRow(obj.ReportItog);
            this.Report2Itog = new Report2TMKRow(obj.Report2Itog);
        }
    }
    get IsSMO1() {
        return this.Report.find(r => r.SMO) !== undefined;
    }
    get IsMO1() {
        return this.Report.find(r => r.MO) !== undefined;
    }
    get IsNMIC1() {
        return this.Report.find(r => r.NMIC) !== undefined;
    }
    get IsSMO2() {
        return this.Report2.find(r => r.SMO) !== undefined;
    }
    get IsMO2() {
        return this.Report2.find(r => r.MO) !== undefined;
    }
    get IsNMIC2() {
        return this.Report2.find(r => r.NMIC) !== undefined;
    }
}
export class ReportTMKRow {
    constructor(obj) {
        this.SUB = null;
        this.SMO = null;
        this.NMIC = null;
        this.NAM_SMOK = null;
        this.MO = null;
        this.NAM_MOK = null;
        this.C = null;
        this.C_V = null;
        this.C_P = null;
        this.C_MEK_TFOMS = null;
        this.C_MEE_TFOMS = null;
        this.C_EKMP_TFOMS = null;
        this.C_MEK_SMO = null;
        this.C_MEE_SMO = null;
        this.C_EKMP_SMO = null;
        this.C_MEK_D_TFOMS = null;
        this.C_MEE_D_TFOMS = null;
        this.C_EKMP_D_TFOMS = null;
        this.C_MEK_D_SMO = null;
        this.C_MEE_D_SMO = null;
        this.C_EKMP_D_SMO = null;
        this.S_MEK_D_TFOMS = null;
        this.S_MEE_D_TFOMS = null;
        this.S_EKMP_D_TFOMS = null;
        this.S_MEK_D_SMO = null;
        this.S_MEE_D_SMO = null;
        this.S_EKMP_D_SMO = null;
        this.S_SUM_TFOMS = null;
        this.S_FINE_TFOMS = null;
        this.S_SUM_SMO = null;
        this.S_FINE_SMO = null;
        this.S_ALL = null;
        this.S_1_1_3 = null;
        this.S_1_2_2 = null;
        this.S_1_3_2 = null;
        this.S_1_4 = null;
        this.S_3_1 = null;
        this.S_3_2_2 = null;
        this.S_3_2_3 = null;
        this.S_3_2_4 = null;
        this.S_3_2_5 = null;
        this.S_3_2_6 = null;
        this.S_3_3_1 = null;
        this.S_3_4 = null;
        this.S_3_5 = null;
        this.S_3_6 = null;
        this.S_3_7 = null;
        this.S_3_8 = null;
        this.S_3_10 = null;
        this.S_4_2 = null;
        this.S_5_1_3 = null;
        this.S_5_3_1 = null;
        this.S_5_4 = null;
        this.S_5_5 = null;
        this.S_5_6 = null;
        this.S_5_7 = null;
        this.S_5_8 = null;
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
    constructor(obj) {
        this.SUB = null;
        this.SMO = null;
        this.NMIC = null;
        this.NAM_SMOK = null;
        this.MO = null;
        this.NAM_MOK = null;
        this.C = null;
        this.C_V = null;
        this.C_P = null;
        this.C_MEE_SMO = null;
        this.C_EKMP_SMO = null;
        this.C_EKMP_SMO_PROC = null;
        this.C_MEE_D_SMO = null;
        this.C_EKMP_D_SMO = null;
        this.S_MEE_D_SMO = null;
        this.S_EKMP_D_SMO = null;
        this.S_SUM_SMO = null;
        this.S_FINE_SMO = null;
        this.S_ALL = null;
        this.S_1_4_3 = null;
        this.S_1_6_1 = null;
        this.S_1_9 = null;
        this.S_1_10 = null;
        this.S_2_1 = null;
        this.S_2_17 = null;
        this.S_3_1_2 = null;
        this.S_3_1_3 = null;
        this.S_3_1_4 = null;
        this.S_3_1_5 = null;
        this.S_3_2_2 = null;
        this.S_3_2_3 = null;
        this.S_3_2_4 = null;
        this.S_3_2_5 = null;
        this.S_3_3 = null;
        this.S_3_4 = null;
        this.S_3_5 = null;
        this.S_3_6 = null;
        this.S_3_8 = null;
        this.S_3_7 = null;
        this.S_3_10 = null;
        this.S_3_11 = null;
        this.S_3_13 = null;
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
    constructor() {
        this.Date1 = null;
        this.Date2 = null;
        this.IsSMO = null;
        this.IsMO = null;
        this.IsNMIC = null;
        this.VID_NHISTORY = [];
    }
}
//# sourceMappingURL=ReportModel.js.map