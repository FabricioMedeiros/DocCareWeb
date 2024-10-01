import { ActivatedRouteSnapshot, ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
import { Specialty } from '../models/specialty';
import { SpecialtyService } from './specialty.service';

export const specialtyResolver: ResolveFn<Specialty> = (route: ActivatedRouteSnapshot) => {
  const specialtyService = inject(SpecialtyService);
  return specialtyService.getSpecialtyById(route.params['id']);
};
