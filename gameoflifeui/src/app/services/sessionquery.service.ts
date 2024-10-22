import { Injectable } from '@angular/core';
import { Query } from '@datorama/akita';
import { SessionState, SessionStore } from './sessionstore.service';

@Injectable({ providedIn: 'root' })
export class SessionQuery extends Query<SessionState> {
  constructor( store: SessionStore) {
    super(store);
  }
}
