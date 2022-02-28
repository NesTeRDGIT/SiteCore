import { IRepository } from "./Repository";
import { Dictionary } from "./IKeyCollection";
import { SPRTimeMark } from "./SPRTimeMark";
import { SPRNotUniTimeMark, INotUniSPR } from "./SPRNotUniTimeMark";

export class SPRModel {
    CODE_MO_Reestr: SPRTimeMark<CODE_MO> = new SPRTimeMark(15, "MCOD");
    CODE_MO: SPRTimeMark<CODE_MO> = new SPRTimeMark(15, "MCOD");
    CODE_SMO_Reestr: SPRTimeMark<SMO> = new SPRTimeMark(15, "SMOCOD");
    CODE_SMO: SPRTimeMark<SMO> = new SPRTimeMark(15, "SMOCOD");
    
    NMIC_VID_NHISTORY: SPRTimeMark<NMIC_VID_NHISTORY> = new SPRTimeMark(15, "ID_VID_NHISTORY");
    NMIC_OPLATA: SPRTimeMark<NMIC_OPLATA> = new SPRTimeMark(15, "ID_OPLATA");
    TMIS: SPRTimeMark<TMIS> = new SPRTimeMark(15, "TMIS_ID");
    NMIC: SPRTimeMark<NMIC> = new SPRTimeMark(15, "NMIC_ID");
    V002: SPRNotUniTimeMark<V002> = new SPRNotUniTimeMark(15, "IDPR");
    F014: SPRNotUniTimeMark<F014> = new SPRNotUniTimeMark(15, "KOD");
    NMIC_CELL: SPRTimeMark<NMIC_CELL> = new SPRTimeMark(15, "CELL");
    NMIC_FULL: SPRTimeMark<NMIC_FULL> = new SPRTimeMark(15, "FULL");
    EXPERTS: SPRTimeMark<EXPERTS> = new SPRTimeMark(15, "N_EXPERT");
    CONTACT_SPR: SPRTimeMark<CONTACT_SPRModel> = new SPRTimeMark(15, "CODE_MO");

    constructor(public repo: IRepository) {

    }


    refreshVariableSPR = async (force: boolean = false) => {
        await this.refreshCODE_MO_ReestrAsync(force);
        await this.refreshCODE_SMO_ReestrAsync(force);

    }

    refreshStaticSPR = async (force: boolean = false) => {
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
    }


    refreshCODE_SMOAsync = async (force: boolean = false) => {
        if (this.CODE_SMO.isNeedUpdate || force) {
            let spr = await this.repo.GetCODE_SMOAsync();
            this.CODE_SMO.UpdateSPR(spr);
        }
    }


    refreshCONTACT_SPRAsync = async (force: boolean = false) => {
        if (this.CONTACT_SPR.isNeedUpdate || force) {
            let spr = await this.repo.GetCONTACT_SPRAsync();
            this.CONTACT_SPR.UpdateSPR(spr);
        }
    }

    refreshCODE_MO_ReestrAsync = async (force: boolean = false) => {
        if (this.CODE_MO_Reestr.isNeedUpdate || force) {
            let spr = await this.repo.GetCODE_MO_ReestrAsync();
            this.CODE_MO_Reestr.UpdateSPR(spr);
        }
    }
    refreshCODE_MOAsync = async (force: boolean = false) => {

        if (this.CODE_MO.isNeedUpdate || force) {
            let spr = await this.repo.GetCODE_MOAsync();
            this.CODE_MO.UpdateSPR(spr);
        }
    }

    refreshCODE_SMO_ReestrAsync = async (force: boolean = false) => {
        if (this.CODE_SMO_Reestr.isNeedUpdate || force) {
            let spr = await this.repo.GetCODE_SMO_ReestrAsync();
            let empty = new SMO(null);
            empty.SMOCOD = "NULL",empty.NAM_SMOK = "Нет данных";
            this.CODE_SMO_Reestr.UpdateSPR(spr,empty);
        }
    }

    refreshNMIC_VID_NHISTORYAsync = async (force: boolean = false) => {
        if (this.NMIC_VID_NHISTORY.isNeedUpdate || force) {
            let spr = await this.repo.GetNMIC_VID_NHISTORYAsync();                      
           
            this.NMIC_VID_NHISTORY.UpdateSPR(spr);
        }
    }
    refreshNMIC_OPLATAAsync = async (force: boolean = false) => {
        if (this.NMIC_OPLATA.isNeedUpdate || force) {
            let spr = await this.repo.GetNMIC_OPLATAAsync();  
            
            this.NMIC_OPLATA.UpdateSPR(spr);
        }
    }

    refreshNMICAsync = async (force: boolean = false) => {
        if (this.NMIC.isNeedUpdate || force) {
            let spr = await this.repo.GetNMICAsync();
            let empty = new NMIC(null);
            empty.NMIC_ID  = null,empty.NMIC_NAME = "Нет данных";
            this.NMIC.UpdateSPR(spr,empty);
        }
    }

    refreshTMISAsync = async (force: boolean = false) => {
        if (this.TMIS.isNeedUpdate || force) {
            let spr = await this.repo.GetTMISAsync();
            let empty = new TMIS(null);
            empty.TMIS_ID  = null,empty.TMS_NAME = "Нет данных";       
            this.TMIS.UpdateSPR(spr,empty);
        }
    }
    refreshV002Async = async (force: boolean = false) => {
        if (this.V002.isNeedUpdate || force) {
            let spr = await this.repo.GetV002Async();     
            let empty = new V002(null);
            empty.IDPR  = null ,empty.PRNAME = "Нет данных";           
            this.V002.UpdateSPR(spr,empty);
        }
    }
  
 
    refreshF014Async = async (force: boolean = false) => {
        if (this.F014.isNeedUpdate || force) {
            let spr = await this.repo.GetF014Async();
            let empty = new F014(null);
            empty.KOD = null,empty.OSN = "Нет данных";   
            this.F014.UpdateSPR(spr,empty);
        }
    }

    refreshNMIC_CELLAsync = async (force: boolean = false) => {

        if (this.NMIC_CELL.isNeedUpdate || force) {
            let spr = await this.repo.GetNMIC_CELLAsync();
            let empty = new NMIC_CELL(null);
            empty.CELL = null,empty.CELL_NAME = "Нет данных";
            this.NMIC_CELL.UpdateSPR(spr,empty);
        }
    }

    refreshEXPERTSAsync = async (force: boolean = false) => {

        if (this.EXPERTS.isNeedUpdate || force) {
            let spr = await this.repo.GetEXPERTSAsync();
            let empty = new EXPERTS(null);
            empty.N_EXPERT = null,empty.FAM = "Нет данных";
            this.EXPERTS.UpdateSPR(spr,empty);
        }
    }

    refreshNMIC_FULLAsync = async (force: boolean = false) => {

        if (this.NMIC_FULL.isNeedUpdate || force) {
            let spr = await this.repo.GetNMIC_FULLAsync();
            
            let empty = new NMIC_FULL(null);
            empty.FULL = null,empty.FULL_NAME = "Нет данных";
            this.NMIC_FULL.UpdateSPR(spr,empty);
        }
    }

}

export class CODE_MO {
    MCOD: string = "";
    NAM_MOP: string = "";
    NAM_MOK: string = "";
    D_END: Date = null;
    get FULL_NAME(): string {
        return `${this.NAM_MOK}${this.MCOD!="" && this.MCOD !=null? `(${this.MCOD})`:''}`;
    }
    constructor(obj: any) {
        if (obj != null) {
            this.MCOD = obj.MCOD;
            this.NAM_MOP = obj.NAM_MOP;
            this.NAM_MOK = obj.NAM_MOK;
            if (obj.D_END != null)
                this.D_END = new Date(obj.D_END);
        }
    }
}

export class SMO {
    SMOCOD: string = "";
    NAM_SMOK: string = "";
    OGRN: string = "";
    TF_OKATO: Date = null;
    get FULL_NAME(): string {
        return `${this.NAM_SMOK}${this.SMOCOD!="NULL" && this.SMOCOD !=null? `(${this.SMOCOD})`:''}`;
    }
    constructor(obj: any) {
        if (obj != null) {
            this.SMOCOD = obj.SMOCOD;
            this.NAM_SMOK = obj.NAM_SMOK;
            this.OGRN = obj.OGRN;
            this.TF_OKATO = obj.TF_OKATO;
        }
    }
}

export class NMIC_VID_NHISTORY {
    ID_VID_NHISTORY: number = null;
    VID_NHISTORY: string = "";
    get FULL_NAME(): string {
        return `${this.VID_NHISTORY}(${this.ID_VID_NHISTORY})`;
    }
    constructor(obj: any) {
        if (obj != null) {
            this.ID_VID_NHISTORY = obj.ID_VID_NHISTORY;
            this.VID_NHISTORY = obj.VID_NHISTORY;
        }
    }
}

export class NMIC_OPLATA {
    ID_OPLATA: number = null;
    OPLATA: string = "";
    get FULL_NAME(): string {
        return `${this.OPLATA}(${this.ID_OPLATA})`;
    }
    constructor(obj: any) {
        if (obj != null) {
            this.ID_OPLATA = obj.ID_OPLATA;
            this.OPLATA = obj.OPLATA;
        }
    }
}


export class TMIS {
    TMIS_ID: number = null;
    TMS_NAME: string = null;
    constructor(obj: any) {
        if (obj != null) {
            this.TMIS_ID = obj.TMIS_ID;
            this.TMS_NAME = obj.TMS_NAME;
        }
    }
}

export class NMIC {
    NMIC_ID: number = null;
    NMIC_NAME: string = null;
    constructor(obj: any) {
        if (obj != null) {
            this.NMIC_ID = obj.NMIC_ID;
            this.NMIC_NAME = obj.NMIC_NAME;
        }
    }
}



export class V002 implements INotUniSPR {
    IDPR: number = null;
    PRNAME: string = null;    
    constructor(obj: any) {
        if (obj != null) {
            this.IDPR = obj.IDPR;
            this.PRNAME = obj.PRNAME;
            this.DATE_B = new Date(obj.DATEBEG);
            if (obj.DATEEND)
                this.DATE_E = new Date(obj.DATEEND);
        }
    }
    DATE_B: Date = null;
    DATE_E: Date = null;
}



export class NMIC_CELL {
    CELL: number = null;
    CELL_NAME: string = null;

    constructor(obj: any) {
        if (obj != null) {
            this.CELL = obj.CELL;
            this.CELL_NAME = obj.CELL_NAME;
        }
    }
}




export class NMIC_FULL {
    FULL: number = null;
    FULL_NAME: string = null;
    constructor(obj: any) {
        if (obj != null) {
            this.FULL = obj.FULL;
            this.FULL_NAME = obj.FULL_NAME;
        }
    }
}

export class EXPERTS {
    N_EXPERT: string = null;
    FAM: string = null;
    IM: string = null;
    OT: string = null;
    get FIO(): string {
        return `${this.FAM??""} ${this.IM??""} ${this.OT??""}`.trim();
    }
    get NAME_NUM(): string {
        return `${this.FIO}${this.N_EXPERT!=null? `(${this.N_EXPERT})`:''}`;
    }
    constructor(obj: any) {
        if (obj != null) {
            this.N_EXPERT = obj.N_EXPERT;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
        }
    }
}



export class F014 implements INotUniSPR {
    DATE_B: Date;
    DATE_E: Date;

    KOD: number = null;
    OSN: string = null;
    KOMMENT: string = null;
   
    get FullName(): string {
        return `${this.OSN}${this.KOMMENT != null ? `-${this.KOMMENT}` : ''}`;
    }

    constructor(obj: any) {
        if (obj != null) {
            this.KOD = obj.KOD;
            this.OSN = obj.OSN;
            this.KOMMENT = obj.KOMMENT;
            this.DATE_B = new Date(obj.DATEBEG);
            if (obj.DATEEND)
                this.DATE_E = new Date(obj.DATEEND);
        }
    }
}


export class CONTACT_SPRModel  {

    CODE_MO: string = null;
    TelAndFio: string[] = [];
    get AllContacts(): string {

        return this.TelAndFio.join(';\r\n');
    }
    constructor(obj: any) {
        if (obj != null) {
            this.CODE_MO = obj.CODE_MO;
            this.TelAndFio = obj.TelAndFio;
        }
    }
}