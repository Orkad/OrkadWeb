import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({ providedIn: "root" })
export class AuthenticationService {
  constructor(
    private httpClient: HttpClient,
    private jwtHelper: JwtHelperService
  ) {}

  login(username: string, password: string): Observable<any> {
    return this.httpClient.post("/authentication/login", {
      username: username,
      password: password,
    });
  }

  logout(): void {
    localStorage.removeItem("jwt");
  }

  loggedIn(): boolean {
    const token = localStorage.getItem("jwt");
    return token && !this.jwtHelper.isTokenExpired(token);
  }
}
