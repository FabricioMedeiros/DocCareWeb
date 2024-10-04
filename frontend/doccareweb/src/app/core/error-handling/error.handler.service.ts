import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { Router } from '@angular/router';

import { NavigationService } from '../../core/services/navigation.service';
import { LocalStorageUtils } from '../../utils/localstorage';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router, private navigationService: NavigationService) { }

    localStorageUtil = new LocalStorageUtils();

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(catchError(error => {
            if (error instanceof HttpErrorResponse) {

                if (error.status === 0) {
                    this.navigationService.allowNavigation();
                    this.router.navigate(['/service-unavailable']);
                } else if (error.status === 401) {
                    this.localStorageUtil.clearLocalUserData();
                    this.router.navigate(['/account/login'], { queryParams: { returnUrl: this.router.url } });
                }

                return throwError(() => error);
            }

            return throwError(() => error);
        }));
    }
}
