import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PlanTripRequest } from '../models/plan-trip-request.model';
import { TripPlanResponse } from '../models/trip-plan-response.model';

@Injectable({
    providedIn: 'root'
})
export class TripPlanner {
    private readonly http = inject(HttpClient);

    private readonly apiUrl = 'https://localhost:7151/api/trips/plan';

    planTrip(request: PlanTripRequest): Observable<TripPlanResponse> {
        return this.http.post<TripPlanResponse>(this.apiUrl, request);
    }
}