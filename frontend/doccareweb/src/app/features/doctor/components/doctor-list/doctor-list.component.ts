import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';
import { filter, map } from 'rxjs/operators';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { DoctorList } from '../../models/doctor';
import { DoctorService } from '../../../../shared/services/doctor.service';

@Component({
  selector: 'app-doctor-list',
  templateUrl: './doctor-list.component.html',
  styleUrls: ['./doctor-list.component.css']
})
export class DoctorListComponent implements OnInit {

  public doctors: DoctorList[] = [];
  errorMessage: string = '';
  selectedDoctor!: DoctorList;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  searchTerm: string = '';
  loadingData: boolean = true;

  @Input() placeholderSearch: string = 'Pesquise pelo nome';
  @Input() initialTermSearch: string = '';

  bsModalRef!: BsModalRef;
  @ViewChild('deleteModal') deleteModal!: TemplateRef<any>;

  constructor(
    private doctorService: DoctorService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {
    this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e)
    ).subscribe((event: NavigationEnd) => {
      if (!event.url.includes('/doctor')) {
        this.doctorService.clearLocalCurrentPageList();
        this.doctorService.clearLocalSearchTerm();
      }
    });
  }

  ngOnInit(): void {
    const storedPage = this.doctorService.getLocalCurrentPageList();
    const storedSearchTerm = this.doctorService.getLocalSearchTerm();
  
    if (storedPage) {
      this.currentPage = parseInt(storedPage, 10);
    }
  
    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }

    this.loadDoctors(this.searchTerm);
  }

  loadDoctors(searchTerm?: string): void {
    this.spinner.show();
    this.loadingData = true;
    this.doctors = [];
    this.searchTerm = searchTerm ?? '';

    this.doctorService.getAllDoctors(this.currentPage, this.pageSize, 'Name', this.searchTerm).subscribe({
      next: response => {
        this.processLoadDoctorsSuccess(response);
      },
      error: error => {
        this.processLoadDoctorsFail(error);
      },
      complete: () => {
        this.processCompleted();
      }
    });
  }

  private processLoadDoctorsSuccess(response: any) {
    this.doctors = response.data.items;
    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  private processLoadDoctorsFail(error: any) {
    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
    this.toastr.error('Ocorreu um erro.', 'Atenção');
    this.spinner.hide();
  }

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  addDoctor() {
    this.doctorService.clearLocalCurrentPageList();
    this.doctorService.clearLocalSearchTerm();
    this.currentPage = 1;
    this.searchTerm = '';
    this.router.navigate(['/doctor/new']);
  }

  editDoctor(doctor: DoctorList) {
    this.doctorService.saveLocalCurrentPageList(this.currentPage);
    this.doctorService.saveLocalSearchTerm(this.searchTerm);
    this.router.navigate(['/doctor/edit', doctor.id]);
  }

  openDeleteModal(template: TemplateRef<any>, doctor: DoctorList) {
    this.selectedDoctor = doctor;
    this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
  }

  confirmDelete(doctor: DoctorList): void {
    this.doctorService.deleteDoctor(doctor.id).subscribe({
      next: success => {
        this.processSuccessDelete(success, doctor);
      },
      error: error => {
        this.processFailDelete(error);
      }
    });
  }

  processSuccessDelete(response: any, doctor: DoctorList) {
    this.bsModalRef.hide();
    this.doctors = this.doctors.filter(item => item.id !== doctor.id);
    this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
  }

  processFailDelete(fail: any) {
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.doctorService.saveLocalSearchTerm(this.searchTerm);

    this.loadDoctors(this.searchTerm);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadDoctors();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.doctorService.saveLocalCurrentPageList(this.currentPage);
    this.loadDoctors(this.searchTerm);
  }
}
