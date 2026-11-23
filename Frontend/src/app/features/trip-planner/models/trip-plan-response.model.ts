import { StationData } from './station-data.model';
import { MetroLineData } from './metro-line-data.model';

export interface TripPlanResponse {
  startStation: StationData;
  destinationStation: StationData;
  segments: TripSegmentResponse[];
  totalStationCount: number;
  totalEstimatedMinutes: number;
  fare: number;
}

export interface TripSegmentResponse {
  metroLine: MetroLineData;
  stations: StationData[];
  stationCount: number;
  transitionCount: number;
  estimatedMinutes: number;
  isTransferAfter: boolean;
}