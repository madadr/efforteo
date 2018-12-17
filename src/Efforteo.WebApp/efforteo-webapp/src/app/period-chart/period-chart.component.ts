import {Component, Input, OnInit} from '@angular/core';
import {UserPeriodStats} from '../model/user-period-stats';

@Component({
  selector: 'app-period-chart',
  templateUrl: './period-chart.component.html',
  styleUrls: ['./period-chart.component.css']
})
export class PeriodChartComponent implements OnInit {
  @Input() stats: UserPeriodStats[];
  @Input() statType = 'distance';
  @Input() chartType = 'line';

  // lineChart
  public chartData: Array<any> = [];
  public chartLabels: Array<any> = [];

  constructor() {
  }

  ngOnInit() {
    this.chartData = new Array<any>();
    this.fillData();
    this.fillLabels();
  }

  private fillData() {
    for (const stat of this.stats) {
      if (this.statType === 'distance') {
        this.chartData.push({data: stat.distance, label: stat.category});
      } else if (this.statType === 'time') {
        const time = stat.time.map(t => (t / 3600).toFixed(2));
        this.chartData.push({data: time, label: stat.category});
      }
    }
  }

  private fillLabels() {
    const first = new Date(this.stats[0].periodStart);

    for (let i = 0; i < this.stats[0].days; ++i) {
      const next = new Date(first);
      next.setDate(next.getDate() + i);

      this.chartLabels.push(next.getDate() + '-' + (next.getMonth() + 1));
    }
  }
}
