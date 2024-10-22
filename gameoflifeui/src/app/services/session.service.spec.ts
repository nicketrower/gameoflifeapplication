import { TestBed } from '@angular/core/testing';
import { SessionQuery } from './sessionquery.service';
import { SessionStore } from './sessionstore.service';
import { Query } from '@datorama/akita';

describe('SessionQuery', () => {
  let service: SessionQuery;
  let store: SessionStore;

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

  it('should fail to initialize if SessionStore is missing (negative test)', () => {
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      providers: [SessionQuery]
      // Intentionally omitting SessionStore to simulate failure
    });

    expect(() => {
      TestBed.inject(SessionQuery);
    }).toThrowError();
  });
});