import { Pipe, PipeTransform } from "@angular/core";
import { ExpType } from "./API/TMKItem";

@Pipe({ name: "S_TIP_NAME" })
export class S_TIP_NAMEPipe implements PipeTransform {
    transform(type: ExpType, ...args: any[]): string {
       
        switch (type) {
            case ExpType.MEK: return "МЭК";
            case ExpType.MEE: return "МЭЭ";
            case ExpType.EKMP: return "ЭКМП";
            default: return "";
        }
    }
}