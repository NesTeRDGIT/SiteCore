var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from "@angular/core";
let SPRTimeMarkPipe = class SPRTimeMarkPipe {
    transform(value, SprTM, NameField, NotVisibleNull = false, ...args) {
        var key = value;
        if (value == null || value == undefined) {
            key = "";
        }
        if (SprTM.SPR.containsKey(key)) {
            return SprTM.SPR.getItem(key)[NameField];
        }
        if (NotVisibleNull && (value == null || value == "" || value == undefined)) {
            return "";
        }
        return `Не удалось найти значение ${value}`;
    }
};
SPRTimeMarkPipe = __decorate([
    Pipe({
        name: "SPRTimeMark"
    })
], SPRTimeMarkPipe);
export { SPRTimeMarkPipe };
//# sourceMappingURL=SPRTimeMarkPipe.js.map