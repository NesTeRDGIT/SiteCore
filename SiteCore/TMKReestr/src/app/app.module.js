var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { NgModule, LOCALE_ID } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from "primeng/table";
import { TabViewModule } from 'primeng/tabview';
import { SplitterModule } from 'primeng/splitter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from "@angular/common/http";
import { YesNoPipe } from "./YesNoPIPE";
import { SPRTimeMarkPipe } from "./SPRTimeMarkPipe";
import { ActualSPRPipe } from "./ActualSPRPipe";
import { S_TIP_NAMEPipe } from "./S_TIP_NAMEPipe";
import { S_OSN_NAMEPipe } from "./S_OSN_NAMEPipe";
import { InputTextareaModule } from 'primeng/inputtextarea';
import { InputNumberModule } from 'primeng/inputnumber';
import { MainComponent } from "./Component/Main/MainComponent";
import { Repository, IRepository } from "./API/Repository";
import { ListboxModule } from 'primeng/listbox';
import { DividerModule } from 'primeng/divider';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ContextMenuModule } from 'primeng/contextmenu';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { InputMaskModule } from 'primeng/inputmask';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { TooltipModule } from 'primeng/tooltip';
import { SelectButtonModule } from 'primeng/selectbutton';
import { DropdownModule } from 'primeng/dropdown';
import { SplitButtonModule } from 'primeng/splitbutton';
import { PanelModule } from 'primeng/panel';
import { CheckboxModule } from 'primeng/checkbox';
import { AccordionModule } from 'primeng/accordion';
import { InputSwitchModule } from 'primeng/inputswitch';
import { URLHelper } from './API/URLHelper';
import { FindMenuComponent } from './Component/FindMenu/FindMenuComponent';
import { EditTMKComponent } from './Component/EditTMK/EditTMKComponent';
import { TmkSmoDataComponent } from './Component/TmkSmoData/TmkSmoDataComponent';
import { ExpertizeEditComponent } from './Component/ExpertizeEdit/ExpertizeEditComponent';
import { ContentLoad } from './Component/ContentLoad/ContentLoad';
import { ReestrTMKComponent } from './Component/ReestrTMK/ReestrTMK';
import { SPRContactComponent } from './Component/SPRContact/SPRContact';
import { SPRContactEditComponent } from './Component/SPRContactEdit/SPRContactEdit';
import { ReportComponent } from './Component/Report/Report';
import { FindPacientComponent } from './Component/FindPacient/FindPacient';
import { FindExpertizeComponent } from './Component/FindExpertize/FindExpertize';
registerLocaleData(localeRu);
let AppModule = class AppModule {
};
AppModule = __decorate([
    NgModule({
        imports: [BrowserModule, FormsModule, BrowserAnimationsModule, TableModule, TabViewModule, HttpClientModule, SplitterModule, ListboxModule, DividerModule, CalendarModule, ProgressSpinnerModule, DialogModule, InputTextModule, AutoCompleteModule,
            ContextMenuModule, InputMaskModule, DropdownModule, SplitButtonModule, PanelModule, SelectButtonModule, CheckboxModule, AccordionModule, TooltipModule, InputSwitchModule, InputTextareaModule, InputNumberModule],
        declarations: [MainComponent, YesNoPipe, S_TIP_NAMEPipe, S_OSN_NAMEPipe, ActualSPRPipe, SPRTimeMarkPipe, FindMenuComponent, EditTMKComponent, TmkSmoDataComponent, ExpertizeEditComponent, ContentLoad, ReestrTMKComponent, SPRContactComponent, SPRContactEditComponent,
            ReportComponent, FindPacientComponent, FindExpertizeComponent],
        providers: [{ provide: IRepository, useClass: Repository },
            { provide: LOCALE_ID, useValue: 'ru' },
            { provide: URLHelper, useClass: URLHelper },
        ],
        bootstrap: [MainComponent]
    })
], AppModule);
export { AppModule };
const isLeapYear = year => ((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0);
const getDaysInMonth = (year, month) => [31, (isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
Date.prototype.isLeapYear = function () {
    return isLeapYear(this.getFullYear());
};
Date.prototype.getDaysInMonth = function () {
    return getDaysInMonth(this.getFullYear(), this.getMonth());
};
Date.prototype.addMonths = function (value) {
    const n = this.getDate();
    this.setDate(1);
    this.setMonth(this.getMonth() + value);
    this.setDate(Math.min(n, this.getDaysInMonth()));
    return this;
};
//# sourceMappingURL=app.module.js.map