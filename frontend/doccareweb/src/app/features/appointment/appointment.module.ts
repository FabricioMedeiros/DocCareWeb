import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxSpinnerModule } from 'ngx-spinner';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PopoverModule } from 'ngx-bootstrap/popover';

import { AppointmentRoutingModule } from './appointment-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { AppointmentComponent } from './appointment.component';
import { AppointmentListComponent } from './components/appointment-list/appointment-list.component';
import { AppointmentFormComponent } from './components/appointment-form/appointment-form.component';
import { AppointmentService } from '../../shared/services/appointment.service';

@NgModule({
  declarations: [
    AppointmentComponent,
    AppointmentListComponent,
    AppointmentFormComponent
  ],
  imports: [
    CommonModule,
    AppointmentRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgxSpinnerModule,
    BsDatepickerModule,
    BsDropdownModule,
    TooltipModule,
    PopoverModule,
    CurrencyMaskModule
  ],
  providers:[AppointmentService]
})
export class AppointmentModule { }
