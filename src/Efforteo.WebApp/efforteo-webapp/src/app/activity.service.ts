import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) {
  }

  addActivity(title: string, category: string, distance: number, time: number, description: string, createdAt: string) {
    console.log('called add activity');
    return this.http.post(`/api/activities/`,
      JSON.stringify({
        title: title,
        category: category,
        distance: distance,
        time: time,
        description: description,
        createdAt: createdAt
      }), {observe: 'response'});
  }

  editActivity(id: string, title: string, category: string, distance: number, time: number, description: string, createdAt: string) {
    console.log('called edit activity');
    return this.http.put(`/api/activities/activity/`,
      JSON.stringify({
        id: id,
        title: title,
        category: category,
        distance: distance,
        time: time,
        description: description,
        createdAt: createdAt
      }), {observe: 'response'});
  }

  getActivity(id: string) {
    console.log('called get activity for id =' + id);
    return this.http.get(`/api/activities/activity/` + id, {observe: 'response'});
  }

  getAllActivities() {
    console.log('called get all activities');
    return this.http.get(`/api/activities`, {observe: 'response'});
  }

  removeActivity(id: string) {
    console.log('called remove activity for id =' + id);
    return this.http.delete(`/api/activities/activity/` + id, {observe: 'response'});
  }
}
