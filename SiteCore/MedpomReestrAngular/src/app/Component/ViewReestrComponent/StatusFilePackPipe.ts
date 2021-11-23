import { Pipe, PipeTransform } from "@angular/core";
import { StatusFilePack, ISTEnum, StepsProcess } from "../../API/LoadReestViewModel";

@Pipe({ name: "StatusFilePackRus" })
export class StatusFilePackRusPipe implements PipeTransform {
    transform(value: StatusFilePack): any {
        switch (value) {
            case StatusFilePack.FLKERR:
                return "Ошибка при проверке";
            case StatusFilePack.FLKOK:
                return "Завершено";
            default:
                return "В работе";
        }
    }
}


@Pipe({ name: "ISTEnumRus" })
export class ISTEnumRusPipe implements PipeTransform {
    transform(value: ISTEnum): any {
        switch (value) {
            case ISTEnum.MAIL:
                return "Почта(VipNet)";
            case ISTEnum.SITE:
                return "Сайт";
        default:
            return ISTEnum[value];
        }
    }
}

@Pipe({ name: "StepsProcessRus" })
export class StepsProcessRusPipe implements PipeTransform {
    transform(value: StepsProcess): any {
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
}