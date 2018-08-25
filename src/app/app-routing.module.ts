import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorComponent } from './outer/error/error.component';
import { AdminGuard } from './inner/admin/admin.guard';

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
    path: '',
    redirectTo: '',
    pathMatch: 'full'
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
