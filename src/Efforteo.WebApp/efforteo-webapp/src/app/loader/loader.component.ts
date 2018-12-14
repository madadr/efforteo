import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {LoadingService} from '../loading.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit, OnDestroy {
  @Input() size = 35;

  constructor(public loadingService: LoadingService) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }
}
