import { Component, Input, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';

import { filter, map } from 'rxjs/operators';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { HealthPlan } from '../../models/healthplan';
import { HealthPlanService } from '../../../../shared/services/healthplan.service';

@Component({
  selector: 'app-healthplan-list',
  templateUrl: './healthplan-list.component.html',
  styleUrls: ['./healthplan-list.component.css']
})
export class HealthplanListComponent implements OnInit {

  public healthPlans: HealthPlan[] = [];
  errorMessage: string = '';
  selectedHealthPlan!: HealthPlan;

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
    private healthPlanService: HealthPlanService,
    private router: Router,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService
  ) {
     this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e) 
    ).subscribe((event: NavigationEnd) => {
      if (!event.url.includes('/healthplan')) {
        this.healthPlanService.clearLocalCurrentPageList();
        this.healthPlanService.clearLocalSearchTerm();
      } 
    });
  }

  ngOnInit(): void {
    const storedPage = this.healthPlanService.getLocalCurrentPageList();
    const storedSearchTerm = this.healthPlanService.getLocalSearchTerm();
  
    if (storedPage) {
      this.currentPage = parseInt(storedPage, 10);
    }
  
    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }

    this.loadHealthPlans(this.searchTerm);
  }

  loadHealthPlans(searchTerm?: string): void {
    this.spinner.show();
    this.loadingData = true;
    this.healthPlans = [];
    this.searchTerm = searchTerm ?? '';

    this.healthPlanService.getAllHealthPlans(this.currentPage, this.pageSize, 'Description', this.searchTerm).subscribe({
      next: response => {
        this.processLoadHealthPlansSuccess(response);
      },
      error: error => {
        this.processLoadHealthPlansFail(error);
      },
      complete: () => {
        this.processCompleted();
      }
    });
  }

  private processLoadHealthPlansSuccess(response: any) {
    this.healthPlans = response.data.items;
    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  private processLoadHealthPlansFail(error: any) {
    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
    this.toastr.error('Ocorreu um erro.', 'Atenção');
    this.spinner.hide();
  }

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  addHealthPlan() {
    this.healthPlanService.clearLocalCurrentPageList();
    this.healthPlanService.clearLocalSearchTerm();
    this.currentPage = 1;
    this.searchTerm = '';
    this.router.navigate(['/healthplan/new']);
  }

  editHealthPlan(healthPlan: HealthPlan) {
    this.healthPlanService.saveLocalCurrentPageList(this.currentPage);
    this.healthPlanService.saveLocalSearchTerm(this.searchTerm);
    this.router.navigate(['/healthplan/edit', healthPlan.id]);
  }

  openDeleteModal(template: TemplateRef<any>, healthPlan: HealthPlan) {
    this.selectedHealthPlan = healthPlan;
    this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
  }

  confirmDelete(healthPlan: HealthPlan): void {
    this.healthPlanService.deleteHealthPlan(healthPlan.id).subscribe({
      next: success => {
        this.processSuccessDelete(success, healthPlan);
      },
      error: error => {
        this.processFailDelete(error);
      }
    });
  }

  processSuccessDelete(response: any, healthPlan: HealthPlan) {
    this.bsModalRef.hide();
    this.healthPlans = this.healthPlans.filter(item => item.id !== healthPlan.id);
    this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
  }

  processFailDelete(fail: any) {
    this.toastr.error('Ocorreu um erro.', 'Atenção');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.healthPlanService.saveLocalSearchTerm(this.searchTerm);

    this.loadHealthPlans(this.searchTerm);
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadHealthPlans();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.healthPlanService.saveLocalCurrentPageList(this.currentPage);
    this.loadHealthPlans(this.searchTerm);
  }
}

// import { Component, Input, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
// import { Router } from '@angular/router';

// import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
// import { ToastrService } from 'ngx-toastr';
// import { NgxSpinnerService } from 'ngx-spinner';

// import { HealthPlan } from '../../models/healthplan';
// import { HealthPlanService } from '../../services/healthplan.service';

// @Component({
//   selector: 'app-healthplan-list',
//   templateUrl: './healthplan-list.component.html',
//   styleUrls: ['./healthplan-list.component.css']
// })
// export class HealthplanListComponent implements OnInit {

//   public healthPlans: HealthPlan[] = [];
//   errorMessage: string = '';
//   selectedHealthPlan!: HealthPlan;

//   currentPage: number = 1;
//   pageSize: number = 10;
//   totalPages: number = 1;
//   searchTerm: string = '';
//   loadingData: boolean = true;

//   @Input() placeholderSearch: string = 'Pesquise pela descrição';
//   @Input() initialTermSearch: string = '';

//   bsModalRef!: BsModalRef;
//   @ViewChild('deleteModal') deleteModal!: TemplateRef<any>;

//   constructor(
//     private healthPlanService: HealthPlanService,
//     private router: Router,
//     private toastr: ToastrService,
//     private spinner: NgxSpinnerService,
//     private modalService: BsModalService
//   ) { }

//   ngOnInit(): void {
//     const storedPage = this.healthPlanService.getLocalCurrentPageList();
//     const storedSearchTerm = this.healthPlanService.getLocalSearchTerm();
  
//     if (storedPage) {
//       this.currentPage = parseInt(storedPage, 10);
//       this.healthPlanService.clearLocalCurrentPageList();
//     }
  
//     if (storedSearchTerm) {
//       this.searchTerm = storedSearchTerm;
//       this.initialTermSearch = storedSearchTerm;

//       this.loadHealthPlans(this.searchTerm);

//       this.healthPlanService.clearLocalSearchTerm(); 
//     } else {
//       this.loadHealthPlans();
//     }
//   }  

//   loadHealthPlans(searchTerm?: string): void {
//     this.spinner.show();
//     this.loadingData = true;
//     this.healthPlans = [];
//     this.searchTerm = searchTerm ?? '';

//     this.healthPlanService.getAllHealtPlans(this.currentPage, this.pageSize, 'Description', this.searchTerm).subscribe({
//       next: response => {
//         this.processLoadHealthPlansSuccess(response);
//       },
//       error: error => {
//         this.processLoadHealthPlansFail(error);
//       },
//       complete: () => {
//         this.processCompleted();
//       }
//     });
//   }

//   private processLoadHealthPlansSuccess(response: any) {
//     this.healthPlans = response.data.items;
//     this.currentPage = response.data.page;
//     this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
//   }

//   private processLoadHealthPlansFail(error: any) {
//     this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro desconhecido.';
//     this.toastr.error('Ocorreu um erro.', 'Atenção');
//     this.spinner.hide();
//   }

//   private processCompleted() {
//     this.loadingData = false;
//     this.spinner.hide();
//   }

//   addHealthPlan() {
//     this.healthPlanService.clearLocalCurrentPageList();
//     this.healthPlanService.clearLocalSearchTerm();
//     this.router.navigate(['/healthplan/new']);
//   }

//   editHealthPlan(healthPlan: HealthPlan) {
//     this.healthPlanService.saveLocalCurrentPageList(this.currentPage);
//     this.healthPlanService.saveLocalSearchTerm(this.searchTerm);
//     this.router.navigate(['/healthplan/edit', healthPlan.id]);
//   }

//   openDeleteModal(template: TemplateRef<any>, healthPlan: HealthPlan) {
//     this.selectedHealthPlan = healthPlan;
//     this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
//   }

//   confirmDelete(healthPlan: HealthPlan): void {
//     this.healthPlanService.deleteHealthPlan(healthPlan.id).subscribe({
//       next: success => {
//         this.processSuccessDelete(success, healthPlan);
//       },
//       error: error => {
//         this.processFailDelete(error);
//       }
//     });
//   }

//   processSuccessDelete(response: any, healthPlan: HealthPlan) {
//     this.bsModalRef.hide();
//     this.healthPlans = this.healthPlans.filter(item => item.id !== healthPlan.id);
//     this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
//   }

//   processFailDelete(fail: any) {
//     this.toastr.error('Ocorreu um erro.', 'Atenção');
//   }

//   onSearch(event: { pageSize: number, term: string }): void {
//     this.pageSize = event.pageSize;
//     this.searchTerm = event.term;
//     this.currentPage = 1;
//     this.healthPlanService.saveLocalSearchTerm(this.searchTerm);

//     this.loadHealthPlans(this.searchTerm);
//   }

//   clearSearch(): void {
//     this.searchTerm = '';
//     this.currentPage = 1;
//     this.loadHealthPlans();
//   }

//   onPageChanged(page: number) {
//     this.currentPage = page;    
//     this.loadHealthPlans(this.searchTerm);
//   }
// }
