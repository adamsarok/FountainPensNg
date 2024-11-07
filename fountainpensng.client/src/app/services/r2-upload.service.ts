import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { catchError, map, Observable, of } from 'rxjs';

interface UploadResult {
  guid?: string;
  errorMsg?: string;
}

@Injectable({
  providedIn: 'root',
})
export class R2UploadService {
  r2ApiUrl = environment.r2ApiUrl;

  constructor(private http: HttpClient) {}

  uploadFile(file: File | null): Observable<UploadResult> {
    if (!file) return of({ errorMsg: 'File not selected' });
    if (!this.r2ApiUrl)
      return of({ errorMsg: 'R2 API not set in envirment variable' });
    return this.http
      .put<UploadResult>(
        `${this.r2ApiUrl}/upload-image?fileName=${encodeURIComponent(
          file.name
        )}`,
        file,
        {}
      )
      .pipe(
        map((r) => {
          return r;
        }),
        catchError((error: HttpErrorResponse) => {
          const errorMsg =
            error.error instanceof ErrorEvent
              ? error.error.message
              : `${error.status} - ${error.message}`;
          return of({ errorMsg });
        })
      );
  }

  getImageUrl(imageObjectKey: string | undefined) {
    if (!imageObjectKey || !this.r2ApiUrl) return '';
    return `${this.r2ApiUrl}/cached-image?key=${encodeURIComponent(
      imageObjectKey
    )}`;
  }
}
