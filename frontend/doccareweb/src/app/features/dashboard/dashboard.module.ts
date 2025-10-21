import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardDataComponent } from './components/dashboard-data/dashboard-data.component';

import { NgxSpinnerModule } from 'ngx-spinner';

import { DashboardComponent } from './dashboard.component';
import { DashBoardService } from './services/dasboard.service';
import { SharedModule } from "../../shared/shared.module";

@NgModule({
  declarations: [  
    DashboardComponent,
    DashboardDataComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    DashboardRoutingModule,
    NgxSpinnerModule,
    SharedModule
],
  providers:[DashBoardService]
})
export class DashboardModule { }
