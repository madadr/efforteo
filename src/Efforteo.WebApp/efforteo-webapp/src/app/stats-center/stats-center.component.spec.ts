import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StatsCenterComponent } from './stats-center.component';

describe('StatsCenterComponent', () => {
  let component: StatsCenterComponent;
  let fixture: ComponentFixture<StatsCenterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StatsCenterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StatsCenterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
