import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable } from "rxjs";

import { BaseService } from "src/app/core/services/base.service";
import { Appointment } from "../../features/appointment/models/appointment";
import { AppointmentStatus } from "src/app/features/appointment/models/appointment-status";

@Injectable()
export class AppointmentService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAllAppointments(page?: number, pageSize?: number, filters?: { [key: string]: string }): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        let url = `${this.UrlServiceV1}appointment`;

        const queryParts: string[] = [];

        if (page !== undefined && pageSize !== undefined) {
            queryParts.push(`pageNumber=${page}`, `pageSize=${pageSize}`);
        }

        if (filters) {
            const filterParams = Object.keys(filters)
                .map(key => `${key}=${filters[key]}`);
            queryParts.push(...filterParams);
        }

        if (queryParts.length > 0) {
            url += `?${queryParts.join('&')}`;
        }

        console.log('url:' + url);
        
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

    updateAppointmentStatus(id: number, status: AppointmentStatus): Observable<any> {
        const headers = this.GetAuthHeaderJson();

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

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageAppointmentList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageAppointmentList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageAppointmentList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermAppointmentList', searchTerm);
    }

    getLocalSearchTerm(): string {                                                 
        return localStorage.getItem('searchTermAppointmentList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermAppointmentList');
    }
}
