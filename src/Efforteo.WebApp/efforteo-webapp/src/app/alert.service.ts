import {Injectable} from '@angular/core';
import {Alert} from './alert';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  alerts: Alert[] = [];

  add(alert: Alert) {
    this.alerts.push(alert);
  }

  remove(alert: Alert) {
    const index: number = this.alerts.indexOf(alert);
    if (index !== -1) {
      this.alerts.splice(index, 1);
    }
  }

  has(alert: Alert) {
    const index: number = this.alerts.indexOf(alert);
    return index !== -1;
  }

  clear() {
    this.alerts = [];
  }
}
