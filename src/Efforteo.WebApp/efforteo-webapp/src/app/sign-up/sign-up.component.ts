import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {first} from 'rxjs/operators';
import {AlertService} from '../alert.service';
import {Alert} from '../alert';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {ConfirmPasswordValidator} from '../form-validators/confirm-password-validator';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
  registerForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private alertService: AlertService) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      name: ['', Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(200)])],
      email: ['', Validators.compose([Validators.required, Validators.email, Validators.maxLength(200)])],
      password: ['', Validators.compose([Validators.required, Validators.minLength(5), Validators.maxLength(200)])],
      confirmPassword: ['', Validators.compose([Validators.required, ConfirmPasswordValidator('password')])]
    }
    );
  }

  get f() { return this.registerForm.controls; } // accessibel from HTML

  onSubmit() {
    this.submitted = true;

    if (this.registerForm.invalid) {
      return;
    }
    this.alertService.clear();

    this.authService.signUp(this.registerForm.controls['name'].value,
                            this.registerForm.controls['email'].value,
                            this.registerForm.controls['password'].value)
      .pipe(first())
      .subscribe(
        (val) => {
          console.log('Sign Up: successful', val);
          this.alertService.add(new Alert('success', 'Successfully registered!'));
          this.submitted = false;
          this.ngOnInit();
        },
        response => {
          console.log('Sign Up: error; response.error.code' + response.error.code);
          console.log('Sign Up: error; response.error.message' + response.error.message);

          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', 'Failed to register.\n' + response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Sorry! Service is currently not available'));
          }
        });
  }
}
