import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PeriodChartComponent } from './period-chart.component';

describe('PeriodChartComponent', () => {
  let component: PeriodChartComponent;
  let fixture: ComponentFixture<PeriodChartComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeriodChartComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeriodChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
