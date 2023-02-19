import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationHubService } from 'src/services/notification-hub.service';
import { User } from 'src/shared/models/User';
import { AuthenticationService } from './authentication/authentication.service';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.css'],
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(
    private authenticationService: AuthenticationService,
    private notificationHubService: NotificationHubService
  ) {}
  connected = false;
  username: string | undefined;

  ngOnInit() {
    this.authenticationService.user$.subscribe((user) => this.setUser(user));
    this.notificationHubService.startListening();
  }

  ngOnDestroy() {
    this.notificationHubService.stopListening();
  }

  setUser(user: User | null) {
    this.connected = user != null;
    this.username = user?.name;
  }
}
