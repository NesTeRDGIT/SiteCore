export class EditErrorSPRViewModel {
    constructor(obj) {
        if (obj !== null) {
            this.Sections = obj.Sections.map(x => new SectionSpr(x));
            this.Top30 = obj.Top30.map(x => new ErrorSpr(x));
        }
    }
}
export class SectionSpr {
    constructor(obj) {
        this.Errors = [];
        if (obj !== null) {
            this.SECTION_NAME = obj.SECTION_NAME;
            this.ID_SECTION = obj.ID_SECTION;
            if (obj.Errors != null)
                this.Errors = obj.Errors.map(x => new ErrorSpr(x));
        }
    }
}
export class ErrorSpr {
    constructor(obj) {
        this.OSN_TFOMS = "";
        this.EXAMPLE = "";
        this.ID_ERR = null;
        this.TEXT = "";
        this.ID_SECTION = null;
        if (obj !== null) {
            this.D_EDIT = obj.D_EDIT;
            this.OSN_TFOMS = obj.OSN_TFOMS;
            this.EXAMPLE = obj.EXAMPLE;
            this.ID_ERR = obj.ID_ERR;
            this.TEXT = obj.TEXT;
            this.ID_SECTION = obj.ID_SECTION;
        }
    }
}
//# sourceMappingURL=ErrorSPRModel.js.map