import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Pen } from '../../dtos/Pen';

@Injectable({
  providedIn: 'root'
})
export class PenService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPens() {
    return this.http.get<Pen[]>(this.baseUrl + 'FountainPens/');
  }
}
