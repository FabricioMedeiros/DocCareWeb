import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthplanComponent } from './healthplan.component';

describe('HealthplanComponent', () => {
  let component: HealthplanComponent;
  let fixture: ComponentFixture<HealthplanComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HealthplanComponent]
    });
    fixture = TestBed.createComponent(HealthplanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
