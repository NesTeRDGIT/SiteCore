import { Pipe, PipeTransform } from "@angular/core";
import { SPRNotUniTimeMark, INotUniSPR } from "./API/SPRNotUniTimeMark";
import { SPRTimeMark } from "./API/SPRTimeMark";

@Pipe({name: "ActualSPR"})
export class ActualSPRPipe<T extends INotUniSPR> implements PipeTransform {
    transform(SprTM: SPRNotUniTimeMark<T>, dt: Date, ...args: any[]): SPRTimeMark<T> {       
        let items = SprTM.values();
      
        let result: T[] = [];
        items.forEach(x => {
            if (x.DATE_B <= dt && ((x.DATE_E ?? dt) >= dt) || x.DATE_B ==null) {
                result.push(x);
            }
        });    
        var spr = new SPRTimeMark<T>(15, SprTM.KeyField);
        spr.UpdateSPR(result);
        return spr;
    }
}