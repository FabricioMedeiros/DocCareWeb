import { Component, ElementRef, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, fromEvent, map, merge, Observable } from 'rxjs';

import { ToastrService, ToastrModule } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { DisplayMessage, GenericValidator, ValidationMessages } from 'src/app/core/validators/generic-form-validation';
import { HealthPlanService } from 'src/app/shared/services/healthplan.service';
import { DoctorService } from 'src/app/shared/services/doctor.service';
import { PatientService } from 'src/app/shared/services/patient.service';
import { AppointmentService } from 'src/app/shared/services/appointment.service';
import { Doctor } from '../../models/doctor';
import { HealthPlan } from '../../models/healthplan';
import { Patient } from '../../models/patient';
import { AppointmentStatus } from '../../models/appointment-status';

@Component({
  selector: 'app-appointment-form',
  templateUrl: './appointment-form.component.html',
  styleUrls: ['./appointment-form.component.css']
})
export class AppointmentFormComponent {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements: ElementRef[] = [];

  errors: any[] = [];
  appointmentForm!: FormGroup;
  doctors: Doctor[] = [];
  patients: Patient[] = [];
  healthPlans: HealthPlan[] = [];
  validationMessages!: ValidationMessages;
  genericValidator!: GenericValidator;
  displayMessage: DisplayMessage = {};

  changesSaved: boolean = true;
  isEditMode: boolean = false;
  currentTab: string = 'appointment-data';
  selectedPatient: Patient = { id: 0, name: '', cpf: '', phone: '', cellPhone: '', healthPlan: { id: 0, description: '', cost: 0 } };
  selectedCost: number = 0;
  currentStatus: AppointmentStatus = 0;

  statusOptions = [
    { value: AppointmentStatus.Scheduled, label: 'Agendado' },
    { value: AppointmentStatus.Confirmed, label: 'Confirmado' },
    { value: AppointmentStatus.Canceled, label: 'Cancelado' },
    { value: AppointmentStatus.Completed, label: 'Concluído' }
  ];

  constructor(
    private fb: FormBuilder,
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private healthPlanService: HealthPlanService,
    private router: Router,
    private route: ActivatedRoute,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.validationMessages = {
      appointmentDate: { required: 'Informe a data' },
      appointmentTime: { required: 'Informe a hora' },
      doctorId: { required: 'Informe o médico' },
      patientId: { required: 'Informe o paciente' },
      healthPlanId: { required: 'Selecione o plano de saúde' },
      cost: { required: 'Informe o valor da consulta' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.appointmentForm = this.fb.group({
      id: ['', []],
      status: [{ value: 0, disabled: !this.isEditMode }, []],
      appointmentDate: ['', [Validators.required]],
      appointmentTime: ['', [Validators.required]],
      doctorId: ['', [Validators.required]],
      patientId: ['', [Validators.required]],
      cpf: [{ value: '', disabled: true }],
      phone: [{ value: '', disabled: true }],
      cellPhone: [{ value: '', disabled: true }],
      healthPlanId: ['', [Validators.required]],
      cost: ['0', [Validators.required]],
      notes: ['']
    });

    this.setStatusOptions();

    const storedDate = this.appointmentService.getLocalCurrentDateList();
    const storedDoctor = this.appointmentService.getLocalCurrentDoctorList();

    if (storedDate) {
      const parsedDate = new Date(storedDate);
      const formattedDate = parsedDate.toISOString().split('T')[0];
      this.appointmentForm.get('appointmentDate')?.setValue(formattedDate);
    }

    if (storedDoctor) {
      this.appointmentForm.get('doctorId')?.setValue(storedDoctor);
    }

    const resolvedData = this.route.snapshot.data['appointment'];

    this.spinner.show();

    if (resolvedData) {
      this.isEditMode = true;
      this.appointmentForm.patchValue({
        ...resolvedData.data,
        doctorId: resolvedData.data.doctor.id,
        patientId: resolvedData.data.patient.id,
        healthPlanId: resolvedData.data.healthPlan.id,
        cpf: resolvedData.data.patient.cpf,
        phone: resolvedData.data.patient.phone,
        cellPhone: resolvedData.data.patient.cellPhone
      });

      this.currentStatus = this.appointmentForm.get('status')?.value;

      this.setStatusOptions();

      if (this.currentStatus === AppointmentStatus.Confirmed) {
        this.appointmentForm.get('appointmentDate')?.disable();
        this.appointmentForm.get('appointmentTime')?.disable();
      }

      if (this.currentStatus === AppointmentStatus.Canceled || this.currentStatus === AppointmentStatus.Completed) {
        this.appointmentForm.disable();
        this.appointmentForm.get('notes')?.enable(); 
      } else {
        this.appointmentForm.get('status')?.enable();
      }
    }

    forkJoin([this.loadDoctors(), this.loadPatients(), this.loadHealthPlans()]).subscribe(() => {
      if (this.isEditMode) {
        this.updatePatientDetails(this.appointmentForm.get('patientId')?.value);
      }
    });

    this.appointmentForm.get('patientId')?.valueChanges.subscribe(patientId => {
      this.updatePatientDetails(patientId);
    });

    this.appointmentForm.get('healthPlanId')?.valueChanges.subscribe(healthPlanId => {
      const selectedHealthPlan = this.healthPlans.find(hp => hp.id === +healthPlanId);
      this.selectedCost = selectedHealthPlan ? selectedHealthPlan.cost : 0;
      this.appointmentForm.get('cost')?.setValue(this.selectedCost);
    });

    this.spinner.hide();
  }

  updatePatientDetails(patientId: number): void {
    const selectedPatient = this.patients.find(p => p.id === +patientId);
    if (selectedPatient) {
      this.appointmentForm.patchValue({
        cpf: selectedPatient.cpf,
        phone: selectedPatient.phone,
        cellPhone: selectedPatient.cellPhone,
        healthPlanId: selectedPatient.healthPlan.id,
        cost: selectedPatient.healthPlan.cost
      });
    }
    else {
      this.appointmentForm.patchValue({
        cpf: '',
        phone: '',
        cellPhone: '',
        healthPlanId: null,
        cost: ''
      });
    }
  }

  ngAfterViewInit(): void {
    let controlBlurs: Observable<any>[] = this.formInputElements
      .map((formControl: ElementRef) => fromEvent(formControl.nativeElement, 'blur'));

    merge(...controlBlurs).subscribe(() => {
      this.displayMessage = this.genericValidator.processMessages(this.appointmentForm);
      this.changesSaved = false;
    });
  }

  setCostBasedOnHealthPlan(healthPlanId: number): void {
    const selectedPlan = this.healthPlans.find(h => h.id === healthPlanId);

    if (selectedPlan) {
      this.appointmentForm.get('cost')?.setValue(selectedPlan.cost || '');
    }
  }

  setTab(tab: string): void {
    this.currentTab = tab;
  }

 
  loadDoctors(): Observable<void> {
    return this.doctorService.getAllDoctors().pipe(map(
      response => {
        this.doctors = response?.data?.items || [];
      }));
  }

  loadPatients(): Observable<void> {
    return this.patientService.getAllPatients().pipe(map(
      response => {
        this.patients = response?.data?.items || [];
      }));
  }

  loadHealthPlans(): Observable<void> {
    return this.healthPlanService.getAllHealthPlans().pipe(map(
      response => {
        this.healthPlans = response?.data?.items || [];
      }));
  }

  saveAppointment() {
    if (this.appointmentForm.dirty && this.appointmentForm.valid) {
      const appointmentData = Object.assign({}, this.appointmentForm.getRawValue());

      if (this.isEditMode) {
        const newStatus = appointmentData.status;

        if ((this.currentStatus !== newStatus) && (!this.canChangeStatus(this.currentStatus, newStatus))) {
          this.toastr.error('Mudança de status não permitida.', 'Atenção');
          return;
        }

        this.appointmentService.updateAppointment(appointmentData).subscribe({
          next: (appointmentSuccess) => {
            this.processSuccess(appointmentSuccess);

            if (this.currentStatus !== newStatus) {
              this.appointmentService.updateAppointmentStatus(appointmentData.id, appointmentData.status).subscribe({
                next: (statusSuccess) => {
                  this.processSuccess(statusSuccess);
                },
                error: (error) => {
                  this.processFail(error);
                }
              });
            }
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      } else {
        this.appointmentService.registerAppointment(appointmentData).subscribe({
          next: (success) => {
            this.processSuccess(success);
          },
          error: (error) => {
            this.processFail(error);
          }
        });
      }
    }

    this.changesSaved = true;
  }

  processSuccess(success: any) {
    this.appointmentForm.reset();
    this.errors = [];

    let toast = this.toastr.success(this.isEditMode ? 'Agendamento alterado com sucesso!' : 'Agendamento cadastrado com sucesso!', 'Atenção!');

    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/appointment/list']);
      });
    }
  }

  processFail(fail: any) {
    this.errors = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  cancel() {
    this.router.navigate(['/appointment/list']);
  }

  private setStatusOptions(): void {
    if (!this.isEditMode) {
      this.appointmentForm.get('status')?.setValue(AppointmentStatus.Scheduled);
      this.appointmentForm.get('status')?.disable();
    } else {
      const statusControl = this.appointmentForm.get('status');
      statusControl?.enable();

      switch (this.currentStatus) {
        case AppointmentStatus.Scheduled:
          this.statusOptions = [
            { value: AppointmentStatus.Scheduled, label: 'Agendado' },
            { value: AppointmentStatus.Confirmed, label: 'Confirmado' },
            { value: AppointmentStatus.Canceled, label: 'Cancelado' },
            { value: AppointmentStatus.Completed, label: 'Concluído' }
          ];
          break;
        case AppointmentStatus.Confirmed:
          this.statusOptions = [
            { value: AppointmentStatus.Confirmed, label: 'Confirmado' },
            { value: AppointmentStatus.Canceled, label: 'Cancelado' },
            { value: AppointmentStatus.Completed, label: 'Concluído' }
          ];
          break;
        case AppointmentStatus.Canceled:
        case AppointmentStatus.Completed:
          statusControl?.disable();
          break;
      }
    }
  }

  canChangeStatus(currentStatus: AppointmentStatus, newStatus: AppointmentStatus): boolean {
    return (currentStatus === AppointmentStatus.Scheduled &&
      (newStatus === AppointmentStatus.Confirmed ||
        newStatus === AppointmentStatus.Canceled ||
        newStatus === AppointmentStatus.Completed)) ||
      (currentStatus === AppointmentStatus.Confirmed &&
        (newStatus === AppointmentStatus.Canceled ||
          newStatus === AppointmentStatus.Completed));
  }
}


