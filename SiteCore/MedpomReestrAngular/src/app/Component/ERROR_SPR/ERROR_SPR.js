var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input, ViewChild } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent';
import { ERROR_EDIT } from '../../Component/ERROR_EDIT/ERROR_EDIT';
import { EditErrorSPRViewModel } from '../../API/ErrorSPRModel';
let ERROR_SPR = class ERROR_SPR extends BaseReportComponent {
    constructor(repo) {
        super();
        this.repo = repo;
        this.AdminMode = false;
        this.model = new EditErrorSPRViewModel(null);
        this.FIND_LIST = null;
        this.findError = (filter) => {
            if (filter === null || filter === "")
                return null;
            const result = [];
            for (let sec of this.model.Sections) {
                for (let err of sec.Errors) {
                    if (err.OSN_TFOMS === filter || err.EXAMPLE.toUpperCase().indexOf(filter.toUpperCase()) !== -1) {
                        result.push(err);
                        if (result.length === 10)
                            break;
                    }
                }
                if (result.length === 10)
                    break;
            }
            return result;
        };
        this.FIND = () => {
            this.FIND_LIST = this.findError(this.filter);
        };
        this.getModel = async () => {
            try {
                this.isLoad = true;
                this.model = await this.repo.getErrorSPR();
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.isLoad = false;
            }
        };
        this.EditError = (err) => {
            try {
                if (err != null) {
                    this.errorEditDialog.ShowDialog(err.ID_ERR, false);
                }
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.AddError = () => {
            try {
                this.errorEditDialog.ShowDialog(null, false);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.ShowError = (err) => {
            try {
                if (err != null) {
                    this.errorEditDialog.ShowDialog(err.ID_ERR, true);
                }
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.DeleteError = async (err) => {
            try {
                if (err != null) {
                    if (confirm("Вы уверены, что хотите удалить ошибку?")) {
                        await this.repo.RemoveErrorSPR(err.ID_ERR);
                        this.getModel();
                    }
                }
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.RefreshModel = () => {
            this.getModel();
        };
        this.getModel();
    }
};
__decorate([
    Input()
], ERROR_SPR.prototype, "AdminMode", void 0);
__decorate([
    ViewChild(ERROR_EDIT)
], ERROR_SPR.prototype, "errorEditDialog", void 0);
ERROR_SPR = __decorate([
    Component({ selector: "ERROR_SPR", templateUrl: "ERROR_SPR.html" })
], ERROR_SPR);
export { ERROR_SPR };
//# sourceMappingURL=ERROR_SPR.js.map