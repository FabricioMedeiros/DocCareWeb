import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthplanFormComponent } from './healthplan-form.component';

describe('HealthplanFormComponent', () => {
  let component: HealthplanFormComponent;
  let fixture: ComponentFixture<HealthplanFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HealthplanFormComponent]
    });
    fixture = TestBed.createComponent(HealthplanFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
