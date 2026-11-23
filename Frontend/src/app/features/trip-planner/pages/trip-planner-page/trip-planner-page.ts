import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { Stations } from '../../../stations/services/stations';
import { Station } from '../../../stations/models/station.model';

import { TripPlanner } from '../../services/trip-planner';
import { TripPlanResponse } from '../../models/trip-plan-response.model';
import { PlanTripRequest } from '../../models/plan-trip-request.model';

@Component({
  imports: [FormsModule],
  selector: 'app-trip-planner-page',
  styleUrl: './trip-planner-page.css',
  templateUrl: './trip-planner-page.html',
})
export class TripPlannerPage implements OnInit {
  private readonly stationsService = inject(Stations);
  private readonly tripPlannerService = inject(TripPlanner);

  stations = signal<Station[]>([]);

  startStationId = signal<number | null>(null);
  destinationStationId = signal<number | null>(null);

  tripPlan = signal<TripPlanResponse | null>(null);

  loading = signal(false);
  planning = signal(false);
  stationError = signal('');
  planError = signal('');

  ngOnInit(): void {
    this.loading.set(true);
    this.stationError.set('');

    this.stationsService.getAll(1, 100).subscribe({
      next: (response) => {
        this.stations.set(response.items);
        this.loading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.stationError.set('Failed to load stations.');
        this.loading.set(false);
      }
    });
  }

  planTrip(): void {
    const startId = this.startStationId();
    const destinationId = this.destinationStationId();

    if (startId === null || destinationId === null) {
      this.planError.set('Please select both stations.');
      return;
    }

    if (startId === destinationId) {
      this.planError.set('Start and destination must be different.');
      return;
    }

    this.planning.set(true);
    this.planError.set('');
    this.tripPlan.set(null);

    const request: PlanTripRequest = {
      startStationId: startId,
      destinationStationId: destinationId
    };

    this.tripPlannerService.planTrip(request).subscribe({
      next: (data) => {
        this.tripPlan.set(data);
        this.planning.set(false);
      },

      error: (err) => {
        console.error(err);
        this.planError.set('Failed to plan trip.');
        this.planning.set(false);
      }
    });
  }
}