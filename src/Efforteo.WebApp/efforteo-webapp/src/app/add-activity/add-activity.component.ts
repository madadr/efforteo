import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';
import {ActivityService} from '../activity.service';
import {map} from 'rxjs/operators';
import {Alert} from '../alert';
import {CategoryValidator} from '../form-validators/category-validator';
import {HttpResponse} from '@angular/common/http';
import {NgbCalendar, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-add-activity',
  templateUrl: './add-activity.component.html',
  styleUrls: ['./add-activity.component.css']
})
export class AddActivityComponent implements OnInit {
  addActivityForm: FormGroup;
  submitted = false;
  categories = ['run', 'ride', 'swim'];

  today = this.calendar.getToday();
  datePickerModel: NgbDateStruct;
  timePickerCtrl = new FormControl('', (control: FormControl) => {
    return null;
  });
  datePickerCtrl = new FormControl('', (control: FormControl) => {
    return null;
  });

  constructor(private formBuilder: FormBuilder,
              private alertService: AlertService,
              private activityService: ActivityService,
              private calendar: NgbCalendar) {
  }

  get f() {
    return this.addActivityForm.controls;
  }

  ngOnInit() {
    this.addActivityForm = this.formBuilder.group({
      title: ['', Validators.compose([Validators.required, Validators.maxLength(100)])],
      category: ['', CategoryValidator(this.categories)],
      distance: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(5000)])],
      timeHour: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(90)])],
      timeMin: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      timeSec: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      description: ['', [Validators.maxLength(1000)]]
    });

    const date = new Date();
    this.timePickerCtrl.setValue({hour: date.getHours(), minute: date.getMinutes()});
    this.datePickerModel = this.calendar.getToday();
  }

  onSubmit() {
    this.submitted = true;

    if (this.addActivityForm.invalid) {
      return;
    }
    this.alertService.clear();

    const timeInSec = this.addActivityForm.controls['timeSec'].value
      + 60 * this.addActivityForm.controls['timeMin'].value
      + 3600 * this.addActivityForm.controls['timeHour'].value;
    const date = new Date(this.datePickerModel.year,
      this.datePickerModel.month - 1,
      this.datePickerModel.day,
      this.timePickerCtrl.value.hour,
      this.timePickerCtrl.value.minute,
      0,
      0).toJSON();
    this.activityService.addActivity(this.addActivityForm.controls['title'].value,
      this.addActivityForm.controls['category'].value,
      this.addActivityForm.controls['distance'].value,
      timeInSec,
      this.addActivityForm.controls['description'].value,
      date)
      .pipe(map((resp: HttpResponse<any>) => {
        console.log('Add activity: successful' + resp);
        if (resp != null) {
          let id = resp.headers.get('location');
          id = id.substring(id.lastIndexOf('/') + 1, id.length + 1);
          console.log('Location: ' + resp.headers.get('location') + ', id=' + id);
          this.alertService.add(new Alert('success', 'Your request is being processed. ' +
            'Your activity will be available', '/show-activity/' + id));
          this.submitted = false;
          this.ngOnInit();
        } else {
          this.alertService.add(new Alert('success', 'Your request is being processed.'));
        }
      }))
      .subscribe(
        () => {
        },
        response => {
          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', 'Failed to add activity. ' + response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
        });
  }
}
