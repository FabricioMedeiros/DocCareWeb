import { Component, OnInit, ViewChild } from '@angular/core';
import { Event, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { PopoverDirective } from 'ngx-bootstrap/popover';

import { AppointmentService } from '../../../../shared/services/appointment.service';
import { DoctorService } from './../../../../shared/services/doctor.service';
import { Appointment, AppointmentList } from './../../models/appointment';
import { Doctor } from './../../models/doctor';
import { DateUtils } from 'src/app/core/utils/date-utils';
import { AppointmentStatus } from '../../models/appointment-status';


@Component({
  selector: 'app-appointment-list',
  templateUrl: './appointment-list.component.html',
  styleUrls: ['./appointment-list.component.css']
})
export class AppointmentListComponent implements OnInit {
  @ViewChild('popover') popover!: PopoverDirective;

  errorMessage: string = '';
  public appointments: AppointmentList[] = [];
  selectedAppointment!: Appointment;
  appointmentStatus = AppointmentStatus;
  doctors: Doctor[] = [];
  selectedDoctor!: number;
  currentDate!: Date;
  loadingData: boolean = true;
  isPopoverVisible = false;

  constructor(
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) 
  {
    this.router.events.pipe(
     filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
     map((e: NavigationEnd) => e) 
   ).subscribe((event: NavigationEnd) => {
     if (!event.url.includes('/appointment')) {
       this.appointmentService.clearLocalCurrentDateList();
       this.appointmentService.clearLocalCurrentDoctorList();
     } 
   });
 }

  ngOnInit(): void {
    this.loadLocalData();
    this.loadDoctors();
    this.loadAppointments();
  }

  private loadLocalData(): void {
    const storedDate = this.appointmentService.getLocalCurrentDateList();
    const storedDoctor = this.appointmentService.getLocalCurrentDoctorList();

    if (storedDate) {
      this.currentDate = new Date(storedDate);
    } else {
      this.currentDate = new Date();
    }

    if (storedDoctor) {
      this.selectedDoctor = +storedDoctor;
    }
  }

  loadAppointments(): void {
    if (!this.currentDate || !this.selectedDoctor) return;
    this.spinner.show();
    this.loadingData = true;
    this.appointments = [];

    try {
      const filters = {
        appointmentDate: DateUtils.formatDateToYYYYMMDD(this.currentDate),
        doctorId: this.selectedDoctor.toString()
      };

      this.appointmentService.getAllAppointments(filters).subscribe({
        next: response => {
          this.processloadAppointmentsSuccess(response);
        },
        error: error => {
          this.processloadAppointmentsFail(error);
        },
        complete: () => {
          this.processCompleted();
        }
      });
    } catch (error) {
      console.error("Erro ao formatar ou processar os filtros:", error);
      this.spinner.hide();
      this.loadingData = false;
    }
  }

  private processloadAppointmentsSuccess(response: any) {
    this.appointments = response.data.items.map((item: any) => ({
      id: item.id,
      appointmentDate: new Date(item.appointmentDate),
      appointmentTime: item.appointmentTime,
      status: item.status,
      doctor: item.doctor,
      patient: item.patient,
      healthPlan: item.healthPlan,
      cost: item.cost,
      notes: item.notes || ''
    }));

    this.appointments.sort((a, b) => {
      const timeA = DateUtils.convertTimeToMilliseconds(a.appointmentTime);
      const timeB = DateUtils.convertTimeToMilliseconds(b.appointmentTime);
      return timeA - timeB; 
    });
;  }

  private processloadAppointmentsFail(error: any) {
    if (error?.status === 401) {
      this.spinner.hide();
      return;
    }

    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
    this.toastr.error('Ocorreu um erro.', 'Atenção');
    this.spinner.hide();
  }

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  loadDoctors(): void {
    this.doctorService.getAllDoctors().subscribe({
      next: (response) => {
        if (response && response.data) {
          this.doctors = response.data.items;
        } else {
          this.doctors = [];
        }
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar os médicos';
        console.error('Erro ao carregar os médicos', err);
      }
    });
  }

  onDateChange(date: Date): void {
    this.currentDate = date;
    this.appointmentService.saveLocalCurrentDateList(date);
    this.loadAppointments();
  }

  onDoctorChange(event: any): void {
    this.selectedDoctor = event.target.value;
    this.appointmentService.saveLocalCurrentDoctorList(this.selectedDoctor);
    this.loadAppointments();
  }

  addAppointment() {
    this.router.navigate(['/appointment/new']);
  }

  editAppointment(appointment: AppointmentList) {
    this.router.navigate(['/appointment/edit', appointment.id]);
  }

  getStatusText(status: AppointmentStatus): string {
    switch (status) {
      case AppointmentStatus.Scheduled:
        return 'Agendado';
      case AppointmentStatus.Confirmed:
        return 'Confirmado';
      case AppointmentStatus.Canceled:
        return 'Cancelado';
      case AppointmentStatus.Completed:
        return 'Concluído';
      default:
        return 'Desconhecido';
    }
  }

  getStatusClass(status: AppointmentStatus): string {
    switch (status) {
      case AppointmentStatus.Scheduled:
        return 'status-scheduled text-center';
      case AppointmentStatus.Confirmed:
        return 'status-confirmed text-center';
      case AppointmentStatus.Canceled:
        return 'status-canceled text-center';
      case AppointmentStatus.Completed:
        return 'status-completed text-center';
      default:
        return '';
    }
  }  

  updateAppointmentStatus(appointment: AppointmentList, status: AppointmentStatus, popover: any) {
    this.appointmentService.updateAppointmentStatus(appointment.id, status).subscribe({
      next: (statusSuccess) => this.processSuccess(statusSuccess, appointment, status),
      error: (error) => this.processFail(error)
    });
  }

  processSuccess(success: any, appointment: AppointmentList, status: AppointmentStatus) {
    appointment.status = status;
    this.popover.hide();
    this.errorMessage = '';
    let toast = this.toastr.success('Status atualizado com sucesso!', 'Atenção!');

    if (toast) {
      toast.onHidden.subscribe(() => {
        this.router.navigate(['/appointment/list']);
      });
    }
  }

  processFail(fail: any) {
    this.errorMessage = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }
}