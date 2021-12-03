import { Component, Output, Input,EventEmitter, OnChanges, SimpleChanges,ViewChild} from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { ERROR_EDIT } from '../../Component/ERROR_EDIT/ERROR_EDIT'
import { SectionSpr,ErrorSpr, EditErrorSPRViewModel } from '../../API/ErrorSPRModel'


import { IRepository } from "../../API/Repository";

@Component({ selector: "ERROR_SPR", templateUrl: "ERROR_SPR.html" })
export class ERROR_SPR extends BaseReportComponent {
    @Input()
    AdminMode: boolean = false;
    model: EditErrorSPRViewModel = new EditErrorSPRViewModel(null);
    filter: string;
    FIND_LIST: ErrorSpr[]= null;

   


    findError = (filter: string): ErrorSpr[] => {
        if (filter === null || filter === "")
            return null;
        const result: ErrorSpr[] = [];

        
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
    }

    FIND = () => {
        this.FIND_LIST = this.findError(this.filter);
    }


    constructor(public repo: IRepository) {
        super();
        
        this.getModel();
    }

    getModel = async () => {
        try {
            this.isLoad = true;
            this.model = await this.repo.getErrorSPR();
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }



  
    @ViewChild(ERROR_EDIT) errorEditDialog: ERROR_EDIT;
    EditError = (err: ErrorSpr) => {
        try {
            if (err != null) {
                this.errorEditDialog.ShowDialog(err.ID_ERR, false);
            }
        } catch (err) {
            alert(err.toString());
        } 
    }

    AddError = () => {
        try {
            this.errorEditDialog.ShowDialog(null, false);
        } catch (err) {
            alert(err.toString());
        }
    }

    ShowError = (err: ErrorSpr) => {
        try {
            if (err != null) {
                this.errorEditDialog.ShowDialog(err.ID_ERR, true);
            }
        } catch (err) {
            alert(err.toString());
        }
    }


    DeleteError = async (err: ErrorSpr) => {
        try {

            if (err != null) {
                if (confirm("Вы уверены, что хотите удалить ошибку?")) {
                    await this.repo.RemoveErrorSPR(err.ID_ERR);
                    this.getModel();
                }
            }
        } catch (err) {
            alert(err.toString());
        }
    }

    RefreshModel = () => {
        this.getModel();
    }
}



