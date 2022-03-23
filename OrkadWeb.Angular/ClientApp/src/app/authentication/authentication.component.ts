import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthenticationService } from './authentication.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css'],
})
export class AuthenticationComponent implements OnInit {
  loading: boolean = false;
  loggedIn: boolean;
  loggedUsername: string | null;
  loginForm: FormGroup;
  error: string;

  constructor(
    private authenticationService: AuthenticationService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: [''],
      password: [''],
    });
    this.authenticationService.user.subscribe((user) => {
      if (user) {
        this.loggedIn = true;
        this.loggedUsername = user.name;
      }
    });
  }

  login(): void {
    this.loading = true;
    this.authenticationService
      .login(this.loginForm.value['username'], this.loginForm.value['password'])
      .subscribe(() => (this.loading = false));
  }

  logout(): void {
    this.authenticationService.logout();
  }
}
