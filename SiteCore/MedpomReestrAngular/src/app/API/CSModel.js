export class TitleResult {
    constructor(obj) {
        this.PersonItems = [];
        if (obj != null) {
            this.TotalRecord = obj.TotalRecord;
            this.PersonItems = obj.PersonItems.map(v => new PersonItem(v));
        }
    }
}
export class PersonItem {
    constructor(obj) {
        this.FAM = "";
        this.IM = "";
        this.OT = "";
        this.DR = "";
        this.POLIS = "";
        this.DOC = "";
        this.STATUS = null;
        this.STATUS_TEXT = "";
        this.CURRENT_SMO = "";
        this.CODE_MO = "";
        this.STATUS_SEND = null;
        this.STATUS_SEND_TEXT = "";
        if (obj != null) {
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            this.DR = obj.DR;
            this.POLIS = obj.POLIS;
            this.DOC = obj.DOC;
            this.STATUS = obj.STATUS;
            this.STATUS_TEXT = obj.STATUS_TEXT;
            this.CURRENT_SMO = obj.CURRENT_SMO;
            this.CS_LIST_IN_ID = obj.CS_LIST_IN_ID;
            this.CODE_MO = obj.CODE_MO;
            this.STATUS_SEND = obj.STATUS_SEND;
            this.STATUS_SEND_TEXT = obj.STATUS_SEND_TEXT;
            this.DATE_CREATE = obj.DATE_CREATE;
        }
    }
    Merge(obj) {
        this.FAM = obj.FAM;
        this.IM = obj.IM;
        this.OT = obj.OT;
        this.DR = obj.DR;
        this.POLIS = obj.POLIS;
        this.DOC = obj.DOC;
        this.STATUS = obj.STATUS;
        this.STATUS_TEXT = obj.STATUS_TEXT;
        this.CURRENT_SMO = obj.CURRENT_SMO;
        this.CS_LIST_IN_ID = obj.CS_LIST_IN_ID;
        this.CODE_MO = obj.CODE_MO;
        this.STATUS_SEND = obj.STATUS_SEND;
        this.STATUS_SEND_TEXT = obj.STATUS_SEND_TEXT;
        this.DATE_CREATE = obj.DATE_CREATE;
    }
}
export var StatusCS_LIST;
(function (StatusCS_LIST) {
    /// <summary>
    /// Новый список
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["New"] = 0] = "New";
    /// <summary>
    /// На отправку
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["OnSend"] = 1] = "OnSend";
    /// <summary>
    /// Отправлен
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["Send"] = 2] = "Send";
    /// <summary>
    /// ФЛК получен
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["FLK"] = 3] = "FLK";
    /// <summary>
    /// Ответ получен
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["Answer"] = 4] = "Answer";
    /// <summary>
    /// Ошибка обработки
    /// </summary>
    StatusCS_LIST[StatusCS_LIST["Error"] = 5] = "Error";
})(StatusCS_LIST || (StatusCS_LIST = {}));
export class PersonItemModel {
    constructor(obj) {
        this.FAM = "";
        this.IM = "";
        this.OT = "";
        this.DR = null;
        this.W = null;
        this.SPOLIS = "";
        this.NPOLIS = "";
        this.DOC_TYPE = "";
        this.DOC_SER = "";
        this.DOC_NUM = "";
        this.SNILS = "";
        if (obj != null) {
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            if (obj.DR != null)
                this.DR = new Date(obj.DR);
            this.W = obj.W;
            this.VPOLIS = obj.VPOLIS;
            this.SPOLIS = obj.SPOLIS;
            this.NPOLIS = obj.NPOLIS;
            this.DOC_TYPE = obj.DOC_TYPE;
            this.DOC_SER = obj.DOC_SER;
            this.DOC_NUM = obj.DOC_NUM;
            this.SNILS = obj.SNILS;
            this.CS_LIST_IN_ID = obj.CS_LIST_IN_ID;
        }
    }
}
export class SprWItemModel {
    constructor(obj) {
        this.ID = null;
        this.NAME = "";
        if (obj != null) {
            this.ID = obj.ID;
            this.NAME = obj.NAME;
        }
    }
    ;
}
export class SprF011ItemModel {
    constructor(obj) {
        this.ID = null;
        this.NAME = "";
        if (obj != null) {
            this.ID = obj.ID;
            this.NAME = obj.NAME;
        }
    }
    ;
}
export class SprVPOLISItemModel {
    constructor(obj) {
        this.ID = null;
        this.NAME = "";
        if (obj != null) {
            this.ID = obj.ID;
            this.NAME = obj.NAME;
        }
    }
}
export class SPRModel {
    constructor(obj) {
        this.F011 = [];
        this.W = [];
        this.VPOLIS = [];
        if (obj != null) {
            this.F011.push(new SprF011ItemModel(null));
            this.F011.push(...obj.F011.map(x => new SprF011ItemModel(x)));
            this.W.push(new SprWItemModel(null));
            this.W.push(...obj.W.map(x => new SprWItemModel(x)));
            this.VPOLIS.push(new SprVPOLISItemModel(null));
            this.VPOLIS.push(...obj.VPOLIS.map(x => new SprVPOLISItemModel(x)));
        }
    }
}
export class PersonView {
    constructor(obj) {
        this.FIO = "";
        this.W = "";
        this.POLIS = "";
        this.DOC = "";
        this.SNILS = "";
        this.STATUS = null;
        this.RESULT = [];
        if (obj != null) {
            this.FIO = obj.FIO;
            if (obj.DR != null)
                this.DR = new Date(obj.DR);
            this.W = obj.W;
            this.POLIS = obj.POLIS;
            this.DOC = obj.DOC;
            this.SNILS = obj.SNILS;
            this.STATUS = obj.STATUS;
            this.RESULT = obj.RESULT.map(x => new PersonViewResult(x));
        }
    }
    ;
    ;
    ;
    get HaveResult() {
        return this.RESULT.length !== 0;
    }
}
export class PersonViewResult {
    constructor(obj) {
        this.ENP = "";
        this.DR = null;
        this.DDEATH = null;
        this.LVL_D = "";
        this.LVL_D_KOD = [];
        this.SMO = [];
        if (obj != null) {
            this.ENP = obj.ENP;
            if (obj.DR != null)
                this.DR = new Date(obj.DR);
            if (obj.DDEATH != null)
                this.DDEATH = new Date(obj.DDEATH);
            this.LVL_D = obj.LVL_D;
            this.LVL_D_KOD = obj.LVL_D_KOD;
            this.SMO = obj.SMO.map(x => new PersonViewResultSMO(x));
        }
    }
    ;
}
export class PersonViewResultSMO {
    constructor(obj) {
        this.ENP = "";
        this.TF_OKATO = "";
        this.NAME_TFK = "";
        this.TYPE_SMO = "";
        this.SMO = "";
        this.SMO_NAME = "";
        this.DATE_B = null;
        this.DATE_E = null;
        this.VPOLIS = "";
        this.SPOLIS = "";
        this.NPOLIS = "";
        this.SMO_OK = "";
        if (obj != null) {
            this.ENP = obj.ENP;
            this.TF_OKATO = obj.TF_OKATO;
            this.NAME_TFK = obj.NAME_TFK;
            this.TYPE_SMO = obj.TYPE_SMO;
            this.SMO = obj.SMO;
            this.SMO_NAME = obj.SMO_NAME;
            if (obj.DATE_B != null)
                this.DATE_B = new Date(obj.DATE_B);
            if (obj.DATE_E != null)
                this.DATE_E = new Date(obj.DATE_E);
            this.VPOLIS = obj.VPOLIS;
            this.SPOLIS = obj.SPOLIS;
            this.NPOLIS = obj.NPOLIS;
            this.SMO_OK = obj.SMO_OK;
        }
    }
    ;
    ;
    ;
    ;
    ;
    ;
    ;
    ;
    ;
}
//# sourceMappingURL=CSModel.js.map