import * as signalR from "@microsoft/signalr";
export class HubConnect {
    constructor() {
        this.NewPackState = (callback) => {
            this.connection.on("NewPackState", (data) => {
                callback(data);
            });
        };
    }
    async Connect() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("../../Notification").build();
        await this.connection.start();
        await this.connection.invoke('Register');
        return this.connection.connectionId;
    }
    async Disconnect() {
        if (this.connection != null) {
            this.connection.off("NewPackState");
            this.connection.stop();
        }
    }
}
//# sourceMappingURL=HUBConnect.js.map