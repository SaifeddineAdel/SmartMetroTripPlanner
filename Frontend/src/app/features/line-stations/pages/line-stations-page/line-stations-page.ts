import { Component, inject, signal } from '@angular/core';

import { LineStations } from '../../services/line-stations';
import { MetroLine } from '../../models/metro-line.model';

@Component({
  selector: 'app-line-stations-page',
  styleUrl: './line-stations-page.css',
  templateUrl: './line-stations-page.html',
})
export class LineStationsPage {
  private readonly lineStationsService = inject(LineStations);

  readonly lines = [
    { id: 1, name: 'Line 1' },
    { id: 2, name: 'Line 2' },
    { id: 3, name: 'Line 3' }
  ];

  selectedLineId = signal<number | null>(null);
  selectedLine = signal<MetroLine | null>(null);

  loading = signal(false);
  error = signal('');

  selectLine(id: number): void {
    this.selectedLineId.set(id);
    this.selectedLine.set(null);

    this.loading.set(true);
    this.error.set('');

    this.lineStationsService.getById(id).subscribe({
      next: (data) => {
        this.selectedLine.set(data);
        this.loading.set(false);
      },
      error: (err) => {
        console.error(err);
        this.error.set('Failed to load metro line stations.');
        this.loading.set(false);
      }
    });
  }
}