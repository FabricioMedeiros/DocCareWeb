import { Address } from './../../models/address';
import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fromEvent, merge, Observable } from 'rxjs';
import { FormCanDeactivate } from 'src/app/core/guards/form-can-deactivate.interface';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/core/validators/generic-form-validation';
import { PatientService } from './../../../../shared/services/patient.service';
import { HealthPlanService } from 'src/app/shared/services/healthplan.service';
import { HealthPlan } from '../../models/healthplan';
import { SearchZipCode } from '../../models/address';
import { Gender } from '../../models/gender.enum';
import { Patient } from '../../models/patient';
import { DateUtils } from 'src/app/core/utils/date-utils';

@Component({
  selector: 'app-patient-form',
  templateUrl: './patient-form.component.html',
  styleUrls: ['./patient-form.component.css']
})
export class PatientFormComponent implements OnInit, AfterViewInit, FormCanDeactivate {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  errors: any[] = [];
  patientForm!: FormGroup;
  patient!: Patient;
  healthPlans: HealthPlan[] = [];
  Gender = Gender;

  validationMessages!: ValidationMessages;
  genericValidator!: GenericValidator;
  displayMessage: DisplayMessage = {};

  currentTab: string = 'personal-data';
  changesSaved: boolean = true;
  isEditMode: boolean = false;

  constructor(
    private fb: FormBuilder,
    private healthPlanService: HealthPlanService,
    private patientService: PatientService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.validationMessages = {
      name: { required: 'Informe o nome' },
      birthDate: { required: 'Informe a data de nascimento' },
      cpf: { required: 'Informe o CPF' },
      rg: { required: 'Informe o RG' },
      gender: { required: 'Informe o sexo' },
      phone: { required: 'Informe o telefone fixo' },
      cellPhone: { required: 'Informe o celular' },
      email: { required: 'Informe o e-mail', email: 'E-mail inválido' },
      healthPlanId: { required: 'Selecione o Plano de Saúde' },
      street: { required: 'Informe o logradouro' },
      number: { required: 'Informe o número' },
      neighborhood: { required: 'Informe o bairro' },
      city: { required: 'Informe a cidade' },
      state: { required: 'Informe o estado' },
      zipCode: { required: 'Informe o cep' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.patientForm = this.fb.group({
      id: ['', []],
      name: ['', [Validators.required]],
      gender: ['', [Validators.required]],
      birthDate: ['', [Validators.required]],
      cpf: ['', [Validators.required]],
      rg: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      healthPlanId: ['', [Validators.required]],
      phone: ['', [Validators.required]],
      cellPhone: ['', [Validators.required]],
      notes: [''],

      address: this.fb.group({
        id: ['', []],
        street: ['', [Validators.required]],
        number: ['', [Validators.required]],
        complement: [''],
        neighborhood: ['', [Validators.required]],
        city: ['', [Validators.required]],
        state: ['', [Validators.required]],
        zipCode: ['', [Validators.required]]
      })
    });

    const resolvedData = this.route.snapshot.data['patient'];   

    if (resolvedData) {
      this.spinner.show();
      this.isEditMode = true;
      this.patientForm.patchValue({
        ...resolvedData.data, 
        healthPlanId: resolvedData.data.healthPlan.id,
        birthDate: DateUtils.formatDateToDDMMYYYY(resolvedData.data.birthDate)
      });
      this.spinner.hide();
    }

    this.loadHealthPlans();
  }

  ngAfterViewInit(): void {
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.patientForm);
      this.changesSaved = false;
    });
  }

  setTab(tab: string): void {
    this.currentTab = tab;
  }

  loadHealthPlans(): void {
    this.healthPlanService.getAllHealthPlans().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.healthPlans = response.data.items;
        } else {
          this.healthPlans = [];
        }
      },
      error: (err) => {
        this.errors.push('Erro ao carregar planos de saúde');
        console.error('Erro ao carregar planos de saúde', err);
      }
    });
  }

  savePatient() {
    if (this.patientForm.dirty && this.patientForm.valid) {
      this.changesSaved = true; 
      
      const patientData = Object.assign({}, this.patientForm.value);
 
      if (this.isEditMode) {
        this.patientService.updatePatient(patientData).subscribe({
          next: (success) => this.processSuccess(success),
          error: (error) => this.processFail(error)
        });
      } else {
        this.patientService.registerPatient(patientData).subscribe({
          next: (success) => this.processSuccess(success),
          error: (error) => this.processFail(error)
        });
      }
    }
  }  

  processSuccess(success: any) {
    this.patientForm.reset();
    this.errors = [];
    let toast = this.toastr.success(this.isEditMode ? 'Paciente alterado com sucesso!' : 'Paciente cadastrado com sucesso!', 'Atenção!');
    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/patient/list']);
      });
    }
  }

  processFail(fail: any) {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  cancel() {
    this.router.navigate(['/patient/list']);
  }

  searchZipCode(zipCode: string | null) {
    if (!zipCode || zipCode.length < 8) return;

    this.patientService.searchZipCode(zipCode).subscribe({
      next: (zipCodeReturned) => {
        this.fillAddressFromZipCode(zipCodeReturned);
      },
      error: (error) => {
        this.processFail(error);
      }
    });
  }

  fillAddressFromZipCode(searchZipCode: SearchZipCode) {
    this.patientForm.patchValue({
      address: {
        street: searchZipCode.logradouro,
        neighborhood: searchZipCode.bairro,
        city: searchZipCode.localidade,
        state: searchZipCode.uf
      }
    });
  }
}
