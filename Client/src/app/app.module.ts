import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { FooterComponent } from './outer/footer/footer.component';
import { ErrorModule } from './outer/error/error.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from './shared/shared.module';
import { AdminGuard } from './inner/admin/admin.guard';
import { LoginModule } from './outer/login/login.module';


@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ErrorModule,
    LoginModule,
    RouterModule,
    NgbModule.forRoot(),
    SharedModule.forRoot()
  ],
  declarations: [
    AppComponent,
    FooterComponent,
  ],
  providers: [
    AdminGuard,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
