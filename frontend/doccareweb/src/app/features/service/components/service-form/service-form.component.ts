import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Service } from '../../models/service';
import { ServiceService } from '../../../../shared/services/service.service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';

@Component({
  selector: 'app-service-form',
  templateUrl: './service-form.component.html',
  styleUrls: ['./service-form.component.css']
})
export class ServiceFormComponent extends BaseFormComponent<Service> implements OnInit {

  constructor(
    protected override fb: FormBuilder,
    private serviceService: ServiceService,
    protected override router: Router,
    protected override route: ActivatedRoute,
    protected override toastr: ToastrService,
    protected override spinner: NgxSpinnerService
  ) {
    super(fb, router, route, toastr, spinner);
    this.validationMessages = {
      name: {
        required: 'Informe o nome',
      }
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['service'];

    if (resolvedData) {
      const serviceData = (resolvedData as any).data ?? resolvedData;
      const data = {
        ...serviceData,
        basePrice: serviceData?.price?.toString() ?? ''
      };

      this.initializeForm({ data });
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]],
      basePrice: ['0', [Validators.pattern('^[0-9]*\.?[0-9]+$')]]
    });
  }

  saveService() {
    if (this.form.dirty && this.form.valid) {
      this.entity = Object.assign({}, this.entity, this.form.value);

      const request = this.serviceService.save(this.entity);

      request.subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Serviço atualizado com sucesso!'
            : 'Serviço cadastrado com sucesso!';
          this.processSuccess(msg, '/service/list');
        },
        error: err => this.processFail(err)
      });

      this.changesSaved = true;
    }
  }
}