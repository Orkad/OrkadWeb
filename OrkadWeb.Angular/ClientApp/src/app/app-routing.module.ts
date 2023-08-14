import { NgModule } from '@angular/core';
import { TransactionComponent } from './transaction/transaction.component';
import { authenticationGuard } from './authentication/authentication.guard';
import { MonthlyBudgetComponent } from './monthly-budget/monthly-budget.component';
import { AuthenticationComponent } from './authentication/authentication.component';
import { RegistrationComponent } from './authentication/registration/registration.component';
import { ConfigurationResolver } from 'src/resolvers/configuration.resolver';
import { EmailConfirmationComponent } from './authentication/email-confirmation/email-confirmation.component';
import { UserListComponent } from './user-list/user-list.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
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
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
