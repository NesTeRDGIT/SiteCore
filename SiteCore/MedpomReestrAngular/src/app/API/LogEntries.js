export class LogEntries {
    constructor(obj) {
        this.TimeGenerated = null;
        this.Message = "";
        this.Type = TypeEntries.message;
        if (obj != null) {
            if (obj.TimeGenerated != null)
                this.TimeGenerated = new Date(obj.TimeGenerated);
            this.Message = obj.Message;
            this.Type = obj.Type;
        }
    }
}
export var TypeEntries;
(function (TypeEntries) {
    TypeEntries[TypeEntries["message"] = 0] = "message";
    TypeEntries[TypeEntries["error"] = 1] = "error";
    TypeEntries[TypeEntries["warning"] = 2] = "warning";
})(TypeEntries || (TypeEntries = {}));
//# sourceMappingURL=LogEntries.js.map