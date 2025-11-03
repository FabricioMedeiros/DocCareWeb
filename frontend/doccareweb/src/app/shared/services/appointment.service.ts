import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable } from 'rxjs';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { Appointment } from 'src/app/features/appointment/models/appointment';
import { AppointmentStatus } from 'src/app/features/appointment/models/appointment-status';


@Injectable()
export class AppointmentService extends GenericCrudService<Appointment> {
  constructor(protected override http: HttpClient) {
    super(http, 'Appointment', 'appointment');
  }

  updateAppointmentStatus(id: number, status: AppointmentStatus): Observable<any> {
    const headers = this.getAuthHeaderJson();

    const statusData = {
      id: id,
      status: status
    };

    return this.http
      .put(`${this.UrlServiceV1}appointment/${id}/status`, statusData, headers)
      .pipe(
            map(this.extractData),
            catchError(this.serviceError)
          );
  }

  saveLocalCurrentDateList(date: Date): void {
        localStorage.setItem('currentDateAppointmentList', date.toISOString());
    }

  getLocalCurrentDateList(): Date | null {
        const storedDate = localStorage.getItem('currentDateAppointmentList');
        const parsedDate = storedDate ? new Date(storedDate) : null;

        return parsedDate && !isNaN(parsedDate.getTime()) ? parsedDate : null;
  }

  clearLocalCurrentDateList(): void {
    localStorage.removeItem('currentDateAppointmentList');
  }

  saveLocalCurrentDoctorList(doctor: number): void {
    localStorage.setItem('currentDoctorAppointmentList', doctor.toString());
  }

    getLocalCurrentDoctorList(): string {
      return localStorage.getItem('currentDoctorAppointmentList') || '';
    }

    clearLocalCurrentDoctorList(): void {
      localStorage.removeItem('currentDoctorAppointmentList');
    }
}