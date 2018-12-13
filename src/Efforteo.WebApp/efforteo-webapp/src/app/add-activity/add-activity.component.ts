import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';
import {ActivityService} from '../activity.service';
import {first} from 'rxjs/operators';
import {Alert} from '../alert';
import {CategoryValidator} from '../form-validators/category-validator';

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
      .pipe(first())
      .subscribe(
        (val) => {
          console.log('Add activity: successful', val);
          this.alertService.add(new Alert('success', 'Your request is being processed'));
          this.submitted = false;
          this.ngOnInit();
        },
        response => {
          // console.log('Add activity: error; response.error' + response.error);
          if (response.error.message != null) {
            this.alertService.add(new Alert('danger', 'Failed to add activity. ' + response.error.message));
          } else {
            this.alertService.add(new Alert('warning', 'Service is currently not available. Please try again later.'));
          }
        });
  }
}
