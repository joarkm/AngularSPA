import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './components/header/header.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { AuthService } from './services/auth.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    NgbModule,
    RouterModule,
  ],
  declarations: [
    HeaderComponent
  ],
  exports: [
    HeaderComponent
  ]
})
export class SharedModule { 
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        AuthService
      ]
    }
  }
}
