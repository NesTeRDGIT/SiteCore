import { SPRTimeMark } from "./SPRTimeMark";
import { SPRNotUniTimeMark } from "./SPRNotUniTimeMark";
export class SPRModel {
    constructor(repo) {
        this.repo = repo;
        this.CODE_MO_Reestr = new SPRTimeMark(15, "MCOD");
        this.CODE_MO = new SPRTimeMark(15, "MCOD");
        this.CODE_SMO_Reestr = new SPRTimeMark(15, "SMOCOD");
        this.CODE_SMO = new SPRTimeMark(15, "SMOCOD");
        this.NMIC_VID_NHISTORY = new SPRTimeMark(15, "ID_VID_NHISTORY");
        this.NMIC_OPLATA = new SPRTimeMark(15, "ID_OPLATA");
        this.TMIS = new SPRTimeMark(15, "TMIS_ID");
        this.NMIC = new SPRTimeMark(15, "NMIC_ID");
        this.V002 = new SPRNotUniTimeMark(15, "IDPR");
        this.F014 = new SPRNotUniTimeMark(15, "KOD");
        this.NMIC_CELL = new SPRTimeMark(15, "CELL");
        this.NMIC_FULL = new SPRTimeMark(15, "FULL");
        this.EXPERTS = new SPRTimeMark(15, "N_EXPERT");
        this.CONTACT_SPR = new SPRTimeMark(15, "CODE_MO");
        this.refreshVariableSPR = async (force = false) => {
            await this.refreshCODE_MO_ReestrAsync(force);
            await this.refreshCODE_SMO_ReestrAsync(force);
        };
        this.refreshStaticSPR = async (force = false) => {
            await this.refreshTMISAsync(force);
            await this.refreshNMICAsync(force);
            await this.refreshNMIC_VID_NHISTORYAsync(force);
            await this.refreshNMIC_OPLATAAsync(force);
            await this.refreshV002Async(force);
            await this.refreshCODE_MOAsync(force);
            await this.refreshF014Async(force);
            await this.refreshNMIC_CELLAsync(force);
            await this.refreshEXPERTSAsync(force);
            await this.refreshNMIC_FULLAsync(force);
            await this.refreshCONTACT_SPRAsync(force);
            await this.refreshCODE_SMOAsync(force);
        };
        this.refreshCODE_SMOAsync = async (force = false) => {
            if (this.CODE_SMO.isNeedUpdate || force) {
                let spr = await this.repo.GetCODE_SMOAsync();
                this.CODE_SMO.UpdateSPR(spr);
            }
        };
        this.refreshCONTACT_SPRAsync = async (force = false) => {
            if (this.CONTACT_SPR.isNeedUpdate || force) {
                let spr = await this.repo.GetCONTACT_SPRAsync();
                this.CONTACT_SPR.UpdateSPR(spr);
            }
        };
        this.refreshCODE_MO_ReestrAsync = async (force = false) => {
            if (this.CODE_MO_Reestr.isNeedUpdate || force) {
                let spr = await this.repo.GetCODE_MO_ReestrAsync();
                this.CODE_MO_Reestr.UpdateSPR(spr);
            }
        };
        this.refreshCODE_MOAsync = async (force = false) => {
            if (this.CODE_MO.isNeedUpdate || force) {
                let spr = await this.repo.GetCODE_MOAsync();
                this.CODE_MO.UpdateSPR(spr);
            }
        };
        this.refreshCODE_SMO_ReestrAsync = async (force = false) => {
            if (this.CODE_SMO_Reestr.isNeedUpdate || force) {
                let spr = await this.repo.GetCODE_SMO_ReestrAsync();
                let empty = new SMO(null);
                empty.SMOCOD = "NULL", empty.NAM_SMOK = "Нет данных";
                this.CODE_SMO_Reestr.UpdateSPR(spr, empty);
            }
        };
        this.refreshNMIC_VID_NHISTORYAsync = async (force = false) => {
            if (this.NMIC_VID_NHISTORY.isNeedUpdate || force) {
                let spr = await this.repo.GetNMIC_VID_NHISTORYAsync();
                this.NMIC_VID_NHISTORY.UpdateSPR(spr);
            }
        };
        this.refreshNMIC_OPLATAAsync = async (force = false) => {
            if (this.NMIC_OPLATA.isNeedUpdate || force) {
                let spr = await this.repo.GetNMIC_OPLATAAsync();
                this.NMIC_OPLATA.UpdateSPR(spr);
            }
        };
        this.refreshNMICAsync = async (force = false) => {
            if (this.NMIC.isNeedUpdate || force) {
                let spr = await this.repo.GetNMICAsync();
                let empty = new NMIC(null);
                empty.NMIC_ID = null, empty.NMIC_NAME = "Нет данных";
                this.NMIC.UpdateSPR(spr, empty);
            }
        };
        this.refreshTMISAsync = async (force = false) => {
            if (this.TMIS.isNeedUpdate || force) {
                let spr = await this.repo.GetTMISAsync();
                let empty = new TMIS(null);
                empty.TMIS_ID = null, empty.TMS_NAME = "Нет данных";
                this.TMIS.UpdateSPR(spr, empty);
            }
        };
        this.refreshV002Async = async (force = false) => {
            if (this.V002.isNeedUpdate || force) {
                let spr = await this.repo.GetV002Async();
                let empty = new V002(null);
                empty.IDPR = null, empty.PRNAME = "Нет данных";
                this.V002.UpdateSPR(spr, empty);
            }
        };
        this.refreshF014Async = async (force = false) => {
            if (this.F014.isNeedUpdate || force) {
                let spr = await this.repo.GetF014Async();
                let empty = new F014(null);
                empty.KOD = null, empty.OSN = "Нет данных";
                this.F014.UpdateSPR(spr, empty);
            }
        };
        this.refreshNMIC_CELLAsync = async (force = false) => {
            if (this.NMIC_CELL.isNeedUpdate || force) {
                let spr = await this.repo.GetNMIC_CELLAsync();
                let empty = new NMIC_CELL(null);
                empty.CELL = null, empty.CELL_NAME = "Нет данных";
                this.NMIC_CELL.UpdateSPR(spr, empty);
            }
        };
        this.refreshEXPERTSAsync = async (force = false) => {
            if (this.EXPERTS.isNeedUpdate || force) {
                let spr = await this.repo.GetEXPERTSAsync();
                let empty = new EXPERTS(null);
                empty.N_EXPERT = null, empty.FAM = "Нет данных";
                this.EXPERTS.UpdateSPR(spr, empty);
            }
        };
        this.refreshNMIC_FULLAsync = async (force = false) => {
            if (this.NMIC_FULL.isNeedUpdate || force) {
                let spr = await this.repo.GetNMIC_FULLAsync();
                let empty = new NMIC_FULL(null);
                empty.FULL = null, empty.FULL_NAME = "Нет данных";
                this.NMIC_FULL.UpdateSPR(spr, empty);
            }
        };
    }
}
export class CODE_MO {
    constructor(obj) {
        this.MCOD = "";
        this.NAM_MOP = "";
        this.NAM_MOK = "";
        this.D_END = null;
        if (obj != null) {
            this.MCOD = obj.MCOD;
            this.NAM_MOP = obj.NAM_MOP;
            this.NAM_MOK = obj.NAM_MOK;
            if (obj.D_END != null)
                this.D_END = new Date(obj.D_END);
        }
    }
    get FULL_NAME() {
        return `${this.NAM_MOK}${this.MCOD != "" && this.MCOD != null ? `(${this.MCOD})` : ''}`;
    }
}
export class SMO {
    constructor(obj) {
        this.SMOCOD = "";
        this.NAM_SMOK = "";
        this.OGRN = "";
        this.TF_OKATO = null;
        if (obj != null) {
            this.SMOCOD = obj.SMOCOD;
            this.NAM_SMOK = obj.NAM_SMOK;
            this.OGRN = obj.OGRN;
            this.TF_OKATO = obj.TF_OKATO;
        }
    }
    get FULL_NAME() {
        return `${this.NAM_SMOK}${this.SMOCOD != "NULL" && this.SMOCOD != null ? `(${this.SMOCOD})` : ''}`;
    }
}
export class NMIC_VID_NHISTORY {
    constructor(obj) {
        this.ID_VID_NHISTORY = null;
        this.VID_NHISTORY = "";
        if (obj != null) {
            this.ID_VID_NHISTORY = obj.ID_VID_NHISTORY;
            this.VID_NHISTORY = obj.VID_NHISTORY;
        }
    }
    get FULL_NAME() {
        return `${this.VID_NHISTORY}(${this.ID_VID_NHISTORY})`;
    }
}
export class NMIC_OPLATA {
    constructor(obj) {
        this.ID_OPLATA = null;
        this.OPLATA = "";
        if (obj != null) {
            this.ID_OPLATA = obj.ID_OPLATA;
            this.OPLATA = obj.OPLATA;
        }
    }
    get FULL_NAME() {
        return `${this.OPLATA}(${this.ID_OPLATA})`;
    }
}
export class TMIS {
    constructor(obj) {
        this.TMIS_ID = null;
        this.TMS_NAME = null;
        if (obj != null) {
            this.TMIS_ID = obj.TMIS_ID;
            this.TMS_NAME = obj.TMS_NAME;
        }
    }
}
export class NMIC {
    constructor(obj) {
        this.NMIC_ID = null;
        this.NMIC_NAME = null;
        if (obj != null) {
            this.NMIC_ID = obj.NMIC_ID;
            this.NMIC_NAME = obj.NMIC_NAME;
        }
    }
}
export class V002 {
    constructor(obj) {
        this.IDPR = null;
        this.PRNAME = null;
        this.DATE_B = null;
        this.DATE_E = null;
        if (obj != null) {
            this.IDPR = obj.IDPR;
            this.PRNAME = obj.PRNAME;
            this.DATE_B = new Date(obj.DATEBEG);
            if (obj.DATEEND)
                this.DATE_E = new Date(obj.DATEEND);
        }
    }
}
export class NMIC_CELL {
    constructor(obj) {
        this.CELL = null;
        this.CELL_NAME = null;
        if (obj != null) {
            this.CELL = obj.CELL;
            this.CELL_NAME = obj.CELL_NAME;
        }
    }
}
export class NMIC_FULL {
    constructor(obj) {
        this.FULL = null;
        this.FULL_NAME = null;
        if (obj != null) {
            this.FULL = obj.FULL;
            this.FULL_NAME = obj.FULL_NAME;
        }
    }
}
export class EXPERTS {
    constructor(obj) {
        this.N_EXPERT = null;
        this.FAM = null;
        this.IM = null;
        this.OT = null;
        if (obj != null) {
            this.N_EXPERT = obj.N_EXPERT;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
        }
    }
    get FIO() {
        var _a, _b, _c;
        return `${(_a = this.FAM) !== null && _a !== void 0 ? _a : ""} ${(_b = this.IM) !== null && _b !== void 0 ? _b : ""} ${(_c = this.OT) !== null && _c !== void 0 ? _c : ""}`.trim();
    }
    get NAME_NUM() {
        return `${this.FIO}${this.N_EXPERT != null ? `(${this.N_EXPERT})` : ''}`;
    }
}
export class F014 {
    constructor(obj) {
        this.KOD = null;
        this.OSN = null;
        this.KOMMENT = null;
        if (obj != null) {
            this.KOD = obj.KOD;
            this.OSN = obj.OSN;
            this.KOMMENT = obj.KOMMENT;
            this.DATE_B = new Date(obj.DATEBEG);
            if (obj.DATEEND)
                this.DATE_E = new Date(obj.DATEEND);
        }
    }
    get FullName() {
        return `${this.OSN}${this.KOMMENT != null ? `-${this.KOMMENT}` : ''}`;
    }
}
export class CONTACT_SPRModel {
    constructor(obj) {
        this.CODE_MO = null;
        this.TelAndFio = [];
        if (obj != null) {
            this.CODE_MO = obj.CODE_MO;
            this.TelAndFio = obj.TelAndFio;
        }
    }
    get AllContacts() {
        return this.TelAndFio.join(';\r\n');
    }
}
//# sourceMappingURL=SPRModel.js.map