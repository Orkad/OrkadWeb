import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class ApiInterceptor implements HttpInterceptor {
  intercept(
    httpRequest: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // if (httpRequest.url.startsWith('api/')) {
    //   return next.handle(httpRequest).pipe(
    //     map((event) => {
    //       return event;
    //     }),
    //     catchError((err, caught) => {
    //       return caught;
    //     })
    //   );
    // }
    return next.handle(httpRequest);
  }
}
