import { Injectable } from '@angular/core';
import { MatLegacySnackBar as MatSnackBar } from '@angular/material/legacy-snack-bar';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  constructor(private snackbar: MatSnackBar) {}

  public error(message: string): void {
    this.snackbar.open(message, undefined, {
      duration: 3000,
    });
  }

  public success(message: string): void {
    this.snackbar.open(message, undefined, {
      duration: 3000,
    });
  }
}
