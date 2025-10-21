import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { ServiceItem } from '../../models/service-item';
import { Service } from 'src/app/features/service/models/service';
import { BaseFormComponent } from 'src/app/shared/components/base-form/base-form.component';
import { GenericValidator } from 'src/app/core/validators/generic-form-validation';
import { CurrencyUtils } from 'src/app/utils/currency-helper';

@Component({
  selector: 'app-service-selector-grid',
  templateUrl: './service-selector-grid.component.html',
  styleUrls: ['./service-selector-grid.component.css']
})
export class ServiceSelectorGridComponent extends BaseFormComponent<any> implements OnInit, OnChanges {
  @Input() items: ServiceItem[] = [];
  @Input() availableServices: Service[] = [];
  @Input() includeSuggestedPrice: boolean = false;
  @Output() itemsChange = new EventEmitter<ServiceItem[]>();

  allServices: Service[] = [];
  filteredServices: Service[] = [];

  searchTerm = '';
  showSuggestions = false;
  selectedService: Service | null = null;

  constructor(
    protected override fb: FormBuilder,
    protected override router: Router,
    protected override route: ActivatedRoute,
    protected override toastr: ToastrService,
    protected override spinner: NgxSpinnerService
  ) {
    super(fb, router, route, toastr, spinner);

    this.validationMessages = {
      name: {
        required: 'Informe o serviço.'
      },
      price: {
        required: 'Informe o preço.'
      }
    };
    this.genericValidator = new GenericValidator(this.validationMessages);
  }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm(): void {
    this.form = this.fb.group({
      search: [''],
      serviceId: ['', [Validators.required]],
      name: ['', [Validators.required]],
      price: ['', [Validators.required]]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['availableServices'] && Array.isArray(this.availableServices)) {
      this.allServices = this.availableServices;
      this.filteredServices = [...this.availableServices];
    }
  }

  filterServices(): void {
    const term = this.form.get('search')?.value?.toLowerCase().trim() ?? '';
    if (term.length === 0) {
      this.filteredServices = [];
      this.showSuggestions = false;
      return;
    }

    this.filteredServices = this.allServices.filter(s =>
      s.name.toLowerCase().includes(term)
    );
    this.showSuggestions = this.filteredServices.length > 0;
  }

  hideSuggestions(): void {
    setTimeout(() => (this.showSuggestions = false), 200);
  }

  selectService(service: Service): void {
    this.selectedService = service;
    this.form.patchValue({
      serviceId: service.id,
      name: service.name,
      price: service.price,
      search: service.name
    });
    this.showSuggestions = false;
    this.displayMessage = this.genericValidator.processMessages(this.form);
  }

  addService(): void {
    if (this.form.invalid || !this.selectedService) {
      this.form.markAllAsTouched();
      this.displayMessage = this.genericValidator.processMessages(this.form);
      return;
    }

    const alreadyExists = this.items.some(i => i.serviceId === this.selectedService!.id);

    if (alreadyExists) {
      this.toastr.warning('Este serviço já foi incluído.', 'Atenção');
      return;
    }

    const rawPrice = this.form.get('price')?.value?.toString().replace(/[^\d,]/g, '').replace(',', '.') ?? '0';
    const price = parseFloat(rawPrice);

    const newItem: ServiceItem = {
      serviceId: this.form.get('serviceId')?.value,
      name: this.form.get('name')?.value,
      price: price
    };

    if (this.includeSuggestedPrice && this.selectedService) {
       (newItem as any).suggestedPrice = this.selectedService.price;
    }

    this.items = [...this.items, newItem];
    this.itemsChange.emit(this.items);

    this.searchTerm = '';
    this.form.reset();
    this.selectedService = null;
    this.displayMessage = {};
  }

  clearSelectedService(): void {
    this.selectedService = null;
    this.form.patchValue({
      search: '',
      serviceId: '',
      name: '',
      price: ''
    });
    this.filteredServices = [...this.allServices];
    this.showSuggestions = false;
  }

  updatePrice(item: ServiceItem, newPrice: any): void {
    const raw = typeof newPrice === 'string'
      ? CurrencyUtils.unformat(newPrice)
      : newPrice;

    item.price = raw;
    this.itemsChange.emit([...this.items]);
  }

  removeItem(serviceId: number): void {
    this.items = this.items.filter(i => i.serviceId !== serviceId);
    this.itemsChange.emit(this.items);
  }

  formatPrice(item: any): void {
    const formatted = CurrencyUtils.format(Number(item.price));
    item.price = CurrencyUtils.unformat(formatted);
  }
}