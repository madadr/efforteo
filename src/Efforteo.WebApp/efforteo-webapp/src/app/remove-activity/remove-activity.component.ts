import {Component, OnDestroy, OnInit} from '@angular/core';
import {Activity} from '../model/activity';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {CategoryValidator} from '../form-validators/category-validator';
import {HttpResponse} from '@angular/common/http';
import {Alert} from '../alert';

@Component({
  selector: 'app-remove-activity',
  templateUrl: './remove-activity.component.html',
  styleUrls: ['./remove-activity.component.css']
})
export class RemoveActivityComponent implements OnInit, OnDestroy {
  id: string;
  activity: Activity = null;
  isOwner: boolean;

  onCreateLoaderName = 'onCreateLoader';
  onSubmitLoaderName = 'onSubmitLoader';

  private sub: any;

  constructor(private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router,
              private toggleService: LoadingService,
              private accountService: AccountService,
              private authService: AuthService,
              private alertService: AlertService,
              private formBuilder: FormBuilder,
              private calendar: NgbCalendar) {
  }

  ngOnInit() {
    this.toggleService.create(this.onCreateLoaderName);
    this.toggleService.create(this.onSubmitLoaderName);
    this.alertService.clear();

    this.sub = this.route.params.subscribe(params => {
      this.id = params['id'];
    });

    this.loadActivityDetails();
  }

  loadActivityDetails() {
    this.toggleService.show(this.onCreateLoaderName);
    this.activityService.getActivity(this.id)
      .pipe(map(resp => {
          this.activity = <Activity>JSON.parse(JSON.stringify(resp.body));
          console.log('Activity = ' + JSON.stringify(this.activity));

          this.checkIfOwner();
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

  checkIfOwner() {
    this.authService.getId()
      .pipe(map(resp => {
          // @ts-ignore
          if (resp.body.id != null && resp.body.id === this.activity.userId) {
            this.isOwner = true;
          }
        }),
        catchError(err => {
          this.isOwner = false;
          return throwError(err);
        }),
        timeout(new Date(new Date().getTime() + 3000)))
      .subscribe(() => {
      });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.toggleService.remove(this.onCreateLoaderName);
    this.toggleService.remove(this.onSubmitLoaderName);
  }

  onSubmit() {
    this.alertService.clear();
    this.toggleService.show(this.onSubmitLoaderName);

    this.activityService.removeActivity(this.activity.id)
      .pipe(map((resp: HttpResponse<any>) => {
        console.log('Remove activity: successful' + resp);
        this.activity = null;
        this.alertService.clear();
        this.router.navigate(['/activities']);
      }),
        finalize(() => {
          this.toggleService.hide(this.onSubmitLoaderName);
      }))
      .subscribe(
        () => {
        },
        response => {
          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', 'Failed to edit activity. ' + response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
        });
  }
}
