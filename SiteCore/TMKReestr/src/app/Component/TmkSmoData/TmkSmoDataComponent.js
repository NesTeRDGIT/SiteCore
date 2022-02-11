var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input } from "@angular/core";
let TmkSmoDataComponent = class TmkSmoDataComponent {
    constructor(repo) {
        this.repo = repo;
        this.IsReadOnly = true;
        this.onSaveProgress = false;
        this.onSaveStatus = null;
        this.onSaveComment = "";
        this.Save = async () => {
            try {
                this.ClearProgress();
                this.onSaveProgress = true;
                let result = await this.repo.SaveSmoDataTMKItemAsync(this.CurrentTMK);
                if (result.Result) {
                    this.onSaveStatus = true;
                    this.onSaveComment = "Данные успешно сохранены";
                    setTimeout(() => { this.onSaveStatus = null; }, 4000);
                }
                else {
                    this.onSaveStatus = false;
                    this.onSaveComment = result.Error;
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onSaveProgress = false;
            }
        };
        this.ClearProgress = () => {
            this.onSaveProgress = false;
            this.onSaveStatus = null;
            this.onSaveComment = "";
        };
    }
    get CurrentTMK() {
        return this._CurrentTMK;
    }
    set CurrentTMK(value) {
        this._CurrentTMK = value;
        this.ClearProgress();
    }
};
__decorate([
    Input()
], TmkSmoDataComponent.prototype, "SPR", void 0);
__decorate([
    Input()
], TmkSmoDataComponent.prototype, "IsReadOnly", void 0);
__decorate([
    Input()
], TmkSmoDataComponent.prototype, "CurrentTMK", null);
TmkSmoDataComponent = __decorate([
    Component({ selector: "TmkSmoData", templateUrl: "TmkSmoDataComponent.html" })
], TmkSmoDataComponent);
export { TmkSmoDataComponent };
//# sourceMappingURL=TmkSmoDataComponent.js.map