import { Component, input } from '@angular/core';
import { MetroLine } from '../../models/metro-line.model';

@Component({
  imports: [],
  selector: 'app-line-station-card',
  styleUrl: './line-station-card.css',
  templateUrl: './line-station-card.html',
})
export class LineStationCard {
  metroLine = input.required<MetroLine>();
}