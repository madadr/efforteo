import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  addActivity(title: string, category: string, distance: number, time: number, description: string) {
    console.log('called add activity');
    return this.http.post(`/api/activities/`,
      JSON.stringify({title: title,
                            category: category,
                            distance: distance,
                            time: time,
                            description: description}));
  }
}
