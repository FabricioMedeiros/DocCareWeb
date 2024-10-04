import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HealthplanComponent } from './healthplan.component';
import { HealthplanListComponent } from './components/healthplan-list/healthplan-list.component';
import { HealthplanFormComponent } from './components/healthplan-form/healthplan-form.component';



@NgModule({
  declarations: [
    HealthplanComponent,
    HealthplanListComponent,
    HealthplanFormComponent
  ],
  imports: [
    CommonModule
  ]
})
export class HealthplanModule { }
