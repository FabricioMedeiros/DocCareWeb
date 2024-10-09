import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { SearchBarComponent } from './components/search-bar/search-bar.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { CurrencyFormatPipe } from './pipes/currencyformat.pipe';


@NgModule({
  declarations: [
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe 
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  exports: [
    PaginationComponent,
    SearchBarComponent,
    CurrencyFormatPipe 
  ],
  providers: []
})
export class SharedModule { }
