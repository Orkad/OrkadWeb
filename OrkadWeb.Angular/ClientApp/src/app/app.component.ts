import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { User } from 'src/shared/models/User';
import { AuthenticationService } from './authentication/authentication.service';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.css'],
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(private authenticationService: AuthenticationService) {}
  connected = false;
  username: string | undefined;

  ngOnInit() {
    this.setUser(this.authenticationService.user);
    this.authenticationService.user$.subscribe((user) => this.setUser(user));
  }

  ngOnDestroy() {}

  setUser(user: User | null) {
    this.connected = user != null;
    this.username = user?.name;
  }
}
