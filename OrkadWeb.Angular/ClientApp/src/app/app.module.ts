import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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

const routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  {
    path: 'expense',
    component: ExpenseComponent,
    canActivate: [AuthenticationGuard],
  },
  { path: 'authentication', component: AuthenticationComponent },
] as Routes;

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthenticationComponent,
    ExpenseComponent,
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
  providers: [AuthenticationGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
