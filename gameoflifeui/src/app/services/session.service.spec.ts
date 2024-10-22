import { TestBed } from '@angular/core/testing';

import { GameoflifeService } from './gameoflife.service';

describe('GameoflifeService', () => {
  let service: GameoflifeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GameoflifeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
