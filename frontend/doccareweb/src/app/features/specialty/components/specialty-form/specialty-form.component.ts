import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Specialty } from '../../models/specialty';
import { SpecialtyService } from '../../../../shared/services/specialty.service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';

@Component({
  selector: 'app-specialty-form',
  templateUrl: './specialty-form.component.html',
  styleUrls: ['./specialty-form.component.css']
})
export class SpecialtyFormComponent extends BaseFormComponent<Specialty> implements OnInit {

  constructor(
    protected override fb: FormBuilder,
    private specialtyService: SpecialtyService,
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

    const resolvedData = this.route.snapshot.data['specialty']; 

    if (resolvedData) {
      this.initializeForm(resolvedData);  
    }
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]],
    });
  }

  saveSpecialty() {
    if (this.form.dirty && this.form.valid) {
      this.entity = Object.assign({}, this.entity, this.form.value);

      if (this.isEditMode) {
        this.specialtyService.updateSpecialty(this.entity).subscribe({
          next: () => {
            this.processSuccess('Especialidade alterada com sucesso!', '/specialty/list');
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      } else {
        this.specialtyService.registerSpecialty(this.entity).subscribe({
          next: () => {
            this.processSuccess('Especialidade cadastrada com sucesso!', '/specialty/list');
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }

    this.changesSaved = true;
  }
}