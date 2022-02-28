var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input, Output, EventEmitter } from "@angular/core";
let FindExpertizeComponent = class FindExpertizeComponent {
    constructor(repo) {
        this.repo = repo;
        this.AddExpertize = new EventEmitter();
        this.isResult = null;
        this.ShowElement = false;
        this.Expertizes = [];
        this.onLoading = false;
        this.FindExpertize = async (TMK_ID) => {
            try {
                this.ShowElement = true;
                this.onLoading = true;
                this.Clear();
                this.Expertizes = await this.repo.FindExpertizeAsync(TMK_ID);
                this.isResult = this.Expertizes.length !== 0;
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onLoading = false;
            }
        };
    }
    Clear(isHide = false) {
        this.isResult = null;
        if (isHide) {
            this.ShowElement = false;
        }
    }
    RaiseAddExpertize(exp) {
        var _a;
        (_a = this.AddExpertize) === null || _a === void 0 ? void 0 : _a.emit(exp);
    }
};
__decorate([
    Input()
], FindExpertizeComponent.prototype, "userInfo", void 0);
__decorate([
    Input()
], FindExpertizeComponent.prototype, "SPR", void 0);
__decorate([
    Output()
], FindExpertizeComponent.prototype, "AddExpertize", void 0);
FindExpertizeComponent = __decorate([
    Component({ selector: "FindExpertize", templateUrl: "FindExpertize.html" })
], FindExpertizeComponent);
export { FindExpertizeComponent };
//# sourceMappingURL=FindExpertize.js.map