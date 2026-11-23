import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Station } from '../models/station.model';
import { PagedResponse } from '../../../shared/models/paged-response.model';
@Injectable({
  providedIn: 'root'
})
export class Stations {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7151/api/stations';

  getAll(
    pageNumber: number,
    pageSize: number
  ): Observable<PagedResponse<Station>> {
    return this.http.get<PagedResponse<Station>>(this.apiUrl, {
      params: {
        pageNumber,
        pageSize
      }
    });
  }
}