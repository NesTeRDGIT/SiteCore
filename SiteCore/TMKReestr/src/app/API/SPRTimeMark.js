import { Dictionary } from "./IKeyCollection";
export class SPRTimeMark {
    constructor(_TimeUpdate, _KeyField) {
        this.SPR = new Dictionary();
        this.DateUpdate = null;
        this.TimeUpdate = 15000;
        this.UpdateSPR = (items, emptyItem = null) => {
            var _a;
            this.DateUpdate = new Date();
            this.SPR.clear();
            if (emptyItem != null) {
                let key = (_a = emptyItem[this.KeyField]) === null || _a === void 0 ? void 0 : _a.toString();
                if (key == undefined || key == null)
                    key = "";
                this.SPR.add(key, emptyItem);
            }
            items.forEach(item => {
                var _a;
                let key = (_a = item[this.KeyField]) === null || _a === void 0 ? void 0 : _a.toString();
                if (key == undefined || key == null)
                    key = "";
                this.SPR.add(key, item);
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
//# sourceMappingURL=SPRTimeMark.js.map