import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BsModalService } from 'ngx-bootstrap/modal';

import { Specialty } from '../../models/specialty';
import { SpecialtyService } from '../../../../shared/services/specialty.service';
import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';

@Component({
  selector: 'app-specialty-list',
  templateUrl: './specialty-list.component.html',
  styleUrls: ['./specialty-list.component.css']
})
export class SpecialtyListComponent extends BaseListComponent<Specialty> implements OnInit {
  constructor(specialtyService: SpecialtyService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(specialtyService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'description';
    this.placeholderSearch = 'Pesquise pela descrição';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/specialty');
  }
}