import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { BaseService } from "src/app/core/services/base.service";
import { HealthPlan } from "../../features/healthplan/models/healthplan";

@Injectable()
export class HealthPlanService extends BaseService {

    constructor(private http: HttpClient) { super(); }

    getAllHealthPlans(page?: number, pageSize?: number, field?: string, value?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}healthplan`;
    
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

    getHealthPlanById(id: number): Observable<HealthPlan> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .get<HealthPlan>(`${this.UrlServiceV1}healthplan/${id}`,  headers)
            .pipe(catchError(super.serviceError));
    }

    registerHealthPlan(healthPlan: HealthPlan): Observable<HealthPlan> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .post<HealthPlan>(`${this.UrlServiceV1}healthplan`, healthPlan, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }

    updateHealthPlan(healthPlan: HealthPlan): Observable<HealthPlan> {
        const headers = this.GetAuthHeaderJson();
        const httpOptions = {
            headers: headers
        };
    
        return this.http
            .put<HealthPlan>(`${this.UrlServiceV1}healthplan/${healthPlan.id}`, healthPlan, headers)
            .pipe(
                map(this.extractData),
                catchError(this.serviceError)
            );
    }
    
    deleteHealthPlan(id: number): Observable<any> {
        const headers = this.GetAuthHeaderJson();

        return this.http
            .delete(`${this.UrlServiceV1}healthplan/${id}`, headers)
            .pipe(catchError(this.serviceError));
    }

    saveLocalCurrentPageList(page: number): void {
        localStorage.setItem('currentPageHealthPlanList', page.toString());
    }

    getLocalCurrentPageList(): string {
        return localStorage.getItem('currentPageHealthPlanList') || '';
    }

    clearLocalCurrentPageList(): void {
        localStorage.removeItem('currentPageHealthPlanList');
    }

    saveLocalSearchTerm(searchTerm: string): void {
        localStorage.setItem('searchTermHealthPlanList', searchTerm);
    }

    getLocalSearchTerm(): string {
        return localStorage.getItem('searchTermHealthPlanList') || '';
    }

    clearLocalSearchTerm(): void {
        localStorage.removeItem('searchTermHealthPlanList');
    }
}
