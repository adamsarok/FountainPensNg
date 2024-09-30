import { TestBed } from '@angular/core/testing';
import { R2UploadService } from './r2-upload.service';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';

describe('R2UploadService', () => {
  let service: R2UploadService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ]
    });
    service = TestBed.inject(R2UploadService);
  });

//

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
