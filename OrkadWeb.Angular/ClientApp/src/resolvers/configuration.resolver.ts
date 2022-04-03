import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { GlobalConfigurationResult } from 'src/api/results/GlobalConfigurationResult';
import { ConfigurationService } from 'src/services/configuration.service';

@Injectable({ providedIn: 'root' })
export class ConfigurationResolver
  implements Resolve<GlobalConfigurationResult>
{
  constructor(private configurationService: ConfigurationService) {}

  resolve():
    | Observable<GlobalConfigurationResult>
    | Promise<GlobalConfigurationResult>
    | GlobalConfigurationResult {
    return this.configurationService.getGlobal();
  }
}
