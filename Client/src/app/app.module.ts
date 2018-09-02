import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { JwtHelperService, JwtModule, JwtInterceptor } from '@auth0/angular-jwt';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from 'app/app-routing.module';

import { AppComponent } from 'app/app.component';
import { FooterComponent } from 'app/outer/footer/footer.component';
import { ErrorModule } from 'app/outer/error/error.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'app/shared/shared.module';
import { AdminGuard } from 'app/inner/admin/admin.guard';
import { LoginModule } from 'app/outer/login/login.module';
import { LOCALSTORAGE } from 'app/shared/constants/localstorage';
import { BLACKLISTED_ROUTES } from 'app/shared/constants/blacklisted-routes';
import { JwtRefreshInterceptor } from './shared/utils/jwt-refresh-interceptor';


@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ErrorModule,
    LoginModule,
    RouterModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:4200/api'],
        blacklistedRoutes: BLACKLISTED_ROUTES
      }
    }),
    NgbModule.forRoot(),
    SharedModule.forRoot()
  ],
  declarations: [
    AppComponent,
    FooterComponent,
  ],
  providers: [
    AdminGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtRefreshInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

export function tokenGetter() {
  return localStorage.getItem(LOCALSTORAGE.JWT);
}