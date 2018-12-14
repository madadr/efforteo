import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  getUserData(userId: string) {
    console.log('called getUserData for userId =' + userId);
    return this.http.get(`/api/accounts/` + userId, {observe: 'response'});
  }
}
