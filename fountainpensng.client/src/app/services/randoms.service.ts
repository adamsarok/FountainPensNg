import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpSuggestionDTO } from '../../dtos/InkedUpSuggestionDTO';

@Injectable({
  providedIn: 'root'
})
export class RandomsService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getRandoms(count: number): Observable<InkedUpSuggestionDTO[]> {
    return this.http.get<InkedUpSuggestionDTO[]>(`${this.baseUrl}/api/randoms/${count}`);
  }
}
