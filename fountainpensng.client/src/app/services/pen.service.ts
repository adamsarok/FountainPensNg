import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { FountainPen } from '../../dtos/FountainPen';

@Injectable({
  providedIn: 'root'
})
export class PenService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPens() {
    return this.http.get<FountainPen[]>(`${this.baseUrl}/api/fountain-pens/`);
  }
  getPen(id: number) {
    return this.http.get<FountainPen>(`${this.baseUrl}/api/fountain-pens/${id}`);
  }
  createPen(pen: FountainPen) {
    return this.http.post<FountainPen>(`${this.baseUrl}/api/fountain-pens/`, pen);
  }
  updatePen(pen: FountainPen) {
    return this.http.put<FountainPen>(`${this.baseUrl}/api/fountain-pens/${pen.id}`, pen);
  }
  deletePen(id: number) {
    return this.http.delete(`${this.baseUrl}/api/fountain-pens/${id}`);
  }
  emptyPen(id: number) {
    return this.http.put<FountainPen>(`${this.baseUrl}/api/fountain-pens/empty/${id}`, null);
  }
}
