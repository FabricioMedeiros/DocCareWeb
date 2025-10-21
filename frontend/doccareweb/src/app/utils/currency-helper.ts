import { CurrencyFormatPipe } from 'src/app/shared/pipes/currency-format.pipe';

export class CurrencyUtils {
  static format(value: number): string {
    const pipe = new CurrencyFormatPipe();
    return pipe.transform(value);
  }

  static unformat(value: string): number {
    const cleaned = value.replace(/[^\d,-]/g, '').replace(',', '.');
    return parseFloat(cleaned);
  }
}