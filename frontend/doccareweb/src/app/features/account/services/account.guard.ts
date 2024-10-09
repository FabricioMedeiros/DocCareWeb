import { CanActivateFn, CanDeactivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { RegisterComponent } from '../components/register/register.component';
import { inject } from '@angular/core';
import { LocalStorageUtils } from 'src/app/core/utils/localstorage';

export const canActivate: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const localStorageUtils = inject(LocalStorageUtils);
  const router = inject(Router);
  const token = localStorageUtils.getTokenUser();

  if (token) {
    const returnUrl = route.queryParams['returnUrl'] || '/home';

    router.navigateByUrl(returnUrl);
    return false;
  }
  return true;
};

export const canDeactivate: CanDeactivateFn<RegisterComponent> = (component: RegisterComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  if (!component.changesSaved) {
    return window.confirm('Tem certeza que deseja abandonar o preenchimento do cadastro?');
  }
  return true;
};
