import { CanActivateFn, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { inject } from '@angular/core';
import { NavigationService } from '../services/navigation.service';

export const navigationGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  const navigationService = inject(NavigationService);
  const router = inject(Router);

  if (navigationService.isNavigationAllowed()) {
    navigationService.resetNavigation();
    return true;
  } else {
    router.navigate(['/']);
    return false;
  }
};



