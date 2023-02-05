import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationService } from './authentication.service';
import { NotificationService } from 'src/services/notification.service';

@Injectable({ providedIn: 'root' })
export class AuthenticationGuard implements CanActivate {
  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router,
    private authenticationService: AuthenticationService,
    private notificationService: NotificationService
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const token = this.jwtHelper.tokenGetter();
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return true;
    }
    this.authenticationService.logout();
    this.router.navigate(['authentication']);
    this.notificationService.error(
      'la session a expirée, veuillez vous reconnecter'
    );
    return false;
  }
}
