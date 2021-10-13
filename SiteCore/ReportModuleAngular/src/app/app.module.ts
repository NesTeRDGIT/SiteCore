import { NgModule, LOCALE_ID, Input  } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from "primeng/table";
import { TabViewModule } from 'primeng/tabview';
import { SplitterModule } from 'primeng/splitter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 
import { HttpClientModule } from "@angular/common/http";
import { YesNoPipe } from "./YesNoPIPE";
import { MainComponent } from "./Component/Main/MainComponent";
import { Repository, IRepository } from "./API/Repository";
import { ListboxModule } from 'primeng/listbox';
import { DividerModule } from 'primeng/divider';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';

import { HmpReportComponent } from "./Component/HMP_REPORT/HMP_REPORT";
import { AbortReportComponent } from "./Component/ABORT_REPORT/ABORT_REPORT";
import { EcoReportComponent } from "./Component/ECO_REPORT/ECO_REPORT";
import { KohlReportComponent } from "./Component/KOHL_REPORT/KOHL_REPORT";
import { OksOnmkReportComponent } from "./Component/OKSONMK_REPORT/OKSONMK_REPORT";
import { ResultPokReportComponent } from "./Component/RESULT_POK_REPORT/RESULT_POK_REPORT";
import { ResultControlComponent } from "./Component/ResultContrReport/ResultControlReport";
import { SmpReportComponent } from "./Component/SMP_REPORT/SMP_REPORT";

import { DBStateComponent } from "./Component/DBState/DBState";
import { PensReportComponent } from "./Component/PENS_REPORT/PENS_REPORT";
import { HmpReportPeriodComponent } from "./Component/HMP_PERIOD_REPORT/HMP_PERIOD_REPORT";




registerLocaleData(localeRu);

@NgModule({
    imports: [BrowserModule, FormsModule, BrowserAnimationsModule, TableModule, TabViewModule, HttpClientModule, SplitterModule, ListboxModule, DividerModule, CalendarModule],
    declarations: [MainComponent, YesNoPipe, HmpReportComponent, AbortReportComponent, EcoReportComponent, KohlReportComponent, OksOnmkReportComponent, ResultPokReportComponent, ResultControlComponent, SmpReportComponent, DBStateComponent, PensReportComponent, HmpReportPeriodComponent],
    providers: [{ provide: IRepository, useClass: Repository },
                { provide: LOCALE_ID, useValue: 'ru' }],
    bootstrap: [MainComponent]
})
export class AppModule {
  
}