import { Component, Output, Input, EventEmitter } from "@angular/core";

//import { IRepositoryCS} from '../../../../API/IRepositoryCS';
import { PersonView } from 'src/app/API/CSModel';
import { IRepositoryCS, ResultOperation } from 'src/app/API/IRepositoryCS';
@Component({ selector: "CSItemView", templateUrl: "CSItemView.html" })
export class CSItemView {
    _display = false;
    @Input()
    get display(): any {
        return this._display;
    }
    set display(value: any) {
        this._display = value;
        if (!value) {
            this.ClearModel();
        }
    }

    model = new PersonView(null);

    isLoad: boolean = false;
    constructor(private repo: IRepositoryCS) {

    }

    public async ShowDialog(CS_LIST_IN_ID:number)
    {
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

    private ClearModel() {
        this.model = new PersonView(null);
    }

}
