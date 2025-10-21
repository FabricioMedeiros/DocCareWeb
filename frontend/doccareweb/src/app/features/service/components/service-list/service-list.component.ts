import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BsModalService } from 'ngx-bootstrap/modal';

import { Service } from '../../models/service';
import { ServiceService } from '../../../../shared/services/service.service';
import { BaseListComponent } from 'src/app/shared/components/base-list/base-list.component';

@Component({
  selector: 'app-service-list',
  templateUrl: './service-list.component.html',
  styleUrls: ['./service-list.component.css']
})
export class ServiceListComponent extends BaseListComponent<Service> implements OnInit {
  constructor(serviceService: ServiceService,
    router: Router,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    modalService: BsModalService) {
    super(serviceService, router, toastr, spinner, modalService);
  }

  override ngOnInit(): void {
    this.fieldSearch = 'name';
    this.placeholderSearch = 'Pesquise pelo nome';
    super.ngOnInit();
  }

  isCurrentRoute(url: string): boolean {
    return url.includes('/service');
  }
}
