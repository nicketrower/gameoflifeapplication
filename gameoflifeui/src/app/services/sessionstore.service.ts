import { Injectable } from '@angular/core';
import { Store, StoreConfig } from '@datorama/akita';

export interface SessionState {
  boardId: number;
  boardName: string;
  boardHeight: allowedDimensions;
  boardWidth: allowedDimensions;
  boardResolution: allowedResolutions;
  showGrid: boolean;
}

export function createInitialState(): SessionState {
  return {
    boardId: DEFAULT_BOARDID,
    boardName: DEFAULT_BOARDNAME,
    boardHeight: DEFAULT_HEIGHT,
    boardWidth: DEFAULT_WIDTH,
    boardResolution: DEFAULT_RESOLUTION,
    showGrid: DEFAULT_SHOW_GRID,
  };
}

@Injectable({ providedIn: 'root' })
@StoreConfig({ name: 'session' })
export class SessionStore extends Store<SessionState> {
  constructor() {
    super(createInitialState());
  }
}

export const dimensionsSelection = [
  400, 800, 1200, 1600, 2000, 2400, 2800, 3200,
] as const;

export type allowedDimensions = typeof dimensionsSelection[number];

export const resolutionsSelection = [5, 10, 20, 40, 50, 80] as const;
export type allowedResolutions = typeof resolutionsSelection[number];

export const DEFAULT_BOARDID = 1;
export const DEFAULT_BOARDNAME = 'Game of Life';
export const DEFAULT_WIDTH = dimensionsSelection[1];
export const DEFAULT_HEIGHT = DEFAULT_WIDTH;
export const DEFAULT_RESOLUTION = resolutionsSelection[1];
export const DEFAULT_SHOW_GRID = false;
