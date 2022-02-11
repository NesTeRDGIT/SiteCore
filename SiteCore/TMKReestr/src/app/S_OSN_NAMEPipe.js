var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from "@angular/core";
let S_OSN_NAMEPipe = class S_OSN_NAMEPipe {
    transform(type, ...args) {
        switch (type) {
            case true: return "Обоснованно";
            case false: return "Необоснованно";
            default: return "";
        }
    }
};
S_OSN_NAMEPipe = __decorate([
    Pipe({ name: "S_OSN_NAME" })
], S_OSN_NAMEPipe);
export { S_OSN_NAMEPipe };
//# sourceMappingURL=S_OSN_NAMEPipe.js.map