export class UserINFO {
    IsTMKAdmin: boolean = false;
    IsTMKUser: boolean = false;
    IsTMKReader: boolean = false;
    IsTMKSmo: boolean = false;
    CodeMO: string = "";
    constructor(_IsTMKAdmin: boolean, _IsTMKUser: boolean, _IsTMKReader: boolean, _IsTMKSmo: boolean, _CodeMO: string) {
        this.IsTMKAdmin = _IsTMKAdmin;
        this.IsTMKUser = _IsTMKUser;
        this.IsTMKReader = _IsTMKReader;
        this.IsTMKSmo = _IsTMKSmo;
        this.CodeMO = _CodeMO;
    }
}
