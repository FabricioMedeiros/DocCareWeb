import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { BaseService } from "src/app/core/services/base.service";
import { Service } from "../../features/service/models/service";


@Injectable()
export class ServiceService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAll(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        let url = `${this.UrlServiceV1}service`;

        const queryParts: string[] = [];

        if (page !== undefined && pageSize !== undefined) {
            queryParts.push(`pageNumber=${page}`, `pageSize=${pageSize}`);
        }

        if (field && value) {
            queryParts.push(`${field}=${encodeURIComponent(value)}`);
        }

        if (queryParts.length > 0) {
            url += `?${queryParts.join('&')}`;
        }

        return this.http
            .get<any>(url, headers)
            .pipe(catchError(super.serviceError));
    }

    getServiceById(id: number): Observable<Service> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<Service>(`${this.UrlServiceV1}service/${id}`, headers)
            .pipe(catchError(super.serviceError));
    }

    registerService(service: Service): Observable<Service> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<Service>(`${this.UrlServiceV1}service`, service, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateService(service: Service): Observable<Service> {
        const headers = this.GetAuthHeaderJson();
        const httpOptions = {
            headers: headers
        };

        return this.http
            .put<Service>(`${this.UrlServiceV1}service/${service.id}`, service, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    delete(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}service/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageServiceList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageServiceList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageServiceList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermServiceList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermServiceList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermServiceList');
    }
}
