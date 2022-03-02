import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AuthenticationService } from "./authentication.service";

@Component({
  selector: "app-authentication",
  templateUrl: "./authentication.component.html",
  styleUrls: ["./authentication.component.css"],
})
export class AuthenticationComponent implements OnInit {
  loading: boolean = false;
  loggedIn: boolean;
  loggedUsername: string;
  loginForm: FormGroup;
  error: string;

  constructor(
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: [""],
      password: [""],
    });
    this.refresh();
    this.authenticationService.user.subscribe((u) => {
      if (u == null) {
        this.loggedIn = false;
        this.loggedUsername = null;
      } else {
        this.loggedIn = true;
        this.loggedUsername = u.name;
      }
    });
  }

  refresh() {
    this.loading = false;
  }

  login(): void {
    this.loading = true;
    this.authenticationService
      .login(this.loginForm.value["username"], this.loginForm.value["password"])
      .subscribe();
  }

  logout(): void {
    this.authenticationService.logout();
    this.refresh();
  }
}
