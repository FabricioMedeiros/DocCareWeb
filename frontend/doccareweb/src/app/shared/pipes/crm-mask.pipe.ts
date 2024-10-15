import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'crmMask'
})
export class CrmMaskPipe implements PipeTransform {
  transform(value?: string): string {
    if (!value) {
      return '';     }

    value = value.replace(/[^0-9a-zA-Z]/g, '');

    if (value.length <= 6) {
      return value; 
    }

    const crmNumber = value.slice(0, 6); 
    const stateCode = value.slice(6, 8).toUpperCase(); 

    return stateCode.length === 2 ? `${crmNumber}/${stateCode}` : crmNumber;
  }
}
