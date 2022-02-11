class ItemOrder {
    constructor(_item, _ord) {
        this.item = _item;
        this.ord = _ord;
    }
}
export class Dictionary {
    constructor() {
        this.items = {};
        this.count = 0;
        this.ord = 0;
    }
    clear() {
        this.items = {};
        this.count = 0;
    }
    add(key, value) {
        if (!this.items.hasOwnProperty(key)) {
            this.count++;
        }
        this.items[key.toString()] = new ItemOrder(value, this.ord);
        this.ord++;
    }
    containsKey(key) {
        return this.items.hasOwnProperty(key);
    }
    size() {
        return this.count;
    }
    getItem(key) {
        return this.items[key].item;
    }
    removeItem(key) {
        let value = this.items[key];
        delete this.items[key];
        this.count--;
        return value.item;
    }
    getKeys() {
        let keySet = [];
        for (let property in this.items) {
            if (this.items.hasOwnProperty(property)) {
                keySet.push(property);
            }
        }
        return keySet;
    }
    values() {
        let values = [];
        for (let property in this.items) {
            if (this.items.hasOwnProperty(property)) {
                let item = this.items[property];
                values[item.ord] = item.item;
            }
        }
        return values;
    }
}
//# sourceMappingURL=IKeyCollection.js.map