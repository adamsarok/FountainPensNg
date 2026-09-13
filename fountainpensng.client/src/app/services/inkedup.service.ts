import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpForListDTO, InkedUpUploadDTO } from '../../dtos/InkedUpDTO';

@Injectable({
  providedIn: 'root'
})
export class InkedupService {

  constructor(private http: HttpClient) { }

  getInkedUps(): Observable<InkedUpForListDTO[]> {
    return this.http.get<InkedUpForListDTO[]>('/api/inked-ups/');
  }
  getInkedUp(id: number): Observable<InkedUpForListDTO> {
    return this.http.get<InkedUpForListDTO>(`/api/inked-ups/${id}`);
  }
  createInkedUp(inkup: InkedUpUploadDTO) {
    return this.http.post<InkedUpUploadDTO>('/api/inked-ups/', inkup);
  }
  updateInkedUp(inkup: InkedUpUploadDTO) {
    return this.http.put<InkedUpUploadDTO>(`/api/inked-ups/${inkup.id}`, inkup);
  }
  deleteInkedUp(id: number) {
    return this.http.delete<InkedUpUploadDTO>(`/api/inked-ups/${id}`);
  }

}
