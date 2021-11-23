var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Pipe } from "@angular/core";
import { StatusFilePack, ISTEnum, StepsProcess } from "../../API/LoadReestViewModel";
let StatusFilePackRusPipe = class StatusFilePackRusPipe {
    transform(value) {
        switch (value) {
            case StatusFilePack.FLKERR:
                return "Ошибка при проверке";
            case StatusFilePack.FLKOK:
                return "Завершено";
            default:
                return "В работе";
        }
    }
};
StatusFilePackRusPipe = __decorate([
    Pipe({ name: "StatusFilePackRus" })
], StatusFilePackRusPipe);
export { StatusFilePackRusPipe };
let ISTEnumRusPipe = class ISTEnumRusPipe {
    transform(value) {
        switch (value) {
            case ISTEnum.MAIL:
                return "Почта(VipNet)";
            case ISTEnum.SITE:
                return "Сайт";
            default:
                return ISTEnum[value];
        }
    }
};
ISTEnumRusPipe = __decorate([
    Pipe({ name: "ISTEnumRus" })
], ISTEnumRusPipe);
export { ISTEnumRusPipe };
let StepsProcessRusPipe = class StepsProcessRusPipe {
    transform(value) {
        switch (value) {
            case StepsProcess.NotInvite:
                return "Первичная проверка не пройдена";
            case StepsProcess.Invite:
                return "Первичная проверка пройдена";
            case StepsProcess.XMLxsd:
                return "Схема верна";
            case StepsProcess.ErrorXMLxsd:
                return "Ошибка схемы документа";
            case StepsProcess.FlkErr:
                return "Ошибка при загрузке файла";
            case StepsProcess.FlkOk:
                return "Проверка завершена";
            default:
                return ISTEnum[value];
        }
    }
};
StepsProcessRusPipe = __decorate([
    Pipe({ name: "StepsProcessRus" })
], StepsProcessRusPipe);
export { StepsProcessRusPipe };
//# sourceMappingURL=StatusFilePackPipe.js.map