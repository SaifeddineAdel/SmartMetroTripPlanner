import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MetroLine } from '../models/metro-line.model';

@Injectable({
  providedIn: 'root'
})
export class LineStations {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7151/api/metro-lines';

  getById(id: number): Observable<MetroLine> {
    return this.http.get<MetroLine>(`${this.apiUrl}/${id}`);
  }
}