import { Component } from '@angular/core';
import { AuthenticationService } from './authentication/authentication.service';

@Component({
  selector: 'app-root',
  styleUrls: ['./app.component.css'],
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(private authenticationService: AuthenticationService) {}
  connected = false;
  username = '';

  ngOnInit(): void {
    this.authenticationService.user.subscribe((u) => {
      this.connected = u != null;
      this.username = u?.name ?? 'anonymous';
    });
  }
}
