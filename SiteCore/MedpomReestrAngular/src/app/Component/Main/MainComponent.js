var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
let MainComponent = class MainComponent {
    constructor(repo, primeNGConfig, elementRef, urlHelper) {
        this.repo = repo;
        this.primeNGConfig = primeNGConfig;
        this.elementRef = elementRef;
        this.urlHelper = urlHelper;
        this.isAdmin = false;
        this.isMO = false;
        this.isCS = false;
        this.ReportType = ReportType;
        this.reportList = [];
        this.onLoading = (value, type) => {
            var val = this.reportList.find(x => x.Type === type);
            if (val != null) {
                val.IsLoad = value;
            }
        };
        this.onSendReestr = () => {
            this.selectedReport = this.reportList.find(x => x.Type === ReportType.ViewMedpom);
        };
        this.isAdmin = this.elementRef.nativeElement.getAttribute("[isAdmin]") === "true";
        this.isMO = this.elementRef.nativeElement.getAttribute("[isMO]") === "true";
        this.isCS = this.elementRef.nativeElement.getAttribute("[isCS]") === "true";
    }
    get selectedReport() {
        return this._selectedReport;
    }
    ;
    set selectedReport(value) {
        this._selectedReport = value;
        this.urlHelper.changeParameter('selectIndex', this._selectedReport.Type);
    }
    ;
    ngOnInit() {
        this.primeNGConfig.setTranslation(new RusPrime());
        if (this.isMO || this.isAdmin) {
            this.reportList.push(new ReportCaption("Загрузка реестров", ReportType.LoadMedpom));
            this.reportList.push(new ReportCaption("Статус проверки", ReportType.ViewMedpom));
            this.reportList.push(new ReportCaption("Справочник ошибок", ReportType.ErrorSPR));
        }
        if (this.isCS || this.isAdmin)
            this.reportList.push(new ReportCaption("Поиск в ЦС", ReportType.FIND_CS));
        if (this.reportList.length !== 0) {
            const selectIndex = +this.urlHelper.getParameter('selectIndex');
            if (!isNaN(selectIndex)) {
                this.selectedReport = this.reportList.find(x => x.Type === selectIndex);
            }
            else {
                this.selectedReport = this.reportList[0];
            }
        }
    }
};
MainComponent = __decorate([
    Component({ selector: "app", templateUrl: "MainComponent.html" })
], MainComponent);
export { MainComponent };
var ReportType;
(function (ReportType) {
    ReportType[ReportType["LoadMedpom"] = 0] = "LoadMedpom";
    ReportType[ReportType["ViewMedpom"] = 1] = "ViewMedpom";
    ReportType[ReportType["ErrorSPR"] = 2] = "ErrorSPR";
    ReportType[ReportType["FIND_CS"] = 3] = "FIND_CS";
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