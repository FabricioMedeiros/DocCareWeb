import { Doctor } from './../../features/doctor/models/doctor';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from "src/app/core/services/base.service";

@Injectable()
export class DoctorService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAllDoctors(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}doctor`;
    
        if (page !== undefined && pageSize !== undefined) {
            url += `?pageNumber=${page}&pageSize=${pageSize}`;
        }
    
        if (field && value) {
            url += `${(page !== undefined && pageSize !== undefined) ? '&' : '?'}filters[${field}]=${value}`;
        }
    
        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }
    
    getDoctorById(id: number): Observable<Doctor> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Doctor>(`${this.UrlServiceV1}doctor/${id}`, headers)
            .pipe(catchError(super.serviceError));
    }

    registerDoctor(doctor: Doctor): Observable<Doctor> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Doctor>(`${this.UrlServiceV1}doctor`, doctor, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateDoctor(doctor: Doctor): Observable<Doctor> {
        const headers = this.GetAuthHeaderJson();
        
        return this.http
            .put<Doctor>(`${this.UrlServiceV1}doctor/${doctor.id}`, doctor, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }
    
    deleteDoctor(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}doctor/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageDoctorList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageDoctorList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageDoctorList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermDoctorList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermDoctorList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermDoctorList');
    }
}
