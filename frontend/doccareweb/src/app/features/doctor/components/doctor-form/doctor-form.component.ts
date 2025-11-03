import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { Doctor } from '../../models/doctor';
import { Specialty } from '../../models/specialty';
import { DoctorService } from 'src/app/shared/services/doctor.service';
import { SpecialtyService } from '../../../../shared/services/specialty.service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';


@Component({
  selector: 'app-doctor-form',
  templateUrl: './doctor-form.component.html',
  styleUrls: ['./doctor-form.component.css']
})
export class DoctorFormComponent extends BaseFormComponent<Doctor> implements OnInit {
  specialties: Specialty[] = []; 
  
  constructor(
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private specialtyService: SpecialtyService,
    private doctorService: DoctorService,
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      name: { required: 'Informe o nome' },
      crm: { required: 'Informe o CRM' },
      email: { required: 'Informe o e-mail', email: 'E-mail inválido' },
      specialtyId: { required: 'Selecione a especialidade' },
      phone: { required: 'Informe o telefone fixo' },
      cellPhone: { required: 'Informe o celular' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();

    const resolvedData = this.route.snapshot.data['doctor'];

    if (resolvedData) {
      this.initializeForm(this.initializeForm({
        data: {
          ...resolvedData?.data,
          specialtyId: resolvedData.data.specialty.id
        }
      }));
    }

    this.loadSpecialties();
  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]],
      crm: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      specialtyId: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      cellPhone: ['', [Validators.required]],
    });
  }

  loadSpecialties(): void {
    this.specialtyService.getAll().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.specialties = response.data.items;
        } else {
          this.specialties = [];
        }
      },
      error: (err) => {
        this.errors.push('Erro ao carregar especialidades');
        console.error('Erro ao carregar especialidades', err);
      }
    });
  }

  saveDoctor() {
    if (this.form.dirty && this.form.valid) {
      this.entity = Object.assign({}, this.form.value);

      const request = this.doctorService.save(this.entity);

      request.subscribe({
        next: () => {
          const msg = this.isEditMode
            ? 'Médico atualizado com sucesso!'
            : 'Médico cadastrado com sucesso!';
          this.processSuccess(msg, '/doctor/list');
        },
        error: err => this.processFail(err)
      });

      this.changesSaved = true;
    }
  }
}