import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, map, Observable } from "rxjs";

import { BaseService } from "src/app/core/services/base.service";
import { Patient } from "src/app/features/patient/models/patient";
import { SearchZipCode } from "src/app/features/patient/models/address";

@Injectable()
export class PatientService extends BaseService {
    constructor(private http: HttpClient) { super(); }

    getAllPatients(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}patient`;
    
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

    getPatientById(id: number): Observable<Patient> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Patient>(`${this.UrlServiceV1}patient/${id}`, headers)
            .pipe(catchError(super.serviceError));
    }

    registerPatient(patient: Patient): Observable<Patient> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Patient>(`${this.UrlServiceV1}Patient`, patient, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updatePatient(patient: Patient): Observable<Patient> {
        const headers = this.GetAuthHeaderJson();
        
        return this.http
            .put<Patient>(`${this.UrlServiceV1}Patient/${patient.id}`, patient, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }
    
    deletePatient(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}Patient/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    searchZipCode(zipCode: string): Observable<SearchZipCode> {
        return this.http
            .get<SearchZipCode>(`https://viacep.com.br/ws/${zipCode}/json/`)
            .pipe(catchError(super.serviceError))
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPagePatientList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPagePatientList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPagePatientList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermPatientList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermPatientList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermPatientList');
    }
}