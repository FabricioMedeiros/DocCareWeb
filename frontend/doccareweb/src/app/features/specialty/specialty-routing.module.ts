import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { SpecialtyAppComponent } from './specialty.app.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';


const routes: Routes = [
  {
      path: '', component: SpecialtyAppComponent,
      children: [
          { path: 'list', component: SpecialtyListComponent},  
      ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SpecialtyRoutingModule { }