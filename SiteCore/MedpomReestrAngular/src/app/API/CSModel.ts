export class TitleResult
{
     TotalRecord:number;
     PersonItems:PersonItem[]=[];
     constructor(obj:any)
     {
         if(obj!=null)
         {
             this.TotalRecord = obj.TotalRecord;
             this.PersonItems = obj.PersonItems.map(v=>new PersonItem(v));
         }
     }

}

export class PersonItem
{
    FAM: string = "";
    IM: string = "";
    OT: string = "";  
    DR: string = "";
    POLIS: string = "";
    DOC: string = "";
    STATUS: boolean|null = null;
    STATUS_TEXT: string = "";
    CURRENT_SMO:string = "";
    CS_LIST_IN_ID:number;
    CODE_MO:string= "";
    STATUS_SEND:StatusCS_LIST|null =null;
    STATUS_SEND_TEXT:string = "";
    DATE_CREATE:Date;
    constructor(obj:any)
    {
        if(obj!=null)
        {
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

    public Merge(obj:PersonItem)
    {
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

export enum StatusCS_LIST
{
    /// <summary>
    /// Новый список
    /// </summary>
    New = 0,
    /// <summary>
    /// На отправку
    /// </summary>
    OnSend = 1,
    /// <summary>
    /// Отправлен
    /// </summary>
    Send = 2,
    /// <summary>
    /// ФЛК получен
    /// </summary>
    FLK = 3,
    /// <summary>
    /// Ответ получен
    /// </summary>
    Answer = 4,
    /// <summary>
    /// Ошибка обработки
    /// </summary>
    Error = 5
}
export class PersonItemModel
{
    FAM: string = "";
    IM: string = "";
    OT: string = "";  
    DR: Date|null = null;
    W:number|null = null;
    VPOLIS:number|null;
    SPOLIS:string = "";
    NPOLIS:string = "";
    DOC_TYPE:string = "";
    DOC_SER:string = "";
    DOC_NUM:string = "";
    SNILS:string = "";
    CS_LIST_IN_ID:number|null;
    constructor(obj:any)
    {
        if(obj!=null) {
           this.FAM = obj.FAM;
           this.IM = obj.IM;
           this.OT = obj.OT;
           if(obj.DR!=null)
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



export class SprWItemModel
{
    ID:number|null = null;;
    NAME:string = "";
    constructor(obj:any)
    {
        if(obj!=null)
        {
           this.ID = obj.ID;
           this.NAME = obj.NAME;
        }
    }
}

export class SprF011ItemModel
{
    ID:string|null = null;;
    NAME:string = "";
    constructor(obj:any)
    {
        if(obj!=null)
        {
           this.ID = obj.ID;
           this.NAME = obj.NAME;
        }
    }
}

export class SprVPOLISItemModel
{
    ID:number|null = null;
    NAME:string = "";
    constructor(obj:any)
    {
        if(obj!=null)
        {
           this.ID = obj.ID;
           this.NAME = obj.NAME;
        }
    }
}


export class SPRModel
{
    F011 :SprF011ItemModel[] = [];
    W :SprWItemModel[] = [];
    VPOLIS:SprVPOLISItemModel[] = [];
    constructor(obj:any)
    {
        if(obj!=null)
        {
           this.F011.push(new SprF011ItemModel(null));
           this.F011.push(...obj.F011.map(x=>new SprF011ItemModel(x)));
           this.W.push(new SprWItemModel(null));
           this.W.push(...obj.W.map(x=>new SprWItemModel(x)));
           this.VPOLIS.push(new SprVPOLISItemModel(null));
           this.VPOLIS.push(...obj.VPOLIS.map(x=>new SprVPOLISItemModel(x)));
        }
    }
}



export class PersonView
{
    FIO: string = "";
    DR: Date;
    W:string = "";;
    POLIS:string = "";;
    DOC:string = "";;
    SNILS:string = "";
    STATUS:boolean|null = null;
    RESULT:PersonViewResult[] = []
    get HaveResult()    {
        return this.RESULT.length !==0;
    }
    constructor(obj:any)
    {
        if(obj!=null) {
           this.FIO = obj.FIO;
           if(obj.DR!=null)
                this.DR = new Date(obj.DR);
           this.W = obj.W;
           this.POLIS = obj.POLIS;
           this.DOC = obj.DOC;
           this.SNILS = obj.SNILS;
           this.STATUS = obj.STATUS;
           this.RESULT = obj.RESULT.map(x=>new PersonViewResult(x));
        }
    }
}

export class PersonViewResult
{
    ENP: string = "";
    DR: Date|null = null;
    DDEATH: Date|null = null;
    LVL_D:string = "";;
    LVL_D_KOD:string[] = [];
    SMO:PersonViewResultSMO[] = [];
    
    constructor(obj:any)
    {
        if(obj!=null) {
           this.ENP = obj.ENP;
           if(obj.DR!=null)
                this.DR = new Date(obj.DR);
           if(obj.DDEATH!=null)
                this.DDEATH = new Date(obj.DDEATH);
           this.LVL_D = obj.LVL_D;
           this.LVL_D_KOD = obj.LVL_D_KOD;
           this.SMO = obj.SMO.map(x=>new PersonViewResultSMO(x));
          
        }
    }
}


export class PersonViewResultSMO
{
    ENP: string = "";
    TF_OKATO:string = "";;
    NAME_TFK:string = "";;
    TYPE_SMO:string = "";;
    SMO:string = "";;
    SMO_NAME:string = "";;
    DATE_B:Date|null = null;
    DATE_E:Date|null = null
    VPOLIS:string = "";;
    SPOLIS:string = "";;
    NPOLIS:string = "";;
    SMO_OK:string = "";;
    constructor(obj:any)
    {
        if(obj!=null) {
           this.ENP = obj.ENP;
           this.TF_OKATO = obj.TF_OKATO;
           this.NAME_TFK = obj.NAME_TFK;
           this.TYPE_SMO = obj.TYPE_SMO;
           this.SMO = obj.SMO;
           this.SMO_NAME = obj.SMO_NAME;
           if(obj.DATE_B!=null)
                this.DATE_B = new Date(obj.DATE_B);
           if(obj.DATE_E!=null)
                this.DATE_E = new Date(obj.DATE_E);
           this.VPOLIS = obj.VPOLIS;
           this.SPOLIS = obj.SPOLIS;
           this.NPOLIS = obj.NPOLIS;
           this.SMO_OK = obj.SMO_OK;
        }
    }
}