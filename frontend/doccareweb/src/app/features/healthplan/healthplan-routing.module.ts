import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';

import { HealthplanComponent } from './healthplan.component';
import { HealthplanFormComponent } from './components/healthplan-form/healthplan-form.component';
import { HealthplanListComponent } from './components/healthplan-list/healthplan-list.component';
import { HealthPlanService } from './services/healthplan.service';



const routes: Routes = [
  {
    path: '', component: HealthplanComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: HealthplanListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: HealthplanFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: HealthplanFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], 
        resolve: { healthPlan: genericResolver(HealthPlanService, (service, id) => service.getHealthPlanById(id))
      }},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HealthPlanRoutingModule { }
