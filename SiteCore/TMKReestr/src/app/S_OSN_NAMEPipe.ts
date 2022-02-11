import { Pipe, PipeTransform } from "@angular/core";
import { ExpType } from "./API/TMKItem";

@Pipe({ name: "S_OSN_NAME" })
export class S_OSN_NAMEPipe implements PipeTransform {
    transform(type: boolean, ...args: any[]): string {
       
        switch (type) {
            case true: return "Обоснованно";
            case false: return "Необоснованно";
            default: return "";
        }
    }
}