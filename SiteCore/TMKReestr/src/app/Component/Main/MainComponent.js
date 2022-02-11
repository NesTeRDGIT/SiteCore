var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component } from "@angular/core";
import { UserINFO } from "../../API/UserINFO";
let MainComponent = class MainComponent {
    constructor(repo, urlHelper, elementRef) {
        this.repo = repo;
        this.urlHelper = urlHelper;
        this.elementRef = elementRef;
        this.TMKViewType = TMKViewType;
        this.CurrentTMKViewType = TMKViewType.TMKReestr;
        let IsTMKAdmin = this.elementRef.nativeElement.getAttribute("[IsTMKAdmin]") === "true";
        let IsTMKUser = this.elementRef.nativeElement.getAttribute("[IsTMKUser]") === "true";
        let IsTMKReader = this.elementRef.nativeElement.getAttribute("[IsTMKReader]") === "true";
        let IsTMKSmo = this.elementRef.nativeElement.getAttribute("[IsTMKSMO]") === "true";
        let CODE_MO = this.elementRef.nativeElement.getAttribute("[CODE_MO]");
        this.userInfo = new UserINFO(IsTMKAdmin, IsTMKUser, IsTMKReader, IsTMKSmo, CODE_MO);
    }
    ngOnInit() {
        let selectIndex = this.urlHelper.getParameter('selectIndex');
        switch (selectIndex) {
            case "1":
                this.CurrentTMKViewType = TMKViewType.TMKReport;
                break;
            case "2":
                this.CurrentTMKViewType = TMKViewType.SPRContact;
                break;
        }
    }
};
MainComponent = __decorate([
    Component({ selector: "app", templateUrl: "MainComponent.html" })
], MainComponent);
export { MainComponent };
var TMKViewType;
(function (TMKViewType) {
    TMKViewType[TMKViewType["TMKReestr"] = 0] = "TMKReestr";
    TMKViewType[TMKViewType["TMKReport"] = 1] = "TMKReport";
    TMKViewType[TMKViewType["SPRContact"] = 2] = "SPRContact";
})(TMKViewType || (TMKViewType = {}));
//# sourceMappingURL=MainComponent.js.map