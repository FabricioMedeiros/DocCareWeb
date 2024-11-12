export class DateUtils {
  public static formatDateToDDMMYYYY(dateString: string): string {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  public static formatDateToYYYYMMDD(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }

  public static convertTimeToMilliseconds(time: any): number {
    if (time instanceof Date) {
      const hours = time.getHours();
      const minutes = time.getMinutes();
      return (hours * 60 + minutes) * 60000;
    }

    if (typeof time === 'string' && time.trim()) {
      const [hours, minutes] = time.split(':').map(num => parseInt(num, 10));
      if (isNaN(hours) || isNaN(minutes)) {
        console.error('Hora inválida:', time);
        return 0;
      }
      return (hours * 60 + minutes) * 60000;
    }

    console.error('Hora inválida:', time);
    return 0;
  }
}