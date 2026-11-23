import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import {
  AdminService,
  MetroLine,
  Station,
  LineStation
} from '../../services/admin.service';

@Component({
  selector: 'app-line-stations-management',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './line-stations-management.html',
  styleUrl: './line-stations-management.css'
})
export class LineStationsManagement implements OnInit {
  private readonly adminService = inject(AdminService);

  lines = signal<MetroLine[]>([]);
  allStations = signal<Station[]>([]);
  lineStations = signal<LineStation[]>([]);

  selectedLineId = signal<number | null>(null);

  loading = signal(false);
  saving = signal(false);

  selectedStationId: number | null = null;
  newSequence = 1;

  availableStations = () => {
    const assignedIds = new Set(
      this.lineStations().map(item => item.stationId)
    );

    return this.allStations().filter(
      station => !assignedIds.has(station.id)
    );
  };

  ngOnInit(): void {
    this.loadLines();
    this.loadAllStations();
  }

  loadLines(): void {
    this.adminService.getMetroLines().subscribe({
      next: lines => this.lines.set(lines)
    });
  }

  loadAllStations(): void {
    this.adminService.getStations(1, 1000).subscribe({
      next: response => this.allStations.set(response.items)
    });
  }

  selectLine(lineId: number | null): void {
    this.selectedLineId.set(lineId);
    this.lineStations.set([]);
    this.selectedStationId = null;
    this.newSequence = 1;

    if (lineId !== null) {
      this.loadLineStations(lineId);
    }
  }

  loadLineStations(lineId: number): void {
    this.loading.set(true);

    this.adminService.getLineStations(lineId).subscribe({
      next: stations => {
        this.lineStations.set(
          [...stations].sort((a, b) => a.sequence - b.sequence)
        );

        this.newSequence = stations.length + 1;
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  addStation(): void {
    const lineId = this.selectedLineId();

    if (lineId === null || this.selectedStationId === null) {
      alert('Please select a station.');
      return;
    }

    if (!Number.isInteger(this.newSequence) || this.newSequence < 1) {
      alert('Sequence must be a positive integer.');
      return;
    }

    this.saving.set(true);

    this.adminService.createLineStation(lineId, {
      stationId: this.selectedStationId,
      sequence: this.newSequence
    }).subscribe({
      next: () => {
        this.saving.set(false);
        this.selectedStationId = null;
        this.loadLineStations(lineId);
      },
      error: () => {
        this.saving.set(false);
        alert(
          'Failed to add station. Check if it is already assigned or if the sequence is valid.'
        );
      }
    });
  }

  updateSequence(item: LineStation, sequence: number): void {
    const lineId = this.selectedLineId();

    if (lineId === null) {
      return;
    }

    if (!Number.isInteger(sequence) || sequence < 1) {
      alert('Sequence must be a positive integer.');
      return;
    }

    if (sequence === item.sequence) {
      return;
    }

    this.adminService.changeLineStationSequence(
      lineId,
      item.stationId,
      { sequence }
    ).subscribe({
      next: () => this.loadLineStations(lineId),
      error: () => {
        alert('Failed to update sequence.');
        this.loadLineStations(lineId);
      }
    });
  }

  removeStation(item: LineStation): void {
    const lineId = this.selectedLineId();

    if (lineId === null) {
      return;
    }

    if (!confirm('Remove this station from the selected line?')) {
      return;
    }

    this.adminService.deleteLineStation(
      lineId,
      item.stationId
    ).subscribe({
      next: () => this.loadLineStations(lineId),
      error: () => alert('Failed to remove station.')
    });
  }

  getStationName(stationId: number): string {
    return this.allStations().find(
      station => station.id === stationId
    )?.name ?? `Station #${stationId}`;
  }
}