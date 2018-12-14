export class Converter {
  static dateTimeStringToDate(inDate: string): Date {
    return new Date(inDate);
  }

  static dateTimeStringToHumanReadableString(createdAt: string) {
    return 'asdas';
  }

  static getTimeString(timeInSec: number): string {
    let time = timeInSec;

    const hour = Math.floor(time / 3600);
    time = time - hour * 3600;
    const min = Math.floor(time / 60);
    time = time - min * 60;
    const sec = time;

    return this.toStringNumber(hour) + ':' + this.toStringNumber(min) + ':' + this.toStringNumber(sec);
  }

  static toStringNumber(num: number): string {
    if (num === 0) {
      return '00';
    } else if (num > 0 && num <= 9) {
      return '0' + num;
    } else {
      return String(num);
    }
  }

  static calculatePace(timeInSec: number, distance: number): string {
    let min = timeInSec / 60;
    if (min === 0 || distance === 0) {
      return '--';
    }

    min = min / distance;

    const mins = this.toStringNumber(Math.floor(min));
    const secs = this.toStringNumber(Math.floor((min - Math.floor(min)) * 60));

    return mins + ':' + secs;
  }

  static calculateSpeed(timeInSec: number, distance: number): string {
    const hours = timeInSec / 3600;
    if (hours === 0 || distance === 0) {
      return '--';
    }
    return String((distance / hours).toFixed(2));
  }
}
