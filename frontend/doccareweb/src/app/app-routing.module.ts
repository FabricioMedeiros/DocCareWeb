import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/navigation/home/home.component';
import { authGuard } from './features/navigation/services/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: 'account', loadChildren: () => import('./features/account/account.module').then(m => m.AccountModule) },
  { path: '**', redirectTo: 'account/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
