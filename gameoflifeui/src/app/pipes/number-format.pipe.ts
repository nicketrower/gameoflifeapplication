import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'numberFormat',
  standalone: true
})
export class NumberFormatPipe implements PipeTransform {
  transform(value: number, args?: any): any {
    const result = new Intl.NumberFormat('de-DE', { useGrouping: true }).format(
      value
    );

    return result;
  }
}
