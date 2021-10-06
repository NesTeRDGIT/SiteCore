

export default class HUBConnect {
    constructor() {
        this.connection = null;
    }


    async Connect() {
        this.connection = new signalR.HubConnectionBuilder().withUrl("../Notification").build();
        await this.connection.start();
        await this.connection.invoke('RegisterForLoadThemeFile');
       return this.connection.connection.connectionId;
    }

    async Disconnect() {
        if (this.connection != null) {

        }
    }

   


    onProgress = (callback) => {

        this.connection.on("Progress", (data) => {
             callback(data);
        });
    };
}



