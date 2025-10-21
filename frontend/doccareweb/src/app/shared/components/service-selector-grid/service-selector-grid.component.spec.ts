import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ServiceSelectorGridComponent } from './service-selector-grid.component';

describe('ServiceSelectorGridComponent', () => {
  let component: ServiceSelectorGridComponent;
  let fixture: ComponentFixture<ServiceSelectorGridComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ServiceSelectorGridComponent]
    });
    fixture = TestBed.createComponent(ServiceSelectorGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
