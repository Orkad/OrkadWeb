import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../../shared/models/User';
import { LoginResponse } from '../../shared/models/LoginResponse';
import { RegisterCommand } from 'src/api/commands/RegisterCommand';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  user = new Subject<User | null>();

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {}

  /** retrieve the unexpired user based on the local token */
  getUser(): User | null {
    const token = localStorage.getItem('jwt');
    if (!token) {
      return null;
    }
    if (this.jwtHelper.isTokenExpired(token)) {
      localStorage.removeItem('jwt');
      return null;
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    return {
      id: decodedToken.user_id,
      name: decodedToken.user_name,
      email: decodedToken.user_email,
    } as User;
  }

  login(username: string, password: string): Observable<LoginResponse> {
    return this.httpClient
      .post<LoginResponse>('api/authentication/login', {
        username: username,
        password: password,
      })
      .pipe(
        map((data) => {
          if (data && data.success && !data.error) {
            localStorage.setItem('jwt', data.token);
            this.user.next(this.getUser());
          }
          return data;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('jwt');
    this.user.next(this.getUser());
  }

  register(command: RegisterCommand): Observable<void> {
    return this.httpClient.post<void>('api/authentication/register', command);
  }
}
