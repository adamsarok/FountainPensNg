import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { SearchResult } from '../../dtos/SearchResult';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FinderService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getSearchResults(fulltext: string): Observable<SearchResult[]> {
    return this.http.get<SearchResult[]>(this.baseUrl + 'Finder/' + fulltext);
  }
}