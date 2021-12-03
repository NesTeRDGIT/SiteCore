import { Component, ViewChild, ElementRef, AfterViewInit, Input } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { PrimeNGConfig } from "primeng/api";
import { Translation } from "primeng/api";
import { URLHelper } from '../../API/URLHelper';

@Component({ selector: "app", templateUrl: "MainComponent.html" })
export class MainComponent {
    isAdmin = false;
    isMO = false;
    isCS = false;
    ReportType = ReportType;
    reportList: ReportCaption[] = [];
    _selectedReport: ReportCaption;
    get selectedReport(): ReportCaption {
        return this._selectedReport;
    };
    set selectedReport(value: ReportCaption) {
        this._selectedReport = value;
        this.urlHelper.changeParameter('selectIndex', this._selectedReport.Type);
    };


    constructor(public repo: IRepository, private primeNGConfig: PrimeNGConfig, private elementRef: ElementRef, private urlHelper: URLHelper) {

        this.isAdmin = this.elementRef.nativeElement.getAttribute("[isAdmin]") === "true";
        this.isMO = this.elementRef.nativeElement.getAttribute("[isMO]") === "true";
        this.isCS = this.elementRef.nativeElement.getAttribute("[isCS]") === "true";



    }

    onLoading = (value, type: ReportType) => {
        var val = this.reportList.find(x => x.Type === type);
        if (val != null) {
            val.IsLoad = value;
        }
    }

    onSendReestr = () => {
        this.selectedReport = this.reportList.find(x => x.Type === ReportType.ViewMedpom);
    }


    ngOnInit(): void {
        this.primeNGConfig.setTranslation(new RusPrime());
        if (this.isMO || this.isAdmin) {
            this.reportList.push(new ReportCaption("Загрузка реестров", ReportType.LoadMedpom));
            this.reportList.push(new ReportCaption("Статус проверки", ReportType.ViewMedpom));
            this.reportList.push(new ReportCaption("Справочник ошибок", ReportType.ErrorSPR));
        }
        if (this.isCS || this.isAdmin)
            this.reportList.push(new ReportCaption("Поиск в ЦС", ReportType.FIND_CS));


        if (this.reportList.length !== 0) {
            const selectIndex: ReportType = +this.urlHelper.getParameter('selectIndex');
            if (!isNaN(selectIndex)) {
                this.selectedReport = this.reportList.find(x => x.Type === selectIndex);
            } else {
                this.selectedReport = this.reportList[0];
            }
        }
    }



}

enum ReportType {
    LoadMedpom,
    ViewMedpom,
    ErrorSPR,
    FIND_CS
}

class ReportCaption {
    Name: string;
    Type: ReportType;
    IsLoad: boolean = false;
    constructor(name: string, type: ReportType) {
        this.Name = name;
        this.Type = type;
    }
}


class RusPrime implements Translation {

    startsWith?: string = "Starts with";
    contains?: string = "Contains";
    notContains?: string = "Not contains";
    endsWith?: string = "Ends with";
    equals?: string = "Equals";
    notEquals?: string = "Not equals";
    noFilter?: string = "No Filter";
    lt?: string = "Less than";
    lte?: string = "Less than or equal to";
    gt?: string = "Greater than";
    gte?: string = "Great then or equals";
    is?: string = "Is";
    isNot?: string = "Is not";
    before?: string = "Before";
    after?: string = "After";
    dateIs?: string = "dateIs";
    dateIsNot?: string = "dateIsNot";
    dateBefore?: string = "dateBefore";
    dateAfter?: string = "dateAfter";
    clear?: string = "Clear";
    apply?: string = "Apply";
    matchAll?: string = "Match All";
    matchAny?: string = "Match Any";
    addRule?: string = "Add Rule";
    removeRule?: string = "Remove Rule";
    accept?: string = "Yes";
    reject?: string = "No";
    choose?: string = "Choose";
    upload?: string = "Upload";
    cancel?: string = "Cancel";
    dayNames?: string[] = ["Воскресение", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"];
    dayNamesShort?: string[] = ["Вск", "Пон", "Вт", "Ср", "Чт", "Пят", "Суб"];
    dayNamesMin?: string[] = ["Вс", "По", "Вт", "Ср", "Чт", "Пт", "Сб"];
    monthNames?: string[] = ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"];
    monthNamesShort?: string[] = ["Янв", "Фев", "Мар", "Апр", "Май", "Июн", "Июл", "Авг", "Сен", "Окт", "Ноя", "Дек"];
    today?: string = "Сегодня";
    weekHeader?: string = "нд";
    weak?: string = "Неделя";
    medium?: string = "Medium";
    strong?: string = "Strong";
    passwordPrompt?: string = "Enter a password";
    emptyMessage?: string = "No results found";
    emptyFilterMessage?: string = "No results found";
}
/*


*/