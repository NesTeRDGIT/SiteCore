import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: "ENUMasString"
})
export class ENUMPipe implements PipeTransform {
    transform(value: number, enumType: any): any {
        return enumType[value];
    }
}