import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { catchError, Observable } from "rxjs";
import { BaseService } from "src/app/core/services/base.service";

@Injectable()
export class DashBoardService extends BaseService {
    constructor(private http: HttpClient) { super(); }

    getDashboardData(doctorId?: string, startDate?: string, endDate?: string): Observable<any> {
        const headers = this.GetAuthHeaderJson();
        
        let url = `${this.UrlServiceV1}dashboard`;
        
        const params: string[] = [];
        if (doctorId) {
          params.push(`doctorId=${doctorId}`);
        }
        if (startDate) {
          params.push(`startDate=${startDate}`);
        }
        if (endDate) {
          params.push(`endDate=${endDate}`);
        }
        
        if (params.length) {
          url += '?' + params.join('&');
        }
        
        return this.http
          .get<any>(url, headers)
          .pipe(catchError(this.serviceError));
      }      
}


