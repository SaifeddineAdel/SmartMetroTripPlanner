import { Component, inject, signal, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService, Station } from '../../services/admin.service';

@Component({
  selector: 'app-stations-management',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './stations-management.html',
  styleUrl: './stations-management.css'
})
export class StationsManagement implements OnInit {
  private readonly adminService = inject(AdminService);

  stations = signal<Station[]>([]);
  loading = signal(false);
  saving = signal(false);
  showForm = signal(false);
  editingStation = signal<Station | null>(null);
  message = signal('');

  pageNumber = signal(1);
  pageSize = 10;
  totalCount = signal(0);
  totalPages = signal(1);

  name = '';
  description = '';

  ngOnInit(): void {
    this.loadStations();
  }

  loadStations(): void {
    this.loading.set(true);

    this.adminService.getStations(this.pageNumber(), this.pageSize)
      .subscribe({
        next: response => {
          this.stations.set(response.items);
          this.totalCount.set(response.totalCount);
          this.totalPages.set(Math.max(response.totalPages, 1));
          this.loading.set(false);
        },
        error: () => {
          this.message.set('Failed to load stations.');
          this.loading.set(false);
        }
      });
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }

    this.pageNumber.set(page);
    this.loadStations();
  }

  openCreate(): void {
    this.editingStation.set(null);
    this.name = '';
    this.description = '';
    this.showForm.set(true);
  }

  openEdit(station: Station): void {
    this.editingStation.set(station);
    this.name = station.name;
    this.description = station.description;
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
    this.editingStation.set(null);
  }

  save(): void {
    if (!this.name.trim() || !this.description.trim()) {
      this.message.set('Name and description are required.');
      return;
    }

    this.saving.set(true);
    this.message.set('');

    const station = this.editingStation();

    if (!station) {
      this.adminService.createStation({
        name: this.name.trim(),
        description: this.description.trim()
      }).subscribe({
        next: () => this.finishSave('Station created successfully.'),
        error: () => this.failSave('Failed to create station.')
      });

      return;
    }

    this.adminService.renameStation(station.id, {
      name: this.name.trim()
    }).subscribe({
      next: () => {
        this.adminService.updateStationDescription(station.id, {
          description: this.description.trim()
        }).subscribe({
          next: () => this.finishSave('Station updated successfully.'),
          error: () => this.failSave(
            'Name was updated, but description update failed.'
          )
        });
      },
      error: () => this.failSave('Failed to update station.')
    });
  }

  deleteStation(station: Station): void {
    if (!confirm(`Delete station "${station.name}"?`)) {
      return;
    }

    this.adminService.deleteStation(station.id).subscribe({
      next: () => {
        this.message.set('Station deleted successfully.');
        this.loadStations();
      },
      error: () => {
        this.message.set('Failed to delete station.');
      }
    });
  }

  private finishSave(message: string): void {
    this.saving.set(false);
    this.closeForm();
    this.message.set(message);
    this.loadStations();
  }

  private failSave(message: string): void {
    this.saving.set(false);
    this.message.set(message);
  }
}