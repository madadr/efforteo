import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';
import {ActivityService} from '../activity.service';
import {catchError, first, map} from 'rxjs/operators';
import {Alert} from '../alert';
import {CategoryValidator} from '../form-validators/category-validator';
import {HttpResponse} from '@angular/common/http';

@Component({
  selector: 'app-add-activity',
  templateUrl: './add-activity.component.html',
  styleUrls: ['./add-activity.component.css']
})
export class AddActivityComponent implements OnInit {
  addActivityForm: FormGroup;
  submitted = false;
  categories = ['run', 'ride', 'swim'];

  constructor(private formBuilder: FormBuilder,
              private alertService: AlertService,
              private activityService: ActivityService) { }

  get f() { return this.addActivityForm.controls; } // accessibel from HTML

  ngOnInit() {
    this.addActivityForm = this.formBuilder.group({
      title: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      category: ['', CategoryValidator(this.categories)],
      distance: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(5000)])],
      timeHour: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(90)])],
      timeMin: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      timeSec: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      description: ['', [Validators.maxLength(1000)]],
    });
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

    this.activityService.addActivity(this.addActivityForm.controls['title'].value,
      this.addActivityForm.controls['category'].value,
      this.addActivityForm.controls['distance'].value,
      timeInSec,
      this.addActivityForm.controls['description'].value)
      .pipe(map((resp: HttpResponse<any>) => {
        console.log('Add activity: successful' + resp);
        if (resp != null) {
          console.log('Location: ' + resp.headers.get('location'));
          this.alertService.add(new Alert('success', 'Your request is being processed. ' +
            'Your activity will be available', '/show-activity/' + resp.headers.get('location')));
          this.submitted = false;
          this.ngOnInit();
        } else {
          this.alertService.add(new Alert('success', 'Your request is being processed.'));
        }
      }))
      .subscribe(
        () => {},
        response => {
          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', 'Failed to add activity. ' + response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
        });
  }
}
