import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from "@angular/router";
import { JwtHelperService } from "@auth0/angular-jwt";
import { AuthenticationService } from "./authentication.service";

@Injectable({ providedIn: "root" })
export class AuthenticationGuard implements CanActivate {
  constructor(
    private jwtHelper: JwtHelperService,
    private router: Router,
    private authenticationService: AuthenticationService
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    if (this.authenticationService.user != null) {
      return true;
    }
    this.router.navigate(["authentication"]);
    return false;
  }
}
