import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpecialtyAppComponent } from './specialty.app.component';

describe('SpecialtyComponent', () => {
  let component: SpecialtyAppComponent;
  let fixture: ComponentFixture<SpecialtyAppComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SpecialtyAppComponent]
    });
    fixture = TestBed.createComponent(SpecialtyAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
