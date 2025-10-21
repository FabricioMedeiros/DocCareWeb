import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ServiceComponent } from './service.component';
import { authGuard } from 'src/app/core/guards/auth.guard';
import { canDeactivateForm } from 'src/app/core/guards/can-deactivate-form.guard';
import { genericResolver } from 'src/app/core/resolvers/generic-resolver';
import { ServiceService } from 'src/app/shared/services/service.service';
import { ServiceFormComponent } from './components/service-form/service-form.component';
import { ServiceListComponent } from './components/service-list/service-list.component';

const routes: Routes = [
  {
    path: '', component: ServiceComponent,
    children: [
      { path: '', redirectTo: 'list', pathMatch: 'full' },
      { path: 'list', component: ServiceListComponent,  canActivate: [authGuard]  }, 
      { path: 'new', component: ServiceFormComponent,  canActivate: [authGuard], canDeactivate: [canDeactivateForm] }, 
      { path: 'edit/:id', component: ServiceFormComponent, 
        canActivate: [authGuard],  
        canDeactivate: [canDeactivateForm], 
        resolve: {service: genericResolver(ServiceService, (service, id) => service.getServiceById(id))}
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ServiceRoutingModule { }
