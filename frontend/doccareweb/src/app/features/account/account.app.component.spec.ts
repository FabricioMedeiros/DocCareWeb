import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountAppComponent } from './account.app.component';

describe('AccountAppComponent', () => {
  let component: AccountAppComponent;
  let fixture: ComponentFixture<AccountAppComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [AccountAppComponent]
    });
    fixture = TestBed.createComponent(AccountAppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
