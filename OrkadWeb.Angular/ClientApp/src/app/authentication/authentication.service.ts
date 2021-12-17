import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable, Subject } from "rxjs";
import { map } from "rxjs/operators";
import { JwtHelperService } from "@auth0/angular-jwt";
import { User } from "../../shared/models/User";
import { LoginResponse } from "../../shared/models/LoginResponse";

@Injectable({ providedIn: "root" })
export class AuthenticationService {
  private userSubject: BehaviorSubject<User>;
  user: Observable<User>;

  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {
    this.userSubject = new BehaviorSubject<User>(this.getUser());
    this.user = this.userSubject.asObservable();
  }

  private getUser(): User {
    const token = localStorage.getItem("jwt");
    if (!token) {
      return null;
    }
    const decodedToken = this.jwtHelper.decodeToken(token);
    return {
      id: decodedToken.user_id,
      name: decodedToken.user_name,
      email: decodedToken.user_email,
    } as User;
  }

  login(username: string, password: string): Observable<boolean> {
    return this.httpClient
      .post<LoginResponse>("/authentication/login", {
        username: username,
        password: password,
      })
      .pipe<boolean>(
        map<LoginResponse, boolean>((data) => {
          if (data && data.success && !data.error) {
            localStorage.setItem("jwt", data.token);
            this.userSubject.next(this.getUser());
            return true;
          }
          return false;
        })
      );
  }

  logout(): void {
    localStorage.removeItem("jwt");
    this.userSubject.next(this.getUser());
  }
}
