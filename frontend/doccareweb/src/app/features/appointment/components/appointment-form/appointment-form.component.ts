import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, map, Observable, Subject, takeUntil, finalize } from 'rxjs';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { AppointmentList } from '../../models/appointment';
import { Patient } from '../../models/patient';
import { Doctor } from '../../models/doctor';
import { HealthPlan } from '../../models/healthplan';
import { AppointmentStatus } from '../../models/appointment-status';
import { AppointmentService } from 'src/app/shared/services/appointment.service';
import { PatientService } from 'src/app/shared/services/patient.service';
import { DoctorService } from 'src/app/shared/services/doctor.service';
import { HealthPlanService } from 'src/app/shared/services/healthplan.service';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';

@Component({
  selector: 'app-appointment-form',
  templateUrl: './appointment-form.component.html',
  styleUrls: ['./appointment-form.component.css']
})
export class AppointmentFormComponent
  extends BaseFormComponent<AppointmentList>
  implements OnInit, AfterViewInit {

  doctors: Doctor[] = [];
  patients: Patient[] = [];
  healthPlans: HealthPlan[] = [];

  currentTab: string = 'appointment-data';
  selectedPatient: Patient = { id: 0, name: '', cpf: '', phone: '', cellPhone: '', healthPlan: { id: 0, description: '', cost: 0 } };
  selectedCost: number = 0;
  currentStatus: AppointmentStatus = 0;

  @ViewChild('startTimeInput') startTimeInput!: ElementRef;
  private readonly destroy$ = new Subject<void>();

  statusOptions: { value: AppointmentStatus; label: string }[] = [];

  private readonly statusLabels: Record<AppointmentStatus, string> = {
    [AppointmentStatus.Scheduled]: 'Agendado',
    [AppointmentStatus.Confirmed]: 'Confirmado',
    [AppointmentStatus.Canceled]: 'Cancelado',
    [AppointmentStatus.Completed]: 'Concluído'
  };

  private readonly statusConfig: Record<number, AppointmentStatus[]> = {
    [AppointmentStatus.Scheduled]: [
      AppointmentStatus.Scheduled,
      AppointmentStatus.Confirmed,
      AppointmentStatus.Canceled,
      AppointmentStatus.Completed
    ],
    [AppointmentStatus.Confirmed]: [
      AppointmentStatus.Confirmed,
      AppointmentStatus.Canceled,
      AppointmentStatus.Completed
    ]
  };

  constructor(
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private readonly appointmentService: AppointmentService,
    private readonly doctorService: DoctorService,
    private readonly patientService: PatientService,
    private readonly healthPlanService: HealthPlanService,
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      appointmentDate: { required: 'Informe a data' },
      startTime: { required: 'Informe a hora inicial' },
      endTime: { required: 'Informe a hora final' },
      doctorId: { required: 'Informe o médico' },
      patientId: { required: 'Informe o paciente' },
      healthPlanId: { required: 'Selecione o plano de saúde' },
      cost: { required: 'Informe o valor da consulta' }
    };

    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
  this.buildForm();
  this.setStatusOptions();

  const storedDate = this.appointmentService.getLocalCurrentDateList();
  const storedDoctor = this.appointmentService.getLocalCurrentDoctorList();

  if (storedDate) {
    const parsedDate = new Date(storedDate);
    const formattedDate = parsedDate.toISOString().split('T')[0];
    this.form.get('appointmentDate')?.setValue(formattedDate);
  }

  if (storedDoctor) {
    this.form.get('doctorId')?.setValue(storedDoctor);
  }

  const resolvedData = this.route.snapshot.data['appointment'];
  this.spinner.show();

  if (resolvedData) {
    this.initializeForm({
      data: {
        ...resolvedData?.data,
        doctorId: resolvedData.data.doctor.id,
        patientId: resolvedData.data.patient.id,
        healthPlanId: resolvedData.data.healthPlan.id,
        cpf: resolvedData.data.patient.cpf,
        phone: resolvedData.data.patient.phone,
        cellPhone: resolvedData.data.patient.cellPhone
      }
    });

    this.currentStatus = this.form.get('status')?.value;
    this.setStatusOptions();

    if (this.currentStatus === AppointmentStatus.Confirmed) {
      this.form.get('appointmentDate')?.disable();
      this.form.get('startTime')?.disable();
      this.form.get('endTime')?.disable();
    }

    if ([AppointmentStatus.Canceled, AppointmentStatus.Completed].includes(this.currentStatus)) {
      this.form.disable();
      this.form.get('notes')?.enable();
    } else {
      this.form.get('status')?.enable();
    }
  }

  forkJoin([this.loadDoctors(), this.loadPatients(), this.loadHealthPlans()])
    .pipe(finalize(() => this.spinner.hide()))
    .subscribe(() => {
      if (this.isEditMode) {
        this.updatePatientDetails(this.form.get('patientId')?.value);
      }

      this.form.get('healthPlanId')?.valueChanges
        .pipe(takeUntil(this.destroy$))
        .subscribe(healthPlanId => {
          const selectedHealthPlan = this.healthPlans.find(hp => hp.id === +healthPlanId);
          if (selectedHealthPlan) {
            this.form.get('cost')?.setValue(selectedHealthPlan.cost);
          }
        });
    });

  this.form.get('patientId')?.valueChanges
    .pipe(takeUntil(this.destroy$))
    .subscribe(patientId => this.updatePatientDetails(patientId));

  this.form.get('startTime')?.valueChanges
    .pipe(takeUntil(this.destroy$))
    .subscribe(startTime => {
      if (startTime && !this.isEditMode) {
        this.form.get('endTime')?.setValue(this.suggestEndTime(startTime), { emitEvent: false });
      }
    });
}

  override ngAfterViewInit(): void {
    super.ngAfterViewInit();
    if (!this.isEditMode && this.startTimeInput) {
      setTimeout(() => this.startTimeInput.nativeElement.focus(), 0);
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();

  }

  buildForm(): void {
    this.form = this.fb.group({
      id: ['', []],
      status: [{ value: 0, disabled: !this.isEditMode }, []],
      appointmentDate: ['', [Validators.required]],
      startTime: ['', [Validators.required]],
      endTime: ['', [Validators.required]],
      doctorId: ['', [Validators.required]],
      patientId: ['', [Validators.required]],
      cpf: [{ value: '', disabled: true }],
      phone: [{ value: '', disabled: true }],
      cellPhone: [{ value: '', disabled: true }],
      healthPlanId: ['', [Validators.required]],
      cost: ['0', [Validators.required]],
      notes: ['']
    });
  }

  updatePatientDetails(patientId: number): void {
  const selectedPatient = this.patients.find(p => p.id === +patientId);
  if (!selectedPatient) return;

  const currentPlanId = this.form.get('healthPlanId')?.value;
  const patientPlanId = selectedPatient.healthPlan?.id ?? null;

  this.form.patchValue({
    cpf: selectedPatient.cpf ?? '',
    phone: selectedPatient.phone ?? '',
    cellPhone: selectedPatient.cellPhone ?? ''
  });

  if (!currentPlanId || currentPlanId === patientPlanId) {
    this.form.get('healthPlanId')?.setValue(patientPlanId);
  }
}

  private suggestEndTime(startTime: string): string {
    const [hours, minutes] = startTime.split(':').map(Number);
    const date = new Date();
    date.setHours(hours, minutes);
    date.setHours(date.getHours() + 1);
    return date.toTimeString().slice(0, 5);
  }

  setCostBasedOnHealthPlan(healthPlanId: number): void {
    const selectedPlan = this.healthPlans.find(h => h.id === healthPlanId);
    if (selectedPlan) {
      this.form.get('cost')?.setValue(selectedPlan.cost || '');
    }
  }

  setTab(tab: string): void {
    this.currentTab = tab;
  }

  loadDoctors(): Observable<void> {
    return this.doctorService.getAll().pipe(map(
      response => { this.doctors = response?.data?.items || []; }
    ));
  }

  loadPatients(): Observable<void> {
    return this.patientService.getAll().pipe(map(
      response => { this.patients = response?.data?.items || []; }
    ));
  }

  loadHealthPlans(): Observable<void> {
    return this.healthPlanService.getAll().pipe(map(
      response => { this.healthPlans = response?.data?.items || []; }
    ));
  }

  saveAppointment() {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;
      const appointmentData = { ...this.form.getRawValue() };
      
      if (this.isEditMode) {
        const newStatus = appointmentData.status;

        if ((this.currentStatus !== newStatus) && (!this.canChangeStatus(this.currentStatus, newStatus))) {
          this.toastr.error('Mudança de status não permitida.', 'Atenção');
          return;
        }

        this.appointmentService.updateAppointment(appointmentData).subscribe({
          next: () => {
            if (this.currentStatus !== newStatus) {
              this.appointmentService.updateAppointmentStatus(appointmentData.id, appointmentData.status).subscribe({
                next: () => this.processSuccess('Agendamento atualizado com sucesso!', '/appointment/list'),
                error: (error) => this.processFail(error)
              });
            } else {
              this.processSuccess('Agendamento atualizado com sucesso!', '/appointment/list');
            }
          },
          error: (error) => this.processFail(error)
        });
      } else {
        this.appointmentService.registerAppointment(appointmentData).subscribe({
          next: () => this.processSuccess('Agendamento cadastrado com sucesso!', '/appointment/list'),
          error: (error) => this.processFail(error)
        });
      }
    }
  }

  private setStatusOptions(): void {
    const statusControl = this.form.get('status');

    if (!this.isEditMode) {
      statusControl?.setValue(AppointmentStatus.Scheduled);
      statusControl?.disable();
      this.statusOptions = [
        { value: AppointmentStatus.Scheduled, label: this.statusLabels[AppointmentStatus.Scheduled] }
      ];
      return;
    }

    statusControl?.enable();

    let allowedStatuses = this.statusConfig[this.currentStatus] ?? [];

    if (!allowedStatuses.includes(this.currentStatus)) {
      allowedStatuses = [this.currentStatus, ...allowedStatuses];
    }

    this.statusOptions = allowedStatuses.map(status => ({
      value: status,
      label: this.statusLabels[status]
    }));

    if ([AppointmentStatus.Canceled, AppointmentStatus.Completed].includes(this.currentStatus)) {
      statusControl?.setValue(this.currentStatus);
      statusControl?.disable();
    }
  }

  canChangeStatus(currentStatus: AppointmentStatus, newStatus: AppointmentStatus): boolean {
    return (currentStatus === AppointmentStatus.Scheduled &&
      [AppointmentStatus.Confirmed, AppointmentStatus.Canceled, AppointmentStatus.Completed].includes(newStatus)) ||
      (currentStatus === AppointmentStatus.Confirmed &&
        [AppointmentStatus.Canceled, AppointmentStatus.Completed].includes(newStatus));
  }
}