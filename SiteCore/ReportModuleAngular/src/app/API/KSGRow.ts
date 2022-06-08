export class KSGRow {
    Year: number;
    Month: number;
    Code_MO: string;
    Nam_MOK: string;
    Usl_OK: string;
    N_KSG: string;
    Name_KSG: string;
    Id_Profil: string;
    Name: string;
    C: number;
    S: number;
    C_P: number;
    S_P: number;

    constructor(obj: any) {
        this.Year = obj.Year;
        this.Month = obj.Month;
        this.Code_MO = obj.Code_MO;
        this.Nam_MOK = obj.Nam_MOK;
        this.Usl_OK = obj.Usl_OK === 1 ? "Стационар": "Дневной стационар";
        this.N_KSG = obj.N_KSG;
        this.Name_KSG = obj.Name_KSG;
        this.Id_Profil = obj.Id_Profil;
        this.Name = obj.Name;
        this.C = obj.C;
        this.S = obj.S;
        this.C_P = obj.C_P;
        this.S_P = obj.S_P;
    }
}
