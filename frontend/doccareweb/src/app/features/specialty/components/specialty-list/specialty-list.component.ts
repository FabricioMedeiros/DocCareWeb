import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Specialty } from '../../models/specialty';
import { SpecialtyService } from '../../services/specialty.service';

@Component({
  selector: 'app-specialty-list',
  templateUrl: './specialty-list.component.html',
  styleUrls: ['./specialty-list.component.css']
})
export class SpecialtyListComponent implements OnInit {
  public specialties: Specialty[] = [];
  errorMessage: string = 'Não foi possível carregar os dados';
  selectedSpecialty!: Specialty;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  searchTerm: string = '';
  loadingData: boolean = true;

  constructor(
    private specialtyService: SpecialtyService, 
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.loadSpecialties();
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.loadSpecialties(this.searchTerm);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadSpecialties();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.loadSpecialties(this.searchTerm);
  }

  loadSpecialties(searchTerm?: string): void {
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
        this.loadingData = false;
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
  }  

  addSpecialty() {
    this.router.navigate(['/specialty/new']);
  }

  editSpecialty(specialty: Specialty) {
    this.router.navigate(['/specialty/edit', specialty.id]);
  }
}
