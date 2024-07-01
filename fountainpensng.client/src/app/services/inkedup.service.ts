import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpForListDTO } from '../../dtos/InkedUpForListDTO';
import { InkedUp } from '../../dtos/InkedUp';

@Injectable({
  providedIn: 'root'
})
export class InkedupService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getInkedUps(): Observable<InkedUpForListDTO[]> {
    return this.http.get<InkedUpForListDTO[]>(this.baseUrl + 'InkedUps/');
  }
  getInkedUp(id: number): Observable<InkedUpForListDTO> {
    return this.http.get<InkedUpForListDTO>(this.baseUrl + 'InkedUps/' + id);
  }
  createInkedUp(inkup: InkedUpForListDTO) {
    return this.http.post<InkedUpForListDTO>(this.baseUrl + 'InkedUps/', inkup);
  }
  updateInkedUp(inkup: InkedUpForListDTO) {
    return this.http.put<InkedUpForListDTO>(this.baseUrl + 'InkedUps/' + inkup.id, inkup);
  }

}
