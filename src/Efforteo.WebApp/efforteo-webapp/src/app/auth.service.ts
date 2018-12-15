import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({providedIn: 'root'})
export class AuthService {
  constructor(private http: HttpClient) { }

  signUp(name: string, email: string, password: string) {
    console.log('called sign up');
    return this.http.post(`/api/authentication/register`, JSON.stringify({name: name, email: email, password: password}));
  }

  signIn(email: string, password: string) {
    console.log('called sign in');
    return this.http.post(`/api/authentication/login`, JSON.stringify({email: email, password: password}));
  }

  logOut() {
    console.log('logout called ');
    localStorage.removeItem('currentUser');
  }

  isAuthenticated() {
    const user = localStorage.getItem('currentUser');
    console.log('currentUser isNull? ' + (user === null).valueOf());
    return user != null;
  }

  getId() {
    console.log('called getId');
    return this.http.get(`/api/authentication/id`, {observe: 'response'});
  }
}
