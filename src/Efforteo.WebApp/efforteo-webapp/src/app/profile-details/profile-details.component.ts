import {Component, Input, OnInit} from '@angular/core';
import {catchError, finalize, map, retry, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {AuthService} from '../auth.service';
import {Account} from '../model/account';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css']
})
export class ProfileDetailsComponent implements OnInit {
  @Input() account: Account;
  isOwner: boolean;

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    this.checkIfOwner();
  }

  public checkIfOwner() {
    this.authService.getId()
      .pipe(map(resp => {
          if (!this.account) {
            this.checkIfOwner();
          }

          // @ts-ignore
          if (resp.body.id != null && resp.body.id === this.account.id) {
            this.isOwner = true;
          }
        }),
        catchError(err => {
          this.isOwner = false;
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        retry(3))
      .subscribe(() => {
      });
  }
}
