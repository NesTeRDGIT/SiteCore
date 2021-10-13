export class PensRow {


CODE_MO: string;
NAME: string;
DISP_M_GR1: number;
DISP_G_GR1: number;
PROF_M_GR1: number;
PROF_G_GR1: number;
DISP_M_GR2: number;
DISP_G_GR2: number;
PROF_M_GR2: number;
PROF_G_GR2: number;

    constructor(obj: any) {
        this.CODE_MO = obj.CODE_MO;
        this.NAME = obj.NAME;
        this.DISP_M_GR1 = obj.DISP_M_GR1;
        this.DISP_G_GR1 = obj.DISP_G_GR1;
        this.PROF_M_GR1 = obj.PROF_M_GR1;
        this.PROF_G_GR1 = obj.PROF_G_GR1;
        this.DISP_M_GR2 = obj.DISP_M_GR2;
        this.DISP_G_GR2 = obj.DISP_G_GR2;
        this.PROF_M_GR2 = obj.PROF_M_GR2;
        this.PROF_G_GR2 = obj.PROF_G_GR2;
        
    }
}