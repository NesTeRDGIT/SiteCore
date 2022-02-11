export class TMKList
{
    Count:number = 0;
    Items:TMKListModel[]=[];
    constructor(obj: any)
    {
        if(obj!=null)
        {
            this.Count = obj.Count;
            this.Items = obj.Items.map((x: any)=>new TMKListModel(x));
        }
    }
}


export  class TMKListModel {
    TMK_ID: number = null;
    ENP: string = null;
    CODE_MO: string = null;
    FIO: string = null;
    DATE_B: Date = null;
    DATE_QUERY: Date | null;
    DATE_PROTOKOL: Date | null;
    DATE_TMK: Date | null;
    SMO: string = null;
    VID_NHISTORY: number = null;
    OPLATA: number = null;   
    STATUS: TMKListModelStatusEnum;
    STATUS_COM: string;
    isEXP: boolean = null;  
    TMIS: number = null;
    NMIC: number = null;

    MEK:TMKListExpModel[] = [];
    MEE:TMKListExpModel[] = [];
    EKMP: TMKListExpModel[] = [];

    get StatusText(): string {
        let result = "";
        switch (this.STATUS) {
            case TMKListModelStatusEnum.Closed: result = "Закрыта"; break;
            case TMKListModelStatusEnum.Open: result = "Открыта";break;
            case TMKListModelStatusEnum.Error: result = `Ошибочная: ${this.STATUS_COM}`; break;
        }
        if (this.isEXP) {
            result += ", есть экспертиза";
        }
        return result;
    }
    constructor(obj: any) {
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
            this.MEK = obj.MEK.map((x:any)=>new TMKListExpModel(x));
            this.MEE =  obj.MEE.map((x:any)=>new TMKListExpModel(x));
            this.EKMP = obj.EKMP.map((x:any)=>new TMKListExpModel(x));
        }
    }
}

export class TMKListExpModel
{  
    DATEACT:Date = null;
    OSN: number[] = [];
    constructor(obj: any) {
        if (obj != null) {        
            if (obj.DATEACT != null)
                this.DATEACT = new Date(obj.DATEACT);
            if (obj.OSN != null)
                this.OSN = obj.OSN;
        }
    }
}


export enum TMKListModelStatusEnum {
    Open = 0,
    Closed = 1,
    Error = -1
}