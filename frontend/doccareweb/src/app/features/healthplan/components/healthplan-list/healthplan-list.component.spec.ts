import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthPlanListComponent } from './healthplan-list.component';

describe('HealthplanListComponent', () => {
  let component: HealthPlanListComponent;
  let fixture: ComponentFixture<HealthPlanListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HealthPlanListComponent]
    });
    fixture = TestBed.createComponent(HealthPlanListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
