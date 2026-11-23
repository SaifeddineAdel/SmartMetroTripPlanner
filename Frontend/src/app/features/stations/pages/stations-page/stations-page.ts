import { Component, OnInit, computed, inject, signal } from '@angular/core';

import { Station } from '../../models/station.model';
import { Stations } from '../../services/stations';
import { StationCard } from '../../components/station-card/station-card';

@Component({
  imports: [StationCard],
  selector: 'app-stations-page',
  styleUrl: './stations-page.css',
  templateUrl: './stations-page.html',
})
export class StationsPage implements OnInit {
  private readonly stationsService = inject(Stations);

  stations = signal<Station[]>([]);

  pageNumber = signal(1);
  pageSize = signal(10);
  totalPages = signal(0);

  hasPreviousPage = signal(false);
  hasNextPage = signal(false);

  pageNumbers = computed(() =>
    Array.from(
      { length: this.totalPages() },
      (_, index) => index + 1
    )
  );

  searchTerm = signal('');

  filteredStations = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();

    if (!term) {
      return this.stations();
    }

    return this.stations().filter(station =>
      station.name.toLowerCase().includes(term)
    );
  });

  loading = signal(false);
  error = signal('');

  ngOnInit(): void {
    this.loadStations();
  }

  loadStations(): void {
    this.loading.set(true);
    this.error.set('');

    this.stationsService.getAll(
      this.pageNumber(),
      this.pageSize()
    ).subscribe({
      next: (response) => {
        this.stations.set(response.items);

        this.pageNumber.set(response.pageNumber);
        this.pageSize.set(response.pageSize);
        this.totalPages.set(response.totalPages);

        this.hasPreviousPage.set(response.hasPreviousPage);
        this.hasNextPage.set(response.hasNextPage);

        this.loading.set(false);
      },

      error: (err) => {
        console.error(err);
        this.error.set('Failed to load stations.');
        this.loading.set(false);
      }
    });
  }

  goToPage(page: number): void {
    if (
      page < 1 ||
      page > this.totalPages() ||
      page === this.pageNumber()
    ) {
      return;
    }

    this.pageNumber.set(page);
    this.loadStations();
  }

  previousPage(): void {
    if (this.hasPreviousPage()) {
      this.goToPage(this.pageNumber() - 1);
    }
  }

  nextPage(): void {
    if (this.hasNextPage()) {
      this.goToPage(this.pageNumber() + 1);
    }
  }
}