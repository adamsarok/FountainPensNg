import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InkListComponent } from './ink-list.component';

describe('InkListComponent', () => {
  let component: InkListComponent;
  let fixture: ComponentFixture<InkListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InkListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InkListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
