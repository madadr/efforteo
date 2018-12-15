import {Component, OnDestroy, OnInit} from '@angular/core';
import {Activity} from '../model/activity';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {catchError, finalize, map} from 'rxjs/operators';
import {throwError} from 'rxjs';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.css']
})
export class ActivitiesComponent implements OnInit, OnDestroy {
  activities: Activity[];

  onCreateLoaderName = 'onInitLoader';

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router,
              private toggleService: LoadingService) {
  }

  ngOnInit() {
    this.toggleService.create(this.onCreateLoaderName);
    this.loadActivities();
  }

  ngOnDestroy() {
    this.toggleService.remove(this.onCreateLoaderName);
  }

  private loadActivities() {
    this.toggleService.show(this.onCreateLoaderName);
    this.activityService.getAllActivites()
      .pipe(map(resp => {
          this.activities = <Activity[]>JSON.parse(JSON.stringify(resp.body));
        }),
        catchError(err => {
          if (err.status >= 500 && err.status < 600) {
            this.router.navigateByUrl('/not-available');
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
