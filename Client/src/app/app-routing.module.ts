import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './outer/error/error.component';
import { AdminGuard } from './inner/admin/admin.guard';
import { LoginComponent } from './outer/login/login.component';

const routes: Routes = [
  {
    path: 'main',
    loadChildren: 'app/inner/main/main.module#MainModule'
  },
  {
    path: 'admin',
    loadChildren: 'app/inner/admin/admin.module#AdminModule',
    canActivate: [AdminGuard]
  },
  {
    path: 'error',
    component: ErrorComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
