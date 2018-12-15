import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BriefActivityComponent } from './brief-activity.component';

describe('BriefActivityComponent', () => {
  let component: BriefActivityComponent;
  let fixture: ComponentFixture<BriefActivityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BriefActivityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BriefActivityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
