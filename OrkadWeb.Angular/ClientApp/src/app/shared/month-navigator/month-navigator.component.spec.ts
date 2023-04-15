import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthNavigatorComponent } from './month-navigator.component';

describe('MonthNavigatorComponent', () => {
  let component: MonthNavigatorComponent;
  let fixture: ComponentFixture<MonthNavigatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MonthNavigatorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonthNavigatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
