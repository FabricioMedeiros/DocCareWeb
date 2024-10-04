import { CanDeactivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { SpecialtyFormComponent } from '../components/specialty-form/specialty-form.component';

export const canDeactivateSpecialtyForm: CanDeactivateFn<SpecialtyFormComponent> = (component: SpecialtyFormComponent, route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  if (!component.changesSaved) {
    return window.confirm('Tem certeza que deseja abandonar o preenchimento do cadastro?');
  }
  return true;
};
