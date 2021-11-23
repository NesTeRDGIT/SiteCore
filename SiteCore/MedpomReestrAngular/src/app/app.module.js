var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
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
import { ENUMPipe } from "./EnumPIPE";
import { SafeHtmlPipe } from "./SafeHtmlPipe";
import { MainComponent } from "./Component/Main/MainComponent";
import { Repository, IRepository } from "./API/Repository";
import { ListboxModule } from 'primeng/listbox';
import { DividerModule } from 'primeng/divider';
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { LoadReestrComponent } from './Component/LoadReestrComponent/LoadReestrComponent';
import { FileLoader } from './Component/FileLoader/FileLoader';
import { ProgressBar } from './Component/ProgressBar/ProgressBar';
import { ViewReestrComponent } from './Component/ViewReestrComponent/ViewReestrComponent';
import { ISTEnumRusPipe, StatusFilePackRusPipe, StepsProcessRusPipe } from "./Component/ViewReestrComponent/StatusFilePackPipe";
import { ContentLoad } from "./Component/ContentLoad/ContentLoad";
import { InstructionDialog } from "./Component/InstructionDialog/InstructionDialog";
import { ERROR_SPR } from "./Component/ERROR_SPR/ERROR_SPR";
import { ERROR_EDIT } from "./Component/ERROR_EDIT/ERROR_EDIT";
import { URLHelper } from './API/URLHelper';
registerLocaleData(localeRu);
let AppModule = class AppModule {
};
AppModule = __decorate([
    NgModule({
        imports: [BrowserModule, FormsModule, BrowserAnimationsModule, TableModule, TabViewModule, HttpClientModule, SplitterModule, ListboxModule, DividerModule, CalendarModule, ProgressSpinnerModule, DialogModule, InputTextModule, AutoCompleteModule, CKEditorModule],
        declarations: [MainComponent, YesNoPipe, ENUMPipe, ISTEnumRusPipe, SafeHtmlPipe, StepsProcessRusPipe, StatusFilePackRusPipe, LoadReestrComponent, FileLoader, ProgressBar, ViewReestrComponent, ContentLoad, InstructionDialog, ERROR_SPR, ERROR_EDIT],
        providers: [{ provide: IRepository, useClass: Repository },
            { provide: LOCALE_ID, useValue: 'ru' },
            { provide: URLHelper, useClass: URLHelper }
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