import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AdminService, MetroLine } from '../../services/admin.service';

@Component({
  selector: 'app-metro-lines-management',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './metro-lines-management.html',
  styleUrl: './metro-lines-management.css'
})
export class MetroLinesManagement implements OnInit {
  private readonly adminService = inject(AdminService);

  lines = signal<MetroLine[]>([]);
  loading = signal(false);
  saving = signal(false);
  showForm = signal(false);
  editingLine = signal<MetroLine | null>(null);
  message = signal('');

  name = '';

  ngOnInit(): void {
    this.loadLines();
  }

  loadLines(): void {
    this.loading.set(true);

    this.adminService.getMetroLines().subscribe({
      next: lines => {
        this.lines.set(lines);
        this.loading.set(false);
      },
      error: () => {
        this.message.set('Failed to load metro lines.');
        this.loading.set(false);
      }
    });
  }

  openCreate(): void {
    this.editingLine.set(null);
    this.name = '';
    this.showForm.set(true);
  }

  openEdit(line: MetroLine): void {
    this.editingLine.set(line);
    this.name = line.name;
    this.showForm.set(true);
  }

  closeForm(): void {
    this.showForm.set(false);
    this.editingLine.set(null);
  }

  save(): void {
    if (!this.name.trim()) {
      this.message.set('Line name is required.');
      return;
    }

    this.saving.set(true);
    this.message.set('');

    const line = this.editingLine();
    const request = { name: this.name.trim() };

    if (line) {
      this.adminService.renameMetroLine(line.id, request).subscribe({
        next: () => {
          this.finishSave('Metro line updated successfully.');
        },
        error: () => {
          this.failSave('Failed to update metro line.');
        }
      });
    } else {
      this.adminService.createMetroLine(request).subscribe({
        next: () => {
          this.finishSave('Metro line created successfully.');
        },
        error: () => {
          this.failSave('Failed to create metro line.');
        }
      });
    }
  }

  private finishSave(message: string): void {
    this.saving.set(false);
    this.closeForm();
    this.message.set(message);
    this.loadLines();
  }

  private failSave(message: string): void {
    this.saving.set(false);
    this.message.set(message);
  }

  deleteLine(line: MetroLine): void {
    if (!confirm(`Delete metro line "${line.name}"?`)) {
      return;
    }

    this.adminService.deleteMetroLine(line.id).subscribe({
      next: () => {
        this.message.set('Metro line deleted successfully.');
        this.loadLines();
      },
      error: () => {
        this.message.set('Failed to delete metro line.');
      }
    });
  }
}