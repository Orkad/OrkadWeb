import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { JwtModule } from "@auth0/angular-jwt";

import { AppComponent } from "./app.component";
import { HomeComponent } from "./home/home.component";
import { FetchDataComponent } from "./fetch-data/fetch-data.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { AuthenticationComponent } from "./authentication/authentication.component";
import {
  MatButtonModule,
  MatCardModule,
  MatDatepickerModule,
  MatDividerModule,
  MatFormFieldModule,
  MatIconModule,
  MatInputModule,
  MatListModule,
  MatNativeDateModule,
  MAT_DATE_LOCALE,
  MAT_DATE_FORMATS,
  MatProgressSpinnerModule,
  MatSidenavModule,
  MatSnackBarModule,
  MatToolbarModule,
  MatDateFormats,
} from "@angular/material";
import { AuthenticationGuard } from "./authentication/authentication.guard";
import { ExpenseComponent } from "./expense/expense.component";
import { MatMomentDateModule } from "@angular/material-moment-adapter";

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    FetchDataComponent,
    AuthenticationComponent,
    ExpenseComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: "", component: HomeComponent, pathMatch: "full" },
      {
        path: "expense",
        component: ExpenseComponent,
        canActivate: [AuthenticationGuard],
      },
      {
        path: "fetch-data",
        component: FetchDataComponent,
        canActivate: [AuthenticationGuard],
      },
      { path: "authentication", component: AuthenticationComponent },
    ]),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => localStorage.getItem("jwt"),
        whitelistedDomains: ["localhost:44365"],
      },
    }),
    BrowserAnimationsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatIconModule,
    MatSidenavModule,
    MatListModule,
    MatMomentDateModule,
    MatDatepickerModule,
  ],
  providers: [
    AuthenticationGuard,
    { provide: MAT_DATE_LOCALE, useValue: "fr" },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        display: {
          dateInput: "DD/MM/YYYY",
          monthYearLabel: "MMMM YYYY",
        },
        parse: {
          dateInput: "DD/MM/YYYY",
        },
      } as MatDateFormats,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
