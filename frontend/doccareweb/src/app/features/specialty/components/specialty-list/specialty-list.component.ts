import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';

import { filter, map } from 'rxjs/operators';

import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { Specialty } from '../../models/specialty';
import { SpecialtyService } from '../../services/specialty.service';

@Component({
  selector: 'app-specialty-list',
  templateUrl: './specialty-list.component.html',
  styleUrls: ['./specialty-list.component.css']
})
export class SpecialtyListComponent implements OnInit {
  public specialties: Specialty[] = [];
  errorMessage: string = '';
  selectedSpecialty!: Specialty;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  searchTerm: string = '';
  loadingData: boolean = true;

  @Input() placeholderSearch: string = 'Pesquise pela descrição';
  @Input() initialTermSearch: string = '';

  bsModalRef!: BsModalRef;
  @ViewChild('deleteModal') deleteModal!: TemplateRef<any>;  

  constructor(
    private specialtyService: SpecialtyService, 
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {
    this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e)
    ).subscribe((event: NavigationEnd) => {
      if (!event.url.includes('/specialty')) {
        this.specialtyService.clearLocalCurrentPageList();
        this.specialtyService.clearLocalSearchTerm();
      } 
    });
  }

  ngOnInit(): void {
    const storedPage = this.specialtyService.getLocalCurrentPageList();
    const storedSearchTerm = this.specialtyService.getLocalSearchTerm();
  
    if (storedPage) {
      this.currentPage = parseInt(storedPage, 10);
    }
  
    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }

    this.loadSpecialties(this.searchTerm);
  }

  loadSpecialties(searchTerm?: string): void {
    this.spinner.show();
    this.loadingData = true;
    this.specialties = [];
    this.searchTerm = searchTerm ?? '';

    this.specialtyService.getAllSpecialties(this.currentPage, this.pageSize, 'Description', this.searchTerm).subscribe({
      next: response => {
        this.processLoadSpecialtiesSuccess(response);
      },
      error: error => {
        this.processLoadSpecialtiesFail(error);
      },
      complete: () => {
        this.processCompleted();
      }
    });
  }

  private processLoadSpecialtiesSuccess(response: any) {
    this.specialties = response.data.items;
    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  private processLoadSpecialtiesFail(error: any) {
    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
    this.toastr.error('Ocorreu um erro.', 'Atenção');
    this.spinner.hide();
  }  

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  addSpecialty() {
    this.specialtyService.clearLocalCurrentPageList();
    this.specialtyService.clearLocalSearchTerm();
    this.currentPage = 1;
    this.searchTerm = '';
    this.router.navigate(['/specialty/new']);
  }

  editSpecialty(specialty: Specialty) {
    this.specialtyService.saveLocalCurrentPageList(this.currentPage);
    this.specialtyService.saveLocalSearchTerm(this.searchTerm);
    this.router.navigate(['/specialty/edit', specialty.id]);
  }

  openDeleteModal(template: TemplateRef<any>, specialty: Specialty) {
    this.selectedSpecialty = specialty;
    this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
  } 

  confirmDelete(specialty: Specialty): void {  
    this.specialtyService.deleteSpecialty(specialty.id).subscribe({
      next: success => {
        this.processSuccessDelete(success, specialty);
      },
      error: error => {
        this.processFailDelete(error);
      }
    });
  }

  processSuccessDelete(response: any, specialty: Specialty) {
    this.bsModalRef.hide();
    this.specialties = this.specialties.filter(item => item.id !== specialty.id);    
    this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
  }

  processFailDelete(fail: any) {
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.specialtyService.saveLocalSearchTerm(this.searchTerm);

    this.loadSpecialties(this.searchTerm);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadSpecialties();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.specialtyService.saveLocalCurrentPageList(this.currentPage);

    this.loadSpecialties(this.searchTerm);
  }
}
