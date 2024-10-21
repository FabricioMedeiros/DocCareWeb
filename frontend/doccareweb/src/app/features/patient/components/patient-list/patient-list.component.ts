import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

import { PatientList } from '../../models/patient';
import { PatientService } from 'src/app/shared/services/patient.service';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-patient-list',
  templateUrl: './patient-list.component.html',
  styleUrls: ['./patient-list.component.css']
})
export class PatientListComponent implements OnInit{

  public patients: PatientList[] = [];
  errorMessage: string = '';
  selectedPatient!: PatientList;

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
    private patientService: PatientService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {
    this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e)
    ).subscribe((event: NavigationEnd) => {
      if (!event.url.includes('/patient')) {
        this.patientService.clearLocalCurrentPageList();
        this.patientService.clearLocalSearchTerm();
      }
    });
  }

  ngOnInit(): void {
    const storedPage = this.patientService.getLocalCurrentPageList();
    const storedSearchTerm = this.patientService.getLocalSearchTerm();
  
    if (storedPage) {
      this.currentPage = parseInt(storedPage, 10);
    }
  
    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }

    this.loadPatients(this.searchTerm);
  }

  loadPatients(searchTerm?: string): void {
    this.spinner.show();
    this.loadingData = true;
    this.patients = [];
    this.searchTerm = searchTerm ?? '';

    this.patientService.getAllPatients(this.currentPage, this.pageSize, 'Name', this.searchTerm).subscribe({
      next: response => {
        this.processLoadPatientsSuccess(response);
      },
      error: error => {
        this.processLoadPatientsFail(error);
      },
      complete: () => {
        this.processCompleted();
      }
    });
  }

  private processLoadPatientsSuccess(response: any) {
    this.patients = response.data.items;
    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  private processLoadPatientsFail(error: any) {
    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
    this.toastr.error('Ocorreu um erro.', 'Atenção');
    this.spinner.hide();
  }

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  addpatient() {
    this.patientService.clearLocalCurrentPageList();
    this.patientService.clearLocalSearchTerm();
    this.currentPage = 1;
    this.searchTerm = '';
    this.router.navigate(['/patient/new']);
  }

  editpatient(patient: PatientList) {
    this.patientService.saveLocalCurrentPageList(this.currentPage);
    this.patientService.saveLocalSearchTerm(this.searchTerm);
    this.router.navigate(['/patient/edit', patient.id]);
  }

  openDeleteModal(template: TemplateRef<any>, patient: PatientList) {
    this.selectedPatient = patient;
    this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
  }

  confirmDelete(patient: PatientList): void {
    this.patientService.deletePatient(patient.id).subscribe({
      next: success => {
        this.processSuccessDelete(success, patient);
      },
      error: error => {
        this.processFailDelete(error);
      }
    });
  }

  processSuccessDelete(response: any, patient: PatientList) {
    this.bsModalRef.hide();
    this.patients = this.patients.filter(item => item.id !== patient.id);
    this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
  }

  processFailDelete(fail: any) {
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.patientService.saveLocalSearchTerm(this.searchTerm);

    this.loadPatients(this.searchTerm);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadPatients();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.patientService.saveLocalCurrentPageList(this.currentPage);
    this.loadPatients(this.searchTerm);
  }

}
