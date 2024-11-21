import { Input, OnInit, TemplateRef, ViewChild, Directive } from '@angular/core';
import { Router, NavigationEnd, Event } from '@angular/router';
import { filter, map } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Directive()
export abstract class BaseListComponent<T> implements OnInit {
  public items: T[] = [];
  errorMessage: string = '';
  selectedItem!: T;

  currentPage: number = 1;
  pageSize: number = 10;
  totalPages: number = 1;
  fieldSearch: string = '';
  searchTerm: string = '';
  loadingData: boolean = true;

  @Input() placeholderSearch: string = '';
  @Input() initialTermSearch: string = '';

  bsModalRef!: BsModalRef;
  @ViewChild('deleteModal') deleteModal!: TemplateRef<any>;

  constructor(
    protected service: any,
    protected router: Router,
    protected toastr: ToastrService,
    protected spinner: NgxSpinnerService,
    protected modalService: BsModalService
  ) {
    this.router.events.pipe(
      filter((e: Event): e is NavigationEnd => e instanceof NavigationEnd),
      map((e: NavigationEnd) => e)
    ).subscribe(event => {
      if (!this.isCurrentRoute(event.url)) {
        this.service.clearLocalCurrentPageList();
        this.service.clearLocalSearchTerm();
      }
    });
  }

  ngOnInit(): void {
    const storedPage = this.service.getLocalCurrentPageList();
    const storedSearchTerm = this.service.getLocalSearchTerm();
  
    if (storedPage) this.currentPage = parseInt(storedPage, 10);
    if (storedSearchTerm) {
      this.searchTerm = storedSearchTerm;
      this.initialTermSearch = storedSearchTerm;
    }

    this.loadItems();
  }

  loadItems(): void {
    this.spinner.show();
    this.loadingData = true;
    this.items = [];
    
    this.service.getAll(this.currentPage, this.pageSize, this.fieldSearch, this.searchTerm).subscribe({
      next: (response: any) => this.processLoadSuccess(response),
      error: (error: any) => this.processLoadFail(error),
      complete: () => this.processCompleted()
    });
  }

  private processLoadSuccess(response: any) {
    this.items = response.data.items;
    this.currentPage = response.data.page;
    this.totalPages = Math.ceil(response.data.totalRecords / response.data.pageSize);
  }

  private processLoadFail(error: any) {
    if (error?.status === 401) {
      this.spinner.hide();
      return;
    }

    this.errorMessage = error.error?.errors?.[0] || 'Ocorreu um erro.';
    this.toastr.error('Ocorreu um erro.', 'Atenção!');
    this.spinner.hide();
  }

  private processCompleted() {
    this.loadingData = false;
    this.spinner.hide();
  }

  addItem(route: string) {
    this.service.clearLocalCurrentPageList();
    this.service.clearLocalSearchTerm();
    this.currentPage = 1;
    this.searchTerm = '';
    this.router.navigate([route]);
  }

  editItem(item: T, route: string) {
    this.service.saveLocalCurrentPageList(this.currentPage);
    this.service.saveLocalSearchTerm(this.searchTerm);
    this.router.navigate([route, (item as any).id]);
  }

  openDeleteModal(template: TemplateRef<any>, item: T) {
    this.selectedItem = item;
    this.bsModalRef = this.modalService.show(template, { class: 'custom-modal-delete' });
  }

  confirmDelete(item: T): void {
    this.service.delete((item as any).id).subscribe({
      next: (success: any) => this.processSuccessDelete(success, item),
      error: (error: any) => this.processFailDelete(error)
    });
  }

  processSuccessDelete(response: any, item: T) {
    this.bsModalRef.hide();
    this.items = this.items.filter(i => (i as any).id !== (item as any).id);
    this.toastr.success('Registro excluído com sucesso!', 'Atenção!');
  }

  processFailDelete(fail: any) {
    this.toastr.error('Ocorreu um erro.', 'Atenção!');
  }

  onSearch(event: { pageSize: number, term: string }): void {
    this.pageSize = event.pageSize;
    this.searchTerm = event.term;
    this.currentPage = 1;
    this.service.saveLocalSearchTerm(this.searchTerm);

    this.loadItems();
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.currentPage = 1;
    this.loadItems();
  }

  onPageChanged(page: number) {
    this.currentPage = page;
    this.service.saveLocalCurrentPageList(this.currentPage);
    this.loadItems();
  }

  abstract isCurrentRoute(url: string): boolean;
}
