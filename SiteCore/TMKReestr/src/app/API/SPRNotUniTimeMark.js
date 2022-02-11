import { Dictionary } from "./IKeyCollection";
export class SPRNotUniTimeMark {
    constructor(_TimeUpdate, _KeyField) {
        this.SPR = new Dictionary();
        this.DateUpdate = null;
        this.TimeUpdate = 15000;
        this.add = (item) => {
            var _a;
            let key = (_a = item[this.KeyField]) === null || _a === void 0 ? void 0 : _a.toString();
            if (key == undefined || key == null)
                key = "";
            if (this.SPR.containsKey(key)) {
                var val = this.SPR.getItem(key);
                val.push(item);
            }
            else {
                this.SPR.add(key, [item]);
            }
        };
        this.values = () => {
            let items = this.SPR.values();
            let result = [];
            items.forEach(item => {
                result.push(...item);
            });
            return result;
        };
        this.UpdateSPR = (items, empty = null) => {
            this.DateUpdate = new Date();
            this.SPR.clear();
            if (empty != null) {
                this.add(empty);
            }
            items.forEach(item => {
                this.add(item);
            });
        };
        this.TimeUpdate = _TimeUpdate * 60 * 1000;
        this.KeyField = _KeyField;
    }
    get isNeedUpdate() {
        if (this.DateUpdate == null)
            return true;
        let now = new Date();
        return now.getTime() - this.DateUpdate.getTime() > this.TimeUpdate;
    }
}
//# sourceMappingURL=SPRNotUniTimeMark.js.map