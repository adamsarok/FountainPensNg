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
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
    if (!this.apiUrl) {
      throw new Error('API URL is not set in environment configuration.');
    }
  }

  uploadFile(file: File | null): Observable<UploadResult> {
    if (!file) return of({ errorMsg: 'File not selected' });
    const url = `${this.apiUrl}/api/images`;
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
    const url = `${this.apiUrl}/api/images/${encodeURIComponent(
      imageObjectKey
    )}`;
    return this.http.get(`${url}`, { responseType: 'text' }).pipe(
      map((r) => r)
    );
  }
}
