import { Injectable, OnDestroy } from '@angular/core';
import { HttpTransportType, HubConnectionBuilder } from '@microsoft/signalr';
import { HubConnection } from '@microsoft/signalr/dist/esm/HubConnection';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ServerEventsService implements OnDestroy {
  private connection: HubConnection;

  public userLoggedIn$: Observable<string>;

  constructor() {
    this.connection = new HubConnectionBuilder()
      .withUrl('/hub/notification', {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
      })
      .build();
    this.userLoggedIn$ = this.signalRObservable<string>('userLoggedIn');

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

  private signalRObservable<T>(methodName: string): Observable<T> {
    const subject = new Subject<T>();
    const observable = subject.asObservable();
    this.connection.on(methodName, (data: T) => subject.next(data));
    return observable;
  }
}
