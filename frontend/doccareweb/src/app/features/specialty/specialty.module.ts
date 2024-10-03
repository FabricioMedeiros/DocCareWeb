import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SpecialtyRoutingModule } from './specialty-routing.module';
import { SpecialtyAppComponent } from './specialty.app.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SpecialtyService } from './services/specialty.service';
import { SpecialtyFormComponent } from './components/specialty-form/specialty-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    SpecialtyAppComponent,
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
  providers:[SpecialtyService]
})
export class SpecialtyModule { }
