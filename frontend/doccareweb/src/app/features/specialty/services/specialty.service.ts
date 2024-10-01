import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from "src/app/services/base.service";
import { Specialty } from "../models/specialty";


@Injectable()
export class SpecialtyService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAllSpecialties(page: number, pageSize: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}specialty?pageNumber=${page}&pageSize=${pageSize}`;

        if (field && value) {
            url += `&filters[${field}]=${value}`;
        }
    
        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }    

    getSpecialtyById(id: number): Observable<Specialty> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Specialty>(`${this.UrlServiceV1}specialty/${id}`,  headers)
            .pipe(catchError(super.serviceError));
    }

    registerSpecialty(specialty: Specialty): Observable<Specialty> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Specialty>(`${this.UrlServiceV1}specialty`, specialty, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateSpecialty(specialty: Specialty): Observable<Specialty> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .put<Specialty>(`${this.UrlServiceV1}specialty`, specialty, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    deleteSpecialty(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}specialty/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageSpecialtyList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageSpecialtyList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageSpecialtyList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermSpecialtyList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermSpecialtyList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermSpecialtyList');
    }
}