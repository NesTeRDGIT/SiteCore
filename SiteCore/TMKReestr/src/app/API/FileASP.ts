export class FileASP {
    FileContents: string;
    ContentType: string;
    FileDownloadName:string;
    constructor(obj: any) {
        if (obj != null) {
            this.FileContents = obj.FileContents;
            this.ContentType = obj.ContentType;
            this.FileDownloadName = obj.FileDownloadName;
        }
    }
}