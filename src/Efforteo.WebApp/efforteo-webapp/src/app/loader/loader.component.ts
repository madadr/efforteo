import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {LoadingService} from '../loading.service';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent {
  @Input() size = 35;
}
