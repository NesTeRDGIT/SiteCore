import { Dictionary } from "./IKeyCollection";

export class SPRTimeMark<T>
{
    constructor(_TimeUpdate: number, _KeyField: string) {
        this.TimeUpdate = _TimeUpdate * 60 * 1000;
        this.KeyField = _KeyField;
    }
    KeyField: string;
    SPR: Dictionary<T> = new Dictionary<T>();
    DateUpdate: Date = null;
    TimeUpdate: number = 15000;
    get isNeedUpdate(): boolean {
        if (this.DateUpdate == null)
            return true;
        let now = new Date();
        return now.getTime() - this.DateUpdate.getTime() > this.TimeUpdate;
    }

    UpdateSPR = (items: T[], emptyItem: T = null) => {
        this.DateUpdate = new Date();
        this.SPR.clear();
        if (emptyItem != null) {
            let key = emptyItem[this.KeyField]?.toString();
            if (key == undefined || key == null)
                key = "";
            this.SPR.add(key, emptyItem);
        }
        items.forEach(item => {
            let key = item[this.KeyField]?.toString();
            if (key == undefined || key == null)
                key = "";
            this.SPR.add(key, item);
        });
    }
}
