import { CanDeactivateFn, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { FormCanDeactivate } from './form-can-deactivate.interface';

export const canDeactivateForm: CanDeactivateFn<FormCanDeactivate> = (
  component: FormCanDeactivate,
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
) => {
  if (!component.changesSaved) {
    return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulário?');
  }
  return true;
};
