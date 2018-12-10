import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  signUp(name: string, email: string, password: string) {
    return this.http.post(`/api/authentication/register`, JSON.stringify({name: name, email: email, password: password}));
  }
}
