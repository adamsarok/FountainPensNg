import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Ink } from '../../dtos/Ink';
import { InkForListDTO } from '../../dtos/InkForListDTO';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InkService {

  constructor(private http: HttpClient) { }

  getInks(): Observable<InkForListDTO[]> {
    return this.http.get<InkForListDTO[]>('/api/inks/');
  }
  createInk(model: Ink) {
    return this.http.post<Ink>('/api/inks/', model);
  }
  getInk(id: number) {
    return this.http.get<Ink>(`/api/inks/${id}`);
  }
  updateInk(ink: Ink) {
    return this.http.put<Ink>(`/api/inks/${ink.id}`, ink);
  }
  deleteInk(id: number) {
    return this.http.delete(`/api/inks/${id}`);
  }
}
