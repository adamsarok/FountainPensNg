import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ColorService {

  constructor(private http: HttpClient) { }

  getCieLchDistance(colorHex: string): Observable<number> {
    const params = new HttpParams()
      .set('color', colorHex);
    
    return this.http.get<number>('/api/colors/cie-lch-distance', { params });
  }
}
