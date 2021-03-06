export class TMKItem {
    constructor(obj) {
        this.PacAuto = null;
        this.PredAuto = null;
        this.TMK_ID = null;
        this.NMIC = null;
        this.TMIS = null;
        this.PROFIL = null;
        this.DATE_TMK = null;
        this.DATE_PROTOKOL = null;
        this.DATE_QUERY = null;
        this.DATE_B = null;
        this.NHISTORY = null;
        this.VID_NHISTORY = null;
        this.DATE_INVITE = null;
        this._ISNOTSMO = false;
        this._ENP = null;
        this._FAM = null;
        this._IM = null;
        this._OT = null;
        this._DR = null;
        this._NOVOR = false;
        this._FAM_P = null;
        this._IM_P = null;
        this._OT_P = null;
        this._DR_P = null;
        this.CODE_MO = null;
        this.STATUS = null;
        this.STATUS_COM = null;
        this.SMO = null;
        this.OPLATA = null;
        this.SMO_COM = null;
        this.DATE_EDIT = null;
        this.Expertize = [];
        if (obj != null) {
            this.TMK_ID = obj.TMK_ID;
            this.NMIC = obj.NMIC;
            this.TMIS = obj.TMIS;
            this.PROFIL = obj.PROFIL;
            if (obj.DATE_TMK)
                this.DATE_TMK = new Date(obj.DATE_TMK);
            if (obj.DATE_PROTOKOL)
                this.DATE_PROTOKOL = new Date(obj.DATE_PROTOKOL);
            if (obj.DATE_QUERY)
                this.DATE_QUERY = new Date(obj.DATE_QUERY);
            if (obj.DATE_B)
                this.DATE_B = new Date(obj.DATE_B);
            this.NHISTORY = obj.NHISTORY;
            this.VID_NHISTORY = obj.VID_NHISTORY;
            if (obj.DATE_INVITE)
                this.DATE_INVITE = new Date(obj.DATE_INVITE);
            this.ISNOTSMO = obj.ISNOTSMO;
            this.ENP = obj.ENP;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            if (obj.DR)
                this.DR = new Date(obj.DR);
            this.NOVOR = obj.NOVOR;
            this.FAM_P = obj.FAM_P;
            this.IM_P = obj.IM_P;
            this.OT_P = obj.OT_P;
            if (obj.DR_P)
                this.DR_P = new Date(obj.DR_P);
            this.CODE_MO = obj.CODE_MO;
            this.STATUS = obj.STATUS;
            this.STATUS_COM = obj.STATUS_COM;
            this.SMO = obj.SMO;
            this.OPLATA = obj.OPLATA;
            this.SMO_COM = obj.SMO_COM;
            this.Expertize = obj.Expertize.map((x) => new Expertize(x));
        }
    }
    get IsAuto() {
        return this.PacAuto != null || this.PredAuto != null;
    }
    SetAuto(val, isPred) {
        this.PacAuto = null;
        this.PredAuto = null;
        if (isPred) {
            this.PredAuto = val;
        }
        else {
            this.PacAuto = val;
        }
    }
    get ISNOTSMO() {
        if (!this.IsAuto)
            return this._ISNOTSMO;
        return false;
    }
    set ISNOTSMO(val) {
        this._ISNOTSMO = val;
    }
    get ENP() {
        var _a, _b;
        if (!this.IsAuto) {
            return this._ENP;
        }
        if (this.PacAuto != null) {
            return (_a = this.PacAuto) === null || _a === void 0 ? void 0 : _a.POLIS;
        }
        else {
            return (_b = this.PredAuto) === null || _b === void 0 ? void 0 : _b.POLIS;
        }
    }
    set ENP(val) {
        this._ENP = val;
    }
    get FAM() {
        var _a;
        if (!this.IsAuto) {
            return this._FAM;
        }
        return (_a = this.PacAuto) === null || _a === void 0 ? void 0 : _a.FAM;
    }
    set FAM(val) {
        this._FAM = val;
    }
    get IM() {
        var _a;
        if (!this.IsAuto) {
            return this._IM;
        }
        return (_a = this.PacAuto) === null || _a === void 0 ? void 0 : _a.IM;
    }
    set IM(val) {
        this._IM = val;
    }
    get OT() {
        var _a;
        if (!this.IsAuto) {
            return this._OT;
        }
        return (_a = this.PacAuto) === null || _a === void 0 ? void 0 : _a.OT;
    }
    set OT(val) {
        this._OT = val;
    }
    get DR() {
        var _a;
        if (!this.IsAuto) {
            return this._DR;
        }
        return (_a = this.PacAuto) === null || _a === void 0 ? void 0 : _a.DR;
    }
    set DR(val) {
        this._DR = val;
    }
    get NOVOR() {
        if (this.PredAuto != null) {
            return true;
        }
        return this._NOVOR;
    }
    set NOVOR(val) {
        this._NOVOR = val;
    }
    get FAM_P() {
        var _a;
        if (!this.IsAuto) {
            return this._FAM_P;
        }
        return (_a = this.PredAuto) === null || _a === void 0 ? void 0 : _a.FAM;
    }
    set FAM_P(val) {
        this._FAM_P = val;
    }
    get IM_P() {
        var _a;
        if (!this.IsAuto) {
            return this._IM_P;
        }
        return (_a = this.PredAuto) === null || _a === void 0 ? void 0 : _a.IM;
    }
    set IM_P(val) {
        this._IM_P = val;
    }
    get OT_P() {
        var _a;
        if (!this.IsAuto) {
            return this._OT_P;
        }
        return (_a = this.PredAuto) === null || _a === void 0 ? void 0 : _a.OT;
    }
    set OT_P(val) {
        this._OT_P = val;
    }
    get DR_P() {
        var _a;
        if (!this.IsAuto) {
            return this._DR_P;
        }
        return (_a = this.PredAuto) === null || _a === void 0 ? void 0 : _a.DR;
    }
    set DR_P(val) {
        this._DR_P = val;
    }
}
export var StatusTMKRow;
(function (StatusTMKRow) {
    StatusTMKRow[StatusTMKRow["Open"] = 0] = "Open";
    StatusTMKRow[StatusTMKRow["Closed"] = 1] = "Closed";
    StatusTMKRow[StatusTMKRow["Error"] = -1] = "Error";
})(StatusTMKRow || (StatusTMKRow = {}));
export class Expertize {
    constructor(obj) {
        this.TMK_ID = null;
        this.EXPERTIZE_ID = null;
        this.S_TIP = null;
        this.DATEACT = null;
        this.NUMACT = null;
        this.ISCOROLLARY = false;
        this.CELL = null;
        this.ISRECOMMENDMEDDOC = false;
        this.ISNOTRECOMMEND = false;
        this.FULL = null;
        this.NOTPERFORM = null;
        this.ISOSN = false;
        this.FIO = null;
        this.N_EXP = null;
        this.OSN = [];
        if (obj != null) {
            this.NUMACT = obj.NUMACT;
            if (obj.DATEACT)
                this.DATEACT = new Date(obj.DATEACT);
            this.FIO = obj.FIO;
            this.FULL = obj.FULL;
            this.ISCOROLLARY = obj.ISCOROLLARY;
            this.ISNOTRECOMMEND = obj.ISNOTRECOMMEND;
            this.CELL = obj.CELL;
            this.ISOSN = obj.ISOSN;
            this.ISRECOMMENDMEDDOC = obj.ISRECOMMENDMEDDOC;
            this.NOTPERFORM = obj.NOTPERFORM;
            this.N_EXP = obj.N_EXP;
            this.S_TIP = obj.S_TIP;
            this.EXPERTIZE_ID = obj.EXPERTIZE_ID;
            this.TMK_ID = obj.TMK_ID;
            this.OSN = obj.OSN.map((x) => new ExpertiseOSN(x));
        }
    }
}
export var ExpType;
(function (ExpType) {
    ExpType[ExpType["MEK"] = 1] = "MEK";
    ExpType[ExpType["MEE"] = 2] = "MEE";
    ExpType[ExpType["EKMP"] = 3] = "EKMP";
})(ExpType || (ExpType = {}));
export class ExpertiseOSN {
    constructor(obj) {
        this.OSN_ID = null;
        this.S_OSN = null;
        this.S_COM = null;
        this.S_SUM = 0;
        this.S_FINE = 0;
        if (obj != null) {
            this.OSN_ID = obj.OSN_ID;
            this.S_OSN = obj.S_OSN;
            this.S_COM = obj.S_COM;
            this.S_SUM = obj.S_SUM;
            this.S_FINE = obj.S_FINE;
        }
    }
}
//# sourceMappingURL=TMKItem.js.map