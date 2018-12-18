import {Component, Input, OnInit} from '@angular/core';
import {Activity} from '../model/activity';
import {ActivityDetailedStats} from '../model/activity-detailed-stats';
import {CategoryDetailedStats} from '../model/category-detailed-stats';
import {Converter} from '../utils/converter';

@Component({
  selector: 'app-activities-stats-detailed',
  templateUrl: './activities-stats-detailed.component.html',
  styleUrls: ['./activities-stats-detailed.component.css']
})
export class ActivitiesStatsDetailedComponent implements OnInit {
  @Input() activities: Activity[];
  @Input() stats: CategoryDetailedStats[];

  public converter = Converter;

  constructor() { }

  ngOnInit() {
  }

  getTitle(stat: ActivityDetailedStats) {
    return this.activities.find(a => a.id === stat.id).title;
  }
}
