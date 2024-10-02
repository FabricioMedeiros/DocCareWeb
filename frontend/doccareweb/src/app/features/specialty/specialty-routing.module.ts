import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SpecialtyAppComponent } from './specialty.app.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SpecialtyFormComponent } from './components/specialty-form/specialty-form.component';
import { specialtyResolver } from './services/specialty.resolve';

const routes: Routes = [
  {
    path: '', component: SpecialtyAppComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: SpecialtyListComponent }, 
      { path: 'new', component: SpecialtyFormComponent }, 
      { path: 'edit/:id', component: SpecialtyFormComponent, resolve: { specialty: specialtyResolver }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SpecialtyRoutingModule { }
