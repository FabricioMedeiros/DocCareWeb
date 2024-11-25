import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, map, Observable } from 'rxjs';

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
export class AppointmentFormComponent extends BaseFormComponent<AppointmentList> implements OnInit {
  doctors: Doctor[] = [];
  patients: Patient[] = [];
  healthPlans: HealthPlan[] = [];

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
    fb: FormBuilder,
    router: Router,
    route: ActivatedRoute,
    toastr: ToastrService,
    spinner: NgxSpinnerService,
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private patientService: PatientService,
    private healthPlanService: HealthPlanService,
  ) {
    super(fb, router, route, toastr, spinner);

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
        this.form.get('appointmentTime')?.disable();
      }

      if (this.currentStatus === AppointmentStatus.Canceled || this.currentStatus === AppointmentStatus.Completed) {
        this.form.disable();
        this.form.get('notes')?.enable(); 
      } else {
        this.form.get('status')?.enable();
      }
    }

    forkJoin([this.loadDoctors(), this.loadPatients(), this.loadHealthPlans()]).subscribe(() => {
      if (this.isEditMode) {
        this.updatePatientDetails(this.form.get('patientId')?.value);
      }
    });

    this.form.get('patientId')?.valueChanges.subscribe(patientId => {
      this.updatePatientDetails(patientId);
    });

    this.form.get('healthPlanId')?.valueChanges.subscribe(healthPlanId => {
      const selectedHealthPlan = this.healthPlans.find(hp => hp.id === +healthPlanId);
      this.selectedCost = selectedHealthPlan ? selectedHealthPlan.cost : 0;
      this.form.get('cost')?.setValue(this.selectedCost);
    });

    this.spinner.hide();
  }

  buildForm(): void {
    this.form = this.fb.group({
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
  }

  updatePatientDetails(patientId: number): void {
    const selectedPatient = this.patients.find(p => p.id === +patientId);
    if (selectedPatient) {
      this.form.patchValue({
        cpf: selectedPatient.cpf,
        phone: selectedPatient.phone,
        cellPhone: selectedPatient.cellPhone,
        healthPlanId: selectedPatient.healthPlan.id,
        cost: selectedPatient.healthPlan.cost
      });
    }
    else {
      this.form.patchValue({
        cpf: '',
        phone: '',
        cellPhone: '',
        healthPlanId: null,
        cost: ''
      });
    }
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
      response => {
        this.doctors = response?.data?.items || [];
      }));
  }

  loadPatients(): Observable<void> {
    return this.patientService.getAll().pipe(map(
      response => {
        this.patients = response?.data?.items || [];
      }));
  }

  loadHealthPlans(): Observable<void> {
    return this.healthPlanService.getAll().pipe(map(
      response => {
        this.healthPlans = response?.data?.items || [];
      }));
  }

  saveAppointment() {
    if (this.form.dirty && this.form.valid) {
      this.changesSaved = true;
      const appointmentData = Object.assign({}, this.form.getRawValue());

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
                error: (error) => this.processFail(error)});
            }
            else{
              this.processSuccess('Agendamento atualizado com sucesso!', '/appointment/list');
            }
          },
          error: (error) => this.processFail(error)
        });
      } else {
        this.appointmentService.registerAppointment(appointmentData).subscribe({
          next: (success) =>  this.processSuccess('Agendamento cadastrado com sucesso!', '/appointment/list'),
                  error: (error) => this.processFail(error)
        });
      }
    }
  }

  private setStatusOptions(): void {
    if (!this.isEditMode) {
      this.form.get('status')?.setValue(AppointmentStatus.Scheduled);
      this.form.get('status')?.disable();
    } else {
      const statusControl = this.form.get('status');
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
