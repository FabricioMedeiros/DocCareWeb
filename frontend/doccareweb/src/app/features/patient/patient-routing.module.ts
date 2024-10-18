import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { PatientService } from 'src/app/shared/services/patient.service';

import { PatientComponent } from './patient.component';
import { PatientFormComponent } from './components/patient-form/patient-form.component';
import { PatientListComponent } from './components/patient-list/patient-list.component';

const routes: Routes = [
  {
    path: '', component: PatientComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: PatientListComponent,  canActivate: [authGuard] }, 
      { path: 'new', component: PatientFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: PatientFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], 
        resolve: { patient: genericResolver(PatientService, (service, id) => service.getPatientById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PatientRoutingModule { }
