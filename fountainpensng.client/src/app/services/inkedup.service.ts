import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpForListDTO, InkedUpUploadDTO } from '../../dtos/InkedUpDTO';

@Injectable({
  providedIn: 'root'
})
export class InkedupService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getInkedUps(): Observable<InkedUpForListDTO[]> {
    return this.http.get<InkedUpForListDTO[]>(`${this.baseUrl}/api/inked-ups/`);
  }
  getInkedUp(id: number): Observable<InkedUpForListDTO> {
    return this.http.get<InkedUpForListDTO>(`${this.baseUrl}/api/inked-ups/${id}`);
  }
  createInkedUp(inkup: InkedUpUploadDTO) {
    return this.http.post<InkedUpUploadDTO>(`${this.baseUrl}/api/inked-ups/`, inkup);
  }
  updateInkedUp(inkup: InkedUpUploadDTO) {
    return this.http.put<InkedUpUploadDTO>(`${this.baseUrl}/api/inked-ups/${inkup.id}`, inkup);
  }
  deleteInkedUp(id: number) {
    return this.http.delete<InkedUpUploadDTO>(`${this.baseUrl}/inked-ups/${id}`);
  }

}
