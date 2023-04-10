import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { NotificationService } from 'src/services/notification.service';
import { AuthenticationService } from './authentication.service';

export const authenticationGuard: CanActivateFn = () => {
  const jwtHelper = inject(JwtHelperService);
  const authenticationService = inject(AuthenticationService);
  const router = inject(Router);
  const notificationService = inject(NotificationService);
  if (!jwtHelper.isTokenExpired()) {
    return true;
  }
  authenticationService.logout();
  router.navigate(['auth']);
  notificationService.error('la session a expirée, veuillez vous reconnecter');
  return false;
};
