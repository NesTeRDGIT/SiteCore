import { Dictionary } from "./IKeyCollection";

export interface INotUniSPR
{
    DATE_B: Date;
    DATE_E: Date;
}

export class SPRNotUniTimeMark<T extends INotUniSPR>
{
    constructor(_TimeUpdate: number, _KeyField: string) {
        this.TimeUpdate = _TimeUpdate * 60 * 1000;
        this.KeyField = _KeyField;
    }
    KeyField: string;
    SPR: Dictionary<T[]> = new Dictionary<T[]>();
    DateUpdate: Date = null;
    TimeUpdate: number = 15000;
    get isNeedUpdate(): boolean {
        if (this.DateUpdate == null)
            return true;
        let now = new Date();
        return now.getTime() - this.DateUpdate.getTime() > this.TimeUpdate;
    }

    add= (item:T)=>
    {
        let key = item[this.KeyField]?.toString();
        if(key==undefined || key==null)
            key = "";
        if (this.SPR.containsKey(key)) {
            var val = this.SPR.getItem(key);
            val.push(item);
        }
        else {
            this.SPR.add(key, [item]);
        }
    }

    values = ():T[] => {
        let items = this.SPR.values();
        let result = [];
        items.forEach(item => {
            result.push(...item);
        });
        return result;
    }

    UpdateSPR = (items: T[], empty:T = null) => {
        this.DateUpdate = new Date();
        this.SPR.clear();
        if (empty != null) {
            this.add(empty);
        }
        items.forEach(item => {
            this.add(item);
        });
    }
}