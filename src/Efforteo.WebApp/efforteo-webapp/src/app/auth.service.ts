import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';

@Injectable({providedIn: 'root'})
export class AuthService {
  constructor(private http: HttpClient,
              private router: Router) {
  }

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
    // console.log('currentUser isNull? ' + (user === null).valueOf());
    return user != null;
  }

  getId() {
    console.log('called getId');
    const user = JSON.parse(localStorage.getItem('currentUser'));
    if (!user || !user.userId) {
      this.router.navigateByUrl(`/sign-in`);
    } else {
      return user.userId;
    }
  }
}
