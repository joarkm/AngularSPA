import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service';
import { SPABgComponent } from './components/spa-bg/spa-bg.component';
import { SPACardComponent } from './components/spa-card/spa-card.component';
import { TableCardComponent } from './components/table-card/table-card.component';
import { AdminService } from './services/admin.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    RouterModule,
  ],
  declarations: [
    HeaderComponent,
    SPABgComponent,
    SPACardComponent,
    TableCardComponent
  ],
  exports: [
    HeaderComponent,
    SPABgComponent,
    SPACardComponent,
    TableCardComponent
  ]
})
export class SharedModule { 
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        AdminService,
        AuthService,
      ]
    }
  }
}
