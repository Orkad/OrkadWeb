import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../../shared/models/User';
import { LoginResponse } from '../../shared/models/LoginResponse';
import { RegisterCommand } from 'src/api/commands/RegisterCommand';
import { Router } from '@angular/router';
import { AuthClient, LoginCommand, LoginResult } from '../web-api-client';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private userSubject: BehaviorSubject<User | null>;
  user$: Observable<User | null>;
  get isConnected(): boolean {
    return !this.jwtHelper.isTokenExpired();
  }
  get connectedUser(): User | null {
    if (this.jwtHelper.isTokenExpired()) {
      return null;
    }
    const decodedToken = this.jwtHelper.decodeToken();
    return <User>{
      id: decodedToken.sub,
      name: decodedToken.name,
      email: decodedToken.email,
      role: decodedToken.role,
      confirmed: decodedToken.confirmed,
    };
  }

  constructor(
    private httpClient: HttpClient,
    private authClient: AuthClient,
    private jwtHelper: JwtHelperService,
    private router: Router
  ) {
    this.userSubject = new BehaviorSubject<User | null>(this.connectedUser);
    this.user$ = this.userSubject.asObservable().pipe(shareReplay());
  }

  login(username: string, password: string): Observable<LoginResult> {
    return this.authClient
      .auth_Login(<LoginCommand>{
        username: username,
        password: password,
      })
      .pipe(
        map((data) => {
          if (data && data.success && !data.error) {
            if (data.token) {
              localStorage.setItem('jwt', data.token);
            }
            this.userSubject.next(this.connectedUser);
          }
          return data;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('jwt');
    this.userSubject.next(null);
    this.router.navigate(['auth']);
  }

  register(
    username: string,
    email: string,
    password: string
  ): Observable<void> {
    return this.httpClient.post<void>('api/auth/register', <RegisterCommand>{
      userName: username,
      email: email,
      password: password,
    });
  }
}
