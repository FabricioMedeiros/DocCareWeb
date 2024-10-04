import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SpecialtyAppComponent } from './specialty.app.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SpecialtyFormComponent } from './components/specialty-form/specialty-form.component';
import { specialtyResolver } from './services/specialty.resolve';
import { canDeactivateSpecialtyForm } from './services/specialty.guard';
import { authGuard } from 'src/app/core/guards/auth.guard';


const routes: Routes = [
  {
    path: '', component: SpecialtyAppComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: SpecialtyListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: SpecialtyFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateSpecialtyForm] }, 
      { path: 'edit/:id', component: SpecialtyFormComponent, canActivate: [authGuard],  canDeactivate: [canDeactivateSpecialtyForm], resolve: { specialty: specialtyResolver }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SpecialtyRoutingModule { }
