import { TestBed } from '@angular/core/testing';
import { BehaviorSubject } from 'rxjs';
import { ButtonRowService, ButtonCommand } from './button-row.service';

describe('ButtonRowService', () => {
  let service: ButtonRowService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ButtonRowService]
    });
    service = TestBed.inject(ButtonRowService);
  });

  it('should be created (positive test)', () => {
    expect(service).toBeTruthy();
  });

  it('should toggle isRunning state (positive test)', () => {
    expect(service.getIsRunning).toBeFalse();
    service.toggleStartStop();
    expect(service.getIsRunning).toBeTrue();
    service.toggleStartStop();
    expect(service.getIsRunning).toBeFalse();
  });

  it('should emit isRunning state changes (positive test)', (done: DoneFn) => {
    const expectedStates = [false, true, false];
    let index = 0;
    service.selectIsRunning.subscribe(state => {
      expect(state).toBe(expectedStates[index]);
      index++;
      if (index === expectedStates.length) {
        done();
      }
    });
    service.toggleStartStop();
    service.toggleStartStop();
  });

  it('should push a valid command (positive test)', (done: DoneFn) => {
    const command: ButtonCommand = 'createRandomBoard';
    service.selectCommands.subscribe(cmd => {
      expect(cmd).toBe(command);
      done();
    });
    service.pushCommand(command);
  });

  it('should push another valid command (positive test)', (done: DoneFn) => {
    const command: ButtonCommand = 'createCleanBoard';
    service.selectCommands.subscribe(cmd => {
      expect(cmd).toBe(command);
      done();
    });
    service.pushCommand(command);
  });

  it('should handle empty command (positive test)', (done: DoneFn) => {
    const command: ButtonCommand = '';
    service.selectCommands.subscribe(cmd => {
      expect(cmd).toBe(command);
      done();
    });
    service.pushCommand(command);
  });

  it('should fail to push an invalid command (negative test)', () => {
    const invalidCommand: any = 'invalidCommand';
    expect(() => service.pushCommand(invalidCommand)).toThrowError();
  });
});