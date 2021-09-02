import { Component, ViewChild, ElementRef, AfterViewInit } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LazyLoadEvent } from "primeng/api";

declare  function Alert(Value:any):any;



@Component({
    selector: "app",
    templateUrl: "./app.component.html"
})

export class AppComponent {

   
    constructor(public httpClient: HttpClient) {

    }
    PageINFO: PageINFO = new PageINFO();
    Items: ONK_REESTR[] = [];
    SluchList: SLUCH_ITEM[] = [];
    loadingSluchList: boolean;
    SelectItems: ONK_REESTR[] = [];
    get SelectItem() {
        if (this.SelectItems.length === 0)
            return new ONK_REESTR(null);
        return this.SelectItems[0];
    }



    loading: boolean;
    loadCustomers(event: LazyLoadEvent) {
        this.PageINFO.CountOnPage = event.rows;
        this.PageINFO.CurrentPage = event.first/event.rows+1;
        this.loading = true;

        this.httpClient.get(`GetONKList?Page=${this.PageINFO.CurrentPage}&&CountOnPage=${this.PageINFO.CountOnPage}`).subscribe(
            data => {
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
            },
            error => { Alert(error); this.loading = false; }
        );
        
    }
    LastONK_REESTR_ID: number = 0;
    index:number=0;
    handleRowSelect(event) {
        const item: ONK_REESTR = event.data;

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
  
    LoadSluchList(ONK_REESTR_ID: number) {
        this.loadingSluchList = true;
        this.SluchList = [];
        this.httpClient.get(`SLList?ONK_REESTR_ID=${ONK_REESTR_ID}`).subscribe(
            data => {
                if (data["Result"] === true) {
                    this.loadingSluchList = false;
                    this.SluchList = [];
                    const Items = data["Value"];
                    Items.forEach(x => this.SluchList.push(new SLUCH_ITEM(x)));
                }
                this.loadingSluchList = false;
            },
            error => { Alert(error); this.loadingSluchList = false; }
        );
    }



}


class PageINFO {
    CountOnPage: number = 50;
    CurrentPage: number = 1;
    PageCount: number = 0;
  
    get totalRecords() {
        return this.CountOnPage * this.PageCount;
    }

}
class ONK_REESTR {
    ONK_REESTR_ID: number;
    ENP: string;
    FAM: string;
    IM: string;
    OT: string;
    DR: Date;
    DDEATH: Date;
    SMO: string;
    DATE_DS_ONK: Date;
    DS_ONK_DISP: boolean;
    DS1: string;
    DS1_FULLNAME: string;
    DS1_DATE: Date;
    DATE_DN: Date;
    DS_ONK_MO: string;
    N_DS_ONK_MO: string;
    DS_ONK_MO_FULLNAME: string;
    DATE_DS_ONK_ONK: Date;
    DS_ONK_MO_ONK: string;
    N_DS_ONK_MO_ONK: string;
    DS_ONK_MO_ONK_FULLNAME: string;
    DATE_BIO: Date;
    MO_BIO: string;
    N_MO_BIO: string;
    MO_BIO_FULLNAME: string;
    DATE_KT: Date;
    MO_KT: string;
    N_MO_KT: string;
    MO_KT_FULLNAME: string;
    DATE_HIM: Date;
    YEAR: number;
    MONTH: number;
    constructor(obj: any) {
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
    SLUCH_Z_ID: number;
    LPU_NAME: number;
    N_USL_OK: number;
    DATE_1: number;
    DATE_2: number;
    N_DS1: number;
    N_RSLT: number;
    DS_ONK: number;
    N_SMO: number;
    YEAR: number;
    MONTH: number;

    constructor(obj: any) {
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
        }
    }


}
