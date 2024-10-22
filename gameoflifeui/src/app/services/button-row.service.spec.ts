import { TestBed } from '@angular/core/testing';

import { ButtonRowService } from './button-row.service';

describe('ButtonRowService', () => {
  let service: ButtonRowService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ButtonRowService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
