import {Component, Input, OnInit} from '@angular/core';
import {UserTotalStats} from '../model/user-total-stats';
import {Converter} from '../utils/converter';

@Component({
  selector: 'app-total-stats',
  templateUrl: './total-stats.component.html',
  styleUrls: ['./total-stats.component.css']
})
export class TotalStatsComponent implements OnInit {
  @Input() stats: UserTotalStats[];

  public converter = Converter;

  constructor() { }

  ngOnInit() {
  }
}
