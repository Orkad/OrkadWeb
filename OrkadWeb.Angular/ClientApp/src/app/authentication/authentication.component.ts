import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthenticationService } from './authentication.service';
import { User } from 'src/shared/models/User';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css'],
})
export class AuthenticationComponent implements OnInit {
  loading: boolean = false;
  loggedIn: boolean;
  loggedUsername: string | undefined;
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
    this.setUser(this.authenticationService.getUser());
    this.authenticationService.user.subscribe((user) => this.setUser(user));
  }

  setUser(user: User | null): void {
    this.loggedIn = !!user;
    this.loggedUsername = user?.name;
  }

  login(): void {
    this.loading = true;
    this.authenticationService
      .login(this.loginForm.value['username'], this.loginForm.value['password'])
      .subscribe((data) => {
        this.error = data.error;
        this.loading = false;
      });
  }

  logout(): void {
    this.authenticationService.logout();
  }
}
