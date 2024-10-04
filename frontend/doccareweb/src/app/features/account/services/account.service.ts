import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { BaseService } from 'src/app/core/services/base.service';
import { User } from '../models/user'; 

@Injectable({
  providedIn: 'root'
})
export class AccountService extends BaseService {

  constructor(private http: HttpClient) {
    super();
  }

  registerUser(user: User) : Observable<User> {
      let response = this.http
          .post(this.UrlServiceV1 + 'auth/register', user, this.GetHeaderJson())
          .pipe(
              map(this.extractData),
              catchError(this.serviceError));

      return response;
  }

  login(user: User) : Observable<User> {
      let response = this.http
          .post(this.UrlServiceV1 + 'auth/login', user, this.GetHeaderJson())
          .pipe(
              map(this.extractData),
              catchError(this.serviceError));

      return response;
  }
}
