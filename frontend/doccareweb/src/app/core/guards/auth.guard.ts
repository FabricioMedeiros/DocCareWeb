import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { inject } from '@angular/core';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const localStorageUtils = inject(LocalStorageUtils);
  const router = inject(Router);
  const token = localStorageUtils.getTokenUser();

  if (token) {
    return true;
  } else {
    router.navigate(['/account/login']);
    return false;
  }
};
