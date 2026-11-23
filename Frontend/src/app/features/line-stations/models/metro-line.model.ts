export interface MetroLine {
  id: number;
  name: string;
  stations: MetroLineStation[];
}

export interface MetroLineStation {
  id: number;
  name: string;
  sequence: number;
}