import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SpecialtyRoutingModule } from './specialty-routing.module';
import { SpecialtyAppComponent } from './specialty.app.component';
import { SpecialtyListComponent } from './components/specialty-list/specialty-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { SpecialtyService } from './services/specialty.service';


@NgModule({
  declarations: [
    SpecialtyAppComponent,
    SpecialtyListComponent
  ],
  imports: [
    CommonModule,
    SpecialtyRoutingModule,
    SharedModule
  ],
  providers:[SpecialtyService]
})
export class SpecialtyModule { }
