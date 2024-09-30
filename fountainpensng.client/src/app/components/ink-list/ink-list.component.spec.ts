import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InkListComponent } from './ink-list.component';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('InkListComponent', () => {
  let component: InkListComponent;
  let fixture: ComponentFixture<InkListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InkListComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    })
    .compileComponents();

    //const httpTesting = TestBed.inject(HttpTestingController);
    //const req = httpTesting.expectOne('/api/config', 'Request to load the configuration');
    //todo: asserts - https://angular.dev/guide/http/testing

    fixture = TestBed.createComponent(InkListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
