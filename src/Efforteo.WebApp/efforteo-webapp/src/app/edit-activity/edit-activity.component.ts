import {Component, OnDestroy, OnInit} from '@angular/core';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {throwError} from 'rxjs';
import {Activity} from '../model/activity';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {CategoryValidator} from '../form-validators/category-validator';
import {HttpResponse} from '@angular/common/http';
import {Alert} from '../alert';
import {NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-edit-activity',
  templateUrl: './edit-activity.component.html',
  styleUrls: ['./edit-activity.component.css']
})
export class EditActivityComponent implements OnInit, OnDestroy {
  id: string;
  activity: Activity = null;
  createdAgo: Date;
  isOwner: boolean;

  editActivityForm: FormGroup;
  categories = ['run', 'ride', 'swim'];

  onCreateLoaderName = 'onInitLoader';
  onSubmitLoaderName = 'onSubmitLoader';

  today = this.calendar.getToday();
  datePickerModel: NgbDateStruct;
  timePickerCtrl = new FormControl('', (control: FormControl) => {
    return null;
  });
  datePickerCtrl = new FormControl('', (control: FormControl) => {
    return null;
  });

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

  get f() {
    return this.editActivityForm.controls;
  } // accessibel from HTML

  ngOnInit() {
    this.toggleService.create(this.onCreateLoaderName);
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

          this.createdAgo = new Date(this.activity.createdAt);
          this.checkIfOwner();
          this.initForm();
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
        timeout(new Date(new Date().getTime() + 3000)),
        finalize(() => {
        }))
      .subscribe(() => {
      });
  }

  initForm() {
    this.editActivityForm = this.formBuilder.group({
      title: [this.activity.title, Validators.compose([Validators.required, Validators.maxLength(100)])],
      category: [this.activity.category, CategoryValidator(this.categories)],
      distance: [this.activity.distance, Validators.compose([Validators.required, Validators.min(0), Validators.max(5000)])],
      timeHour: [Math.floor(this.activity.time / 3600), Validators.compose([Validators.required, Validators.min(0), Validators.max(90)])],
      timeMin: [Math.floor((this.activity.time % 3600) / 60),
        Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      timeSec: [this.activity.time % 60, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      description: [this.activity.description, [Validators.maxLength(1000)]],
    });

    const date = new Date(this.activity.createdAt);
    this.timePickerCtrl.setValue({hour: date.getHours(), minute: date.getMinutes()});
    this.datePickerModel = new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate());
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
    this.toggleService.remove(this.onCreateLoaderName);
  }

  onSubmit() {
    if (this.editActivityForm.invalid) {
      return;
    }
    this.alertService.clear();

    const timeInSec = this.editActivityForm.controls['timeSec'].value
      + 60 * this.editActivityForm.controls['timeMin'].value
      + 3600 * this.editActivityForm.controls['timeHour'].value;

    const date = new Date(
      this.datePickerModel.year,
      this.datePickerModel.month - 1,
      this.datePickerModel.day,
      this.timePickerCtrl.value.hour,
      this.timePickerCtrl.value.minute,
      0,
      0).toJSON();

    this.activityService.editActivity(this.activity.id,
      this.editActivityForm.controls['title'].value,
      this.editActivityForm.controls['category'].value,
      this.editActivityForm.controls['distance'].value,
      timeInSec,
      this.editActivityForm.controls['description'].value,
      date
      )
      .pipe(map((resp: HttpResponse<any>) => {
        console.log('Edit activity: successful' + resp);
        this.alertService.add(new Alert('success', 'Your activity was updated successfully!'));
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
