import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../../shared/models/User';
import { LoginResponse } from '../../shared/models/LoginResponse';
import { RegisterCommand } from 'src/api/commands/RegisterCommand';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private userSubject: BehaviorSubject<User | null>;
  user$: Observable<User | null>;

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {
    this.userSubject = new BehaviorSubject<User | null>(this.readToken());
    this.user$ = this.userSubject.asObservable().pipe(shareReplay());
  }

  /** retrieve the unexpired user based on the local token */
  readToken(): User | null {
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

  saveToken(token: string | null) {
    if (token == null) {
      localStorage.removeItem('jwt');
      return;
    }
    localStorage.setItem('jwt', token);
  }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>('api/auth/login', {
        username: username,
        password: password,
      })
      .pipe(
        map((data) => {
          if (data && data.success && !data.error) {
            this.saveToken(data.token);
            this.userSubject.next(this.readToken());
          }
          return data;
        })
      );
  }

  logout(): void {
    this.saveToken(null);
    this.userSubject.next(null);
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

  getConnectedUser() {
    return this.userSubject.getValue();
  }
}
