import { Component, ViewChild, ElementRef, AfterViewInit, Input } from "@angular/core";

import { IRepository } from "../../API/Repository";
import { PrimeNGConfig } from "primeng/api";
import { Translation } from "primeng/api";

@Component({ selector: "app", templateUrl: "MainComponent.html" })
export class MainComponent {
    isAdmin = false;
    isZpz = false;
    isOms = false;

    ReportType = ReportType;
    reportList: ReportCaption[] = [];
    selectedReport: ReportCaption;

    constructor(public repo: IRepository, private primeNGConfig: PrimeNGConfig, private elementRef: ElementRef) {

        this.isAdmin = this.elementRef.nativeElement.getAttribute("[isAdmin]") === "true";
        this.isZpz = this.elementRef.nativeElement.getAttribute("[isZpz]") === "true";
        this.isOms = this.elementRef.nativeElement.getAttribute("[isOms]") === "true";
    }

    onLoading = (value, type: ReportType) => {
        var val = this.reportList.find(x => x.Type === type);
        if (val != null) {
            val.IsLoad = value;           
        }
    }


    ngOnInit(): void {
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
        if (this.isAdmin || this.isOms)
            this.reportList.push(new ReportCaption("КСГ", ReportType.KSG));


        if (this.isAdmin || this.isZpz)
            this.reportList.push(new ReportCaption("Результативность", ReportType.RESULT));
        if (this.isAdmin || this.isZpz)
            this.reportList.push(new ReportCaption("Результаты контрольно-экспертных мероприятий", ReportType.RESULT_CONTROL));


        if (this.reportList.length!==0)
            this.selectedReport = this.reportList[0];
      

    }
}

enum ReportType {
    HMP,
    HMP_PERIOD,
    ABORT,
    ECO,
    KOHL,
    OKS_ONMK,
    RESULT,
    RESULT_CONTROL,
    SMP,
    DB_State,
    PENS,
    DISP,
    KV2_MTR,
    DLI,
    KSG
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