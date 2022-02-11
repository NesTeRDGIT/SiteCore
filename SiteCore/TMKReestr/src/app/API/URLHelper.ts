import { Location } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable()
export class  URLHelper {
    constructor(private location: Location) {

    }

    changeParameter(parameterName: string, parameterValue:any) {
        const urlAr = this.location.path().split('?');
        const url = urlAr[0];
        this.location.replaceState(`${url}/?${parameterName}=${parameterValue}`);
    }

    getParameter = (parameterName: string): string => {
        var url = new URL(document.URL);
        var urlsp = url.searchParams;
        return encodeURI(urlsp.get(parameterName));
    }
}

