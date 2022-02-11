export interface IKeyCollection<T> {
    add(key: string, value: T);
    containsKey(key: string): boolean;
    size(): number;
    getItem(key: string): T;
    removeItem(key: string): T;
    getKeys(): string[];
    values(): T[];
    clear(): void;
  }

  class ItemOrder<T>
  {
    constructor(_item: T, _ord: number) {
      this.item = _item;
      this.ord = _ord;
    }
    item:T;
    ord:number
  }
  export class Dictionary<T> implements IKeyCollection<T> {
    clear(): void {
        this.items = {};
        this.count = 0;
    }
    private items: { [index: string]: ItemOrder<T> } = {};
    private count: number = 0;
    private ord:number =0;
    add(key: string, value: T) {
      if (!this.items.hasOwnProperty(key)) {
        this.count++;
      }
      this.items[key.toString()] = new ItemOrder(value,this.ord);
      this.ord++;
    }
  
    containsKey(key: string): boolean {
      return this.items.hasOwnProperty(key);
    }
  
    size(): number {
      return this.count;
    }
  
    getItem(key: string): T {
      return this.items[key].item;
    }
  
    removeItem(key: string): T {
      let value = this.items[key];  
      delete this.items[key];
      this.count--;  
      return value.item;
    }
  
    getKeys(): string[] {
      let keySet: string[] = [];  
      for (let property in this.items) {
        if (this.items.hasOwnProperty(property)) {
          keySet.push(property);
        }
      }  
      return keySet;
    }
  
    values(): T[] {
     
      let values: T[] = [];
      for (let property in this.items) {     
        if (this.items.hasOwnProperty(property)) {
          let item = this.items[property];         
          values[item.ord] = item.item;
        }
      }
      return values;
    }
  }