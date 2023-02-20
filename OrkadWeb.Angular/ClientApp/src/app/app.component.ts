import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { ServerEventsService } from 'src/services/ServerEventsService';
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
    private serverEventsService: ServerEventsService
  ) {}
  connected = false;
  username: string | undefined;

  ngOnInit() {
    this.authenticationService.user$.subscribe((user) => this.setUser(user));
    this.serverEventsService.userLoggedIn$.subscribe((str) =>
      console.log('user ' + str + ' connected')
    );
  }

  setUser(user: User | null) {
    this.connected = user != null;
    this.username = user?.name;
  }
}
