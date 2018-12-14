import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {Activity} from '../model/activity';
import {catchError, delay, finalize, map} from 'rxjs/operators';
import {Observable, throwError} from 'rxjs';
import {LoadingService} from '../loading.service';
import {Converter} from '../utils/converter';
import {AccountService} from '../account.service';

@Component({
  selector: 'app-show-activity',
  templateUrl: './show-activity.component.html',
  styleUrls: ['./show-activity.component.css'],
  providers: [ LoadingService ]
})
export class ShowActivityComponent implements OnInit, OnDestroy {
  id: string;
  activity: Activity;
  userName = '';
  createdAgo: Date;

  onCreateLoaderName = 'onInitLoader';
  userNmaeLoaderName = 'userNameLoader';
  paceLoaderName = 'paceLoader';
  speedLoaderName = 'speedLoader';

  public converter = Converter;

  private sub: any;

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router,
              private toggleService: LoadingService,
              private accountService: AccountService) {
  }

  ngOnInit() {
    this.createLoaders();

    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.loadActivityDetails();
    this.loadUserName();
  }

  createLoaders() {
    this.toggleService.create(this.onCreateLoaderName);
    this.toggleService.create(this.userNmaeLoaderName);
    this.toggleService.create(this.paceLoaderName);
    this.toggleService.create(this.speedLoaderName);
  }

  loadActivityDetails() {
    this.toggleService.show(this.onCreateLoaderName);
    this.activityService.getActivity(this.id)
      .pipe(map(resp => {
          console.log('before');
          this.activity = <Activity>JSON.parse(JSON.stringify(resp.body));
          console.log('after');
          console.log('Activity = ' + JSON.stringify(this.activity));

          if (this.activity == null) {
            this.router.navigateByUrl('/not-found');
          }
          this.createdAgo = new Date(this.activity.createdAt);
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

  loadUserName() {
    this.toggleService.show(this.onCreateLoaderName);
    this.accountService.getUserData(this.activity.userId);
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.toggleService.remove(this.onCreateLoaderName);
    this.toggleService.create(this.userNmaeLoaderName);
    this.toggleService.remove(this.paceLoaderName);
    this.toggleService.remove(this.speedLoaderName);
  }
}
