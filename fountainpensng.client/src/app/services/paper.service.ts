import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Paper } from '../../dtos/Paper';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaperService {

  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getPapers(): Observable<Paper[]> {
    return this.http.get<Paper[]>(this.baseUrl + 'Papers/');
  }
  createPaper(model: Paper) {
    return this.http.post<Paper>(this.baseUrl + 'Papers/', model);
  }
  getPaper(id: number) {
    return this.http.get<Paper>(this.baseUrl + 'Papers/' + id);
  }
  updatePaper(paper: Paper) {
    return this.http.put<Paper>(this.baseUrl + 'Papers/' + paper.id, paper);
  }
  deletePaper(id: number) {
    return this.http.delete(this.baseUrl + 'Papers/' + id);
  }
}
