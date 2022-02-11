var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { Expertize, ExpertiseOSN, ExpType } from "../../API/TMKItem";
let ExpertizeEditComponent = class ExpertizeEditComponent {
    constructor(repo) {
        this.repo = repo;
        this.CurrentExpertize = new Expertize(null);
        this.onChange = new EventEmitter();
        this.ExpTypeEnum = ExpType;
        this.OpenType = ExpertizeEditType.New;
        this.Display = false;
        this.ErrList = [];
        this.AddOsn = () => {
            this.CurrentExpertize.OSN.push(new ExpertiseOSN(null));
        };
        this.RemoveOsn = (osn) => {
            const index = this.CurrentExpertize.OSN.indexOf(osn);
            if (index > -1) {
                this.CurrentExpertize.OSN.splice(index, 1);
            }
        };
        this.onSaveProgress = false;
        this.Save = async () => {
            try {
                this.onSaveProgress = true;
                let result = await this.repo.EditExpertizeAsync(this.CurrentExpertize);
                if (result.Result) {
                    this.onChange.emit();
                    this.Close();
                }
                else {
                    this.ErrList = result.ErrMessage;
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onSaveProgress = false;
            }
        };
    }
    ConvertToDate(obj) {
        if (obj)
            return new Date(obj);
        return null;
    }
    Close() {
        this.ErrList = [];
        this.Display = false;
        this.CurrentExpertize = new Expertize(null);
    }
    ShowCreateNewExpertize(type, TMK_ID) {
        this.CurrentExpertize = new Expertize(null);
        this.CurrentExpertize.S_TIP = type;
        this.CurrentExpertize.TMK_ID = TMK_ID;
        this.Display = true;
    }
    EditCreateNewExpertize(exp) {
        this.CurrentExpertize = new Expertize(exp);
        this.Display = true;
    }
};
__decorate([
    Input()
], ExpertizeEditComponent.prototype, "CurrentExpertize", void 0);
__decorate([
    Input()
], ExpertizeEditComponent.prototype, "SPR", void 0);
__decorate([
    Output()
], ExpertizeEditComponent.prototype, "onChange", void 0);
ExpertizeEditComponent = __decorate([
    Component({ selector: "ExpertizeEdit", templateUrl: "ExpertizeEditComponent.html" })
], ExpertizeEditComponent);
export { ExpertizeEditComponent };
var ExpertizeEditType;
(function (ExpertizeEditType) {
    ExpertizeEditType[ExpertizeEditType["New"] = 0] = "New";
    ExpertizeEditType[ExpertizeEditType["Edit"] = 1] = "Edit";
    ExpertizeEditType[ExpertizeEditType["Read"] = 2] = "Read";
})(ExpertizeEditType || (ExpertizeEditType = {}));
//# sourceMappingURL=ExpertizeEditComponent.js.map