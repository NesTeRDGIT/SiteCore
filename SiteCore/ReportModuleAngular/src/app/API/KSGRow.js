export class KSGRow {
    constructor(obj) {
        this.Year = obj.Year;
        this.Month = obj.Month;
        this.Code_MO = obj.Code_MO;
        this.Nam_MOK = obj.Nam_MOK;
        this.Usl_OK = obj.Usl_OK === 1 ? "Стационар" : "Дневной стационар";
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
//# sourceMappingURL=KSGRow.js.map