var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Input } from "@angular/core";
//import { IRepositoryCS} from '../../../../API/IRepositoryCS';
import { PersonView } from 'src/app/API/CSModel';
let CSItemView = class CSItemView {
    constructor(repo) {
        this.repo = repo;
        this._display = false;
        this.model = new PersonView(null);
        this.isLoad = false;
    }
    get display() {
        return this._display;
    }
    set display(value) {
        this._display = value;
        if (!value) {
            this.ClearModel();
        }
    }
    async ShowDialog(CS_LIST_IN_ID) {
        try {
            this.isLoad = true;
            this.display = true;
            this.model = await this.repo.GetPersonView(CS_LIST_IN_ID);
        }
        catch (err) {
            alert(err.toString());
        }
        finally {
            this.isLoad = false;
        }
    }
    ClearModel() {
        this.model = new PersonView(null);
    }
};
__decorate([
    Input()
], CSItemView.prototype, "display", null);
CSItemView = __decorate([
    Component({ selector: "CSItemView", templateUrl: "CSItemView.html" })
], CSItemView);
export { CSItemView };
//# sourceMappingURL=CSItemView.js.map