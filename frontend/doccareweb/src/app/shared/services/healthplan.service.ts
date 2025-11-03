import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { GenericCrudService } from 'src/app/core/services/generic-crud.service';
import { HealthPlan } from 'src/app/features/healthplan/models/healthplan';

@Injectable()
export class HealthPlanService extends GenericCrudService<HealthPlan> {
  constructor(protected override http: HttpClient) {
    super(http, 'HealthPlan', 'healthplan');
  }
}