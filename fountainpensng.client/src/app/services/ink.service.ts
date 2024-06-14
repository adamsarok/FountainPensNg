import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Ink } from '../../dtos/Ink';

@Injectable({
  providedIn: 'root'
})
export class InkService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getInks() {
    return this.http.get<Ink[]>(this.baseUrl + 'Inks/');
  }

  createInk(model: any) {
    return this.http.post<Ink>(this.baseUrl + 'Inks/', model);
  }
}
