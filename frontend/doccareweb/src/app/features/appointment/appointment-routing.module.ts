import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';

import { AppointmentComponent } from './appointment.component';
import { AppointmentFormComponent } from './components/appointment-form/appointment-form.component';
import { AppointmentListComponent } from './components/appointment-list/appointment-list.component';
import { AppointmentService } from '../../shared/services/appointment.service';

const routes: Routes = [
  {
    path: '', component: AppointmentComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: AppointmentListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: AppointmentFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: AppointmentFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], 
        resolve: { appointment: genericResolver(AppointmentService, (service, id) => service.getAppointmentById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AppointmentRoutingModule { }
