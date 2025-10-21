import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { CurrencyMaskModule } from 'ng2-currency-mask';
import { NgxSpinnerModule } from 'ngx-spinner';

import { ServiceRoutingModule } from './service-routing.module';
import { ServiceListComponent } from './components/service-list/service-list.component';
import { ServiceFormComponent } from './components/service-form/service-form.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ServiceComponent } from './service.component';

@NgModule({
  declarations: [
    ServiceComponent,
    ServiceListComponent,
    ServiceFormComponent
  ],
  imports: [
    CommonModule,
    ServiceRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule,
    CurrencyMaskModule
  ]
})
export class ServiceModule { }
