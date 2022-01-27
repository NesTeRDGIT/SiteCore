import { Component, Output, Input, EventEmitter, OnChanges, SimpleChanges } from "@angular/core";
import { BaseReportComponent } from '../../Component/BaseReportComponent'
import { SectionSpr, ErrorSpr, EditErrorSPRViewModel } from '../../API/ErrorSPRModel'

import * as ClassicEditor from '../../ClassicEditor/ckeditor.js';



import { IRepository } from "../../API/Repository";

@Component({ selector: "ERROR_EDIT", templateUrl: "ERROR_EDIT.html", styleUrls: ["ERROR_EDIT.css"] })
export class ERROR_EDIT {
    model: ErrorSpr = new ErrorSpr(null);
    public Editor = ClassicEditor;
    display: boolean = false;
    isLoad = false;
    
    @Output() onSave: EventEmitter<boolean> = new EventEmitter();
    ID_ERR: number | null;
    ReadOnly: boolean = true;


    public async ShowDialog(ID_ERR: number, ReadOnly: boolean) {
        try {
            this.isLoad = true;
            this.ID_ERR = ID_ERR;
            this.ReadOnly = ReadOnly;
            this.display = true;
            await this.FillSpr();
            await this.getModel();
        }
        catch (err) {
            alert(err.toString())
        }
        finally {
            this.isLoad = false;
        }

    }


    public onReady(editor) {
        const toolbarElement = editor.ui.view.toolbar.element;
        editor.on('change:isReadOnly', (evt, propertyName, isReadOnly) => {
            if (isReadOnly) {
                toolbarElement.style.display = 'none';
            } else {
                toolbarElement.style.display = 'flex';
            }
        });
        if (editor.isReadOnly) {
            toolbarElement.style.display = 'none';
        }
    }

    SectionSpr: SectionSpr[] = [];
    SectionSprFiltered: SectionSpr[] = [];
    SelectedSection: SectionSpr = null;


    filterSectionSpr(event) {
        const filtered: any[] = [];
        const query = event.query;

        for (let i = 0; i < this.SectionSpr.length; i++) {
            const item = this.SectionSpr[i];
            if (item.SECTION_NAME.toLowerCase().indexOf(query.toLowerCase()) === 0) {
                filtered.push(item);
            }
        }
        this.SectionSprFiltered = filtered;
    }
   


    constructor(public repo: IRepository) {

    }

    getModel = async () => {
        try {
            if (this.ID_ERR === null) {
                this.model = new ErrorSpr(null);
                this.model.D_BEGIN = new Date();
                return;
            }
            this.model = await this.repo.getError(this.ID_ERR);         
            this.SelectedSection = this.SectionSpr.find(x => x.ID_SECTION === this.model.ID_SECTION);
        } catch (err) {
            alert(err.toString());
        }
    }

    FillSpr = async () => {
        try {
            this.SectionSpr = await this.repo.getSections();
        } catch (err) {
            alert(err.toString());
        }
    }

    errorList: string[] = [];
    SaveError = async () => {
        try {
            this.isLoad = true;
            if (this.SelectedSection != null) {
                this.model.ID_SECTION = this.SelectedSection.ID_SECTION;
            }
            let result: string[];
           
            if (this.ID_ERR === null) {
                result = await this.repo.AddErrorSPR(this.model);
            } else {
                result = await this.repo.EditErrorSPR(this.model);
            }
            this.errorList = result;
            if (result.length === 0) {
                this.display = false;
                this.onSave.emit(true);
            }
        } catch (err) {
            alert(err.toString());
        } finally {
            this.isLoad = false;
        }
    }
}



