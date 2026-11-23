import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Station {
  id: number;
  name: string;
  description: string;
}

export interface MetroLineStation {
  id: number;
  name: string;
  sequence: number;
}

export interface MetroLine {
  id: number;
  name: string;
  stations: MetroLineStation[];
}

export interface LineStation {
  metroLineId: number;
  stationId: number;
  sequence: number;
}

export interface PagedResponse<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7151/api';

  getStations(
    pageNumber = 1,
    pageSize = 10
  ): Observable<PagedResponse<Station>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    return this.http.get<PagedResponse<Station>>(
      `${this.apiUrl}/stations`,
      { params }
    );
  }

  createStation(request: {
    name: string;
    description: string;
  }): Observable<Station> {
    return this.http.post<Station>(
      `${this.apiUrl}/stations`,
      request
    );
  }

  renameStation(
    id: number,
    request: { name: string }
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/stations/${id}`,
      request
    );
  }

  updateStationDescription(
    id: number,
    request: { description: string }
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/stations/${id}/description`,
      request
    );
  }

  deleteStation(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/stations/${id}`
    );
  }

  getMetroLines(): Observable<MetroLine[]> {
    return this.http.get<MetroLine[]>(
      `${this.apiUrl}/metro-lines`
    );
  }

  createMetroLine(request: {
    name: string;
  }): Observable<MetroLine> {
    return this.http.post<MetroLine>(
      `${this.apiUrl}/metro-lines`,
      request
    );
  }

  renameMetroLine(
    id: number,
    request: { name: string }
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/metro-lines/${id}`,
      request
    );
  }

  deleteMetroLine(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/metro-lines/${id}`
    );
  }

  getLineStations(metroLineId: number): Observable<LineStation[]> {
    return this.http.get<LineStation[]>(
      `${this.apiUrl}/metro-lines/${metroLineId}/stations`
    );
  }

  createLineStation(
    metroLineId: number,
    request: {
      stationId: number;
      sequence: number;
    }
  ): Observable<LineStation> {
    return this.http.post<LineStation>(
      `${this.apiUrl}/metro-lines/${metroLineId}/stations`,
      request
    );
  }

  changeLineStationSequence(
    metroLineId: number,
    stationId: number,
    request: { sequence: number }
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/metro-lines/${metroLineId}/stations/${stationId}/sequence`,
      request
    );
  }

  deleteLineStation(
    metroLineId: number,
    stationId: number
  ): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/metro-lines/${metroLineId}/stations/${stationId}`
    );
  }
}