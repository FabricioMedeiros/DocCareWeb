import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { DoctorFormComponent } from './components/doctor-form/doctor-form.component';
import { DoctorListComponent } from './components/doctor-list/doctor-list.component';
import { DoctorComponent } from './doctor.component';
import { DoctorService } from '../../shared/services/doctor.service';

const routes: Routes = [
  {
    path: '', component: DoctorComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: DoctorListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: DoctorFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: DoctorFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], resolve: {
        doctor: genericResolver(DoctorService, (service, id) => service.getById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DoctorRoutingModule { }
