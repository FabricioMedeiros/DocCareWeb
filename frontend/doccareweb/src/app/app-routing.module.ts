import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './features/navigation/components/home/home.component';
import { NotFoundComponent } from './features/navigation/components/not-found/not-found.component';
import { ServiceUnavailableComponent } from './features/navigation/components/service-unavailable/service-unavailable.component';
import { navigationGuard } from './core/guards/navigation.guard';
import { authGuard } from './core/guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'account/login', pathMatch: 'full' },
  { path: 'home', component: HomeComponent, canActivate: [authGuard] },
  { path: 'account', loadChildren: () => import('./features/account/account.module').then(m => m.AccountModule) },
  { path: 'specialty', loadChildren: () => import('./features/specialty/specialty.module').then(m => m.SpecialtyModule) },
  { path: 'service-unavailable', component: ServiceUnavailableComponent, canActivate: [navigationGuard] },
  { path: 'not-found', component: NotFoundComponent, canActivate: [navigationGuard] },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
