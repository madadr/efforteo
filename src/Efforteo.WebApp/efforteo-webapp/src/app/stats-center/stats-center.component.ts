import {Component, OnDestroy, OnInit} from '@angular/core';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {StatsService} from '../stats.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {Alert} from '../alert';
import {Activity} from '../model/activity';
import {ActivityDetailedStats} from '../model/activity-detailed-stats';
import {CategoryDetailedStats} from '../model/category-detailed-stats';

@Component({
  selector: 'app-stats-center',
  templateUrl: './stats-center.component.html',
  styleUrls: ['./stats-center.component.css']
})
export class StatsCenterComponent implements OnInit, OnDestroy {
  activities: Activity[];
  activitiesAvailable = false;
  stats: CategoryDetailedStats[];
  statsAvailable = false;
  userId: string = this.authService.getId();

  activityLoaderName = 'activityLoader';
  statsLoaderName = 'statsLoader';

  activitiesLoadFailAlert = new Alert('danger', 'Activity service is not available. Failed to fetch activity data. Try to refresh later.');
  statsLoadFailAlert = new Alert('danger', 'Stats service is not available. Failed to fetch stats data. Try to refresh later.');

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
    this.toggleService.create(this.statsLoaderName);
    this.loadData();
  }

  ngOnDestroy(): void {
    this.toggleService.remove(this.statsLoaderName);
  }

  private loadData() {
    if (!this.statsAvailable) {
      this.loadDetailedStatsData();
    }
    if (!this.activitiesAvailable) {
      this.loadActivityData();
    }
  }

  private loadDetailedStatsData() {
    this.toggleService.show(this.statsLoaderName);
    this.statsService.getDetailedStats(this.userId)
      .pipe(map(resp => {
          const stats = <CategoryDetailedStats[]>JSON.parse(JSON.stringify(resp.body));

          if (stats != null) {
            this.stats = stats;
            console.log(JSON.stringify(this.stats));
            this.alertService.remove(this.statsLoadFailAlert);
          }

          this.statsAvailable = true;
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.statsLoadFailAlert)) {
              this.alertService.add(this.statsLoadFailAlert);
              this.statsAvailable = false;
            }
          }
          return throwError(err);
        }),
        finalize(() => {
          this.toggleService.hide(this.statsLoaderName);
        }))
      .subscribe(() => {
      });
  }

  private loadActivityData() {
    this.toggleService.show(this.activityLoaderName);
    this.activityService.getAllActivites()
      .pipe(map(resp => {
          const activities = <Activity[]>JSON.parse(JSON.stringify(resp.body));
          if (activities) {
            this.activities = activities;
          }

          this.activitiesAvailable = true;
        }),
        catchError(err => {
          if (err.status >= 500 || err.status < 600) {
            if (!this.alertService.has(this.activitiesLoadFailAlert)) {
              this.alertService.add(this.activitiesLoadFailAlert);
              this.activitiesAvailable = false;
            }
          }
          return throwError(err);
        }),
        finalize(() => {
          this.toggleService.hide(this.activityLoaderName);
        }))
      .subscribe(() => {
      });
  }
}
