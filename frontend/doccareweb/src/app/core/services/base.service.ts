import { HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { LocalStorageUtils } from 'src/app/core/utils/localstorage'; 
import { environment } from 'src/environments/environment';

export abstract class BaseService {
  public LocalStorage = new LocalStorageUtils();
  protected UrlServiceV1: string = environment.apiUrlv1;

  protected getHeaderJson() {
    return {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
  }

  protected getAuthHeaderJson() {
    return {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.LocalStorage.getTokenUser()}`
      })
    };
  }

  protected buildQueryParams(
    filters?: { [key: string]: string },
    page?: number,
    pageSize?: number
  ): string {
    const parts: string[] = [];

    if (page !== undefined && pageSize !== undefined) {
      parts.push(`pageNumber=${page}`, `pageSize=${pageSize}`);
    }

    if (filters) {
      parts.push(...Object.entries(filters).map(([key, value]) =>
        `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
      ));
    }

    return parts.length > 0 ? `?${parts.join('&')}` : '';
  }

  protected extractData(response: any) {
    if (response === null || response === undefined) return {};
    return response.data !== undefined ? response.data : response;
  }

  protected serviceError(response: HttpErrorResponse | any) {
    let customError: any[] = [];
    let customResponse = { error: { errors: [] as any[] } };

    if (response instanceof HttpErrorResponse) {
      if (response.statusText === 'Unknown Error') {
        customError.push('Ocorreu um erro desconhecido');
        response.error.errors = customError;
      }
    }

    if (response?.status === 500) {
      customError.push('Ocorreu um erro no processamento, tente novamente mais tarde ou contate o nosso suporte.');
      customResponse.error.errors = customError;
      return throwError(customResponse);
    }

    console.error(response);
    return throwError(response);
  }

  protected localKey(entityName: string, suffix: string) {
    return `${entityName}_${suffix}`;
  }

  protected saveLocal(entityName: string, suffix: string, value: string) {
    localStorage.setItem(this.localKey(entityName, suffix), value);
  }

  protected getLocal(entityName: string, suffix: string): string | null {
    return localStorage.getItem(this.localKey(entityName, suffix));
  }

  protected clearLocal(entityName: string, suffix: string) {
    localStorage.removeItem(this.localKey(entityName, suffix));
  }
}