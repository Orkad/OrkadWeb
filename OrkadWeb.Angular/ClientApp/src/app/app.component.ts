import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ServerEventsService } from 'src/services/ServerEventsService';
import { User } from 'src/shared/models/User';
import { AuthenticationService } from './authentication/authentication.service';
import { HealthService } from 'src/services/health.service';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.css'],
  templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
  constructor(
    private authenticationService: AuthenticationService,
    private serverEventsService: ServerEventsService,
    private router: Router,
    private healthService: HealthService
  ) {}
  isApiOnline = false;
  connected = false;
  username: string | undefined;
  isAdmin: boolean;

  ngOnInit() {
    this.authenticationService.user$.subscribe((user) => this.setUser(user));
    this.serverEventsService.userLoggedIn$.subscribe((str) => console.log(str));
    this.healthService.healthStatus$.subscribe((ok) => (this.isApiOnline = ok));
  }

  setUser(user: User | null) {
    this.connected = user != null;
    this.username = user?.name;
    this.isAdmin = user?.role === 'Admin';
  }

  disconnect() {
    this.authenticationService.logout();
    this.router.navigate(['auth']);
  }
}
