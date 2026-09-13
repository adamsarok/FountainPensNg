import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';

interface UploadResult {
  guid?: string;
  errorMsg?: string;
}

@Injectable({
  providedIn: 'root',
})
export class R2UploadService {

  constructor(private http: HttpClient) { }

  uploadFile(file: File | null): Observable<UploadResult> {
    if (!file) return of({ errorMsg: 'File not selected' });
    const url = '/api/images';
    const formData = new FormData();
    formData.append('file', file);
    return this.http.put<UploadResult>(`${url}`, formData, {}).pipe(
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

  getImageUrl(imageObjectKey: string | undefined): Observable<string> {
    if (!imageObjectKey) return of('');
    const url = `/api/images/${encodeURIComponent(
      imageObjectKey
    )}`;
    return this.http.get(`${url}`, { responseType: 'text' }).pipe(
      map((r) => r)
    );
  }
}
