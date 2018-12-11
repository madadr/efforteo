import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {first, map} from 'rxjs/operators';
import {Alert} from './alert';
import {JwtToken} from './model/JwtToken';
import {log} from 'util';

@Injectable({providedIn: 'root'})
export class AuthService {
  constructor(private http: HttpClient) { }

  signUp(name: string, email: string, password: string) {
    return this.http.post(`/api/authentication/register`, JSON.stringify({name: name, email: email, password: password}));
  }

  signIn(email: string, password: string) {
    console.log('called sign in');
    this.http.post(`/api/authentication/login`, JSON.stringify({email: email, password: password}))
      .pipe(first())
      .subscribe(
        (response) => {
          console.log('Sign in: successful', response);

          // @ts-ignore
          localStorage.setItem('currentUser', JSON.stringify({token: response.token, expires: response.expires}));

          return null;
        },
        response => {
          console.log('Sign in: error; response.error.code' + response.error.code);
          console.log('Sign in: error; response.error.message' + response.error.message);

          return response.error;
        });
  }

  logOut() {
    console.log('logout called ');
    localStorage.removeItem('currentUser');
  }

  isAuthenticated() {
    const user = localStorage.getItem('currentUser');
    console.log('currentUser = ' + user);
    return user != null;
    // hasToken();
    // this.http.get('/api/authentication/id')
    //   .pipe(first())
    //   .subscribe(
    //     (val) => {
    //       return true;
    //       // console.log('Sign Up: successful', val);
    //       // this.alertService.add(new Alert('success', 'Successfully registered!'));
    //       // this.submitted = false;
    //       // this.ngOnInit();
    //     },
    //     response => {
    //       return false;
    //       // console.log('Sign Up: error; response.error.code' + response.error.code);
    //       // console.log('Sign Up: error; response.error.message' + response.error.message);
    //       // this.alertService.add(new Alert('danger', 'Failed to register.\n' + response.error.message));
    //     });

    // return
  }
}
