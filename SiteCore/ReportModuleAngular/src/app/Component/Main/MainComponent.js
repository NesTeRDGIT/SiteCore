var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
let MainComponent = class MainComponent {
    constructor(repo, primeNGConfig, elementRef) {
        this.repo = repo;
        this.primeNGConfig = primeNGConfig;
        this.elementRef = elementRef;
        this.isAdmin = false;
        this.isZpz = false;
        this.isOms = false;
        this.ReportType = ReportType;
        this.reportList = [];
        this.onLoading = (value, type) => {
            var val = this.reportList.find(x => x.Type === type);
            if (val != null) {
                val.IsLoad = value;
            }
        };
        this.isAdmin = this.elementRef.nativeElement.getAttribute("[isAdmin]") === "true";
        this.isZpz = this.elementRef.nativeElement.getAttribute("[isZpz]") === "true";
        this.isOms = this.elementRef.nativeElement.getAttribute("[isOms]") === "true";
    }
    ngOnInit() {
        this.primeNGConfig.setTranslation(new RusPrime());
        this.reportList.push(new ReportCaption("Текущее состояние БД", ReportType.DB_State));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Текущее ВМП", ReportType.HMP));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("ВМП за период", ReportType.HMP_PERIOD));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Аборты", ReportType.ABORT));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("ЭКО", ReportType.ECO));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Кохлеарная имплантация", ReportType.KOHL));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("ОКС\\ОНМК", ReportType.OKS_ONMK));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Скорая медицинская помощь", ReportType.SMP));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Пожилые", ReportType.PENS));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("Диспансеризация", ReportType.DISP));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("КВ2(МТР)", ReportType.KV2_MTR));
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("ДЛИ", ReportType.DLI));
        if (this.isAdmin || this.isZpz)
            this.reportList.push(new ReportCaption("Результативность", ReportType.RESULT));
        if (this.isAdmin || this.isZpz)
            this.reportList.push(new ReportCaption("Результаты контрольно-экспертных мероприятий", ReportType.RESULT_CONTROL));
        if (this.reportList.length !== 0)
            this.selectedReport = this.reportList[0];
    }
};
MainComponent = __decorate([
    Component({ selector: "app", templateUrl: "MainComponent.html" })
], MainComponent);
export { MainComponent };
var ReportType;
(function (ReportType) {
    ReportType[ReportType["HMP"] = 0] = "HMP";
    ReportType[ReportType["HMP_PERIOD"] = 1] = "HMP_PERIOD";
    ReportType[ReportType["ABORT"] = 2] = "ABORT";
    ReportType[ReportType["ECO"] = 3] = "ECO";
    ReportType[ReportType["KOHL"] = 4] = "KOHL";
    ReportType[ReportType["OKS_ONMK"] = 5] = "OKS_ONMK";
    ReportType[ReportType["RESULT"] = 6] = "RESULT";
    ReportType[ReportType["RESULT_CONTROL"] = 7] = "RESULT_CONTROL";
    ReportType[ReportType["SMP"] = 8] = "SMP";
    ReportType[ReportType["DB_State"] = 9] = "DB_State";
    ReportType[ReportType["PENS"] = 10] = "PENS";
    ReportType[ReportType["DISP"] = 11] = "DISP";
    ReportType[ReportType["KV2_MTR"] = 12] = "KV2_MTR";
    ReportType[ReportType["DLI"] = 13] = "DLI";
})(ReportType || (ReportType = {}));
class ReportCaption {
    constructor(name, type) {
        this.IsLoad = false;
        this.Name = name;
        this.Type = type;
    }
}
class RusPrime {
    constructor() {
        this.startsWith = "Starts with";
        this.contains = "Contains";
        this.notContains = "Not contains";
        this.endsWith = "Ends with";
        this.equals = "Equals";
        this.notEquals = "Not equals";
        this.noFilter = "No Filter";
        this.lt = "Less than";
        this.lte = "Less than or equal to";
        this.gt = "Greater than";
        this.gte = "Great then or equals";
        this.is = "Is";
        this.isNot = "Is not";
        this.before = "Before";
        this.after = "After";
        this.dateIs = "dateIs";
        this.dateIsNot = "dateIsNot";
        this.dateBefore = "dateBefore";
        this.dateAfter = "dateAfter";
        this.clear = "Clear";
        this.apply = "Apply";
        this.matchAll = "Match All";
        this.matchAny = "Match Any";
        this.addRule = "Add Rule";
        this.removeRule = "Remove Rule";
        this.accept = "Yes";
        this.reject = "No";
        this.choose = "Choose";
        this.upload = "Upload";
        this.cancel = "Cancel";
        this.dayNames = ["Воскресение", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"];
        this.dayNamesShort = ["Вск", "Пон", "Вт", "Ср", "Чт", "Пят", "Суб"];
        this.dayNamesMin = ["Вс", "По", "Вт", "Ср", "Чт", "Пт", "Сб"];
        this.monthNames = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
        this.monthNamesShort = ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"];
        this.today = "Сегодня";
        this.weekHeader = "нд";
        this.weak = "Неделя";
        this.medium = "Medium";
        this.strong = "Strong";
        this.passwordPrompt = "Enter a password";
        this.emptyMessage = "No results found";
        this.emptyFilterMessage = "No results found";
    }
}
/*


*/ 
//# sourceMappingURL=MainComponent.js.map