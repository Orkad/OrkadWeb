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
import { ExpenseComponent } from './expense/expense.component';
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
    path: 'expenses',
    component: ExpenseComponent,
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
    AuthenticationComponent,
    ExpenseComponent,
    RegistrationComponent,
    TransactionComponent,
    MonthPickerComponent,
    ConfirmDialogComponent,
    MonthlyBudgetComponent,
    MonthlyChargeFormDialogComponent,
    MonthlyIncomeFormDialogComponent,
    MonthlyBudgetOverviewComponent,
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
    NgChartsModule,
  ],
  providers: [AuthenticationGuard, localProvider],
  bootstrap: [AppComponent],
})
export class AppModule {}
