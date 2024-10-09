import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';

import { SpecialtyService } from './services/specialty.service';
import { SpecialtyComponent } from './specialty.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SpecialtyFormComponent } from './components/specialty-form/specialty-form.component';



const routes: Routes = [
  {
    path: '', component: SpecialtyComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: SpecialtyListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: SpecialtyFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: SpecialtyFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], resolve: {
        specialty: genericResolver(SpecialtyService, (service, id) => service.getSpecialtyById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SpecialtyRoutingModule { }
