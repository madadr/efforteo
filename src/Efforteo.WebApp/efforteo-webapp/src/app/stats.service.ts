import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class StatsService {

  constructor(private http: HttpClient) {
  }

  getStats(activityId: string) {
    console.log('called get stats for activityId =' + activityId);
    return this.http.get(`/api/stats/activity/` + activityId, {observe: 'response'});
  }

  getPeriodStats(userId: string, days: number) {
    console.log('called get period stats for userId =' + userId + ', days = ' + days);
    return this.http.get(`/api/stats/period/` + userId + '/' + days, {observe: 'response'});
  }

  getTotalStats(userId: string) {
    console.log('called get total stats for userId =' + userId);
    return this.http.get(`/api/stats/total/` + userId, {observe: 'response'});
  }

  getDetailedStats(userId: string) {
    console.log('called get total stats for userId =' + userId);
    return this.http.get(`/api/stats/detailed/` + userId, {observe: 'response'});
  }
}
