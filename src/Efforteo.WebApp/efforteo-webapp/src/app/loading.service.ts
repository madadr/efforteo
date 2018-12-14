import {Injectable, OnInit} from '@angular/core';
import {BehaviorSubject, Observable, of, Subject} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  toggleMap: { [uniqueKey: string]: any } = {};

  create(key: string) {
    this.toggleMap[key] = null;
  }

  remove(key: string) {
    delete this.toggleMap[key];
  }

  isShown(key: string): boolean {
    return this.toggleMap[key];
  }

  show(key: string) {
    this.toggleMap[key] = true;
  }

  hide(key: string) {
    this.toggleMap[key] = false;
  }
}
