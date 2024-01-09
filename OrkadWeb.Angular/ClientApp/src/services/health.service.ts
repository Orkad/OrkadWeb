import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {
  BehaviorSubject,
  catchError,
  interval,
  map,
  of,
  startWith,
  switchMap,
} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HealthService {
  private healthStatusSubject = new BehaviorSubject<boolean>(false);

  constructor(private httpClient: HttpClient) {
    this.checkPeriodically();
  }

  get healthStatus$() {
    return this.healthStatusSubject.asObservable();
  }

  private checkPeriodically() {
    interval(5000)
      .pipe(
        startWith(0),
        switchMap(() =>
          this.httpClient.get('/health', { responseType: 'text' }).pipe(
            catchError(() => {
              console.error('Health check failed:');
              return of('unhealthy');
            }),
            map(
              (response: string) => response.trim().toLowerCase() === 'healthy'
            )
          )
        )
      )
      .subscribe((isHealthy) => {
        this.healthStatusSubject.next(isHealthy);
      });
  }
}
