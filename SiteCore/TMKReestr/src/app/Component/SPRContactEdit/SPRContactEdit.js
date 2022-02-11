var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input, Output, EventEmitter } from "@angular/core";
import { SPRContactModel } from "../../API/SPRContactModel";
let SPRContactEditComponent = class SPRContactEditComponent {
    constructor(repo) {
        this.repo = repo;
        this.onChange = new EventEmitter();
        this.onLoading = true;
        this.model = new SPRContactModel(null);
        this.Display = false;
        this.ErrMessage = [];
        this.Close = () => {
            this.Display = false;
            this.model = new SPRContactModel(null);
            this.ErrMessage = [];
        };
        this.onSaveProgress = false;
        this.Save = async () => {
            try {
                this.onSaveProgress = true;
                debugger;
                let result = await this.repo.EditSPRContactAsync(this.model);
                if (result.Result) {
                    this.onChange.emit(null);
                    this.Close();
                }
                else {
                    this.ErrMessage = result.ErrMessage;
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
    async FILL_SPR() {
        try {
            this.onLoading = true;
            await this.SPR.refreshCODE_MOAsync(false);
            this.onLoading = false;
        }
        catch (err) {
            alert(`Ошибка получение справочников: ${err.toString()}`);
        }
    }
    async Edit(item) {
        this.Display = true;
        await this.FILL_SPR();
        this.model = new SPRContactModel(item);
    }
    async New() {
        this.Display = true;
        await this.FILL_SPR();
        this.model = new SPRContactModel(null);
        this.model.CODE_MO = this.userInfo.CodeMO;
    }
};
__decorate([
    Input()
], SPRContactEditComponent.prototype, "userInfo", void 0);
__decorate([
    Output()
], SPRContactEditComponent.prototype, "onChange", void 0);
__decorate([
    Input()
], SPRContactEditComponent.prototype, "SPR", void 0);
SPRContactEditComponent = __decorate([
    Component({ selector: "SPRContactEdit", templateUrl: "SPRContactEdit.html" })
], SPRContactEditComponent);
export { SPRContactEditComponent };
var EditTMKType;
(function (EditTMKType) {
    EditTMKType[EditTMKType["New"] = 0] = "New";
    EditTMKType[EditTMKType["Edit"] = 1] = "Edit";
    EditTMKType[EditTMKType["Read"] = 2] = "Read";
})(EditTMKType || (EditTMKType = {}));
//# sourceMappingURL=SPRContactEdit.js.map