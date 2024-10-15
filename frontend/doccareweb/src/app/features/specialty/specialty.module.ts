import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SpecialtyRoutingModule } from './specialty-routing.module';
import { SpecialtyComponent } from './specialty.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SpecialtyFormComponent } from './components/specialty-form/specialty-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    SpecialtyComponent,
    SpecialtyListComponent,
    SpecialtyFormComponent,
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    SpecialtyRoutingModule,
    SharedModule,
    NgxSpinnerModule
  ],
  providers:[]
})
export class SpecialtyModule { }
