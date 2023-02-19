import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root',
})
export class NotificationHubService {
  hub: HubConnection;
  constructor(private notificationService: NotificationService) {
    this.hub = new HubConnectionBuilder().withUrl('/hub/notification').build();

    this.hub.on('userLoggedIn', (username) => {
      this.notificationService.success(
        "Un nouvel utilisateur s'est connecté: " + username
      );
    });
  }

  startListening() {
    this.hub
      .start()
      .then(() => console.log('start listening on /hub/notification'))
      .catch((err) => console.log('cannot listen on /hub/notification' + err));
  }

  stopListening() {
    this.hub
      .stop()
      .then(() => console.log('stop listening on /hub/notification'));
  }
}
