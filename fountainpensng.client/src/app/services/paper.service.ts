import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Paper } from '../../dtos/Paper';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaperService {

  constructor(private http: HttpClient) { }

  getPapers(): Observable<Paper[]> {
    return this.http.get<Paper[]>('/api/papers/');
  }
  createPaper(model: Paper) {
    return this.http.post<Paper>('/api/papers/', model);
  }
  getPaper(id: number) {
    return this.http.get<Paper>(`/api/papers/${id}`);
  }
  updatePaper(paper: Paper) {
    return this.http.put<Paper>(`/api/papers/${paper.id}`, paper);
  }
  deletePaper(id: number) {
    return this.http.delete(`/api/papers/${id}`);
  }
}
