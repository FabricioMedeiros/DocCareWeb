import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MenuLoginComponent } from './menu-login.component';

describe('MenuLoginComponent', () => {
  let component: MenuLoginComponent;
  let fixture: ComponentFixture<MenuLoginComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [MenuLoginComponent]
    });
    fixture = TestBed.createComponent(MenuLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
