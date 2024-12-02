import { Component, Input, OnInit, ViewChild } from '@angular/core';
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
  isFilteringByDateDoctor: boolean = true;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  searchTerm: string = '';

  @Input() placeholderSearch: string = 'Pesquise pelo nome do paciente';
  @Input() initialTermSearch: string = '';

  constructor(
    private appointmentService: AppointmentService,
    private doctorService: DoctorService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {
    this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e)
    ).subscribe((event: NavigationEnd) => {
      if (!event.url.includes('/appointment')) {
        this.appointmentService.clearLocalCurrentDateList();
        this.appointmentService.clearLocalCurrentDoctorList();
        this.appointmentService.clearLocalSearchTerm();
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
    const storedPage = this.appointmentService.getLocalCurrentPageList();
    const storedSearchTerm = this.appointmentService.getLocalSearchTerm();

    if (storedDate) {
      this.currentDate = new Date(storedDate);
    } else {
      this.currentDate = new Date();
    }

    if (storedDoctor) {
      this.selectedDoctor = +storedDoctor;
    }

    if (storedPage) {
      this.currentPage = parseInt(storedPage, 10);
    }

    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }
  }

  loadAppointments(): void {
    this.spinner.show();
    this.loadingData = true;
    this.appointments = [];

    try {
      const filters: any = {};

      const hasPatientFilter = this.searchTerm && this.searchTerm.trim() !== '';
      const hasDoctorFilter = !!this.selectedDoctor;
      const hasDateFilter = !!this.currentDate;

      if (!hasPatientFilter && (!hasDoctorFilter || !hasDateFilter)) {
        this.spinner.hide();
        return;
      }

      if (hasPatientFilter) {
        this.isFilteringByDateDoctor = false;
        filters['Patient[Name]'] = this.searchTerm.trim();

        const statusArray = [AppointmentStatus.Scheduled, AppointmentStatus.Confirmed];
        filters['Status'] = encodeURIComponent(statusArray.join(','));
      }

      if (!hasPatientFilter && (hasDoctorFilter && hasDateFilter)) {
        this.isFilteringByDateDoctor = true;
        filters['AppointmentDate'] = DateUtils.formatDateToYYYYMMDD(this.currentDate);
        filters['Doctor[Id]'] = this.selectedDoctor.toString();
      }

      this.appointmentService.getAllAppointments(this.currentPage, this.pageSize, filters).subscribe({
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
      const dateA = new Date(a.appointmentDate).getTime();
      const dateB = new Date(b.appointmentDate).getTime();

      if (dateA !== dateB) {
        return dateA - dateB;
      }

      const timeA = DateUtils.convertTimeToMilliseconds(a.appointmentTime);
      const timeB = DateUtils.convertTimeToMilliseconds(b.appointmentTime);
      return timeA - timeB;
    });
  }

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
    this.doctorService.getAll().subscribe({
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
    this.appointmentService.clearLocalSearchTerm();
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

  updateAppointmentStatus(appointment: AppointmentList, status: AppointmentStatus, popover: PopoverDirective) {
    this.appointmentService.updateAppointmentStatus(appointment.id, status).subscribe({
      next: (statusSuccess) => this.processSuccess(statusSuccess, appointment, status, popover),
      error: (error) => this.processFail(error)
    });
  }

  processSuccess(success: any, appointment: AppointmentList, status: AppointmentStatus, popover: PopoverDirective) {
    appointment.status = status;
      popover.hide();

    this.errorMessage = '';
    this.toastr.success('Status atualizado com sucesso!', 'Atenção!');
  }


  processFail(fail: any) {
    this.errorMessage = fail.error.errors;
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.appointmentService.saveLocalSearchTerm(this.searchTerm);

    this.loadAppointments();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadAppointments();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.appointmentService.saveLocalCurrentPageList(this.currentPage);
    this.loadAppointments();
  }
}
