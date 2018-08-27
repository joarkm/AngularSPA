import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SPABgComponent } from './spa-bg.component';

describe('SPABgComponent', () => {
  let component: SPABgComponent;
  let fixture: ComponentFixture<SPABgComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SPABgComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SPABgComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
