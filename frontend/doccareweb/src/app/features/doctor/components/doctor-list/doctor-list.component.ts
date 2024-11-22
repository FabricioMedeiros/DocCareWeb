import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BsModalService } from 'ngx-bootstrap/modal';

import { DoctorService } from '../../../../shared/services/doctor.service';
import { DoctorList } from '../../models/doctor';
import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';

@Component({
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent extends BaseListComponent<DoctorList> implements OnInit {

  constructor(doctorService: DoctorService, 
              router: Router, 
              toastr: ToastrService,
              spinner: NgxSpinnerService, 
              modalService: BsModalService) {
    super(doctorService, router, toastr, spinner, modalService);
  }
  
  override ngOnInit(): void { 
    this.fieldSearch = 'name'; 
    this.placeholderSearch = 'Pesquise pelo nome do médico'; 
    super.ngOnInit(); 
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/doctor');
  }
}