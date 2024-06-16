import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InkedupListComponent } from './inkedup-list.component';

describe('InkedupListComponent', () => {
  let component: InkedupListComponent;
  let fixture: ComponentFixture<InkedupListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InkedupListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InkedupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
