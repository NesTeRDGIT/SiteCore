var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from "@angular/core";
import { ExpType } from "./API/TMKItem";
let S_TIP_NAMEPipe = class S_TIP_NAMEPipe {
    transform(type, ...args) {
        switch (type) {
            case ExpType.MEK: return "МЭК";
            case ExpType.MEE: return "МЭЭ";
            case ExpType.EKMP: return "ЭКМП";
            default: return "";
        }
    }
};
S_TIP_NAMEPipe = __decorate([
    Pipe({ name: "S_TIP_NAME" })
], S_TIP_NAMEPipe);
export { S_TIP_NAMEPipe };
//# sourceMappingURL=S_TIP_NAMEPipe.js.map