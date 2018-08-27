import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SPACardComponent } from './spa-card.component';

describe('SPACardComponent', () => {
  let component: SPACardComponent;
  let fixture: ComponentFixture<SPACardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SPACardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SPACardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
