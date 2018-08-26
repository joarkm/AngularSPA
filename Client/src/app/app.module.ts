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


@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ErrorModule,
    RouterModule,
    NgbModule.forRoot(),
    SharedModule
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
