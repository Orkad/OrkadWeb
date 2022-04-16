import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule, Provider } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { ExpenseComponent } from './expense/expense.component';
import { MaterialModule } from '../shared/modules/material.module';
import fr from '@angular/common/locales/fr';
import { registerLocaleData } from '@angular/common';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { ConfigurationResolver } from 'src/resolvers/configuration.resolver';
import { TransactionComponent } from './transaction/transaction.component';

registerLocaleData(fr);

const routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'transactions',
    component: TransactionComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'authentication',
    component: AuthenticationComponent,
  },
  {
    path: 'authentication/registration',
    component: RegistrationComponent,
    resolve: {
      config: ConfigurationResolver,
    },
  },
] as Routes;

const localProvider = {
  provide: LOCALE_ID,
  useValue: 'fr-FR',
} as Provider;

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthenticationComponent,
    ExpenseComponent,
    RegistrationComponent,
    TransactionComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('jwt'),
        allowedDomains: ['localhost:44365'],
      },
    }),
    MaterialModule,
  ],
  providers: [AuthenticationGuard, localProvider],
  bootstrap: [AppComponent],
})
export class AppModule {}
