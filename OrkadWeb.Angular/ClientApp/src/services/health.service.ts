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
  tap,
} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HealthService {
  private healthStatusSubject = new BehaviorSubject<boolean>(false);
  isHealthy: boolean;
  public unhealthyDate: Date | null = null;

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
            ),
            tap((ok: boolean) => {
              if (ok === false && this.isHealthy === true) {
                // turns of
                this.unhealthyDate = new Date();
              }
              this.isHealthy = ok;
            })
          )
        )
      )
      .subscribe((isHealthy) => {
        this.healthStatusSubject.next(isHealthy);
      });
  }
}
