import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthPlanFormComponent } from './healthplan-form.component';

describe('HealthplanFormComponent', () => {
  let component: HealthPlanFormComponent;
  let fixture: ComponentFixture<HealthPlanFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HealthPlanFormComponent]
    });
    fixture = TestBed.createComponent(HealthPlanFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
