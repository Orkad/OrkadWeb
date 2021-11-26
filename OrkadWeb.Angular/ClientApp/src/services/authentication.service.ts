import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class AuthenticationService {
    constructor(private httpClient: HttpClient) { }

    login(username:string, password:string) : Observable<any> {
        return this.httpClient.post('/authentication/login', { username: username, password: password });
    }
}

