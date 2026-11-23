import { Component, input } from '@angular/core';
import { Station } from '../../models/station.model';

@Component({
  imports: [],
  selector: 'app-station-card',
  styleUrl: './station-card.css',
  templateUrl: './station-card.html',
})
export class StationCard {
  station = input.required<Station>();

  isFlipped = false;

  toggleFlip(): void {
    this.isFlipped = !this.isFlipped;
  }
}