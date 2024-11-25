import { ToastrService } from 'ngx-toastr';
import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { Router } from '@angular/router';

import { NavigationService } from '../../core/services/navigation.service';
import { LocalStorageUtils } from '../utils/localstorage';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(
        private router: Router,
        private navigationService: NavigationService,
        private toastr: ToastrService
    ) { }

    localStorageUtil = new LocalStorageUtils();

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 0) {
                    this.navigationService.allowNavigation();
                    this.router.navigate(['/service-unavailable']);
                } else if (error.status === 401) {
                    this.localStorageUtil.clearLocalUserData();

                    this.toastr.clear();
                    this.toastr.warning('Sessão encerrada. Faça login novamente.', 'Atenção');
                    this.router.navigate(['/account/login'], { queryParams: { returnUrl: this.router.url } });

                    return throwError(() => new HttpErrorResponse({
                        error: { message: 'Sessão encerrada.' },
                        status: 401,
                        statusText: 'Unauthorized'
                    }));
                } else {
                    this.navigationService.allowNavigation();
                }

                return throwError(() => error);
            })
        );
    }
}
