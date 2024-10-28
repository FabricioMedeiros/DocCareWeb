import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable } from "rxjs";

import { BaseService } from "src/app/core/services/base.service";
import { Appointment } from "../../features/appointment/models/appointment";

@Injectable()
export class AppointmentService extends BaseService {
    
    constructor(private http: HttpClient) { super(); }

    getAllAppointments(filters?: { [key: string]: string }): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}appointment`;
    
        if (filters) {
            const filterParams = Object.keys(filters)
                .map(key => `${key}=${filters[key]}`) 
                .join('&');
            
            url += `?${filterParams}`;
        }

        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }
    
    getAppointmentById(id: number): Observable<Appointment> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Appointment>(`${this.UrlServiceV1}appointment/${id}`, headers)
            .pipe(catchError(super.serviceError));
    }

    registerAppointment(appointment: Appointment): Observable<Appointment> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Appointment>(`${this.UrlServiceV1}appointment`, appointment, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateAppointment(appointment: Appointment): Observable<Appointment> {
        const headers = this.GetAuthHeaderJson();
        
        return this.http
            .put<Appointment>(`${this.UrlServiceV1}appointment/${appointment.id}`, appointment, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateAppointmentStatus(appointment: Appointment): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        return this.http
            .put(`${this.UrlServiceV1}appointment/${appointment.id}/status`, appointment.status,  headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    } 
    
    saveLocalCurrentDateList(date: Date): void {
        localStorage.setItem('currentDateAppointmentList', date.toString());
    }
    
    getLocalCurrentDateList(): Date | null {
        const storedDate = localStorage.getItem('currentDateAppointmentList');
        return storedDate ? new Date(storedDate) : null;
    }
}
