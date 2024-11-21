import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { HealthPlan } from '../../models/healthplan';
import { HealthPlanService } from '../../../../shared/services/healthplan.service';
import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';

@Component({
  selector: 'app-healthplan-list',
  templateUrl: './healthplan-list.component.html',
  styleUrls: ['./healthplan-list.component.css']
})
export class HealthPlanListComponent extends BaseListComponent<HealthPlan> implements OnInit {

  constructor(healthPlanService: HealthPlanService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(healthPlanService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'description';
    this.placeholderSearch = 'Pesquise pela descrição';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/healthplan');
  }
}