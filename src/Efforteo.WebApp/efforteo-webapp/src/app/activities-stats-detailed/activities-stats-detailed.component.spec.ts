import {async, ComponentFixture, TestBed} from '@angular/core/testing';

import {ActivitiesStatsDetailedComponent} from './activities-stats-detailed.component';

describe('ActivitiesStatsDetailedComponent', () => {
  let component: ActivitiesStatsDetailedComponent;
  let fixture: ComponentFixture<ActivitiesStatsDetailedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ActivitiesStatsDetailedComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActivitiesStatsDetailedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
