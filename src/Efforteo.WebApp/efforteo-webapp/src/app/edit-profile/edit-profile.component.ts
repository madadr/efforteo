import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {Account} from '../model/account';
import {ActivatedRoute, Router} from '@angular/router';
import {ActivityService} from '../activity.service';
import {LoadingService} from '../loading.service';
import {AccountService} from '../account.service';
import {StatsService} from '../stats.service';
import {AuthService} from '../auth.service';
import {AlertService} from '../alert.service';
import {catchError, finalize, map, timeout} from 'rxjs/operators';
import {Alert} from '../alert';
import {throwError} from 'rxjs';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {NgbCalendar, NgbDate, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';
import {HttpResponse} from '@angular/common/http';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit, OnDestroy {
  account: Account = null;
  accountAvailable = false;

  editProfileForm: FormGroup;
  today = this.calendar.getToday();
  datePickerModel: NgbDateStruct;
  datePickerCtrl = new FormControl('', (control: FormControl) => {
    return null;
  });

  accountLoaderName = 'accountLoader';
  onSubmitLoaderName = 'onSubmitLoader';

  accountLoadFailAlert = new Alert('danger', 'Account service is not available. Try again later.');

  get f() {
    return this.editProfileForm.controls;
  }

  constructor(private formBuilder: FormBuilder,
              private route: ActivatedRoute,
              private activityService: ActivityService,
              private router: Router,
              private toggleService: LoadingService,
              private accountService: AccountService,
              private statsService: StatsService,
              private authService: AuthService,
              private alertService: AlertService,
              private calendar: NgbCalendar) {
  }

  ngOnInit() {
    this.checkIfOwner();
    this.toggleService.create(this.accountLoaderName);
    this.toggleService.create(this.onSubmitLoaderName);
    this.loadAccountData();
  }

  ngOnDestroy(): void {
    this.toggleService.remove(this.accountLoaderName);
    this.toggleService.remove(this.onSubmitLoaderName);
    this.alertService.clear();
  }

  public checkIfOwner() {
  }

  private loadAccountData() {
    this.toggleService.show(this.accountLoaderName);
    this.accountService.getUserData(this.authService.getId())
      .pipe(map(resp => {
          const account = <Account>JSON.parse(JSON.stringify(resp.body));

          if (account != null) {
            this.account = account;
            this.alertService.remove(this.accountLoadFailAlert);
            this.initForm();
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
        finalize(() => {
          this.toggleService.hide(this.accountLoaderName);
        }))
      .subscribe(() => {
      });
  }

  private initForm() {
    this.editProfileForm = this.formBuilder.group({
        name: [this.account.name, Validators.compose([Validators.required, Validators.minLength(2), Validators.maxLength(200)])],
        email: [this.account.email, Validators.compose([Validators.required, Validators.email, Validators.maxLength(200)])],
        location: [this.account.location, Validators.maxLength(100)],
        weight: [this.account.weight, Validators.compose([Validators.min(0), Validators.max(500)])]
      }
    );

    const date = new Date(this.account.birthday);
    this.datePickerModel = new NgbDate(date.getFullYear(), date.getMonth() + 1, date.getDate());
  }

  onSubmit() {
    if (this.editProfileForm.invalid) {
      return;
    }
    this.toggleService.show(this.onSubmitLoaderName);
    this.alertService.clear();

    let date: string = null;
    if (this.datePickerModel !== null) {
      date = new Date(
        this.datePickerModel.year,
        this.datePickerModel.month - 1,
        this.datePickerModel.day,
        0,
        0,
        0,
        0).toJSON();
    }

    this.accountService.editProfile(this.editProfileForm.controls['email'].value,
      this.editProfileForm.controls['name'].value,
      this.editProfileForm.controls['location'].value,
      this.editProfileForm.controls['weight'].value,
      date
    )
      .pipe(map((resp: HttpResponse<any>) => {
          console.log('Edit account: successful' + resp);
          this.alertService.add(new Alert('success', 'Your acount was updated!'));
        }),
        catchError(err => {
          if (err.error && err.error.message) {
            this.alertService.add(new Alert('danger', 'Failed to edit account. ' + err.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
          return throwError(err);
        }),
        finalize(() => {
          this.toggleService.hide(this.onSubmitLoaderName);
        }))
      .subscribe(
        () => {
        });

  }
}
