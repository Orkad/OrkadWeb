import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { interval, Observable, Subject, timer } from 'rxjs';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '../../shared/models/User';
import { LoginResponse } from '../../shared/models/LoginResponse';
import { RegisterCommand } from 'src/api/commands/RegisterCommand';
import { Router } from '@angular/router';
import { NotificationService } from 'src/services/notification.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  user = new Subject<User | null>();

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  checkTokenExpiration() {
    return timer(0, 3000).pipe(
      map(() => {
        const token = localStorage.getItem('jwt');
        if (!token) {
          return false;
        }
        if (this.jwtHelper.isTokenExpired(token)) {
          this.logout();
          this.router.navigate(['/authentication']);
          this.notificationService.error(
            'la session a expirée, veuillez vous reconnecter'
          );
          return false;
        }
        this.user.next(this.getUser(token));
        return true;
      })
    );
  }

  /** retrieve the unexpired user based on the local token */
  getUser(token: string): User | null {
    if (!token) {
      return null;
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    return <User>{
      id: decodedToken.user_id,
      name: decodedToken.user_name,
      email: decodedToken.user_email,
    };
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
            this.user.next(this.getUser(data.token));
          }
          return data;
        })
      );
  }

  logout(): void {
    localStorage.removeItem('jwt');
    this.user.next(null);
  }

  register(command: RegisterCommand): Observable<void> {
    return this.httpClient.post<void>('api/authentication/register', command);
  }
}
