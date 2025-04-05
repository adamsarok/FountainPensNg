import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ColorService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCieLchDistance(colorHex: string): Observable<number> {
    const params = new HttpParams()
      .set('color', colorHex);
    
    return this.http.get<number>(`${this.baseUrl}Color/CieLchDistance`, { params });
  }
}
