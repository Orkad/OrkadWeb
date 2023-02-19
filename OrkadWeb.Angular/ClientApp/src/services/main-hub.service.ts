import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';

@Injectable({
  providedIn: 'root',
})
export class MainHubService {
  mainHub: HubConnection;
  constructor(private httpClient: HttpClient) {
    this.mainHub = new HubConnectionBuilder().withUrl('/hubs/main').build();

    this.mainHub.on('notify', (data) => {
      console.log(data);
    });

    this.mainHub
      .start()
      .then(() => console.log('connection started'))
      .catch((err) =>
        console.log('error while establishing signalr connection: ' + err)
      );
  }
}
