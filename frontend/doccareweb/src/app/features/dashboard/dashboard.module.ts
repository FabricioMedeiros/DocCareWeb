import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardDataComponent } from './components/dashboard-data/dashboard-data.component';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  declarations: [  
    DashboardComponent,
    DashboardDataComponent
  ],
  imports: [
    CommonModule,
    DashboardRoutingModule
  ]
})
export class DashboardModule { }
