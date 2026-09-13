import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { InkedUpSuggestionDTO } from '../../dtos/InkedUpSuggestionDTO';

@Injectable({
  providedIn: 'root'
})
export class RandomsService {

  constructor(private http: HttpClient) { }

  getRandoms(count: number): Observable<InkedUpSuggestionDTO[]> {
    return this.http.get<InkedUpSuggestionDTO[]>(`/api/randoms/${count}`);
  }
}
