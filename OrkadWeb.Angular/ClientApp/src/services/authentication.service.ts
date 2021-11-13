import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    login(username:string, password:string ) {
        return this.httpClient.post('/authentication/login', {username, password})
            .subscribe(res => {
                console.log(res);
            });
    }
}

