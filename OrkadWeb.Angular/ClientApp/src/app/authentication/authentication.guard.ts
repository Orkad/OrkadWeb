import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { NotificationService } from 'src/services/notification.service';
import { AuthenticationService } from './authentication.service';

export const authenticationGuard: CanActivateFn = () => {
  const authenticationService = inject(AuthenticationService);
  const notificationService = inject(NotificationService);
  if (authenticationService.isConnected) {
    return true;
  }
  authenticationService.logout();
  notificationService.error('la session a expirée, veuillez vous reconnecter');
  return false;
};
