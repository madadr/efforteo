import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {Activity} from '../model/activity';
import {catchError, map} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
  selector: 'app-show-activity',
  templateUrl: './show-activity.component.html',
  styleUrls: ['./show-activity.component.css']
})
export class ShowActivityComponent implements OnInit, OnDestroy {
  id: string;
  activity: Activity;

  private sub: any;

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router) { }

  ngOnInit() {
    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.activityService.getActivity(this.id)
      .pipe(map(resp => {
        this.activity = <Activity>JSON.parse(JSON.stringify(resp.body));
        console.log('Activity = ' + JSON.stringify(this.activity));

        if (this.activity == null) {
          this.router.navigateByUrl('/not-found');
        }
      }))
      .subscribe(() => {});
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
