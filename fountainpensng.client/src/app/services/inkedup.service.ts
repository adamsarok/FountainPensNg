import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpForListDTO } from '../../dtos/InkedUpForListDTO';

@Injectable({
  providedIn: 'root'
})
export class InkedupService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getInkedUps(): Observable<InkedUpForListDTO[]> {
    return this.http.get<InkedUpForListDTO[]>(this.baseUrl + 'InkedUps/');
  }
}
