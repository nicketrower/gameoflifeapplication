import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule,HttpTestingController } from '@angular/common/http/testing';
import { GameoflifeService } from './gameoflife.service';
import { SessionStore, SessionState, createInitialState, DEFAULT_BOARDID, DEFAULT_BOARDNAME,DEFAULT_HEIGHT, DEFAULT_WIDTH, DEFAULT_RESOLUTION, DEFAULT_SHOW_GRID } from './sessionstore.service';
import { environment } from '../../environments/environment';

describe('GameOfLifeService', () => {
  let service: GameoflifeService;
  let httpMock: HttpTestingController;
  let initialState: SessionState = {
    boardId: DEFAULT_BOARDID,
    boardName: DEFAULT_BOARDNAME,
    boardHeight: DEFAULT_HEIGHT,
    boardWidth: DEFAULT_WIDTH,
    boardResolution: DEFAULT_RESOLUTION,
    showGrid: DEFAULT_SHOW_GRID,
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [environment, SessionStore,HttpTestingController,HttpClientTestingModule],
      providers: [GameoflifeService]
    });

    service = TestBed.inject(GameoflifeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});