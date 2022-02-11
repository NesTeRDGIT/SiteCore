var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Injectable } from '@angular/core';
let URLHelper = class URLHelper {
    constructor(location) {
        this.location = location;
        this.getParameter = (parameterName) => {
            var url = new URL(document.URL);
            var urlsp = url.searchParams;
            return encodeURI(urlsp.get(parameterName));
        };
    }
    changeParameter(parameterName, parameterValue) {
        const urlAr = this.location.path().split('?');
        const url = urlAr[0];
        this.location.replaceState(`${url}/?${parameterName}=${parameterValue}`);
    }
};
URLHelper = __decorate([
    Injectable()
], URLHelper);
export { URLHelper };
//# sourceMappingURL=URLHelper.js.map