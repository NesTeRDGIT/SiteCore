export class LoadReestViewModel {
    ConnectWCFon: boolean;
    ReestrEnabled: boolean;
    TypePriem: boolean;
    WithSing: boolean;
    CODE_MO: string;
    NAME_OK: string;
    FileList: FileItem[];
    constructor(obj: any) {
        if (obj != null) {
            this.ConnectWCFon = obj.ConnectWCFon;
            this.ReestrEnabled = obj.ReestrEnabled;
            this.TypePriem = obj.TypePriem;
            this.WithSing = obj.WithSing;
            this.CODE_MO = obj.CODE_MO;
            this.NAME_OK = obj.NAME_OK;
            this.FileList = obj.FileList.map(x => new FileItem(x));
        } 
    }

    get IsActive() {
        return this.ConnectWCFon && this.ReestrEnabled;
    }
}

export class FileItemBase {
    ID:number;
    STATUS: STATUS_FILE;
    STATUS_NAME:string;
    FILENAME: string;
    TYPE_FILE: TYPEFILE|null;
    TYPE_FILE_NAME: string;
    COMENT: string;
    constructor(obj) {
        if (obj != null) {
            this.ID = obj.ID;
            this.STATUS = obj.STATUS;
            this.STATUS_NAME = obj.STATUS_NAME;
            this.FILENAME = obj.FILENAME;
            this.TYPE_FILE = obj.TYPE_FILE != null ? obj.TYPE_FILE : null;
            this.TYPE_FILE_NAME = obj.TYPE_FILE_NAME;
            this.COMENT = obj.COMENT;
        }
    }
}

export class FileItem extends FileItemBase {
    FILE_L: FileItemBase;

    constructor(obj) {
        if (obj != null) {
            super(obj);
            this.FILE_L = obj.FILE_L != null ? new FileItemBase(obj.FILE_L) : null;
        }
    }
}


export enum STATUS_FILE {
    NOT_INVITE = 0,
    INVITE = 1,
    XML_VALID = 2,
    XML_NOT_VALID = 3
}


export class ErrorItem {
    Error: string;
    ErrorT: ErrorTypeEnum;
    constructor(obj: any) {
        if (obj != null) {
            this.Error = obj.Error;
            this.ErrorT = obj.ErrorT;
        }
    }
}

export enum ErrorTypeEnum {
    Text = 0,
    Error = 1
}

export enum TYPEFILE {
    H = 0,
    T = 1,
    DP = 2,
    DV = 3,
    DO = 4,
    DS = 5,
    DU = 6,
    DF = 7,
    DD = 8,
    DR = 9,
    LH = 10,
    LT = 11,
    LP = 12,
    LV = 13,
    LO = 14,
    LS = 15,
    LU = 16,
    LF = 17,
    LD = 18,
    LR = 19,
    C = 20,
    LC = 21,
    DA = 22,
    DB = 23,
    LA = 24,
    LB = 25
}





export class ViewReestViewModel {
    ConnectWCFon: boolean;
    FP: FilePacket = null;
    
    constructor(obj: any) {
        if (obj != null) {
            this.ConnectWCFon = obj.ConnectWCFon;
            if (obj.FP != null)
                this.FP = new FilePacket(obj.FP);
        }
    }

   
}


export class FilePacket {
    CodeMO: string;
    CaptionMO: string;
    Date: Date;
    IST: ISTEnum;
    CommentSite: string;
    Order: number;
    WARNNING: string;
    isResult: string;
    Status: StatusFilePack;
    FileList: FileItemView[];
    constructor(obj: any) {
        if (obj != null) {
            this.CodeMO = obj.CodeMO;
            this.CaptionMO = obj.CaptionMO;
            this.Date = obj.Date;
            this.IST = obj.IST;
            this.Order = obj.Order;
            this.WARNNING = obj.WARNNING;
            this.isResult = obj.isResult;
            this.Status = obj.Status;
            this.FileList = obj.FileList.map(x => new FileItemView(x));
        }
    }

    get HasWarning() {
        return this.WARNNING !== null && this.WARNNING !== "";
    }
}



export class FileItemViewBase {
    Comment: string;
    FileName: string;
    Type: TYPEFILE | null;
    TYPE_NAME: string;
    Process: StepsProcess;

    constructor(obj) {
        if (obj != null) {
            this.Comment = obj.Comment;
            this.FileName = obj.FileName;
            this.TYPE_NAME = obj.TYPE_NAME;
            this.Type = obj.Type != null ? obj.Type : null;
            this.Process = obj.Process; }
    }
}

export class FileItemView extends FileItemViewBase {
    FILE_L: FileItemViewBase;
    constructor(obj) {
        if (obj != null) {
            super(obj);
            this.FILE_L = obj.FILE_L != null ? new FileItemViewBase(obj.FILE_L) : null;
        }
    }
}


export enum ISTEnum {
    MAIL = 1,
    SITE = 2
}

export enum StatusFilePack {
    Open = 0,
    Close = 1,
    XMLSchemaOK = 2,
    FLKOK = 3,
    FLKERR = 4
}


export enum StepsProcess {
    NotInvite = 0,
    Invite = 1,
    XMLxsd = 2,
    ErrorXMLxsd = 3,
    FlkOk = 4,
    FlkErr = 5
}