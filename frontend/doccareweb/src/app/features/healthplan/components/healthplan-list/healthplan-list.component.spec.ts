import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthplanListComponent } from './healthplan-list.component';

describe('HealthplanListComponent', () => {
  let component: HealthplanListComponent;
  let fixture: ComponentFixture<HealthplanListComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [HealthplanListComponent]
    });
    fixture = TestBed.createComponent(HealthplanListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
