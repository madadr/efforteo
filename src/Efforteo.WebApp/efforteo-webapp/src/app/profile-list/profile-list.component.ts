import {Component, OnDestroy, OnInit} from '@angular/core';
import {AccountBrief} from '../model/account-brief';
import {Alert} from '../alert';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {StatsService} from '../stats.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {UserTotalStats} from '../model/user-total-stats';
import {throwError} from 'rxjs';

@Component({
  selector: 'app-profile-list',
  templateUrl: './profile-list.component.html',
  styleUrls: ['./profile-list.component.css']
})
export class ProfileListComponent implements OnInit, OnDestroy {
  accountList: AccountBrief[] = null;
  accountsAvailable = false;

  onCreateLoaderName = 'onCreateLoader';
  accountLoadFailAlert = new Alert('danger', 'Account service is not available. Failed to fetch account data. Try to refresh later.');

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router,
              private toggleService: LoadingService,
              private accountService: AccountService,
              private statsService: StatsService,
              private authService: AuthService,
              private alertService: AlertService) {
  }

  ngOnInit() {
    this.toggleService.create(this.onCreateLoaderName);
    this.loadAccountList();
  }

  ngOnDestroy(): void {
    this.toggleService.remove(this.onCreateLoaderName);
  }

  private loadAccountList() {
    this.toggleService.show(this.onCreateLoaderName);
    this.accountService.getAllUsers()
      .pipe(map(resp => {
          const accounts = <AccountBrief[]>JSON.parse(JSON.stringify(resp.body));

          if (accounts != null) {
            this.accountList = accounts;
            this.alertService.remove(this.accountLoadFailAlert);
            this.accountsAvailable = true;
          }
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.accountLoadFailAlert)) {
              this.alertService.add(this.accountLoadFailAlert);
              this.accountsAvailable = false;
            }
          }
          return throwError(err);
        }),
        finalize(() => {
          this.toggleService.hide(this.onCreateLoaderName);
        }))
      .subscribe(() => {
      });
  }
}
