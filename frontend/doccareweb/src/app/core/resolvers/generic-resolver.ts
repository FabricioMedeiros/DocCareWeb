import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { inject, ProviderToken } from '@angular/core';
import { Observable } from 'rxjs';

export const genericResolver = <T>(
  serviceType: ProviderToken<any>,  
  resolveMethod: (service: any, id: string) => Observable<T> 
): ResolveFn<T> => {
  return (route: ActivatedRouteSnapshot) => {
    const service = inject(serviceType);  
    const id = route.params['id']; 
    
    return resolveMethod(service, id); 
  };
};
