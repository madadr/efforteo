import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';
import {Router} from '@angular/router';
import {first} from 'rxjs/operators';
import {Alert} from '../alert';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder,
              private authService: AuthService,
              private alertService: AlertService,
              private router: Router) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });

    this.authService.logOut();
  }

  get f() { return this.loginForm.controls; } // accessibel from HTML

  onSubmit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }
    this.alertService.clear();

    console.log('email = ' + this.loginForm.controls['email'].value + ', pass = ' + this.loginForm.controls['password'].value)

    this.authService.signIn(this.loginForm.controls['email'].value, this.loginForm.controls['password'].value)
      .pipe(first())
      .subscribe(
        (response) => {
          console.log('Sign in: successful', response);

          // @ts-ignore
          localStorage.setItem('currentUser', JSON.stringify({token: response.token, expires: response.expires}));

          this.router.navigate(['/dashboard']);
        },
        response => {
          console.log('Sign up: error; response.error' + response.error);
          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
        });
  }
}
