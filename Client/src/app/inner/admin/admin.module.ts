import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { RouterModule } from '@angular/router';
import { AccountsModule } from './accounts/accounts.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    AdminRoutingModule,
    AccountsModule
  ],
  declarations: [AdminComponent]
})
export class AdminModule { }
