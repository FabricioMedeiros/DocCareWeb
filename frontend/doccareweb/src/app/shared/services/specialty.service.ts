import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { Specialty } from 'src/app/features/specialty/models/specialty';

@Injectable()
export class SpecialtyService extends GenericCrudService<Specialty> {
  constructor(protected override http: HttpClient) {
    super(http, 'Specialty', 'specialty');
  }
}