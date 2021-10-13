export class DataBaseStateRow {
    POK: string;
    VALUE: string;
    constructor(obj: any) {
        if (obj != null) {
            this.POK = obj.POK;
            this.VALUE = obj.VALUE;
        }
    }


}