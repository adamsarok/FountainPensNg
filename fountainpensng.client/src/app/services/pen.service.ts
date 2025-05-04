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
    return this.http.get<FountainPen[]>(this.baseUrl + 'fountain-pens/');
  }
  getPen(id: number) {
    return this.http.get<FountainPen>(this.baseUrl + 'fountain-pens/' + id);
  }
  createPen(pen: FountainPen) {
    return this.http.post<FountainPen>(this.baseUrl + 'fountain-pens/', pen);
  }
  updatePen(pen: FountainPen) {
    return this.http.put<FountainPen>(this.baseUrl + 'fountain-pens/' + pen.id, pen);
  }
  deletePen(id: number) {
    return this.http.delete(this.baseUrl + 'fountain-pens/' + id);
  }
}
