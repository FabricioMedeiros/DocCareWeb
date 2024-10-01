import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialtyFormComponent } from './specialty-form.component';

describe('SpecialtyFormComponent', () => {
  let component: SpecialtyFormComponent;
  let fixture: ComponentFixture<SpecialtyFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SpecialtyFormComponent]
    });
    fixture = TestBed.createComponent(SpecialtyFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
