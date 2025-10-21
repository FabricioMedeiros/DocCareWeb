import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { HealthPlan } from '../../models/healthplan';
import { HealthPlanService } from 'src/app/shared/services/healthplan.service';
import { ServiceService } from 'src/app/shared/services/service.service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { ServiceItem } from 'src/app/shared/models/service-item';
import { Service } from 'src/app/features/service/models/service';

@Component({
  selector: 'app-health-plan-form',
  templateUrl: './healthplan-form.component.html',
  styleUrls: ['./healthplan-form.component.css']
})
export class HealthPlanFormComponent extends BaseFormComponent<HealthPlan> implements OnInit {
  serviceItems: ServiceItem[] = [];
  allServices: Service[] = [];

  currentTab: string = 'info';

  constructor(
    protected override fb: FormBuilder,
    private healthPlanService: HealthPlanService,
    private serviceService: ServiceService,
    protected override router: Router,
    protected override route: ActivatedRoute,
    protected override toastr: ToastrService,
    protected override spinner: NgxSpinnerService
  ) {
    super(fb, router, route, toastr, spinner);
    this.validationMessages = {
      name: {
        required: 'Informe o nome do plano'
      }
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    this.serviceService.getAll().subscribe({
      next: response => {
        this.allServices = response.data.items;
      },
      error: err => console.error('Erro ao carregar serviços:', err)
    });

    const resolvedResponse = this.route.snapshot.data['healthPlan'];
    const resolvedData = resolvedResponse?.data; 

    if (resolvedData) {
      this.initializeForm(resolvedData);  
      
      this.form.patchValue({
        id: resolvedData.id,
        name: resolvedData.name
      });

      this.serviceItems = [...(resolvedData.items ?? [])];
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]]
    });
  }

  saveHealthPlan(): void {
    if (this.form.dirty && this.form.valid) {
      this.entity = {
        ...this.form.value,
        items: this.serviceItems
      };

      const request = this.isEditMode
        ? this.healthPlanService.updateHealthPlan(this.entity)
        : this.healthPlanService.registerHealthPlan(this.entity);

      request.subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Plano de saúde alterado com sucesso!'
            : 'Plano de saúde cadastrado com sucesso!';
          this.processSuccess(msg, '/healthplan/list');
        },
        error: err => this.processFail(err)
      });

      this.changesSaved = true;
    }
  }

  onItemsChanged(updatedItems: ServiceItem[]): void {
    this.serviceItems  = updatedItems;
    this.form.markAsDirty();
  }

  setTab(tab: string): void {
    this.currentTab = tab;
  }
}