import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { Doctor } from 'src/app/features/doctor/models/doctor';

@Injectable()
export class DoctorService extends GenericCrudService<Doctor> {
  constructor(protected override http: HttpClient) {
    super(http, 'Doctor', 'doctor');
  }
}