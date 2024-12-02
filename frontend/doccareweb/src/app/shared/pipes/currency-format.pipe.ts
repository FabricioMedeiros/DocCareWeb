import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyFormat'
})
export class CurrencyFormatPipe implements PipeTransform {
  transform(value: number): string {
    if (value !== null && value !== undefined) {
      return value.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    }
    return '';
  }
}

