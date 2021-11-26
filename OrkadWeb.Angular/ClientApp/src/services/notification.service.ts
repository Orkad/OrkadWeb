import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    constructor(private snackbar: MatSnackBar){}

    public error(message:string):void
    {
        this.snackbar.open(message, null, {
            duration: 3000,
        });
    }
}