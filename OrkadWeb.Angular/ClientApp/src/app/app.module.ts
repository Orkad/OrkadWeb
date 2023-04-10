import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { LOCALE_ID, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { registerLocaleData } from '@angular/common';
import fr from '@angular/common/locales/fr';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgChartsModule } from 'ng2-charts';
import { ConfigurationResolver } from 'src/resolvers/configuration.resolver';
import { ApiInterceptor } from 'src/services/api.interceptor';
import { MaterialModule } from '../shared/modules/material.module';
import { AppComponent } from './app.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { authenticationGuard } from './authentication/authentication.guard';
import { EmailConfirmationComponent } from './authentication/email-confirmation/email-confirmation.component';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { MonthlyBudgetOverviewComponent } from './monthly-budget/monthly-budget-overview/monthly-budget-overview.component';
import { MonthlyBudgetComponent } from './monthly-budget/monthly-budget.component';
import { MonthlyChargeFormDialogComponent } from './monthly-budget/monthly-charge-form-dialog/monthly-charge-form-dialog.component';
import { MonthlyIncomeFormDialogComponent } from './monthly-budget/monthly-income-form-dialog/monthly-income-form-dialog.component';
import { ConfirmDialogComponent } from './shared/dialog/confirm-dialog/confirm-dialog.component';
import { DisplayComponent } from './shared/display/display.component';
import { MonthPickerComponent } from './shared/month-picker/month-picker.component';
import { ExpenseFormDialogComponent } from './transaction/expense-form-dialog/expense-form-dialog.component';
import { TransactionComponent } from './transaction/transaction.component';
import { UserListComponent } from './user-list/user-list.component';

registerLocaleData(fr);

const routes = [
  {
    path: '',
    component: TransactionComponent,
    canActivate: [authenticationGuard],
  },
  {
    path: 'budget',
    component: MonthlyBudgetComponent,
    canActivate: [authenticationGuard],
  },
  {
    path: 'auth',
    component: AuthenticationComponent,
  },
  {
    path: 'auth/register',
    component: RegistrationComponent,
    resolve: {
      config: ConfigurationResolver,
    },
  },
  {
    path: 'auth/confirm',
    component: EmailConfirmationComponent,
  },
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [authenticationGuard],
  },
] as Routes;

@NgModule({
  declarations: [
    AppComponent,
    AuthenticationComponent,
    RegistrationComponent,
    TransactionComponent,
    MonthPickerComponent,
    ConfirmDialogComponent,
    MonthlyBudgetComponent,
    MonthlyChargeFormDialogComponent,
    MonthlyIncomeFormDialogComponent,
    MonthlyBudgetOverviewComponent,
    ExpenseFormDialogComponent,
    UserListComponent,
    EmailConfirmationComponent,
    DisplayComponent,
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
    MatMomentDateModule,
    NgChartsModule,
  ],
  providers: [
    {
      provide: LOCALE_ID,
      useValue: 'fr-FR',
    },
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'fr-FR',
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: {
          dateInput: 'DD/MM/YYYY',
        },
        display: {
          dateInput: 'DD/MM/YYYY',
          monthYearLabel: 'MMMM YYYY',
        },
      },
    },
    { provide: HTTP_INTERCEPTORS, useClass: ApiInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
