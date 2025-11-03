import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { Patient } from 'src/app/features/patient/models/patient';
import { SearchZipCode } from 'src/app/features/patient/models/address';


@Injectable()
export class PatientService extends GenericCrudService<Patient> {
  constructor(protected override http: HttpClient) {
    super(http, 'Patient', 'patient');
  }

   searchZipCode(zipCode: string): Observable<SearchZipCode> {
        return this.http
            .get<SearchZipCode>(`https://viacep.com.br/ws/${zipCode}/json/`)
            .pipe(catchError(super.serviceError))
    }
}