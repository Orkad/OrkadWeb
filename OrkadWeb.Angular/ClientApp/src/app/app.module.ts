import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationComponent } from './authentication/authentication.component';
import { AuthenticationGuard } from './authentication/authentication.guard';
import { ExpenseComponent } from './expense/expense.component';
import { MaterialModule } from './material.module';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthenticationComponent,
    ExpenseComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      {
        path: 'expense',
        component: ExpenseComponent,
        canActivate: [AuthenticationGuard],
      },
      { path: 'authentication', component: AuthenticationComponent },
    ]),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem('jwt'),
        allowedDomains: ['localhost:44365'],
      },
    }),
    BrowserAnimationsModule,
    MaterialModule,
  ],
  providers: [AuthenticationGuard],
  bootstrap: [AppComponent],
})
export class AppModule {}
