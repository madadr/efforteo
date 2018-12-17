import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StatsService {

  constructor(private http: HttpClient) { }

  getStats(activityId: string) {
    console.log('called get stats for activityId =' + activityId);
    return this.http.get(`/api/stats/activity/` + activityId, {observe: 'response'});
  }

  getPeriodStats(userId: string, days: number) {
    console.log('called get stats for userId =' + userId + ', days = ' + days);
    return this.http.get(`/api/stats/period/` + userId + '/' + days, {observe: 'response'});
  }
}
