import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {AlertService} from '../alert.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-add-activity',
  templateUrl: './add-activity.component.html',
  styleUrls: ['./add-activity.component.css']
})
export class AddActivityComponent implements OnInit {
  addActivityForm: FormGroup;
  submitted = false;

  constructor(private formBuilder: FormBuilder,
              private alertService: AlertService,
              private router: Router) { }

  get f() { return this.addActivityForm.controls; } // accessibel from HTML

  ngOnInit() {
    this.addActivityForm = this.formBuilder.group({
      title: ['', Validators.compose([Validators.required, Validators.maxLength(50)])],
      category: [''],
      distance: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(5000)])],
      timeHour: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(90)])],
      timeMin: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      timeSec: [0, Validators.compose([Validators.required, Validators.min(0), Validators.max(59)])],
      description: ['', [Validators.maxLength(1000)]],
    });
  }

  onSubmit() {
    this.submitted = true;

    
  }
}
