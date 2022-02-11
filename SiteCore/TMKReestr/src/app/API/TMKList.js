export class TMKList {
    constructor(obj) {
        this.Count = 0;
        this.Items = [];
        if (obj != null) {
            this.Count = obj.Count;
            this.Items = obj.Items.map((x) => new TMKListModel(x));
        }
    }
}
export class TMKListModel {
    constructor(obj) {
        this.TMK_ID = null;
        this.ENP = null;
        this.CODE_MO = null;
        this.FIO = null;
        this.DATE_B = null;
        this.SMO = null;
        this.VID_NHISTORY = null;
        this.OPLATA = null;
        this.isEXP = null;
        this.TMIS = null;
        this.NMIC = null;
        this.MEK = [];
        this.MEE = [];
        this.EKMP = [];
        if (obj != null) {
            this.ENP = obj.ENP;
            this.CODE_MO = obj.CODE_MO;
            this.FIO = obj.FIO;
            if (obj.DATE_B != null)
                this.DATE_B = new Date(obj.DATE_B);
            if (obj.DATE_QUERY != null)
                this.DATE_QUERY = new Date(obj.DATE_QUERY);
            if (obj.DATE_PROTOKOL != null)
                this.DATE_PROTOKOL = new Date(obj.DATE_PROTOKOL);
            if (obj.DATE_TMK != null)
                this.DATE_TMK = new Date(obj.DATE_TMK);
            this.SMO = obj.SMO;
            this.VID_NHISTORY = obj.VID_NHISTORY;
            this.OPLATA = obj.OPLATA;
            this.STATUS = obj.STATUS;
            this.STATUS_COM = obj.STATUS_COM;
            this.isEXP = obj.isEXP;
            this.TMK_ID = obj.TMK_ID;
            this.TMIS = obj.TMIS;
            this.NMIC = obj.NMIC;
            this.MEK = obj.MEK.map((x) => new TMKListExpModel(x));
            this.MEE = obj.MEE.map((x) => new TMKListExpModel(x));
            this.EKMP = obj.EKMP.map((x) => new TMKListExpModel(x));
        }
    }
    get StatusText() {
        let result = "";
        switch (this.STATUS) {
            case TMKListModelStatusEnum.Closed:
                result = "Закрыта";
                break;
            case TMKListModelStatusEnum.Open:
                result = "Открыта";
                break;
            case TMKListModelStatusEnum.Error:
                result = `Ошибочная: ${this.STATUS_COM}`;
                break;
        }
        if (this.isEXP) {
            result += ", есть экспертиза";
        }
        return result;
    }
}
export class TMKListExpModel {
    constructor(obj) {
        this.DATEACT = null;
        this.OSN = [];
        if (obj != null) {
            if (obj.DATEACT != null)
                this.DATEACT = new Date(obj.DATEACT);
            if (obj.OSN != null)
                this.OSN = obj.OSN;
        }
    }
}
export var TMKListModelStatusEnum;
(function (TMKListModelStatusEnum) {
    TMKListModelStatusEnum[TMKListModelStatusEnum["Open"] = 0] = "Open";
    TMKListModelStatusEnum[TMKListModelStatusEnum["Closed"] = 1] = "Closed";
    TMKListModelStatusEnum[TMKListModelStatusEnum["Error"] = -1] = "Error";
})(TMKListModelStatusEnum || (TMKListModelStatusEnum = {}));
//# sourceMappingURL=TMKList.js.map