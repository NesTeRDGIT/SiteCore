var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FindPacientSelected } from "../../API/FindPacientModel";
let FindPacientComponent = class FindPacientComponent {
    constructor(repo) {
        this.repo = repo;
        this.onChangeSelected = new EventEmitter();
        this.isResult = null;
        this.ShowElement = false;
        this._isPred = false;
        this._isActive = false;
        this.Pacients = [];
        this.CurrPacient = null;
        this.onLoading = false;
        this._ActualDt = null;
        this.FindPacient = async (ENP) => {
            try {
                this.ShowElement = true;
                this.onLoading = true;
                this.Clear();
                this.Pacients = await this.repo.FindPacientAsync(ENP);
                this.isResult = this.Pacients.length !== 0;
                if (this.isResult) {
                    this.isActive = true;
                    this.FindCurrPacient();
                }
                else {
                    this.isActive = false;
                    this.RaiseSelectChange();
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onLoading = false;
            }
        };
    }
    get isPred() {
        return this._isPred;
    }
    set isPred(val) {
        this._isPred = val;
        this.RaiseSelectChange();
    }
    get isActive() {
        return this._isActive;
    }
    set isActive(val) {
        this._isActive = val;
        this.RaiseSelectChange();
    }
    get ActualDt() {
        return this._ActualDt;
    }
    set ActualDt(value) {
        if (value != null) {
            this._ActualDt = new Date(value.getFullYear(), value.getMonth(), value.getDate());
        }
        else {
            this._ActualDt = null;
        }
        this.FindCurrPacient();
    }
    Clear(isHide = false) {
        this.isResult = null;
        this.CurrPacient = null;
        this._isPred = false;
        this._isActive = false;
        if (isHide) {
            this.ShowElement = false;
        }
        this.RaiseSelectChange();
    }
    FindCurrPacient() {
        this.CurrPacient = null;
        this.Pacients.forEach(p => {
            if (p.DBEG <= this.ActualDt && (p.DSTOP >= this.ActualDt || p.DSTOP == null) || p.DSTOP == null && this.ActualDt == null) {
                this.CurrPacient = p;
            }
        });
        this.RaiseSelectChange();
    }
    RaiseSelectChange() {
        var _a;
        (_a = this.onChangeSelected) === null || _a === void 0 ? void 0 : _a.emit(new FindPacientSelected(this.isPred, this.isActive ? this.CurrPacient : null));
    }
};
__decorate([
    Input()
], FindPacientComponent.prototype, "userInfo", void 0);
__decorate([
    Input()
], FindPacientComponent.prototype, "SPR", void 0);
__decorate([
    Output()
], FindPacientComponent.prototype, "onChangeSelected", void 0);
__decorate([
    Input()
], FindPacientComponent.prototype, "ActualDt", null);
FindPacientComponent = __decorate([
    Component({ selector: "FindPacient", templateUrl: "FindPacient.html", styleUrls: ['FindPacient.scss'] })
], FindPacientComponent);
export { FindPacientComponent };
//# sourceMappingURL=FindPacient.js.map