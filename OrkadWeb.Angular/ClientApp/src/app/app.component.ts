import { Component } from "@angular/core";
import { AuthenticationService } from "./authentication/authentication.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
})
export class AppComponent {
  constructor(private authenticationService: AuthenticationService) {}
  connected = false;
  username: string = null;

  ngOnInit(): void {
    this.authenticationService.user.subscribe((u) => {
      this.connected = !!u;
      if (this.connected) {
        this.username = u.name;
      } else {
        this.username = null;
      }
    });
  }
}
