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
    return this.http.get<FountainPen[]>(this.baseUrl + 'FountainPens/');
  }
  getPen(id: number) {
    return this.http.get<FountainPen>(this.baseUrl + 'FountainPens/' + id);
  }
  createPen(pen: FountainPen) {
    return this.http.post<FountainPen>(this.baseUrl + 'FountainPens/', pen);
  }
  updatePen(pen: FountainPen) {
    return this.http.put<FountainPen>(this.baseUrl + 'FountainPens/' + pen.id, pen);
  }
  deletePen(id: number) {
    return this.http.delete(this.baseUrl + 'FountainPens/' + id);
  }
}
