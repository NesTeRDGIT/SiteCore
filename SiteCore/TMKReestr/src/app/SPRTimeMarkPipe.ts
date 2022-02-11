import { Pipe, PipeTransform } from "@angular/core";
import { SPRTimeMark } from "./API/SPRTimeMark";

@Pipe({
    name: "SPRTimeMark"
})
export class SPRTimeMarkPipe<T> implements PipeTransform {
    transform(value: string, SprTM: SPRTimeMark<T>, NameField: string,NotVisibleNull:boolean=false, ...args: any[]): any {
        var key = value;
       
        if (value == null || value == undefined) {
            key = "";
        }
        if (SprTM.SPR.containsKey(key)) {
            return SprTM.SPR.getItem(key)[NameField];
        }
        if (NotVisibleNull && (value == null || value == "" || value==undefined)) {
            return "";
        }
        return `Не удалось найти значение ${value}`;
    }
}