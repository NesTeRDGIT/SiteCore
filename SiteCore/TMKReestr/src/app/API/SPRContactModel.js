export class SPRContactModel {
    constructor(obj) {
        this.ID_CONTACT_INFO = null;
        this.FAM = null;
        this.IM = null;
        this.OT = null;
        this.TEL = null;
        this.CODE_MO = null;
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
//# sourceMappingURL=SPRContactModel.js.map