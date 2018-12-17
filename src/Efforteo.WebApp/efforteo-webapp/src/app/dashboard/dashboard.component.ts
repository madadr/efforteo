import {Component, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {Activity} from '../model/activity';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {AuthService} from '../auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit, OnDestroy {
  lastCommunityActivity: Activity = null;
  lastUserActivity: Activity = null;

  lastCommunityActivityLoaderName = 'communityLoader';
  lastUserActivityLoaderName = 'userLoader';
  private userId: string;

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private authService: AuthService,
              private router: Router,
              private toggleService: LoadingService) {
  }

  ngOnInit() {
    this.toggleService.create(this.lastCommunityActivityLoaderName);
    this.toggleService.create(this.lastUserActivityLoaderName);

    this.loadData();
  }

  ngOnDestroy() {
    this.toggleService.remove(this.lastCommunityActivityLoaderName);
    this.toggleService.remove(this.lastUserActivityLoaderName);
  }

  private loadData() {
    this.toggleService.show(this.lastCommunityActivityLoaderName);
    this.authService.getId()
      .pipe(map(resp => {
          // @ts-ignore
          if (resp.body.id != null) {
            // @ts-ignore
            this.userId = resp.body.id;

            this.loadLastActivities();
          }
        }),
        catchError(err => {
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
          this.toggleService.hide(this.lastCommunityActivityLoaderName);
        } ))
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
