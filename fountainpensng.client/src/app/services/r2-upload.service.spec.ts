import { TestBed } from '@angular/core/testing';

import { R2UploadService } from './r2-upload.service';

describe('R2UploadService', () => {
  let service: R2UploadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(R2UploadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
