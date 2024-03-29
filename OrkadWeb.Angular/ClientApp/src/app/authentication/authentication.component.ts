import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { User } from 'src/shared/models/User';
import { AuthenticationService } from './authentication.service';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.css'],
})
export class AuthenticationComponent implements OnInit {
  loading = false;
  loggedIn: boolean;
  user: User | null;
  loginForm: UntypedFormGroup;
  error?: string;

  constructor(
    private authenticationService: AuthenticationService,
    private fb: UntypedFormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: [''],
      password: [''],
    });
    this.authenticationService.user$.subscribe((user) => this.setUser(user));
  }

  setUser(user: User | null): void {
    this.loggedIn = !!user;
    this.user = user;
  }

  login(): void {
    if (this.loginForm.invalid) {
      return;
    }
    this.loading = true;
    this.authenticationService
      .login(this.loginForm.value['username'], this.loginForm.value['password'])
      .subscribe((data) => {
        this.error = data.error;
        this.loading = false;
      });
  }

  displayRole(role: string) {
    switch (role) {
      case 'Admin':
        return 'Administrateur';
      case 'User':
        return 'Utilisateur';
      default:
        return '';
    }
  }

  resend() {}
}
