export class SPRContactModel {
    ID_CONTACT_INFO: number = null;
    FAM: string = null;
    IM: string = null;
    OT: string = null;
    TEL: string = null;
    CODE_MO: string = null;
    constructor(obj: any) {
        if (obj != null) {
            this.ID_CONTACT_INFO = obj.ID_CONTACT_INFO;
            this.FAM = obj.FAM;
            this.IM = obj.FAM;
            this.OT = obj.OT;
            this.TEL = obj.TEL;
            this.CODE_MO = obj.CODE_MO;
        }
    }
}