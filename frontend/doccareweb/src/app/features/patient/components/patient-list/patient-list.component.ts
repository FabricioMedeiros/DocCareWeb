import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { PatientList } from '../../models/patient';
import { PatientService } from 'src/app/shared/services/patient.service';
import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.css']
})
export class PatientListComponent extends BaseListComponent<PatientList> implements OnInit{

  constructor(patientPlanService: PatientService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(patientPlanService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'name';
    this.placeholderSearch = 'Pesquise pelo nome';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/patient');
  }
}