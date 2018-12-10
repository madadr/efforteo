import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent implements OnInit {
  loginForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private alertService: AlertService) { }

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  get f() { return this.loginForm.controls; } // accessibel from HTML

  onSubmit() {
    this.submitted = true;

    if (this.loginForm.invalid) {
      return;
    }
    this.alertService.clear();

    // this.authService.singIn(this.loginForm.controls['email'].value,
    //   this.loginForm.controls['password'].value)
    //   .pipe(first())
    //   .subscribe(
    //     (val) => {
    //       console.log('Sign In: successful', val);
    //       this.alertService.add(new Alert('success', 'Successfully registered!'));
    //       this.submitted = false;
    //       this.ngOnInit();
    //     },
    //     response => {
    //       console.log('Sign Up: error; response.error.code' + response.error.code);
    //       console.log('Sign Up: error; response.error.message' + response.error.message);
    //       this.alertService.add(new Alert('danger', 'Failed to register.\n' + response.error.message));
    //     });
  }
}
