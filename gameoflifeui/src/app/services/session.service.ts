import { Injectable } from '@angular/core';
import {
  allowedDimensions,
  allowedResolutions,
  SessionState,
  SessionStore,
} from './sessionstore.service';
import { GameoflifeService } from './gameoflife.service';

@Injectable({ providedIn: 'root' })
export class SessionService {
  constructor(private _sessionStore: SessionStore, private _gofService: GameoflifeService) {}

  updateHeight(height: allowedDimensions) {
    this._sessionStore.update({ boardHeight: height });
  }

  updateWidth(width: allowedDimensions) {
    this._sessionStore.update({ boardWidth: width });
  }

  updateResolution(resolution: allowedResolutions) {
    this._sessionStore.update({ boardResolution: resolution });
  }

  update(state: Partial<SessionState>) {
    this._sessionStore.update({ ...state });
    //this._gofService.update({ ...state });
  }
}
