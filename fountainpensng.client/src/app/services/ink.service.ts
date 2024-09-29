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
    return this.http.get<InkForListDTO[]>(this.baseUrl + 'Inks/');
  }
  createInk(model: Ink) {
    return this.http.post<Ink>(this.baseUrl + 'Inks/', model);
  }
  getInk(id: number) {
    return this.http.get<Ink>(this.baseUrl + 'Inks/' + id);
  }
  updateInk(ink: Ink) {
    return this.http.put<Ink>(this.baseUrl + 'Inks/' + ink.id, ink);
  }
  deleteInk(id: number) {
    return this.http.delete(this.baseUrl + 'Inks/' + id);
  }
}
