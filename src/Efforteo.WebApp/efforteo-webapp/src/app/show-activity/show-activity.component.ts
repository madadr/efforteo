import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {Activity} from '../model/activity';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {LoadingService} from '../loading.service';
import {Converter} from '../utils/converter';
import {AccountService} from '../account.service';
import {AlertService} from '../alert.service';
import {Alert} from '../alert';
import {AuthService} from '../auth.service';
import {StatsService} from '../stats.service';
import {ActivityStats} from '../model/activity-stats';
import {Account} from '../model/account';

@Component({
  selector: 'app-show-activity',
  templateUrl: './show-activity.component.html',
  styleUrls: ['./show-activity.component.css'],
  providers: [LoadingService]
})
export class ShowActivityComponent implements OnInit, OnDestroy {
  id: string;
  activity: Activity = null;
  userName: string = null;
  createdAgo: Date;
  activityStats: ActivityStats = null;
  isOwner: boolean;

  onCreateLoaderName = 'onInitLoader';
  userNameLoaderName = 'userNameLoader';
  userNameLoadFailAlert = new Alert('danger', 'Account service is not available. Failed to fetch username. Try to refresh later.');
  statsLoadFailAlert = new Alert('danger', 'Stats service is not available. Failed to fetch statistics. Try to refresh later.');
  paceLoaderName = 'paceLoader';
  speedLoaderName = 'speedLoader';

  public converter = Converter;

  private sub: any;

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
    this.createLoaders();
    this.alertService.clear();

    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.loadActivityDetails();
  }

  createLoaders() {
    this.toggleService.create(this.onCreateLoaderName);
    this.toggleService.create(this.userNameLoaderName);
    this.toggleService.create(this.paceLoaderName);
    this.toggleService.create(this.speedLoaderName);
  }

  loadActivityDetails() {
    this.toggleService.show(this.onCreateLoaderName);
    this.activityService.getActivity(this.id)
      .pipe(map(resp => {
          this.activity = <Activity>JSON.parse(JSON.stringify(resp.body));
          console.log('Activity = ' + JSON.stringify(this.activity));
          this.checkIfOwner();

          if (this.activity == null) {
            this.router.navigateByUrl('/not-found');
          }
          this.createdAgo = new Date(this.activity.createdAt);
          this.loadUserName();
          this.loadStats();
        }),
        catchError(err => {
          if (err.status >= 500 && err.status < 600) {
            this.router.navigateByUrl('/not-available');
          } else {
            this.router.navigateByUrl('/not-found');
          }
          return throwError(err);
        }),
        finalize(() => {
          this.toggleService.hide(this.onCreateLoaderName);
        }))
      .subscribe(() => {
      });
  }

  public loadUserName() {
    this.toggleService.show(this.userNameLoaderName);
    this.accountService.getUserData(this.activity.userId)
      .pipe(map(resp => {
          const account = <Account>JSON.parse(JSON.stringify(resp.body));
          console.log('Account = ' + JSON.stringify(account));

          if (account != null) {
            this.userName = account.name;
            this.alertService.remove(this.userNameLoadFailAlert);
          }
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.userNameLoadFailAlert)) {
              this.alertService.add(this.userNameLoadFailAlert);
            }
          }
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.userNameLoaderName);
        }))
      .subscribe(() => {
      });
  }

  private loadStats() {
    this.toggleService.show(this.paceLoaderName);
    this.toggleService.show(this.speedLoaderName);
    this.statsService.getStats(this.id)
      .pipe(map(resp => {
          const activityStats = <ActivityStats>JSON.parse(JSON.stringify(resp.body));
          console.log('ActivityStats = ' + JSON.stringify(activityStats));

          if (activityStats != null) {
            this.activityStats = activityStats;
            this.alertService.remove(this.statsLoadFailAlert);
          }
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.statsLoadFailAlert)) {
              this.alertService.add(this.statsLoadFailAlert);
            }
          }
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.paceLoaderName);
          this.toggleService.hide(this.speedLoaderName);
        }))
      .subscribe(() => {
      });
  }

  public checkIfOwner() {
    this.isOwner = this.authService.getId() === this.activity.userId;
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.toggleService.remove(this.onCreateLoaderName);
    this.toggleService.remove(this.userNameLoaderName);
    this.toggleService.remove(this.paceLoaderName);
    this.toggleService.remove(this.speedLoaderName);
  }
}
