import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    SharedModule
  ],
  declarations: [
    LoginComponent
  ]
})
export class LoginModule { }
