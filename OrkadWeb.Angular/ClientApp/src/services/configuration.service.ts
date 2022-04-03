import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, shareReplay } from 'rxjs';
import { GlobalConfigurationResult } from 'src/api/results/GlobalConfigurationResult';

@Injectable({ providedIn: 'root' })
export class ConfigurationService {
  constructor(private httpClient: HttpClient) {}

  private cachedGlobalConfig$: Observable<GlobalConfigurationResult>;

  getGlobal(): Observable<GlobalConfigurationResult> {
    if (!this.cachedGlobalConfig$) {
      this.cachedGlobalConfig$ = this.httpClient
        .get<GlobalConfigurationResult>('api/config/global')
        .pipe(shareReplay(1));
    }
    return this.cachedGlobalConfig$;
  }
}
