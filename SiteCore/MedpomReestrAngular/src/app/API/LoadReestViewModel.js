export class LoadReestViewModel {
    constructor(obj) {
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
    constructor(obj) {
        if (obj != null) {
            super(obj);
            this.FILE_L = obj.FILE_L != null ? new FileItemBase(obj.FILE_L) : null;
        }
    }
}
export var STATUS_FILE;
(function (STATUS_FILE) {
    STATUS_FILE[STATUS_FILE["NOT_INVITE"] = 0] = "NOT_INVITE";
    STATUS_FILE[STATUS_FILE["INVITE"] = 1] = "INVITE";
    STATUS_FILE[STATUS_FILE["XML_VALID"] = 2] = "XML_VALID";
    STATUS_FILE[STATUS_FILE["XML_NOT_VALID"] = 3] = "XML_NOT_VALID";
})(STATUS_FILE || (STATUS_FILE = {}));
export class ErrorItem {
    constructor(obj) {
        if (obj != null) {
            this.Error = obj.Error;
            this.ErrorT = obj.ErrorT;
        }
    }
}
export var ErrorTypeEnum;
(function (ErrorTypeEnum) {
    ErrorTypeEnum[ErrorTypeEnum["Text"] = 0] = "Text";
    ErrorTypeEnum[ErrorTypeEnum["Error"] = 1] = "Error";
})(ErrorTypeEnum || (ErrorTypeEnum = {}));
export var TYPEFILE;
(function (TYPEFILE) {
    TYPEFILE[TYPEFILE["H"] = 0] = "H";
    TYPEFILE[TYPEFILE["T"] = 1] = "T";
    TYPEFILE[TYPEFILE["DP"] = 2] = "DP";
    TYPEFILE[TYPEFILE["DV"] = 3] = "DV";
    TYPEFILE[TYPEFILE["DO"] = 4] = "DO";
    TYPEFILE[TYPEFILE["DS"] = 5] = "DS";
    TYPEFILE[TYPEFILE["DU"] = 6] = "DU";
    TYPEFILE[TYPEFILE["DF"] = 7] = "DF";
    TYPEFILE[TYPEFILE["DD"] = 8] = "DD";
    TYPEFILE[TYPEFILE["DR"] = 9] = "DR";
    TYPEFILE[TYPEFILE["LH"] = 10] = "LH";
    TYPEFILE[TYPEFILE["LT"] = 11] = "LT";
    TYPEFILE[TYPEFILE["LP"] = 12] = "LP";
    TYPEFILE[TYPEFILE["LV"] = 13] = "LV";
    TYPEFILE[TYPEFILE["LO"] = 14] = "LO";
    TYPEFILE[TYPEFILE["LS"] = 15] = "LS";
    TYPEFILE[TYPEFILE["LU"] = 16] = "LU";
    TYPEFILE[TYPEFILE["LF"] = 17] = "LF";
    TYPEFILE[TYPEFILE["LD"] = 18] = "LD";
    TYPEFILE[TYPEFILE["LR"] = 19] = "LR";
    TYPEFILE[TYPEFILE["C"] = 20] = "C";
    TYPEFILE[TYPEFILE["LC"] = 21] = "LC";
    TYPEFILE[TYPEFILE["DA"] = 22] = "DA";
    TYPEFILE[TYPEFILE["DB"] = 23] = "DB";
    TYPEFILE[TYPEFILE["LA"] = 24] = "LA";
    TYPEFILE[TYPEFILE["LB"] = 25] = "LB";
})(TYPEFILE || (TYPEFILE = {}));
export class ViewReestViewModel {
    constructor(obj) {
        this.FP = null;
        if (obj != null) {
            this.ConnectWCFon = obj.ConnectWCFon;
            if (obj.FP != null)
                this.FP = new FilePacket(obj.FP);
        }
    }
}
export class FilePacket {
    constructor(obj) {
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
    constructor(obj) {
        if (obj != null) {
            this.Comment = obj.Comment;
            this.FileName = obj.FileName;
            this.TYPE_NAME = obj.TYPE_NAME;
            this.Type = obj.Type != null ? obj.Type : null;
            this.Process = obj.Process;
        }
    }
}
export class FileItemView extends FileItemViewBase {
    constructor(obj) {
        if (obj != null) {
            super(obj);
            this.FILE_L = obj.FILE_L != null ? new FileItemViewBase(obj.FILE_L) : null;
        }
    }
}
export var ISTEnum;
(function (ISTEnum) {
    ISTEnum[ISTEnum["MAIL"] = 1] = "MAIL";
    ISTEnum[ISTEnum["SITE"] = 2] = "SITE";
})(ISTEnum || (ISTEnum = {}));
export var StatusFilePack;
(function (StatusFilePack) {
    StatusFilePack[StatusFilePack["Open"] = 0] = "Open";
    StatusFilePack[StatusFilePack["Close"] = 1] = "Close";
    StatusFilePack[StatusFilePack["XMLSchemaOK"] = 2] = "XMLSchemaOK";
    StatusFilePack[StatusFilePack["FLKOK"] = 3] = "FLKOK";
    StatusFilePack[StatusFilePack["FLKERR"] = 4] = "FLKERR";
})(StatusFilePack || (StatusFilePack = {}));
export var StepsProcess;
(function (StepsProcess) {
    StepsProcess[StepsProcess["NotInvite"] = 0] = "NotInvite";
    StepsProcess[StepsProcess["Invite"] = 1] = "Invite";
    StepsProcess[StepsProcess["XMLxsd"] = 2] = "XMLxsd";
    StepsProcess[StepsProcess["ErrorXMLxsd"] = 3] = "ErrorXMLxsd";
    StepsProcess[StepsProcess["FlkOk"] = 4] = "FlkOk";
    StepsProcess[StepsProcess["FlkErr"] = 5] = "FlkErr";
})(StepsProcess || (StepsProcess = {}));
//# sourceMappingURL=LoadReestViewModel.js.map