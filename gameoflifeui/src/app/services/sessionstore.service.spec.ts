import { TestBed } from '@angular/core/testing';
import { SessionStore, SessionState, createInitialState, DEFAULT_BOARDID, DEFAULT_BOARDNAME,DEFAULT_HEIGHT, DEFAULT_WIDTH, DEFAULT_RESOLUTION, DEFAULT_SHOW_GRID } from './sessionstore.service';

describe('SessionStore', () => {
  let service: SessionStore;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SessionStore]
    });
    service = TestBed.inject(SessionStore);
  });

  it('should be created (positive test)', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with the correct default state (positive test)', () => {
    const initialState: SessionState = {
      boardId: DEFAULT_BOARDID,
      boardName: DEFAULT_BOARDNAME,
      boardHeight: DEFAULT_HEIGHT,
      boardWidth: DEFAULT_WIDTH,
      boardResolution: DEFAULT_RESOLUTION,
      showGrid: DEFAULT_SHOW_GRID,
    };
    expect(service.getValue()).toEqual(initialState);
  });
});