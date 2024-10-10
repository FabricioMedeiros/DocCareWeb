import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DoctorRoutingModule } from './doctor-routing.module';
import { DoctorComponent } from './doctor.component';
import { DoctorFormComponent } from './components/doctor-form/doctor-form.component';
import { DoctorListComponent } from './components/doctor-list/doctor-list.component';


@NgModule({
  declarations: [
    DoctorComponent,
    DoctorFormComponent,
    DoctorListComponent
  ],
  imports: [
    CommonModule,
    DoctorRoutingModule
  ]
})
export class DoctorModule { }
