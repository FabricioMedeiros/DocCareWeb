import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';

import { HealthplanComponent } from './healthplan.component';
import { HealthPlanFormComponent } from './components/healthplan-form/healthplan-form.component';
import { HealthPlanListComponent } from './components/healthplan-list/healthplan-list.component';
import { HealthPlanService } from '../../shared/services/healthplan.service';

const routes: Routes = [
  {
    path: '', component: HealthplanComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: HealthPlanListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: HealthPlanFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: HealthPlanFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], 
        resolve: { healthPlan: genericResolver(HealthPlanService, (service, id) => service.getById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HealthPlanRoutingModule { }
