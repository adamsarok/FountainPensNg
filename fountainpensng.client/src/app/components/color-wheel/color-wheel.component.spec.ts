import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ColorWheelComponent } from './color-wheel.component';
import { ActivatedRoute } from '@angular/router';

describe('ColorWheelComponent', () => {
  let component: ColorWheelComponent;
  let fixture: ComponentFixture<ColorWheelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ColorWheelComponent],
      providers: [
        { provide: ActivatedRoute, useValue: {} }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ColorWheelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
