import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, map, catchError } from "rxjs";

import { BaseService } from "src/app/core/services/base.service";
import { User } from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  registerUser(user: User): Observable<User> {
    return this.http
      .post(this.UrlServiceV1 + 'auth/register', user, this.getHeaderJson())
      .pipe(
        map((res) => this.extractData(res)),
        catchError((err) => this.serviceError(err))
      );
  }

  login(user: User): Observable<User> {
    return this.http
      .post(this.UrlServiceV1 + 'auth/login', user, this.getHeaderJson())
      .pipe(
        map((res) => this.extractData(res)),
        catchError((err) => this.serviceError(err))
      );
  }
}
