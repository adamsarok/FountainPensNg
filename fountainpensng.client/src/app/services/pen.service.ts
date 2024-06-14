import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { FountainPen } from '../../dtos/FountainPen';
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PenService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPens() {
    return this.http.get<FountainPen[]>(this.baseUrl + 'FountainPens/');
  }

  createPen(model: any) {
    return this.http.post<FountainPen>(this.baseUrl + 'FountainPens/', model);
  }
}
