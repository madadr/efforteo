import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {Activity} from '../model/activity';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {AuthService} from '../auth.service';
import {StatsService} from '../stats.service';
import {UserPeriodStats} from '../model/user-period-stats';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  lastCommunityActivity: Activity = null;
  lastUserActivity: Activity = null;
  stats: UserPeriodStats[] = null;

  statsLoaderName = 'statsLoader';
  lastCommunityActivityLoaderName = 'communityLoader';
  lastUserActivityLoaderName = 'userLoader';
  private userId: string;

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private authService: AuthService,
              private statsService: StatsService,
              private router: Router,
              private toggleService: LoadingService) {
  }

  ngOnInit() {
    this.toggleService.create(this.statsLoaderName);
    this.toggleService.create(this.lastCommunityActivityLoaderName);
    this.toggleService.create(this.lastUserActivityLoaderName);

    this.loadData();
  }

  ngOnDestroy() {
    this.toggleService.remove(this.statsLoaderName);
    this.toggleService.remove(this.lastCommunityActivityLoaderName);
    this.toggleService.remove(this.lastUserActivityLoaderName);
  }

  private loadData() {
    this.toggleService.show(this.statsLoaderName);
    this.toggleService.show(this.lastCommunityActivityLoaderName);
    this.toggleService.show(this.lastUserActivityLoaderName);
    this.authService.getId()
      .pipe(map(resp => {
          // @ts-ignore
          if (resp.body.id != null) {
            // @ts-ignore
            this.userId = resp.body.id;

            this.loadStatsData();
            this.loadLastActivities();
          }
        }),
        catchError(err => {
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.statsLoaderName);
          this.toggleService.hide(this.lastCommunityActivityLoaderName);
          this.toggleService.hide(this.lastUserActivityLoaderName);
        } ))
      .subscribe(() => {
      });
  }

  private loadStatsData() {
    this.statsService.getPeriodStats(this.userId, 7)
      .pipe(map(resp => {
          const stats = <UserPeriodStats[]>JSON.parse(JSON.stringify(resp.body));

          if (stats != null) {
            this.stats = stats;
            this.toggleService.hide(this.statsLoaderName);
          }
        }),
        catchError(err => {
          return throwError(err);
        }),
        finalize(() => {
        }))
      .subscribe(() => {
      });

  }

  private loadLastActivities() {
    this.activityService.getAllActivites()
      .pipe(map(resp => {
          const activities = <Activity[]>JSON.parse(JSON.stringify(resp.body));
          let activity = activities.find(a => a.userId !== this.userId);
          if (activity != null) {
            this.lastCommunityActivity = activity;
            this.toggleService.hide(this.lastCommunityActivityLoaderName);
          }
          activity = activities.find(a => a.userId === this.userId);
          if (activity != null) {
            this.lastUserActivity = activity;
            this.toggleService.hide(this.lastUserActivityLoaderName);
          }
        }),
        catchError(err => {
          if (err.status >= 500 && err.status < 600) {
            this.router.navigateByUrl('/not-available');
          }
          return throwError(err);
        }),
        finalize(() => {
        }))
      .subscribe(() => {
      });
  }
}
