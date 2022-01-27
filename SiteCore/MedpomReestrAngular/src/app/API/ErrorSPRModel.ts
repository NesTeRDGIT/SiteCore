
export class EditErrorSPRViewModel {
    Sections: SectionSpr[];
    Top30: ErrorSpr[];
    constructor(obj: any) {
        if (obj !== null) {
            this.Sections = obj.Sections.map(x=>new SectionSpr(x));
            this.Top30 = obj.Top30.map(x => new ErrorSpr(x));
        }
    }
}



export class SectionSpr {
    SECTION_NAME: string;
    ID_SECTION: number;
    Errors: ErrorSpr[] = [];

    constructor(obj: any) {
        if (obj !== null) {
            this.SECTION_NAME = obj.SECTION_NAME;
            this.ID_SECTION = obj.ID_SECTION;
            if (obj.Errors!= null)
                this.Errors = obj.Errors.map(x=>new ErrorSpr(x));
        }
    }

}

export class ErrorSpr {
    D_EDIT: Date;
    OSN_TFOMS: string = "";
    EXAMPLE: string = "";
    ID_ERR: number | null = null;
    TEXT: string = "";
    ID_SECTION: number | null = null;
    D_BEGIN : Date | null = null;
    D_END : Date | null = null;
    IsMEK : boolean;


    SELECTED:boolean;
    constructor(obj: any) {
       
        if (obj !== null) {
            this.D_EDIT = obj.D_EDIT;
            this.OSN_TFOMS = obj.OSN_TFOMS;
            this.EXAMPLE = obj.EXAMPLE;
            this.ID_ERR = obj.ID_ERR;
            this.TEXT = obj.TEXT;
            this.ID_SECTION = obj.ID_SECTION;
            if(obj.D_BEGIN!=null)
                this.D_BEGIN = new Date(obj.D_BEGIN);
            if(obj.D_END!=null)
                this.D_END = new Date(obj.D_END);
            this.IsMEK = obj.ISMEK;
        }
    }
}