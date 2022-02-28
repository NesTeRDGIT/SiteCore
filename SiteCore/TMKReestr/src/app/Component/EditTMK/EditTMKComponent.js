var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, ViewChild, Input, Output, EventEmitter } from "@angular/core";
import { SPRModel } from "../../API/SPRModel";
import { TMKItem, StatusTMKRow, ExpType } from "../../API/TMKItem";
import { ExpertizeEditComponent } from "../../Component/ExpertizeEdit/ExpertizeEditComponent";
import { FindPacientComponent } from "../FindPacient/FindPacient";
import { FindExpertizeComponent } from "../FindExpertize/FindExpertize";
let EditTMKComponent = class EditTMKComponent {
    constructor(repo) {
        this.repo = repo;
        this.onChange = new EventEmitter();
        this.ExpType = ExpType;
        this.EditTMKType = EditTMKType;
        this.TypeOpen = EditTMKType.New;
        this.StatusTMKRow = StatusTMKRow;
        this.ErrMessage = [];
        this.CurrentTMK = new TMKItem(null);
        this.onLoading = true;
        this.SPR = new SPRModel(this.repo);
        this.Display = false;
        this.IsReadOnly = true;
        this.onOperationProgress = false;
        this.onSaveProgress = false;
        this.Save = async () => {
            try {
                this.onOperationProgress = this.onSaveProgress = true;
                let result;
                if (this.TypeOpen == EditTMKType.New) {
                    result = await this.repo.AddTMKItemAsync(this.CurrentTMK);
                }
                else {
                    result = await this.repo.EditTMKItemAsync(this.CurrentTMK);
                }
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
                this.onOperationProgress = this.onSaveProgress = false;
            }
        };
        this.onDeleteProgress = false;
        this.Delete = async () => {
            try {
                if (confirm("Вы уверены, что хотите удалить запись?")) {
                    this.onOperationProgress = this.onDeleteProgress = true;
                    let result = await this.repo.DeleteTMKItemAsync([this.CurrentTMK]);
                    if (result.Result) {
                        this.onChange.emit(null);
                        this.Close();
                    }
                    else {
                        this.ErrMessage = result.ErrMessage;
                    }
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onOperationProgress = this.onDeleteProgress = false;
            }
        };
        this.onSetAsMTRProgress = false;
        this.SetAsMTR = async () => {
            try {
                if (confirm("Вы уверены, что хотите отметить запись как МТР?")) {
                    this.onOperationProgress = this.onSetAsMTRProgress = true;
                    let result = await this.repo.SetAsMtrTMKItemAsync(this.CurrentTMK);
                    if (result.Result) {
                        this.CurrentTMK = await this.repo.getTMKItemAsync(this.CurrentTMK.TMK_ID);
                        this.onChange.emit(this.CurrentTMK.TMK_ID);
                    }
                    else {
                        this.ErrMessage = result.ErrMessage;
                    }
                }
            }
            catch (err) {
                alert(err.toString());
            }
            finally {
                this.onOperationProgress = this.onSetAsMTRProgress = false;
            }
        };
        this.AddExpertize = async (type, exp = null) => {
            try {
                this.ExpertizeEditWin.ShowCreateNewExpertize(type, this.CurrentTMK.TMK_ID, exp);
                this.onChange.emit(this.CurrentTMK.TMK_ID);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.EditExpertize = async (item) => {
            try {
                this.ExpertizeEditWin.EditCreateNewExpertize(item);
                this.onChange.emit(this.CurrentTMK.TMK_ID);
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.DelExpertize = async (item) => {
            try {
                if (confirm("Вы уверены, что хотите удалить экспертизу'?")) {
                    let result = await this.repo.DeleteExpertizeAsync([item]);
                    if (result) {
                        this.refreshModel();
                        this.onChange.emit(this.CurrentTMK.TMK_ID);
                    }
                    else {
                        alert(result.Error);
                    }
                }
            }
            catch (err) {
                alert(err.toString());
            }
        };
        this.IsTFOMS = () => {
            return this.CurrentTMK.SMO == "75";
        };
        this.refreshModel = async () => {
            this.CurrentTMK = await this.repo.getTMKItemAsync(this.CurrentTMK.TMK_ID);
        };
        this.FindPacient = async () => {
            this.FindPacientPanel.FindPacient(this.CurrentTMK.ENP);
        };
        this.FindExpertize = async () => {
            this.FindExpertizePanel.FindExpertize(this.CurrentTMK.TMK_ID);
        };
    }
    async FILL_SPR() {
        try {
            await this.SPR.refreshStaticSPR(false);
            await this.SPR.refreshVariableSPR(false);
        }
        catch (err) {
            alert(`Ошибка получение справочников: ${err.toString()}`);
        }
    }
    ConvertToDate(obj) {
        if (obj)
            return new Date(obj);
        return null;
    }
    async Show(TMK_ID) {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.Read;
            this.IsReadOnly = true;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }
    async Edit(TMK_ID) {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.Edit;
            this.IsReadOnly = false;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = await this.repo.getTMKItemAsync(TMK_ID);
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }
    async New() {
        try {
            this.onLoading = true;
            this.TypeOpen = EditTMKType.New;
            this.IsReadOnly = false;
            this.Display = true;
            await this.FILL_SPR();
            this.CurrentTMK = new TMKItem(null);
            this.CurrentTMK.CODE_MO = this.userInfo.CodeMO;
            this.CurrentTMK.DATE_EDIT = this.CurrentTMK.DATE_INVITE = new Date();
            this.CurrentTMK.STATUS = StatusTMKRow.Open;
        }
        catch (err) {
            alert(err.toString());
            this.Close();
        }
        finally {
            this.onLoading = false;
        }
    }
    Close() {
        var _a, _b;
        this.Display = false;
        this.CurrentTMK = new TMKItem(null);
        this.ErrMessage = [];
        (_a = this.FindPacientPanel) === null || _a === void 0 ? void 0 : _a.Clear(true);
        (_b = this.FindExpertizePanel) === null || _b === void 0 ? void 0 : _b.Clear(true);
    }
    FindPacient_onChangeSelected(item) {
        setTimeout(() => { this.CurrentTMK.SetAuto(item.Pacient, item.isPred); });
    }
    ENP_KeyDown(event) {
        if (event.key === "Enter" && !this.IsReadOnly) {
            event.preventDefault();
            this.FindPacient();
        }
    }
    AddFindExpertize(exp) {
        if (exp != null) {
            this.AddExpertize(exp.S_TIP, exp);
        }
    }
};
__decorate([
    Input()
], EditTMKComponent.prototype, "userInfo", void 0);
__decorate([
    Output()
], EditTMKComponent.prototype, "onChange", void 0);
__decorate([
    ViewChild(ExpertizeEditComponent)
], EditTMKComponent.prototype, "ExpertizeEditWin", void 0);
__decorate([
    ViewChild(FindPacientComponent)
], EditTMKComponent.prototype, "FindPacientPanel", void 0);
__decorate([
    ViewChild(FindExpertizeComponent)
], EditTMKComponent.prototype, "FindExpertizePanel", void 0);
EditTMKComponent = __decorate([
    Component({ selector: "EditTMK", templateUrl: "EditTMKComponent.html", styleUrls: ["EditTMKComponent.scss"] })
], EditTMKComponent);
export { EditTMKComponent };
var EditTMKType;
(function (EditTMKType) {
    EditTMKType[EditTMKType["New"] = 0] = "New";
    EditTMKType[EditTMKType["Edit"] = 1] = "Edit";
    EditTMKType[EditTMKType["Read"] = 2] = "Read";
})(EditTMKType || (EditTMKType = {}));
//# sourceMappingURL=EditTMKComponent.js.map