import { TestBed } from '@angular/core/testing';
import { SessionQuery } from './sessionquery.service';
import { SessionStore } from './sessionstore.service';
import { SessionState } from './sessionstore.service';
import { Query } from '@datorama/akita';

describe('SessionQuery', () => {
  let service: SessionQuery;
  let store: SessionStore;
  let state: SessionState;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SessionQuery, SessionStore]
    });
    store = TestBed.inject(SessionStore);
    service = TestBed.inject(SessionQuery);
  });

  it('should be created (positive test)', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with the correct store (positive test)', () => {
    expect(service['store']).toBe(store);
  });

});