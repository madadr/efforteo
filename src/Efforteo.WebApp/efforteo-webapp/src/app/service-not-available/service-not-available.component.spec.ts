import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceNotAvailableComponent } from './service-not-available.component';

describe('ServiceNotAvailableComponent', () => {
  let component: ServiceNotAvailableComponent;
  let fixture: ComponentFixture<ServiceNotAvailableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ServiceNotAvailableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ServiceNotAvailableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
