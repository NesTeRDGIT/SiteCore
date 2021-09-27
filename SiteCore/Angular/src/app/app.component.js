var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
let AppComponent = class AppComponent {
    constructor(httpClient) {
        this.httpClient = httpClient;
        this.PageINFO = new PageINFO();
        this.Items = [];
        this.SluchList = [];
        this.SelectItems = [];
        this.LastONK_REESTR_ID = 0;
        this.index = 0;
    }
    get SelectItem() {
        if (this.SelectItems.length === 0)
            return new ONK_REESTR(null);
        return this.SelectItems[0];
    }
    loadCustomers(event) {
        this.PageINFO.CountOnPage = event.rows;
        this.PageINFO.CurrentPage = event.first / event.rows + 1;
        this.loading = true;
        this.httpClient.get(`GetONKList?Page=${this.PageINFO.CurrentPage}&&CountOnPage=${this.PageINFO.CountOnPage}`).subscribe(data => {
            if (data["Result"] === true) {
                const Pages = data["Value"]["Pages"];
                this.PageINFO.CountOnPage = Pages.CountOnPage;
                this.PageINFO.CurrentPage = Pages.CurrentPage;
                this.PageINFO.PageCount = Pages.PageCount;
                this.Items = [];
                const Items = data["Value"]["Items"];
                Items.forEach(x => this.Items.push(new ONK_REESTR(x)));
            }
            this.loading = false;
        }, error => { Alert(error); this.loading = false; });
    }
    handleRowSelect(event) {
        const item = event.data;
        if (item.ONK_REESTR_ID !== this.LastONK_REESTR_ID) {
            this.index = 0;
            this.LastONK_REESTR_ID = item.ONK_REESTR_ID;
        }
    }
    OnChangeSelectTab(e) {
        const index = e.index;
        const ONK_REESTR_ID = this.SelectItem.ONK_REESTR_ID;
        if (index === 1) {
            this.LoadSluchList(ONK_REESTR_ID);
        }
    }
    LoadSluchList(ONK_REESTR_ID) {
        this.loadingSluchList = true;
        this.SluchList = [];
        this.httpClient.get(`SLList?ONK_REESTR_ID=${ONK_REESTR_ID}`).subscribe(data => {
            if (data["Result"] === true) {
                this.loadingSluchList = false;
                this.SluchList = [];
                const Items = data["Value"];
                Items.forEach(x => this.SluchList.push(new SLUCH_ITEM(x)));
            }
            this.loadingSluchList = false;
        }, error => { Alert(error); this.loadingSluchList = false; });
    }
};
AppComponent = __decorate([
    Component({
        selector: "app",
        templateUrl: "./app.component.html"
    })
], AppComponent);
export { AppComponent };
class PageINFO {
    constructor() {
        this.CountOnPage = 50;
        this.CurrentPage = 1;
        this.PageCount = 0;
    }
    get totalRecords() {
        return this.CountOnPage * this.PageCount;
    }
}
class ONK_REESTR {
    constructor(obj) {
        if (obj != null) {
            this.ONK_REESTR_ID = obj.ONK_REESTR_ID;
            this.ENP = obj.ENP;
            this.FAM = obj.FAM;
            this.IM = obj.IM;
            this.OT = obj.OT;
            this.DR = obj.DR;
            this.DDEATH = obj.DDEATH;
            this.SMO = obj.SMO;
            this.DATE_DS_ONK = obj.DATE_DS_ONK;
            this.DS_ONK_DISP = obj.DS_ONK_DISP;
            this.DS1 = obj.DS1;
            this.DS1_FULLNAME = obj.DS1_FULLNAME;
            this.DS1_DATE = obj.DS1_DATE;
            this.DATE_DN = obj.DATE_DN;
            this.DS_ONK_MO = obj.DS_ONK_MO;
            this.N_DS_ONK_MO = obj.N_DS_ONK_MO;
            this.DS_ONK_MO_FULLNAME = obj.DS_ONK_MO_FULLNAME;
            this.DATE_DS_ONK_ONK = obj.DATE_DS_ONK_ONK;
            this.DS_ONK_MO_ONK = obj.DS_ONK_MO_ONK;
            this.N_DS_ONK_MO_ONK = obj.N_DS_ONK_MO_ONK;
            this.DS_ONK_MO_ONK_FULLNAME = obj.DS_ONK_MO_ONK_FULLNAME;
            this.DATE_BIO = obj.DATE_BIO;
            this.MO_BIO = obj.MO_BIO;
            this.N_MO_BIO = obj.N_MO_BIO;
            this.MO_BIO_FULLNAME = obj.MO_BIO_FULLNAME;
            this.DATE_KT = obj.DATE_KT;
            this.MO_KT = obj.MO_KT;
            this.N_MO_KT = obj.N_MO_KT;
            this.MO_KT_FULLNAME = obj.MO_KT_FULLNAME;
            this.DATE_HIM = obj.DATE_HIM;
            this.YEAR = obj.YEAR;
            this.MONTH = obj.MONTH;
        }
    }
}
class SLUCH_ITEM {
    constructor(obj) {
        if (obj != null) {
            this.SLUCH_Z_ID = obj.SLUCH_Z_ID;
            this.LPU_NAME = obj.LPU_NAME;
            this.N_USL_OK = obj.N_USL_OK;
            this.DATE_1 = obj.DATE_1;
            this.DATE_2 = obj.DATE_2;
            this.N_DS1 = obj.N_DS1;
            this.N_RSLT = obj.N_RSLT;
            this.DS_ONK = obj.DS_ONK;
            this.N_SMO = obj.N_SMO;
            this.YEAR = obj.YEAR;
            this.MONTH = obj.MONTH;
            this.DSCHET = obj.DSCHET;
            this.NSCHET = obj.NSCHET;
        }
    }
}
//# sourceMappingURL=app.component.js.map