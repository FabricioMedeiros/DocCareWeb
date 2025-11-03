import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { Service } from 'src/app/features/service/models/service';

@Injectable()
export class ServiceService extends GenericCrudService<Service> {
  constructor(protected override http: HttpClient) {
    super(http, 'Service', 'service');
  }
}