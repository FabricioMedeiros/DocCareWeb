import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { NgxSpinnerModule } from 'ngx-spinner';

import { SharedModule } from 'src/app/shared/shared.module';
import { HealthPlanRoutingModule } from './healthplan-routing.module';
import { HealthplanComponent } from './healthplan.component';
import { HealthPlanListComponent } from './components/healthplan-list/healthplan-list.component';
import { HealthPlanFormComponent } from './components/healthplan-form/healthplan-form.component';
import { HealthPlanService } from '../../shared/services/healthplan.service';
import { CurrencyMaskModule } from 'ng2-currency-mask';

@NgModule({
  declarations: [
    HealthplanComponent,
    HealthPlanListComponent,
    HealthPlanFormComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HealthPlanRoutingModule,
    SharedModule,
    NgxSpinnerModule,
    CurrencyMaskModule
  ],
  providers: [HealthPlanService]
})
export class HealthplanModule { }
