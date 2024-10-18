import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';

import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CurrencyFormatPipe } from './pipes/currency-format.pipe';
import { SpecialtyService } from './services/specialty.service';
import { DoctorService } from './services/doctor.service';
import { PhoneMaskPipe } from './pipes/phone-mask.pipe';
import { CrmMaskPipe } from './pipes/crm-mask.pipe';
import { PatientService } from './services/patient.service';
import { CpfMaskPipe } from './pipes/cpf-mask.pipe';
import { HealthPlanService } from './services/healthplan.service';

@NgModule({
  declarations: [
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe,
    PhoneMaskPipe,
    CrmMaskPipe,
    CpfMaskPipe 
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxMaskDirective, 
    NgxMaskPipe 
  ],
  exports: [
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe,
    PhoneMaskPipe,
    CrmMaskPipe,
    CpfMaskPipe,
    NgxMaskDirective, 
    NgxMaskPipe 
  ],
  providers: [provideNgxMask(), SpecialtyService, DoctorService, PatientService, HealthPlanService]
})
export class SharedModule { }
