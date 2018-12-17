import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {StatsService} from '../stats.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {UserPeriodStats} from '../model/user-period-stats';
import {Alert} from '../alert';
import {Account} from '../model/account';
import {UserTotalStats} from '../model/user-total-stats';

@Component({
  selector: 'app-show-profile',
  templateUrl: './show-profile.component.html',
  styleUrls: ['./show-profile.component.css']
})
export class ShowProfileComponent implements OnInit, OnDestroy {
  userId: string;
  // totalStats: UserTotalStats[] = null;
  stats7: UserPeriodStats[] = null;
  stats7available = false;
  stats30: UserPeriodStats[] = null;
  stats30available = false;
  account: Account = null;
  accountAvailable = false;
  private sub: any;

  stats7LoaderName = 'stats7Loader';
  stats30LoaderName = 'stats30Loader';
  accountLoaderName = 'accountLoader';

  accountLoadFailAlert = new Alert('danger', 'Account service is not available. Failed to fetch account data. Try to refresh later.');
  statsLoadFailAlert = new Alert('danger', 'Stats service is not available. Failed to fetch statistics. Try to refresh later.');


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
    this.toggleService.create(this.stats7LoaderName);
    this.toggleService.create(this.stats30LoaderName);
    this.toggleService.create(this.accountLoaderName);
    this.sub = this.route.params.subscribe(params => {
      this.userId = params['id'];
    });
    this.loadStats7Data();
    this.loadStats30Data();
    this.loadAccountData();
  }

  ngOnDestroy(): void {
    this.toggleService.remove(this.stats7LoaderName);
    this.toggleService.remove(this.stats30LoaderName);
    this.toggleService.remove(this.accountLoaderName);
    this.sub.unsubscribe();
  }

  private loadStats7Data() {
    this.toggleService.show(this.stats7LoaderName);
    this.statsService.getPeriodStats(this.userId, 7)
      .pipe(map(resp => {
          const stats = <UserPeriodStats[]>JSON.parse(JSON.stringify(resp.body));

          if (stats != null) {
            this.stats7 = stats;
            this.alertService.remove(this.statsLoadFailAlert);
          }

          this.stats7available = true;
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.statsLoadFailAlert)) {
              this.alertService.add(this.statsLoadFailAlert);
              this.stats7available = false;
            }
          }
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.stats7LoaderName);
        }))
      .subscribe(() => {
      });
  }

  private loadStats30Data() {
    this.toggleService.show(this.stats30LoaderName);
    this.statsService.getPeriodStats(this.userId, 30)
      .pipe(map(resp => {
          const stats = <UserPeriodStats[]>JSON.parse(JSON.stringify(resp.body));

          if (stats != null) {
            this.stats30 = stats;
            this.alertService.remove(this.statsLoadFailAlert);
            this.stats30available = true;
          }
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.statsLoadFailAlert)) {
              this.alertService.add(this.statsLoadFailAlert);
              this.stats30available = false;
            }
          }
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.stats30LoaderName);
        }))
      .subscribe(() => {
      });
  }

  private loadAccountData() {
    this.toggleService.show(this.accountLoaderName);
    this.accountService.getUserData(this.userId)
      .pipe(map(resp => {
          const account = <Account>JSON.parse(JSON.stringify(resp.body));

          if (account != null) {
            this.account = account;
            this.alertService.remove(this.accountLoadFailAlert);
          }

          this.accountAvailable = true;
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.accountLoadFailAlert)) {
              this.alertService.add(this.accountLoadFailAlert);
              this.accountAvailable = false;
            }
          }
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.accountLoaderName);
        }))
      .subscribe(() => {
      });
  }
}
