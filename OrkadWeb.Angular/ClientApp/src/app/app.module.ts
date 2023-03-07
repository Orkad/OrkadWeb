import { BrowserModule } from '@angular/platform-browser';
import { LOCALE_ID, NgModule, Provider } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule, Routes, Route } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { MaterialModule } from '../shared/modules/material.module';
import fr from '@angular/common/locales/fr';
import { registerLocaleData } from '@angular/common';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { ConfigurationResolver } from 'src/resolvers/configuration.resolver';
import { TransactionComponent } from './transaction/transaction.component';
import { MonthPickerComponent } from './shared/month-picker/month-picker.component';
import { ConfirmDialogComponent } from './shared/dialog/confirm-dialog/confirm-dialog.component';
import { NgChartsModule } from 'ng2-charts';
import { MonthlyBudgetComponent } from './monthly-budget/monthly-budget.component';
import { MonthlyChargeFormDialogComponent } from './monthly-budget/monthly-charge-form-dialog/monthly-charge-form-dialog.component';
import { MonthlyIncomeFormDialogComponent } from './monthly-budget/monthly-income-form-dialog/monthly-income-form-dialog.component';
import { MonthlyBudgetOverviewComponent } from './monthly-budget/monthly-budget-overview/monthly-budget-overview.component';
import { ExpenseFormDialogComponent } from './transaction/expense-form-dialog/expense-form-dialog.component';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { UserListComponent } from './user-list/user-list.component';

registerLocaleData(fr);

const routes = [
  {
    path: '',
    component: TransactionComponent,
    canActivate: [AuthenticationGuard],
  },
  {
    path: 'budget',
    component: MonthlyBudgetComponent,
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
  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthenticationGuard],
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
    AuthenticationGuard,
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
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
