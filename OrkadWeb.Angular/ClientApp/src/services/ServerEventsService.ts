import { Injectable, OnDestroy } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ServerEventsService implements OnDestroy {
  private connection: HubConnection;
  private userLoggedInSubject = new Subject<string>();

  public get userLoggedIn$() {
    return this.userLoggedInSubject.asObservable();
  }

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/hub/notification')
      .build();
    this.connection.on('userLoggedIn', (username) =>
      this.userLoggedInSubject.next(username)
    );

    this.connection
      .start()
      .then(() => console.log('ServerEventsService start listening'))
      .catch((err) => console.log('ServerEventsService cannot connect' + err));
  }
  ngOnDestroy(): void {
    this.connection
      .stop()
      .then(() => console.log('ServerEventsService stop listening'));
  }
}
