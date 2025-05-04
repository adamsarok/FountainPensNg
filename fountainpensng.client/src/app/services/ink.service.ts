import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Ink } from '../../dtos/Ink';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InkService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getInks(): Observable<InkForListDTO[]> {
    return this.http.get<InkForListDTO[]>(this.baseUrl + 'inks/');
  }
  createInk(model: Ink) {
    return this.http.post<Ink>(this.baseUrl + 'inks/', model);
  }
  getInk(id: number) {
    return this.http.get<Ink>(this.baseUrl + 'inks/' + id);
  }
  updateInk(ink: Ink) {
    return this.http.put<Ink>(this.baseUrl + 'inks/' + ink.id, ink);
  }
  deleteInk(id: number) {
    return this.http.delete(this.baseUrl + 'inks/' + id);
  }
}
