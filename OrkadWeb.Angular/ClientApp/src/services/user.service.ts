import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterCommand } from 'src/api/commands/RegisterCommand';

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(private httpClient: HttpClient) {}
  register(
    username: string,
    email: string,
    password: string
  ): Observable<void> {
    return this.httpClient.post<void>('api/user/register', {
      userName: username,
      email: email,
      password: password,
    } as RegisterCommand);
  }
}
