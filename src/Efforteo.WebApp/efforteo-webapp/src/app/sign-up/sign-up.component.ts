import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent implements OnInit {
  @Input()
  name = '';
  @Input()
  email = '';
  @Input()
  password = '';

  constructor() { }

  ngOnInit() {
  }

  signUp() {
    console.log('this.name');
    console.log(this.name);
    // this.api.login(
    //   this.email,
    //   this.password
    // )
    //   .subscribe(
    //     r => {
    //       if (r.token) {
    //         this.customer.setToken(r.token);
    //         this.router.navigateByUrl('/dashboard');
    //       }
    //     },
    //     r => {
    //       alert(r.error.error);
    //     });
  }
}
