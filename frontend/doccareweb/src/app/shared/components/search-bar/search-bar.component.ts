import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-search-bar',
  templateUrl: './search-bar.component.html',
  styleUrls: ['./search-bar.component.css']
})
export class SearchBarComponent implements OnInit {
  @Input() placeholder: string = 'Pesquisar...'; 
  @Input() initialSearchTerm: string = '';
  @Input() pageSizeOptions: number[] = [10, 30, 50]; 
  @Output() search: EventEmitter<{ pageSize: number, term: string }> = new EventEmitter();
  @Output() clear: EventEmitter<void> = new EventEmitter<void>(); 

  selectedPageSize: number = 10; 
  searchTerm: string = '';

  constructor() { }

  ngOnInit(): void {
    this.searchTerm = this.initialSearchTerm;
  }

  onSearch(): void {
    this.emitSearch();
  }

  onPageSizeChange(): void {
    this.emitSearch(); 
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.emitSearch();
    this.clear.emit(); 
  }

  private emitSearch(): void {
    this.search.emit({
      pageSize: this.selectedPageSize,
      term: this.searchTerm.trim()
    });
  }
}
