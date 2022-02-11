var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from "@angular/core";
import { SPRTimeMark } from "./API/SPRTimeMark";
let ActualSPRPipe = class ActualSPRPipe {
    transform(SprTM, dt, ...args) {
        let items = SprTM.values();
        let result = [];
        items.forEach(x => {
            var _a;
            if (x.DATE_B <= dt && ((_a = x.DATE_E) !== null && _a !== void 0 ? _a : dt >= dt) || x.DATE_B == null) {
                result.push(x);
            }
        });
        var spr = new SPRTimeMark(15, SprTM.KeyField);
        spr.UpdateSPR(result);
        return spr;
    }
};
ActualSPRPipe = __decorate([
    Pipe({
        name: "ActualSPR"
    })
], ActualSPRPipe);
export { ActualSPRPipe };
//# sourceMappingURL=ActualSPRPipe.js.map