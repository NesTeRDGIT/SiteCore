import * as signalR from "@microsoft/signalr";
export class HubConnect {
    async Connect() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("../../Notification").build();
        await this.connection.start();
        return this.connection.connectionId;
    }
    async Disconnect() {
        if (this.connection != null) {
            this.connection.off("NewCSListState");
            this.connection.stop();
        }
    }
    async RegisterNewCSListState(callback) {
        await this.connection.invoke("RegisterNewCSListState");
        this.connection.on("NewCSListState", (data) => {
            callback(data);
        });
    }
    async RegisterNewPackState(callback) {
        await this.connection.invoke("RegisterNewPackState");
        this.connection.on("NewPackState", (data) => {
            callback(data);
        });
    }
}
//# sourceMappingURL=HUBConnect.js.map